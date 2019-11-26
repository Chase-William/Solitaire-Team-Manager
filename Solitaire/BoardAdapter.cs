﻿using Android.Content;
using Android.Views;
using Android.Widget;
using Solitaire.Lang;
using System.Collections.Generic;
using System.Linq;

namespace Solitaire
{
    public class BoardAdapter : BaseAdapter<Board>
    {
        List<Board> boards;
        MainActivity callerActivity;
        public List<View> listViewChildren = new List<View>();
        public List<string> boardNames = new List<string>();

        public BoardAdapter(List<Board> _boards, MainActivity _callerActivity)
        {
            boards = _boards;
            callerActivity = _callerActivity;
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
                var detailsBoardBtn   = view.FindViewById<ImageButton>(Resource.Id.detailsBoardBtn);                

                // Will launch the details activity of the board when clicked
                detailsBoardBtn.Click += delegate
                {
                    Intent detailsOfBoard = new Intent(callerActivity, typeof(DetailsBoardActivity));
                    detailsOfBoard.PutExtra("BoardId", GetItemId(position));
                    callerActivity.StartActivity(detailsOfBoard);
                };


                view.Tag = new BoardViewHolder() { Name = name, TotalDecks = totalDecks, TotalCards = totalCards, TotalContributors = totalContributors, DetailsBoardBtn = detailsBoardBtn };
            }

            BoardViewHolder holder = (BoardViewHolder)view.Tag;
           
            // My dumb way of managing the views and whether they are highlighted for deletion.
            // If the name of the board is within the list then it is up for deletion
            if (boardNames.Contains(boards[position].Name))
            {
                view.SetBackgroundResource(Resource.Color.deleteColor);
            }
            else
            {
                view.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }
            
            holder.Name.Text       = boards[position].Name;
            holder.TotalDecks.Text = boards[position].Decks.Count.ToString();
            holder.TotalCards.Text = boards[position].Cards.Count.ToString();
            
            // https://www.tutorialsteacher.com/linq/linq-set-operators-distinct
            // Need number of contributors
            // var board = boards.ElementAt(position);            

            holder.TotalContributors.Text = boards.ElementAt(position).QueryBoardDistinctContributorsForInstance().Count.ToString();

            // Attempting to get references to each view for customization in listboardsfrag
            listViewChildren.Add(view);

            return view;
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