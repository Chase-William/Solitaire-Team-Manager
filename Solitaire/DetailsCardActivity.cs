using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Syncfusion.SfKanban.Android;

namespace Solitaire
{
    [Activity(Label = "DetailsCardActivity")]
    public class DetailsCardActivity : AppCompatActivity
    {
        long kanbanModelId;
        KanbanModel clickedKanbanModel;
        private const int EDIT_ACTIVITY_CODE = 3;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.details_card_activity);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "Card Details";
            SetSupportActionBar(toolbar);

            // Getting the Id of the kanbanModel so we can setup a pointer to it
            kanbanModelId =  this.Intent.GetLongExtra("kanbanModelId", -1);
            
            // Getting our kanbanModel from our static Sfkanban inside UseBoardActivity
            // IEnumberables are annoying because then I gotta cast everything
            
            clickedKanbanModel = UseBoardActivity.thisKanban.ItemsSource.Cast<KanbanModel>().Single(kanbanModel => kanbanModel.ID == kanbanModelId);
            
            

            // Setting the textviews to display the information about the kanbanModel
            FindViewById<TextView>(Resource.Id.cardNameTextView).Text = clickedKanbanModel.Title;
            FindViewById<TextView>(Resource.Id.cardDescriptionTextView).Text = clickedKanbanModel.Description;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.details_card_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // Comparing the title of the toolbar btns to a string to determine which was clicked
            switch (item.TitleFormatted.ToString())
            {
                case "Edit":
                    // Starting an activity to edit our kanbanModel
                    // We pass the Id of the kanbanModel to the edit Activity so we can basically get a pointer back to the original kanbanModel
                    // I really don't like passing all the data via PutExtras... really like just passing a primary key or something to a collection
                    Intent editCardActivity = new Intent(this, typeof(EditCardActivity));
                    editCardActivity.PutExtra("kanbanModelId", kanbanModelId);                        
                    StartActivityForResult(editCardActivity, EDIT_ACTIVITY_CODE);
                    break;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        /// 
        /// 
        ///     When the EditCardActivity finishe we will check to see if the values were saved, therefore we must signal the UseBoardActivity to update its UI
        ///       
        /// 
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            // If the resultCode is equal to Result.Ok then we will manually tell the UI to refresh
            if (requestCode == EDIT_ACTIVITY_CODE && resultCode == Result.Ok)
            {
                SetResult(Result.Ok);
            }            
        }


        // When this resume we reassign the screen the proper values
        protected override void OnResume()
        {
            base.OnResume();
            FindViewById<TextView>(Resource.Id.cardNameTextView).Text = clickedKanbanModel.Title;
            FindViewById<TextView>(Resource.Id.cardDescriptionTextView).Text = clickedKanbanModel.Description;
        }
    }
}