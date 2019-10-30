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
        public CreateBoardDialog(Context _context) : base(_context) { callerInstance = (ListBoardsMainActivity)_context; this.Show(); }

        ListBoardsMainActivity callerInstance;

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
                if (name != "" && TestData.boards.All(boardName => boardName.Name != name))
                {
                    var newBoard = new Board(name, FindViewById<EditText>(Resource.Id.descriptionTextDialog).Text.Trim());
                    id = newBoard.Id;
                    TestData.boards.Add(newBoard);
                }                    
                else
                {
                    Toast.MakeText(callerInstance, "This Name is already used.", ToastLength.Long);
                    return;
                }
                
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