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











// TODO: When scrolling through the listview of boards and when recycling them the colored ones will reapear on the board that wasn't selected
//          Need to make it so when the dataset changes the damn color is set to default transparent












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
                if (DeleteBtnState) DeactivateDeleteBtnInterface(boardListView.GetBoardAdapter());
                var createboardDialog = new CreateBoardDialog(callerActivity);                
            };

            deleteBoardBtn = view.FindViewById<ImageButton>(Resource.Id.deleteBoardBtn);

            deleteBoardBtn.Click += OnDeleteBoardButtonClicked; 

            boardListView = view.FindViewById<ListView>(Resource.Id.boardListView);
            
            boardListView.Adapter = new BoardAdapter(AssetManager.boards, callerActivity);
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
            if (DeleteBtnState) DeactivateDeleteBtnInterface(boardListView.GetBoardAdapter());
            else                ActivateDeleteBtnInterface();
            
        }

        /// 
        /// 
        ///     Will generate views needed for the user to delete boards and set appropriate ItemClick Handler
        /// 
        /// 
        private void ActivateDeleteBtnInterface()
        {
            DeleteBtnState = true;

            ItemClickedForCustomHandler = SelectBoardsForDeletion;

            // Will contain the x, y coords of where the deleteBoardBtn is
            int[] coords = new int[2];
            // Gets the location and assigns it to our coords array
            deleteBoardBtn.GetLocationOnScreen(coords);

            int subBtnWidth = 170;
            int subBtnHeight = 170;

            // will cancel the deletion of all the selected boards
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
            // Cancels the deletion of whatever boards we clicked. 
            cancelDeleteBtn.Click += delegate
            {
                // Deactivate the deletion "mode"
                DeactivateDeleteBtnInterface(boardListView.GetBoardAdapter());
            };

            // commit btn that will delete all the selected boards
            var commitDeleteBtn = new ImageButton(callerActivity)
            {
                TooltipText = "Commit",
                TranslationX = coords[0] + deleteBoardBtn.Width / 2f - subBtnWidth / 2f,
                TranslationY = coords[1] - deleteBoardBtn.Height - 260f,
                LayoutParameters = new ViewGroup.LayoutParams(subBtnWidth, subBtnHeight)
            };
            commitDeleteBtn.SetImageResource(Resource.Drawable.commit_icon);
            commitDeleteBtn.SetBackgroundColor(Color.Transparent);
            commitDeleteBtn.SetScaleType(ImageView.ScaleType.FitCenter);
            // Last step to delete the boards from the collection
            commitDeleteBtn.Click += delegate {

                // Remove all boards that are inside the boardToDelete from the AssetManager collection (main collection)
                // Getting a reference to the boardAdapter
                var boardAdapter = boardListView.GetBoardAdapter();               

                // Removing all the boards from that list that are inside the boardAdapters list of boards to be deleted
                AssetManager.boards.RemoveAll(boardAdapter.boardToDelete.Contains);

                // Telling the adapter the underlying dataset has changed (AssetManager.boards)... Really I had something like this for the Sfkanban stuff in UseBoardActivity
                boardAdapter.NotifyDataSetChanged();                
                                
                // After the user has deleted the boards, we can deactivate this "mode"
                DeactivateDeleteBtnInterface(boardAdapter);
            };

            // Adding the views to the main view
            boardFragLayout.AddView(cancelDeleteBtn);
            boardFragLayout.AddView(commitDeleteBtn);
        }

        /// 
        /// 
        ///     Will remove views for deleting boards from ui and set appropriate ItemClick Handler
        /// 
        ///
        private void DeactivateDeleteBtnInterface(BoardAdapter boardAdapter)
        {
            DeleteBtnState = false;
            ItemClickedForCustomHandler = SelectBoardForUse;
            // Clearing the list of references of boards to delete
            boardAdapter.boardToDelete.Clear();
            ResetBoardViewsColorToDefault();
            boardFragLayout.RemoveViewAt(boardFragLayout.ChildCount - 2);
            boardFragLayout.RemoveViewAt(boardFragLayout.ChildCount - 1);
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
            // args.View, ((View)sender), args.Parent... all == the listview

            // Getting our boards adapter using my own 
            var boardAdapter = boardListView.GetBoardAdapter();

            // Getting a reference to the view (LinearLayout) then using that ref and accessing its Tag (BoardViewHolder) to get the name of the board
            // Once we obtain the board's name we can lookup that board instance and add it to the list of boards to be deleted
            View selectedView = boardAdapter.listViewChildren.Single(view => view.Equals(args.View));
            Board selectedBoard = AssetManager.boards.Single(board => board.Name == ((BoardViewHolder)selectedView.Tag).Name.Text);

            // If the user hasn't selected this board before add it and to the list for deletion
            if (!boardAdapter.boardToDelete.Contains(selectedBoard))
            {
                selectedView.SetBackgroundResource(Resource.Color.deleteColor);    
                boardAdapter.boardToDelete.Add(selectedBoard);
            }
            // If the user has selected this board and now clicks it again, remove it from the list
            else
            {
                selectedView.SetBackgroundColor(Color.Transparent);
                boardAdapter.boardToDelete.Remove(selectedBoard);
            }
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
            boardListView.Adapter = new BoardAdapter(AssetManager.boards, callerActivity);
        }
    }
}