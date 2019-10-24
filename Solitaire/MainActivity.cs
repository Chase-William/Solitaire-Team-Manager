using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Solitaire.Lang;
using System.Collections.Generic;
using Android.Views;
using Android.Content;
using Syncfusion.SfKanban.Android;

namespace Solitaire
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        ListView boardListView;
        List<Board> boards = new List<Board>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            // Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTYwMzA1QDMxMzcyZTMzMmUzME9GL1JjUEZoc09tTDJrdEtEdXgvUkZRQXQrMzBESzVCY2djRmExc0lFOTg9");

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            boards = TestData.boards;

            // Setting our toolbar to our custom created one we included into main_activity.xml
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "Projects";
            SetSupportActionBar(toolbar);            

            boardListView = FindViewById<ListView>(Resource.Id.boardListView);

            //boardListView.ItemClick += ProjectListViewItemClicked;
            boardListView.Adapter = new BoardAdapter(boards);
            boardListView.ItemClick += BoardListViewItemClicked;
        }

        ///
        ///
        ///     Handles ProjectListView item clicks
        /// 
        /// 
        private void BoardListViewItemClicked(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent useSelectedBoard = new Intent(this, typeof(UseBoardActivity));
            useSelectedBoard.PutExtra("Id", e.Id);
            StartActivity(useSelectedBoard);
        }

        ///
        /// 
        ///     Initalizes & Applies our custom toolbar
        /// 
        ///
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.main_menu, menu);
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
                case "New Board":
                    Intent UseDefaultBoard = new Intent(this, typeof(UseBoardActivity));
                    // Passing in -1 will signify to the UseBoardActivity that this instance is a default board                    
                    UseDefaultBoard.PutExtra("Id", -1);
                    StartActivity(UseDefaultBoard);
                    break;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }










        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}