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
    public class CreateCardDialog : Dialog
    {
        UseBoardActivity callerInstance;
        private List<string> categories = new List<string>();

        public CreateCardDialog(UseBoardActivity _context) : base(_context) { callerInstance = _context; this.Show(); }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.create_card_dialog);

            var createDeckBtn = FindViewById<Button>(Resource.Id.createDeckBtn);
            var cancelDeckBtn   = FindViewById<Button>(Resource.Id.cancelDeckBtn);
            var categorySpinner = FindViewById<Spinner>(Resource.Id.categorySpinnerDialog);


            //
            //      IMPORTANT: this list is used for the Spinner's Adapter!     
            //
            foreach (var deck in UseBoardActivity.thisKanban.Columns)
            {
                // A Deck only is allowed 1 category right now hence we index 0
                categories.Add((string)deck.Categories[0]);
            }


            // TODO: Customize the spinner adapter


            // Creating our adapter for our spinner which allows the user to choose which category this shall be assigned to
            var adapter = new ArrayAdapter<string>(callerInstance, Android.Resource.Layout.SimpleSpinnerItem, categories);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            categorySpinner.Adapter = adapter;

            // Dismisses dialog and creates a new card
            createDeckBtn.Click += (e, a) =>
            {
                string name = FindViewById<EditText>(Resource.Id.nameEditTextDialog).Text.Trim();

                // Getting the titles of the kanbanModels within the IEnumerable list, kanbanmodels are the cards basically
                List<string> names = new List<string>();
                foreach (KanbanModel item in UseBoardActivity.thisKanban.ItemsSource) 
                { 
                    names.Add(item.Title); 
                }



                // TODO: Use Linq instead of the foreach loop above




                // Checks to make sure that the name doesn't already exist and isn't a space filled string
                if (name != "" && names.All(usedName => usedName != name))
                    callerInstance.AddCard(name, FindViewById<EditText>(Resource.Id.descriptionTextDialog).Text.Trim(), categorySpinner.SelectedItem.ToString());
                else
                {
                    Toast.MakeText(callerInstance, "This Name is already used.", ToastLength.Long);
                    return;
                }                    
                Dismiss();
            };

            // Dismisses the dialog without creating a new card
            cancelDeckBtn.Click += (e, a) =>
            {
                Dismiss();
            };            
        }
    }
}