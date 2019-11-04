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
        private const int EDIT_ACTIVITY_CODE = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.details_card_activity);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "Card Details";
            SetSupportActionBar(toolbar);

            //card =  this.Intent.GetLongExtra("ID", -1);

            // Setting our custom toolbar
            //var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            //toolbar.Title = "";
            //SetSupportActionBar(toolbar);

            FindViewById<TextView>(Resource.Id.cardNameTextView).Text = "asdas";
            FindViewById<TextView>(Resource.Id.cardDescriptionTextView).Text = "asd";
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
                    Intent editCardActivity = new Intent(this, typeof(EditCardActivity));
                    StartActivityForResult(editCardActivity, EDIT_ACTIVITY_CODE);
                    break;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        /// 
        /// 
        ///     Will handle the edit activity being finished
        /// 
        /// 
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (resultCode == Result.Ok && requestCode == EDIT_ACTIVITY_CODE)
            {

            }            
        }
    }
}