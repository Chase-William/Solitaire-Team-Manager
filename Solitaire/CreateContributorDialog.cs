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
        Context context;

        public CreateContributorDialog(Context _context) : base(_context)
        {
            this.Show();
            context = _context;
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
                if ((UseBoardActivity)context is UseBoardActivity)
                {
                    UseBoardActivity thisContext = (UseBoardActivity)context;
                    AssetManager.contributors.Add(new Lang.Contributor(contributorName.Text, contributorEmail.Text));
                }
                else
                {
                    AssetManager.contributors.Add(new Lang.Contributor(contributorName.Text, contributorEmail.Text));
                }

                Dismiss();
            };

            FindViewById<Button>(Resource.Id.cancelNewContributor).Click += delegate
            {
                Dismiss();
            };
        }

    }
}