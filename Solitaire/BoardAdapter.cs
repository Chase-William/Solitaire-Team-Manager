using Android.Content;
using Android.Views;
using Android.Widget;
using Solitaire.Lang;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Solitaire
{
    /// 
    ///     Will provide extra instructions to adapter to modify its behavior
    /// 
    public enum BoardAdapterMode { DeleteBoards, UseBoard }

    public class BoardAdapter : BaseAdapter<Board>
    {
        List<Board> boards;
        MainActivity callerActivity;
        public List<View> listViewChildren = new List<View>();
        public List<Board> boardsToDelete = new List<Board>();
        private BoardAdapterMode boardAdapterMode;
        private EventHandler useBoardHandlerHandle;
        

        public BoardAdapterMode BoardAdapterMode
        {
            get { return boardAdapterMode; }
            set { 
                boardAdapterMode = value;

                if (value == BoardAdapterMode.UseBoard)
                {
                    listViewChildren?.ForEach((view) => {
                        var accessibilityBtn = view.FindViewById<ImageButton>(Resource.Id.accessibilityBtn);
                        accessibilityBtn.SetImageResource(Resource.Drawable.details_icon);
                        accessibilityBtn.Click += useBoardHandlerHandle;
                        accessibilityBtn.Tag = true;
                    });
                }
                else if (value == BoardAdapterMode.DeleteBoards)
                {
                    listViewChildren?.ForEach((view) => {
                        var accessibilityBtn = view.FindViewById<ImageButton>(Resource.Id.accessibilityBtn);
                        accessibilityBtn.SetImageResource(Resource.Drawable.select_for_remove);
                        accessibilityBtn.Click -= useBoardHandlerHandle;
                        accessibilityBtn.Tag = true;
                    });
                }                    
            }
        }

        public BoardAdapter(List<Board> _boards, MainActivity _callerActivity, BoardAdapterMode _boardAdapterMode)
        {
            boards = _boards;
            callerActivity = _callerActivity;
            BoardAdapterMode = _boardAdapterMode;
        }

        public override Board this[int position] { get { return boards[position]; } }
        public override int Count { get { return boards.Count; } }
        public override long GetItemId(int position) { return boards[position].Id; }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;         

            if (view == null)
            {                
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.board_row, parent, false);

                var name              = view.FindViewById<TextView>(Resource.Id.boardName);
                var totalDecks        = view.FindViewById<TextView>(Resource.Id.totalDecks);
                var totalCards        = view.FindViewById<TextView>(Resource.Id.totalCards);
                var totalContributors = view.FindViewById<TextView>(Resource.Id.totalContributors);
                var accessibilityBtn  = view.FindViewById<ImageButton>(Resource.Id.accessibilityBtn);                

                ///
                ///     If our handle is null assign a value (A method that will actually be doing the handling)
                ///
                useBoardHandlerHandle = useBoardHandlerHandle ?? ((object sender, EventArgs e) => {
                    Intent detailsOfBoard = new Intent(callerActivity, typeof(DetailsBoardActivity));
                    detailsOfBoard.PutExtra("BoardId", GetItemId(position));
                    callerActivity.StartActivity(detailsOfBoard);
                });

                view.Tag = new BoardViewHolder() { Name = name, TotalDecks = totalDecks, TotalCards = totalCards, TotalContributors = totalContributors, DetailsBoardBtn = accessibilityBtn };                
            }

            BoardViewHolder holder = (BoardViewHolder)view.Tag;
           

            switch (BoardAdapterMode)
            {                
                case BoardAdapterMode.UseBoard:
                    UIUpdatesForUsingBoards(view, boards[position]);
                    break;
                case BoardAdapterMode.DeleteBoards:
                    UIUpdatesForDeletingBoards(view, boards[position]);
                    break;
            }
            
            holder.Name.Text       = boards[position].Name;
            holder.TotalDecks.Text = boards[position].Decks.Count.ToString();
            holder.TotalCards.Text = boards[position].Cards.Count.ToString();                                

            holder.TotalContributors.Text = boards.ElementAt(position).QueryBoardDistinctContributors().Count.ToString();

            // Attempting to get references to each view for customization in listboardsfrag
            if (!listViewChildren.Contains(view))
                listViewChildren.Add(view);

            return view;
        }

        /// 
        ///    Updates the views when deleting them
        /// 
        private void UIUpdatesForDeletingBoards(View _view, Board _board)
        {
            // If the board is within the list then it is up for deletion
            if (boardsToDelete.Contains(_board))
            {
                _view.SetBackgroundResource(Resource.Color.deleteColor);                
            }
            else
            {
                _view.SetBackgroundColor(Android.Graphics.Color.Transparent);                
            }

            var imageBtn = _view.FindViewById<ImageButton>(Resource.Id.accessibilityBtn);
            // The view's accessibilityBtn hasn't been set to the correct image yet.. do so
            if (!(bool)imageBtn.Tag)
            {
                imageBtn.SetImageResource(Resource.Drawable.select_for_remove);
                imageBtn.Tag = true;
            }
        } 

        /// 
        ///     Updates the views for useing them
        /// 
        private void UIUpdatesForUsingBoards(View _view, Board _board)
        {
            var imageBtn = _view.FindViewById<ImageButton>(Resource.Id.accessibilityBtn);
            if (!(bool)imageBtn.Tag)
            {
                _view.FindViewById<ImageButton>(Resource.Id.accessibilityBtn).SetImageResource(Resource.Drawable.details_icon);
                _view.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }            
        }
    }
    
    public class BoardViewHolder : Java.Lang.Object
    {
        public TextView Name { get; set; }
        public TextView TotalDecks { get; set; }
        public TextView TotalCards { get; set; }
        public TextView TotalContributors { get; set; }
        public ImageButton DetailsBoardBtn { get; set; }
    }
}