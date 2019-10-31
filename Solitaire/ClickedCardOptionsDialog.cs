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
using Syncfusion.SfKanban.Android;

namespace Solitaire
{
    public class ClickedCardOptionsDialog : Dialog
    {
        UseBoardActivity callerInstance;
        string name, description;

        public ClickedCardOptionsDialog(UseBoardActivity _context, string _name, string _description) : base(_context) 
        {
            callerInstance = _context;
            name = _name;
            description = _description;
            this.Show(); 
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.clicked_card_options_dialog);

            var editCard = FindViewById<Button>(Resource.Id.editCardBtn);
            var detailsCard = FindViewById<Button>(Resource.Id.detailsCardBtn);

            /*
                I really wanted to just use the unsafe or fixed keyword and pass in a memory address so I didnt need to pass each property..
                But you can't using memory address getting boy "&" on managed types... non-structs, woulda been funish
            */

            // Launches the edit card activity
            editCard.Click += (e, a) =>
            {
                Dismiss();
                // Starting the edit Activity
                Intent editCardActivity = new Intent(callerInstance, typeof(EditCardActivity));
                editCardActivity.PutExtra("Name", name);
                editCardActivity.PutExtra("Description", description);
                callerInstance.StartActivityForResult(editCardActivity, 1);
            };

            // Launches the details card activity
            detailsCard.Click += (e, a) =>
            {                
                Dismiss();
                // Starting the details Activity
                Intent detailsCardActivity = new Intent(callerInstance, typeof(DetailsCardActivity));
                detailsCardActivity.PutExtra("Name", name);
                detailsCardActivity.PutExtra("Description", description);
                callerInstance.StartActivity(detailsCardActivity);
            };            
        }
    }
}