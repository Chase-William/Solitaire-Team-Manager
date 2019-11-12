using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Solitaire
{
    ///
    /// 
    ///     Provides a way for the user to choose whether they want to create a new contact or use a pre-existing contact 
    /// 
    ///
    public class ContributorOptionsDialog : Dialog
    {
        UseBoardActivity callerInstance;
        public ContributorOptionsDialog(Context _context) : base(_context) 
        { 
            callerInstance = (UseBoardActivity)_context; this.Show();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.contributor_options_dialog);


            // Starts the activity to create a new contributor
            FindViewById<Button>(Resource.Id.addNewContributor).Click += delegate
            {
                Intent createContributor = new Intent(callerInstance, typeof(CreateContributorActivity));
                callerInstance.StartActivity(createContributor);
                Dismiss();
            };
            FindViewById<Button>(Resource.Id.addExistingContributor).Click += delegate
            {               
                new AddExistingContributorDialog(callerInstance);
                Dismiss();
            };
        }
    }
}