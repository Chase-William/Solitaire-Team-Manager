using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Views.InputMethods;
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
    [Activity(Label = "EditBoardActivity")]
    public class EditBoardActivity : AppCompatActivity
    {
        EditText boardNameEditText, boardDescriptionEditText;
        Board thisBoard;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.edit_board_activity);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "Edit Board";
            SetSupportActionBar(toolbar);

            thisBoard = AssetManager.boards.Single(board => board.Id == this.Intent.GetLongExtra("BoardId", -1));

            FindViewById<ListView>(Resource.Id.allContributorsForBoard).Adapter = new ContributorsAdapter(thisBoard.QueryBoardDistinctContributors(), this);
            FindViewById<TextView>(Resource.Id.totalDecks).Text = thisBoard.Decks.Count.ToString();
            FindViewById<TextView>(Resource.Id.totalCards).Text = thisBoard.Cards.Count.ToString();

            boardNameEditText = FindViewById<EditText>(Resource.Id.boardNameEditText);
            boardDescriptionEditText = FindViewById<EditText>(Resource.Id.boardDescriptionEditText);

            boardNameEditText.Text = thisBoard.Name;
            boardDescriptionEditText.Text = thisBoard.Description;
        }

        /// 
        /// 
        ///     Closes the keyboard when anything that is not the keyboard is pressed
        /// 
        /// 
        public override bool OnTouchEvent(MotionEvent e)
        {
            InputMethodManager inputMM = (InputMethodManager)GetSystemService(Context.InputMethodService);
            inputMM.HideSoftInputFromWindow(boardNameEditText.WindowToken, 0);
            return base.OnTouchEvent(e);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.edit_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // Comparing the title of the toolbar btns to a string to determine which was clicked
            switch (item.ItemId)
            {
                case Resource.Id.saveEdit:
                    string name = boardNameEditText.Text.Trim();
                    // Checks to make sure this name doesnt not already exist within the respective board, because we will be using the board's title as the category name
                    if (name == "" || name == null)
                    {
                        Toast.MakeText(this, "Name field cannot be empty.", ToastLength.Short).Show();
                        break;
                    }                    

                    // If the board list doesn't contain any boards then we can skip testing used names
                    if (AssetManager.boards.Count > 0 && name != thisBoard.Name)
                    {
                        // If the board's name is already used then we inform the user
                        // We also DO NOT create an instance of the board 
                        if (AssetManager.boards.Any(boardName => boardName.Name == name))                           
                        {                          
                            Toast.MakeText(this, $"{name} name is already being used.", ToastLength.Short).Show();
                            break;
                        }
                    }

                    thisBoard.Name = boardNameEditText.Text;
                    thisBoard.Description = boardDescriptionEditText.Text;
                    Finish();
                    break;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}