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
    public class ClickedCardOptionsDialog : Dialog
    {
        UseBoardActivity callerInstance;

        public ClickedCardOptionsDialog(Context _context) : base(_context) { callerInstance = (UseBoardActivity)_context; this.Show(); }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.clicked_card_options_dialog);

            var editCard = FindViewById<Button>(Resource.Id.editCardBtn);
            var detailsCard = FindViewById<Button>(Resource.Id.detailsCardBtn);

            // Launches the edit card activity
            editCard.Click += (e, a) =>
            {
                Dismiss();
                Intent editCardActivity = new Intent(callerInstance, typeof(EditCard));

                //TODO: Add ID Extra

                callerInstance.StartActivity(editCardActivity);
            };

            // Launches the details card activity
            detailsCard.Click += (e, a) =>
            {                
                Dismiss();
                Intent detailsCardActivity = new Intent(callerInstance, typeof(DetailsCard));

                //TODO: Add ID Extra

                callerInstance.StartActivity(detailsCardActivity);
            };
        }
    }
}