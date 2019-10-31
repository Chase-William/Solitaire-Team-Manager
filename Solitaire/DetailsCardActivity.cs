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
    [Activity(Label = "DetailsCard")]
    public class DetailsCardActivity : AppCompatActivity
    {
        KanbanModel card;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.details_card_activity);

            //card =  this.Intent.GetLongExtra("ID", -1);

            // Setting our custom toolbar
            //var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            //toolbar.Title = "";
            //SetSupportActionBar(toolbar);



            FindViewById<TextView>(Resource.Id.cardNameTextView).Text = "asdas";
            FindViewById<TextView>(Resource.Id.cardDescriptionTextView).Text = "asd";
        }

        ///
        /// 
        ///     Initalizes & Applies our custom toolbar
        /// 
        ///
        //public override bool OnCreateOptionsMenu(IMenu menu)
        //{
        //    MenuInflater.Inflate(Resource.Menu.main_menu, menu);
        //    return base.OnCreateOptionsMenu(menu);
        //}

        /////
        ///// 
        /////     Determines which toolbar button was pressed
        ///// 
        /////
        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //    switch (item.TitleFormatted.ToString())
        //    {
        //        case " ":
        //            // do something
        //            break;
        //        default:
        //            break;
        //    }
        //    return base.OnOptionsItemSelected(item);
        //}
    }
}