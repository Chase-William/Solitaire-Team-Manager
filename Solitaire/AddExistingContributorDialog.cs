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
        UseBoardActivity callerInstance;
        List<Contributor> selectedContribtors = new List<Contributor>();

        public AddExistingContributorDialog(Context _context) : base(_context)
        {
            callerInstance = (UseBoardActivity)_context;
            this.Show();
        }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.add_existing_contributor);

            ListView ExisitingContributors = FindViewById<ListView>(Resource.Id.existingContacts);

            ExisitingContributors.Adapter = new ExisitingContributorsAdapter(callerInstance.thisBoard.Contributors);
            ExisitingContributors.ItemClick += ContributorSelected;
            FindViewById<Button>(Resource.Id.add_button).Click += delegate
            {
                // TODO: take selected and add them to the board
                
                //callerInstance.thisBoard.Contributors.Add()
                Dismiss();
            };
            FindViewById<Button>(Resource.Id.cancel_button).Click += delegate
            {
                // TODO: Add existing contributors logic here and like how will be show them

                Dismiss();
            };
        }

        private void ContributorSelected(object sender, AdapterView.ItemClickEventArgs e)
        {
            Contributor contr = callerInstance.thisBoard.Contributors[e.Position];
            foreach(Contributor contributor in selectedContribtors)
            {
                if(contributor == contr)
                {
                    selectedContribtors.Remove(contributor);
                    break;
                }
            }
            selectedContribtors.Add(contr);
        }
    }
}