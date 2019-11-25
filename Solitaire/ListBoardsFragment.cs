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
using System.Drawing;
using Android.Graphics.Drawables;
using System;

namespace Solitaire
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class ListBoardsFragment : Android.Support.V4.App.Fragment
    {
        private event Action ToggleDeleteBoardInterface;
        MainActivity callerActivity;
        ListView boardListView;
        AbsoluteLayout boardFragLayout;
        ImageButton deleteBoardBtn;
        bool DeleteBtnState = false;

        public ListBoardsFragment(MainActivity _context) { callerActivity = _context; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.list_boards_fragment, container, false);

            boardFragLayout = view.FindViewById<AbsoluteLayout>(Resource.Id.boardFragLayout);
            // When the Add New Boards button is pressed it will trigger the parent Activity to create it
            view.FindViewById<ImageButton>(Resource.Id.addNewBoardBtn).Click += delegate 
            {
                // First we need to de-activate the delete btns if active
                ToggleDeleteBoardInterface?.Invoke();
                callerActivity.GenericActionRequest(new Action(() =>
                {
                    var createboardDialog = new CreateBoardDialog(callerActivity);
                }));
            };

            deleteBoardBtn = view.FindViewById<ImageButton>(Resource.Id.deleteBoardBtn);

            ToggleDeleteBoardInterface = ActivateDeleteBtnInterface;
            deleteBoardBtn.Click += OnDeleteBoardButtonClicked; 

            boardListView = view.FindViewById<ListView>(Resource.Id.boardListView);
            boardListView.Adapter = new BoardAdapter(AssetManager.boards, callerActivity);
            boardListView.ItemClick += (e, a) =>
            {
                callerActivity.GenericActionRequest(new System.Action(() =>
                {
                    Intent useSelectedBoard = new Intent(callerActivity, typeof(UseBoardActivity));
                    useSelectedBoard.PutExtra("BoardId", a.Id);
                    callerActivity.StartActivity(useSelectedBoard);
                }));
            };            
            return view;
        }

        /// 
        ///         
        ///     Sets the ItemSelected to delete mode and will generate the buttons for deleting boards
        /// 
        /// 
        private void OnDeleteBoardButtonClicked(object sender, System.EventArgs e)
        {
            if (DeleteBtnState) ToggleDeleteBoardInterface = DeactivateDeleteBtnInterface;
            else                ToggleDeleteBoardInterface = ActivateDeleteBtnInterface;
            ToggleDeleteBoardInterface?.Invoke();
        }

        /// 
        /// 
        ///     Will generate views needed for the user to delete boards and set appropriate ItemClick Handler
        /// 
        /// 
        private void ActivateDeleteBtnInterface()
        {
            DeleteBtnState = true;

            // Will contain the x, y coords of where the deleteBoardBtn is
            int[] coords = new int[2];
            deleteBoardBtn.GetLocationOnScreen(coords);

            int subBtnWidth = 170;
            int subBtnHeight = 170;

            var cancelDeleteBtn = new ImageButton(callerActivity)
            {
                TooltipText = "Cancel",
                TranslationX = coords[0] + deleteBoardBtn.Width / 2f - subBtnWidth / 2f,
                TranslationY = coords[1] - deleteBoardBtn.Height - 130f,
                LayoutParameters = new ViewGroup.LayoutParams(subBtnWidth, subBtnHeight),               
            };
            cancelDeleteBtn.SetImageResource(Resource.Drawable.cancel_icon);
            cancelDeleteBtn.SetBackgroundColor(Android.Graphics.Color.Transparent);
            // https://thoughtbot.com/blog/android-imageview-scaletype-a-visual-guide
            cancelDeleteBtn.SetScaleType(ImageView.ScaleType.FitCenter);

            var commitDeleteBtn = new ImageButton(callerActivity)
            {
                TooltipText = "Commit",
                TranslationX = coords[0] + deleteBoardBtn.Width / 2f - subBtnWidth / 2f,
                TranslationY = coords[1] - deleteBoardBtn.Height - 260f,
                LayoutParameters = new ViewGroup.LayoutParams(subBtnWidth, subBtnHeight)
            };
            commitDeleteBtn.SetImageResource(Resource.Drawable.commit_icon);
            commitDeleteBtn.SetBackgroundColor(Android.Graphics.Color.Transparent);
            // https://thoughtbot.com/blog/android-imageview-scaletype-a-visual-guide
            commitDeleteBtn.SetScaleType(ImageView.ScaleType.FitCenter);

            boardFragLayout.AddView(cancelDeleteBtn);
            boardFragLayout.AddView(commitDeleteBtn);
        }

        /// 
        /// 
        ///     Will remove views for deleting boards from ui and set appropriate ItemClick Handler
        /// 
        ///
        private void DeactivateDeleteBtnInterface()
        {
            DeleteBtnState = false;
            boardFragLayout.RemoveViewAt(boardFragLayout.ChildCount - 2);
            boardFragLayout.RemoveViewAt(boardFragLayout.ChildCount - 1);
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
            boardListView.Adapter = new BoardAdapter(AssetManager.boards, callerActivity);
        }
    }
}