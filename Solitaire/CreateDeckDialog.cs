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
    public class CreateDeckDialog : Dialog
    {
        UseBoardActivity callerInstance;

        public CreateDeckDialog(UseBoardActivity _context) : base(_context) { callerInstance = _context; this.Show(); }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.create_deck_dialog);

            var createDeckBtn = FindViewById<Button>(Resource.Id.createDeckBtn);
            var cancelDeckBtn = FindViewById<Button>(Resource.Id.cancelDeckBtn);

            // Dismisses dialog and creates a new deck
            createDeckBtn.Click += (e, a) =>
            {
                string name = FindViewById<EditText>(Resource.Id.nameEditTextDialog).Text.Trim();

                // Checks to make sure this name doesnt not already exist within the respective board, because we will be using the board's title as the category name
                if (name != "" && callerInstance.thisKanban.Columns.All(deck => deck.Title != name))
                    callerInstance.AddDeck(name, FindViewById<EditText>(Resource.Id.descriptionTextDialog).Text.Trim());
                else
                {
                    Toast.MakeText(callerInstance, "This Name is already used.", ToastLength.Long);
                    return;
                }

                Dismiss();
            };

            // Dismisses the dialog without creating a new deck
            cancelDeckBtn.Click += (e, a) =>
            {
                Dismiss();
            };
        }     
    }
}