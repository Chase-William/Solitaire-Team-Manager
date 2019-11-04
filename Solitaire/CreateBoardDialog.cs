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
            SetContentView(Resource.Layout.create_deck_dialog);

            var createDeckBtn = FindViewById<Button>(Resource.Id.createDeckBtn);
            var cancelDeckBtn = FindViewById<Button>(Resource.Id.cancelDeckBtn);

            // Dismisses dialog and creates a new deck
            createDeckBtn.Click += (e, a) =>
            {
                string name = FindViewById<EditText>(Resource.Id.nameEditTextDialog).Text.Trim();
                long id;

                // Checks to make sure this name doesnt not already exist within the respective board, because we will be using the board's title as the category name
                if (name == "" || name == null) return;

                // If the board doesn't contain any boards then we can skip testing used names
                if (AssetManager.boards.Count > 0)
                {
                    // Checks to make sure the board's name isn't already used
                    if (AssetManager.boards.All(boardName => boardName.Name == name))
                    {
                        return;
                    }
                    // If it is then we tell the user
                    else
                    {
                        Toast.MakeText(callerInstance, "This Name is already used.", ToastLength.Short);
                        return;
                    }
                }

                var newBoard = new Board(name, FindViewById<EditText>(Resource.Id.descriptionTextDialog).Text.Trim());
                id = newBoard.Id;
                AssetManager.boards.Add(newBoard);

                Intent useCreateBoard = new Intent(callerInstance, typeof(UseBoardActivity));
                useCreateBoard.PutExtra("Id", id);
                useCreateBoard.PutExtra("NeedInit", true);

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