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
    [Activity(Label = "EditCard")]
    public class EditCardActivity : AppCompatActivity
    {
        EditText cardNameEditText;
        EditText cardDescriptionEditText;
        KanbanModel clickedKanbanModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.edit_card_activity);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "Edit Card";
            SetSupportActionBar(toolbar);

            // Finding our kanbanModel inside the item source while using the intent extra we put from the calling activity
            clickedKanbanModel = UseBoardActivity.thisKanban.ItemsSource.Cast<KanbanModel>().Single(kanbanModel => kanbanModel.ID == this.Intent.GetLongExtra("kanbanModelId", -1));    




            // TODO: Might need to check name for null and other unacceptable values




            // Setting up the pointers to the TextViews
            cardNameEditText = FindViewById<EditText>(Resource.Id.cardNameEditText);
            cardDescriptionEditText = FindViewById<EditText>(Resource.Id.cardDescriptionEditText);
            // Assigning the textviews the current values of the kanbanModel
            cardNameEditText.Text = clickedKanbanModel.Title;
            cardDescriptionEditText.Text = clickedKanbanModel.Description;            
        }

        /// 
        /// 
        ///     Saving the data to the KanbanModel (NOT THE BOARD'S CARD) and returning to the details activity
        /// 
        ///
        private void SaveAndFinishedEditing()
        {
            // Array of all the names to check with
            string[] names = UseBoardActivity.kanbanModels.Where(kanban => !kanban.Equals(this.clickedKanbanModel)).Select(kanban => kanban.Title).ToArray();

            string name = cardNameEditText.Text.Trim();
            // Checks to make sure that the name doesn't already exist and isn't a space filled string
            if (name != "" && names.All(usedName => usedName != name))
            {
                clickedKanbanModel.Title = cardNameEditText.Text;
                clickedKanbanModel.Description = cardDescriptionEditText.Text;
                SetResult(Result.Ok);
                Finish();
            }
            else
            {
                Toast.MakeText(this, "This name is already being used.", ToastLength.Long).Show();
                return;
            }                                           
        }

        ///
        /// 
        ///     Initalizes & Applies our custom toolbar
        /// 
        ///
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.edit_card_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        ///
        /// 
        ///     Determines which toolbar button was pressed
        /// 
        ///
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // Comparing the title of the toolbar btns to a string to determine which was clicked
            switch (item.TitleFormatted.ToString())
            {
                case "Save":
                    SaveAndFinishedEditing();
                    break;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}