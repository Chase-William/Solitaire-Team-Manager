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

using Solitaire.Lang;

namespace Solitaire
{
    [Activity(Label = "DetailsBoardActivity")]
    public class DetailsBoardActivity : AppCompatActivity
    {
        TextView boardNameTextView, boardDescriptionTextView;
        Board thisBoard;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.details_board_activity);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "Board Details";
            SetSupportActionBar(toolbar);

            thisBoard = AssetManager.boards.Single(board => board.Id == this.Intent.GetLongExtra("BoardId", -1));

            boardNameTextView = FindViewById<TextView>(Resource.Id.boardNameTextView);
            boardDescriptionTextView = FindViewById<TextView>(Resource.Id.boardDescriptionTextView);
            boardNameTextView.Text = thisBoard.Name;
            boardDescriptionTextView.Text = thisBoard.Description;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.details_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // Comparing the title of the toolbar btns to a string to determine which was clicked
            switch (item.ItemId)
            {
                case Resource.Id.edit:
                    Intent editBoardActivity = new Intent(this, typeof(EditBoardActivity));
                    editBoardActivity.PutExtra("BoardId", thisBoard.Id);
                    StartActivity(editBoardActivity);
                    break;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }


        protected override void OnResume()
        {
            base.OnResume();
            FindViewById<TextView>(Resource.Id.boardNameTextView).Text = thisBoard.Name;
            FindViewById<TextView>(Resource.Id.boardDescriptionTextView).Text = thisBoard.Description;
        }
    }
}