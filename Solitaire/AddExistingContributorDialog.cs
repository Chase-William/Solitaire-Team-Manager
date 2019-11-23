using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.Accessibility;
using Android.Widget;
using Java.Interop;
using Java.Lang;
using Solitaire.Lang;

namespace Solitaire
{
    public class AddExistingContributorDialog : Dialog
    {
        UseBoardActivity callingActivity;
        List<Contributor> selectedContribtors = new List<Contributor>();
        Context context;
        public AddExistingContributorDialog(Context _context) : base(_context)
        {
            callingActivity = (UseBoardActivity)_context;
            this.Show();
            context = _context;
        }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.add_existing_contributor);

            ListView ExisitingContributors = FindViewById<ListView>(Resource.Id.existingContacts);

            ExisitingContributors.Adapter = new ContributorsAdapter(AssetManager.contributors, callingActivity);
            //ExisitingContributors.ItemClick += ContributorSelected;
            FindViewById<Button>(Resource.Id.add_button).Click += delegate
            {
                // TODO: take selected and add them to the board

                new CreateContributorDialog(context);
                Dismiss();
            };
            FindViewById<Button>(Resource.Id.cancel_button).Click += delegate
            {
                // TODO: Add existing contributors logic here and like how will be show them

                Dismiss();
            };
        }

        //private void ContributorSelected(object sender, AdapterView.ItemClickEventArgs e)
        //{
        //    Contributor contr = callingActivity.thisBoard.Contributors[e.Position];
        //    foreach(Contributor contributor in selectedContribtors)
        //    {
        //        if(contributor == contr)
        //        {
        //            selectedContribtors.Remove(contributor);
        //            break;
        //        }
        //    }
        //    selectedContribtors.Add(contr);
        //}
    }
}