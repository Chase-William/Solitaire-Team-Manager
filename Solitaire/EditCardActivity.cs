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
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.edit_card_activity);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "Edit Card";
            SetSupportActionBar(toolbar);
                          

            // Need to figure out how to convert this IEnumerable... even IQueryable didn't work...
            
            cardNameEditText = FindViewById<EditText>(Resource.Id.cardNameEditText);
            cardDescriptionEditText = FindViewById<EditText>(Resource.Id.cardDescriptionEditText);

            cardNameEditText.Text = this.Intent.GetStringExtra("Name");
            cardDescriptionEditText.Text = this.Intent.GetStringExtra("Description");
        }

        private void SaveAndFinishedEditing()
        {
            Intent returnData = new Intent();
            returnData.PutExtra("Name", cardNameEditText.Text);
            returnData.PutExtra("Description", cardDescriptionEditText.Text);
            SetResult(Result.Ok, returnData);
            Finish();
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