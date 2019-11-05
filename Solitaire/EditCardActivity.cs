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
        KanbanModel kanbanModelptr;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.edit_card_activity);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "Edit Card";
            SetSupportActionBar(toolbar);

            // Finding our kanbanModel inside the item source while using the intent extra we put from the calling activity
            kanbanModelptr = UseBoardActivity.thisKanban.ItemsSource.Cast<KanbanModel>().Single(kanbanModel => kanbanModel.ID == this.Intent.GetLongExtra("kanbanModelId", -1));    




            // TODO: Might need to check name for null and other unacceptable values




            // Setting up the pointers to the TextViews
            cardNameEditText = FindViewById<EditText>(Resource.Id.cardNameEditText);
            cardDescriptionEditText = FindViewById<EditText>(Resource.Id.cardDescriptionEditText);
            // Assigning the textviews the current values of the kanbanModel
            cardNameEditText.Text = kanbanModelptr.Title;
            cardDescriptionEditText.Text = kanbanModelptr.Description;            
        }

        /// 
        /// 
        ///     Saving the data to the KanbanModel (NOT THE BOARD'S CARD) and returning to the details activity
        /// 
        ///
        private void SaveAndFinishedEditing()
        {
            List<string> names = new List<string>();
            foreach (KanbanModel item in UseBoardActivity.thisKanban.ItemsSource)
            {
                names.Add(item.Title);
            }
            string name = cardNameEditText.Text.Trim();
            // Checks to make sure that the name doesn't already exist and isn't a space filled string
            if (name != "" && names.All(usedName => usedName != name))
            {
                kanbanModelptr.Title = cardNameEditText.Text;
                kanbanModelptr.Description = cardDescriptionEditText.Text;
                SetResult(Result.Ok);
                Finish();
            }
            else
            {
                Toast.MakeText(this, "This Name is already used.", ToastLength.Long);
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