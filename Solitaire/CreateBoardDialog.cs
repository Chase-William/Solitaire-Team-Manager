using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Solitaire.Lang;

namespace Solitaire
{
    public class CreateBoardDialog : Dialog
    {
        public CreateBoardDialog(MainActivity _context) : base(_context) { callerInstance = _context; this.Show(); }

        MainActivity callerInstance;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.create_board_dialog);

            var dialogLayout = FindViewById<LinearLayout>(Resource.Id.inputDataLayout);
            var createDeckBtn = FindViewById<Button>(Resource.Id.createBoardBtn);
            var cancelDeckBtn = FindViewById<Button>(Resource.Id.cancelBoardBtn);


            // Dismisses dialog and creates a new deck
            createDeckBtn.Click += (e, a) =>
            {
                var nameEditText = FindViewById<EditText>(Resource.Id.nameEditTextDialog);
                string name = nameEditText.Text.Trim();
                long boardId;

                // Checks to make sure this name doesnt not already exist within the respective board, because we will be using the board's title as the category name
                if (name == "" || name == null)
                {
                    Toast.MakeText(callerInstance, "Name field cannot be empty.", ToastLength.Short).Show();
                    return;
                }

                // If the board doesn't contain any boards then we can skip testing used names
                if (AssetManager.boards.Count > 0)
                {
                    // If the board's name is already used then we inform the user
                    // We also DO NOT create an instance of the board 
                    if (AssetManager.boards.Any(boardName => boardName.Name == name))
                    {
                        Toast.MakeText(callerInstance, $"{name} name is already being used.", ToastLength.Short).Show();
                        return;
                    }
                }

                var newBoard = new Board(name, FindViewById<EditText>(Resource.Id.descriptionTextDialog).Text.Trim());
                boardId = newBoard.Id;
                AssetManager.boards.Add(newBoard);

                Intent useCreateBoard = new Intent(callerInstance, typeof(UseBoardActivity));
                useCreateBoard.PutExtra("BoardId", boardId);
                useCreateBoard.PutExtra("IsNew", true);

                Dismiss();
                callerInstance.StartActivity(useCreateBoard);
            };

            // Dismisses the dialog without creating a new deck
            cancelDeckBtn.Click += (e, a) =>
            {
                Dismiss();
            };
        }
    }
}