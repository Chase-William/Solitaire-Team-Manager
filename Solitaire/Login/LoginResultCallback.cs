using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common.Apis;
using Java.Lang;

namespace Solitaire
{
    public class LoginResultCallback : Object, IResultCallback
    {
        public GoogleLoginActivity Activity { get; set; }

        public void OnResult(Object result)
        {
            var googleSignInResult = result as GoogleSignInResult;
            Activity.HideProgressDialog();
            Activity.HandleSignInResult(googleSignInResult);
        }
    }
}
