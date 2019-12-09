using Android.App;
using Android.OS;
using Android.Widget;
using Android.Views;
using Android.Content;
using Android.Graphics;
using System.Collections.Generic;
using Solitaire.Lang;
using System.Linq;
using System;
using Xamarin.Essentials;

namespace Solitaire
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class ListBoardsFragment : Android.Support.V4.App.Fragment
    {
        private event Action<object, AdapterView.ItemClickEventArgs> ItemClickedForCustomHandler;
        MainActivity callerActivity;
        ListView boardListView;
        AbsoluteLayout boardFragLayout;
        ImageButton deleteBoardBtn;
        bool DeleteBtnState = false;
        // BoardAdapter boardAdapter;

        // Contains references to boards that will be deleted if committed
        // public List<Board> boardsToDelete = new List<Board>();

        public ListBoardsFragment(MainActivity _context) { 
            callerActivity = _context;
            // Default "mode" for clicked boards will be to open them and launch the UseBoardActivity
            ItemClickedForCustomHandler = SelectBoardForUse;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.list_boards_fragment, container, false);

            boardFragLayout = view.FindViewById<AbsoluteLayout>(Resource.Id.boardFragLayout);
            // When the Add New Boards button is pressed it will trigger the parent Activity to create it
            view.FindViewById<ImageButton>(Resource.Id.addNewBoardBtn).Click += delegate 
            {                
                // First we need to de-activate the delete btns if active
                if (DeleteBtnState) DeactivateDeleteBtnUI(boardListView.GetBoardAdapter());
                var createboardDialog = new CreateBoardDialog(callerActivity);                
            };

            deleteBoardBtn = view.FindViewById<ImageButton>(Resource.Id.deleteBoardBtn);

            deleteBoardBtn.Click += OnDeleteBoardButtonClicked; 

            boardListView = view.FindViewById<ListView>(Resource.Id.boardListView);
            
            boardListView.Adapter = new BoardAdapter(AssetManager.boards, callerActivity, BoardAdapterMode.DeleteBoards);
            boardListView.ItemClick += (e, a) =>
            {
                ItemClickedForCustomHandler?.Invoke(e, a);
            };            
            return view;
        }

        /// 
        ///         
        ///     Sets the ItemSelected to delete mode and will generate the buttons for deleting boards
        /// 
        /// 
        private void OnDeleteBoardButtonClicked(object sender, EventArgs e)
        {
            var boardAdapter = boardListView.GetBoardAdapter();

            if (DeleteBtnState) DeactivateDeleteBtnUI(boardAdapter);
            else                ActivateDeleteBtnUI(boardAdapter);
            
        }

        /// 
        /// 
        ///     Will generate views needed for the user to delete boards and set appropriate ItemClick Handler
        /// 
        /// 
        private void ActivateDeleteBtnUI(BoardAdapter _boardAdapter)
        {
            DeleteBtnState = true;

            ItemClickedForCustomHandler = SelectBoardsForDeletion;

            // Will contain the x, y coords of where the deleteBoardBtn is
            int[] vec2 = new int[2];
            // Gets the location of our delete btn and assigns it to our coords array
            deleteBoardBtn.GetLocationOnScreen(vec2);
            
            const int SUB_BTN_WIDTH = 170;
            const int SUB_BTN_HEIGHT = 170;

            // will cancel the deletion of all the selected boards
            var cancelDeleteBtn = new ImageButton(callerActivity)
            {
                TooltipText = "Cancel",
                TranslationX = vec2[0] + deleteBoardBtn.Width / 2f - SUB_BTN_WIDTH / 2f,
                TranslationY = vec2[1] - deleteBoardBtn.Height - 130f,
                LayoutParameters = new ViewGroup.LayoutParams(SUB_BTN_WIDTH, SUB_BTN_HEIGHT),               
            };
            cancelDeleteBtn.SetImageResource(Resource.Drawable.cancel_icon);
            cancelDeleteBtn.SetBackgroundColor(Color.Transparent);
            // https://thoughtbot.com/blog/android-imageview-scaletype-a-visual-guide
            cancelDeleteBtn.SetScaleType(ImageView.ScaleType.FitCenter);
            // Cancels the deletion of whatever boards we clicked. 
            cancelDeleteBtn.Click += delegate
            {
                // Deactivate the deletion "mode"
                DeactivateDeleteBtnUI(_boardAdapter);
            };

            // commit btn that will delete all the selected boards
            var commitDeleteBtn = new ImageButton(callerActivity)
            {
                TooltipText = "Commit",
                TranslationX = vec2[0] + deleteBoardBtn.Width / 2f - SUB_BTN_WIDTH / 2f,
                TranslationY = vec2[1] - deleteBoardBtn.Height - 260f,
                LayoutParameters = new ViewGroup.LayoutParams(SUB_BTN_WIDTH, SUB_BTN_HEIGHT)
            };
            commitDeleteBtn.SetImageResource(Resource.Drawable.commit_icon);
            commitDeleteBtn.SetBackgroundColor(Color.Transparent);
            commitDeleteBtn.SetScaleType(ImageView.ScaleType.FitCenter);
            // Last step to delete the boards from the collection
            commitDeleteBtn.Click += delegate {                

                // Removing all the boards from that list that are inside the boardAdapters list of boards to be deleted
                AssetManager.boards.RemoveAll(_boardAdapter.boardsToDelete.Contains);

                // Telling the adapter the underlying dataset has changed (AssetManager.boards)... Really I had something like this for the Sfkanban stuff in UseBoardActivity
                _boardAdapter.NotifyDataSetChanged();                
                                
                // After the user has deleted the boards, we can deactivate this "mode"
                DeactivateDeleteBtnUI(_boardAdapter);
            };

            // Providing user feedback
            Vibration.Vibrate(AssetManager.VibrateTime);
            deleteBoardBtn.SetImageResource(Resource.Drawable.delete_icon_pressed);            

            // Adding the views to the main view
            boardFragLayout.AddView(cancelDeleteBtn);
            boardFragLayout.AddView(commitDeleteBtn);

            // Setting the mode of the boardadapter to delete mode
            _boardAdapter.BoardAdapterMode = BoardAdapterMode.DeleteBoards;
        }

        /// 
        /// 
        ///     Will remove views for deleting boards from ui and set appropriate ItemClick Handler
        /// 
        ///
        private void DeactivateDeleteBtnUI(BoardAdapter boardAdapter)
        {
            DeleteBtnState = false;
            ItemClickedForCustomHandler = SelectBoardForUse;
            // Clearing the list of references of boards to delete
            boardAdapter.boardsToDelete.Clear();
            ResetBoardViewsColorToDefault();
            boardFragLayout.RemoveViewAt(boardFragLayout.ChildCount - 2);
            boardFragLayout.RemoveViewAt(boardFragLayout.ChildCount - 1);
            deleteBoardBtn.SetImageResource(Resource.Drawable.delete_icon);

            // Setting the mode of the boardadapter to delete mode
            boardAdapter.BoardAdapterMode = BoardAdapterMode.UseBoard;
        }

        /// 
        /// 
        ///     Adds all the selected boards to a temp list that reflects the original list.
        ///         When the user hits the commitDeleteBtn all the boards in this temp collection will be removed from the original collection.
        ///         When the user hits the cancelDeleteBtn the temp list will be freed from memory and the original collection will be unchanged.
        /// 
        /// 
        private void SelectBoardsForDeletion(object sender, AdapterView.ItemClickEventArgs args)
        {
            LinearLayout senderLinearLayout = (LinearLayout)args.View;

            BoardAdapter boardAdapter = boardListView.GetBoardAdapter();

            string boardName = ((BoardViewHolder)boardAdapter.listViewChildren.Single(linearLayout => linearLayout.Equals(senderLinearLayout)).Tag).Name.Text;

            boardAdapter.SelectForDelete(senderLinearLayout, senderLinearLayout.FindViewById<ImageButton>(Resource.Id.accessibilityBtn), boardName);
        }

        ///
        /// 
        ///     When clicked this will toggle
        /// 
        /// 
        private void SelectBoardForUse(object sender, AdapterView.ItemClickEventArgs args)
        {
            Intent useSelectedBoard = new Intent(callerActivity, typeof(UseBoardActivity));
            useSelectedBoard.PutExtra("BoardId", args.Id);
            callerActivity.StartActivity(useSelectedBoard);
        }

        ///
        /// 
        ///     Resets all the Views representing boards to the default color
        /// 
        /// 
        private void ResetBoardViewsColorToDefault()
        {
            foreach (var view in boardListView.GetBoardAdapter().listViewChildren)
            {   
                view.SetBackgroundColor(Color.Transparent);
            }
            // Ye i'll just stay with the dumb foreach for now
            //boardListView.GetBoardAdapter().listViewChildren.Where(view => view.SolidColor == Resources.GetColor(Resource.Color.deleteColor)).ToList().ForEach(view => view.SetBackgroundColor(Color.Transparent));
        }

        // Refreshing our screen when we resume
        public override void OnResume()
        {
            base.OnResume();
            boardListView.Adapter = new BoardAdapter(AssetManager.boards, callerActivity, BoardAdapterMode.UseBoard);
        }
    }
}