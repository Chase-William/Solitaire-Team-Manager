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
    class CreateContributorDialog : Dialog
    {
        Context callerActivity;

        public CreateContributorDialog(Context _context) : base(_context)
        {
            this.Show();
            callerActivity = _context;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.add_new_contributor);

            TextView contributorName = FindViewById<TextView>(Resource.Id.contributorName);
            TextView contributorEmail = FindViewById<TextView>(Resource.Id.contributorEmail);

            // Starts the activity to create a new contributor
            FindViewById<Button>(Resource.Id.addNewContributor).Click += delegate
            {
                string email = contributorEmail.Text.Trim();

                if (email == "" || email == null)
                {
                    Toast.MakeText(callerActivity, "Email field cannot be empty.", ToastLength.Short).Show();
                    return;
                }

                if (AssetManager.contributors.Count > 0)
                {
                    if (AssetManager.contributors.Any(contributor => contributor.Email == email))
                    {
                        Toast.MakeText(callerActivity, $"Email {email} is already being used.", ToastLength.Short).Show();
                        return;
                    }
                }

                AssetManager.contributors.Add(new Lang.Contributor(contributorName.Text.Trim(), email));

                Dismiss();
            };

            FindViewById<Button>(Resource.Id.cancelNewContributor).Click += delegate
            {
                Dismiss();
            };
        }

    }
}