using Android.Views;
using Android.Widget;
using Solitaire.Lang;
using System.Collections.Generic;

namespace Solitaire
{
    public class BoardAdapter : BaseAdapter<Board>
    {
        List<Board> boards = new List<Board>();

        public BoardAdapter(List<Board> _boards)
        {
            boards = _boards;
        }

        public override Board this[int position] { get { return boards[position]; } }

        public override int Count { get { return boards.Count; } }
        public override long GetItemId(int position) { return boards[position].Id; }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            if (view == null)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.board_row, parent, false);

                var name = view.FindViewById<TextView>(Resource.Id.boardName);
                var totalDecks =  view.FindViewById<TextView>(Resource.Id.totalDecks);
                var totalCards = view.FindViewById<TextView>(Resource.Id.totalCards);

                view.Tag = new BoardViewHolder() { Name = name, TotalDecks = totalDecks, TotalCards = totalCards };
            }

            BoardViewHolder holder = (BoardViewHolder)view.Tag;
            holder.Name.Text = boards[position].Name;
            holder.TotalDecks.Text = boards[position].Decks.Count.ToString();
            holder.TotalCards.Text = boards[position].Cards.Count.ToString();

            return view;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BoardViewHolder : Java.Lang.Object
    {
        public TextView Name { get; set; }
        public TextView TotalDecks { get; set; }
        public TextView TotalCards { get; set; }
    }
}