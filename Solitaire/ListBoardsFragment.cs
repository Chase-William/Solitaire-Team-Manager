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
using System.Net.Sockets;
using Android.Support.V4.App;

namespace Solitaire
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class ListBoardsFragment : Android.Support.V4.App.Fragment
    {
        MainActivity parentActivityPtr;
        ListView boardListView;

        public ListBoardsFragment(MainActivity _context) { parentActivityPtr = _context; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.list_boards_fragment, container, false);

            view.FindViewById<Button>(Resource.Id.addNewBoardBtn).Click += delegate 
            {
                parentActivityPtr.GenericActionRequest(new System.Action(() =>
                {
                    var createboardDialog = new CreateBoardDialog(parentActivityPtr);
                }));
            };

            boardListView = view.FindViewById<ListView>(Resource.Id.boardListView);
            boardListView.Adapter = new BoardAdapter(AssetManager.boards);
            boardListView.ItemClick += (e, a) =>
            {
                parentActivityPtr.GenericActionRequest(new System.Action(() =>
                {
                    Intent useSelectedBoard = new Intent(parentActivityPtr, typeof(UseBoardActivity));
                    useSelectedBoard.PutExtra("Id", a.Id);
                    parentActivityPtr.StartActivity(useSelectedBoard);
                }));
            };            
            return view;
        }

        //protected override void OnCreate(Bundle savedInstanceState)
        //{            
        //    // Register Syncfusion license
        //    Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTYwMzA1QDMxMzcyZTMzMmUzME9GL1JjUEZoc09tTDJrdEtEdXgvUkZRQXQrMzBESzVCY2djRmExc0lFOTg9");

        //    base.OnCreate(savedInstanceState);
        //    Xamarin.Essentials.Platform.Init(this, savedInstanceState);
        //    SetContentView(Resource.Layout.list_boards_activity);


        //    // Should be called when splash is called in future version
        //    AssetManager.ReadAllBoardsFromFile();


        //    // Setting our toolbar to our custom created one we included into main_activity.xml
        //    //var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
        //    //toolbar.Title = "Projects";
        //    //base.SetSupportActionBar(toolbar);            

        //    boardListView = FindViewById<ListView>(Resource.Id.boardListView);

        //    // Checking to make sure we actually have data that needs to be displayed
        //    if (AssetManager.boards.Count > 0)
        //    {
        //        boardListView.Adapter = new BoardAdapter(AssetManager.boards);
        //        boardListView.ItemClick += BoardListViewItemClicked;
        //    }            


        //    // Checking if connection to server is available,  providing feedback to user about connection status
        //    //if (ClientManager.TryServerConnection())
        //    //    Toast.MakeText(this, "Connection Established", ToastLength.Long).Show();
        //    //else
        //    //    Toast.MakeText(this, "Connection Failure", ToastLength.Long).Show();
        //}

        ///
        ///
        ///     Handles ProjectListView item clicks
        /// 
        /// 
        //private void BoardListViewItemClicked(object sender, AdapterView.ItemClickEventArgs e)
        //{            
        //    Intent useSelectedBoard = new Intent(this, typeof(UseBoardActivity));
        //    useSelectedBoard.PutExtra("Id", e.Id);            
        //    StartActivity(useSelectedBoard);
        //}

        ///
        /// 
        ///     Initalizes & Applies our custom toolbar
        /// 
        ///
        //public override bool OnCreateOptionsMenu(IMenu menu)
        //{
        //    MenuInflater.Inflate(Resource.Menu.list_boards_menu, menu);
        //    return base.OnCreateOptionsMenu(menu);
        //}

        ///
        /// 
        ///     Determines which toolbar button was pressed
        /// 
        ///        



        // Refreshing our screen when we resume
        public override void OnResume()
        {
            base.OnResume();
            boardListView.Adapter = new BoardAdapter(AssetManager.boards);
        }
    }
}