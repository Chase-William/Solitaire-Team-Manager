using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Common.Apis;
using Android.Support.V7.App;
using Android.Gms.Common;
using Android.Util;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Auth.Api;

/*
 
    I did NOT write all this code myself <Chase Roth>, although alot of it has been modified and or studied.
    Alot of it comes from Google API documentation in order for it work and be completed in a timely fashion.

*/
namespace Solitaire
{
    // IMPORTANT: NoHistory needs to be true so our app doesn't refer to this as the "home" or "index" 
    [Activity(Theme = "@style/ThemeOverlay.MyNoTitleActivity", NoHistory = true)]
    [Register("com.xamarin.signinquickstart.MainActivity")]
    public class GoogleLoginActivity : AppCompatActivity, GoogleApiClient.IOnConnectionFailedListener
    {
        private const string SIGNED_IN_FMT = "Signed in as: ";
        private const string SIGNED_OUT = "Signed out";        

        // Code used for activity result to identify
        const int RC_SIGN_IN = 9001;

        GoogleApiClient thisUsersGoogleApiClient;
        TextView mStatusTextView;
        Dialog mProgressDialog;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.google_login_activity);

            mStatusTextView = FindViewById<TextView>(Resource.Id.status);

            // Could've used the View.IOnClickListener but this looks cleaner and simpler in my opinion
            // Even use keyword "delegate" instead of Lambda because I dont wannt show the params if we arn't using them
            FindViewById(Resource.Id.sign_in_button).Click += delegate { SignIn(); };
            FindViewById(Resource.Id.sign_out_button).Click += delegate { SignOut(); };
            FindViewById(Resource.Id.disconnect_button).Click += delegate { RevokeAccess(); };


            // Configure sign-in to request the user's ID, email address, and basic
            // profile. ID and basic profile are included in DEFAULT_SIGN_IN.           
            var gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestEmail()
                .Build();

            // [START build_client]
            // Build a GoogleApiClient with access to the Google Sign-In API and the
            // options specified by gso.
            thisUsersGoogleApiClient = new GoogleApiClient.Builder(this)
                    .EnableAutoManage(this /* FragmentActivity */, this /* OnConnectionFailedListener */)                    
                    .AddOnConnectionFailedListener(this)
                    .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
                    .Build();
            // [END build_client]

            // [START customize_button]
            // Set the dimensions of the sign-in button.
            var signInButton = FindViewById<SignInButton>(Resource.Id.sign_in_button);
            signInButton.SetSize(SignInButton.SizeStandard);
            // [END customize_button]
        }

        /// 
        /// 
        ///     Called immediately after OnCreate() and attempts to automatically login the user
        ///     off cached credentials
        ///
        ///
        protected override void OnStart()
        {
            base.OnStart();

            // Attempts to login the user using their possibly cached credentials and will "out" to the params
            OptionalPendingResult possibleCachedCredentials = Auth.GoogleSignInApi.SilentSignIn(thisUsersGoogleApiClient);
            // If we successfully got the cached credentials the OptionalPendingResult will be marked as "done":
            if (possibleCachedCredentials.IsDone)
            {
                // Since we got the credentials we can get cast it to the GoogleSignInResult
                // "as" keyword will return "null" if the cast fails
                // Call the HandleSignInResult
                HandleSignInResult(possibleCachedCredentials.Get() as GoogleSignInResult);
            }
            // We couldn't find the credentials within the cach, therefore start login process:
            else
            {
                // If the user has not previously signed in on this device or the sign-in has expired,
                // this asynchronous branch will attempt to sign in the user silently.  Cross-device
                // single sign-on will occur in this branch.
                ShowProgressDialog();
                possibleCachedCredentials.SetResultCallback(new LoginResultCallback { Activity = this });
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            HideProgressDialog();
        }
      
        /// 
        /// 
        ///     Getting the data back about the attempt to Authorize the user
        /// 
        ///
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            // Result returned from launching the Intent from GoogleSignInApi.getSignInIntent(...);
            if (requestCode == RC_SIGN_IN)
            {
                var result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                HandleSignInResult(result);
            }
        }

        ///
        /// 
        ///     Handles the sign in result by telling our app what to do next based off the result
        /// 
        ///
        public void HandleSignInResult(GoogleSignInResult result)
        {
            if (result.IsSuccess)
            {
                // Signed in successfully, show authenticated UI.
                AssetManager.thisGoogleAccount = result.SignInAccount;
                mStatusTextView.Text = $"{SIGNED_IN_FMT} { AssetManager.thisGoogleAccount.DisplayName}";
                UpdateUI(true);

                // Starting our application now that the user has successfully logged in
                StartActivity(new Intent(this, typeof(MainActivity)));
            }
            else
            {
                // User has signed out so update our UI
                UpdateUI(false);
            }
        }

        /// 
        /// 
        ///     When the user wants to login in this method will start the signin process        
        ///     Then once the first method is done it will trigger the "SignOutResultCallback" to execute hence the "callback" part
        ///     
        ///
        void SignIn()
        {
            var attemptSignIn = Auth.GoogleSignInApi.GetSignInIntent(thisUsersGoogleApiClient);
            StartActivityForResult(attemptSignIn, RC_SIGN_IN);
        }

        /// 
        /// 
        ///     When the user wants to loggout this will start the process like SignIn() with a callback
        /// 
        ///
        void SignOut()
        {
            Auth.GoogleSignInApi.SignOut(thisUsersGoogleApiClient).SetResultCallback(new LoggoutResultCallback { Activity = this });
        }

        /// 
        /// 
        ///     When the user is denied access this will start the process like SignIn() with a callback
        /// 
        ///
        void RevokeAccess()
        {
            Auth.GoogleSignInApi.RevokeAccess(thisUsersGoogleApiClient).SetResultCallback(new LoggoutResultCallback { Activity = this });
        }

        /// 
        /// 
        ///     If the connection failed then we inform the user
        ///                
        /// 
        public void OnConnectionFailed(ConnectionResult result)
        {
            Toast.MakeText(this, "Connection Failed. Make sure your wifi is enabled.", ToastLength.Short);
        }

        protected override void OnStop()
        {
            base.OnStop();
            thisUsersGoogleApiClient.Disconnect();
        }

        public void ShowProgressDialog()
        {
            if (mProgressDialog == null)
            {
                mProgressDialog = new Dialog(this);
                //mProgressDialog.SetMessage(GetString(Resource.String.loading));
                //mProgressDialog.Indeterminate = true;
            }

            mProgressDialog.Show();
        }

        public void HideProgressDialog()
        {
            if (mProgressDialog != null && mProgressDialog.IsShowing)
            {
                mProgressDialog.Hide();
            }
        }

        ///
        /// 
        ///     Will update our UI's buttons based on the results of our signin attempt
        /// 
        /// 
        public void UpdateUI(bool isSignedIn)
        {
            if (isSignedIn)
            {
                FindViewById(Resource.Id.sign_in_button).Visibility = ViewStates.Gone;
                FindViewById(Resource.Id.sign_out_and_disconnect).Visibility = ViewStates.Visible;
            }
            else
            {
                mStatusTextView.Text = SIGNED_OUT;
                Toast.MakeText(this, "Sign in failed.", ToastLength.Short).Show();
                FindViewById(Resource.Id.sign_in_button).Visibility = ViewStates.Visible;
                FindViewById(Resource.Id.sign_out_and_disconnect).Visibility = ViewStates.Gone;
            }
        }        
    }
}


