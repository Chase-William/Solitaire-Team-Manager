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
using Syncfusion.SfKanban.Android;
using Solitaire.Lang;
using Android.Views.InputMethods;

namespace Solitaire
{
    public enum ManipulatingContributorMode { Adding, Removing, SettingLeader }

    [Activity(Label = "EditCard")]
    public class EditCardActivity : AppCompatActivity
    {        
        public EditText cardNameEditText, cardDescriptionEditText;
        public KanbanModelWrapper clickedKanbanModel;
        // ListView contributorsListView;
        public List<Contributor> contributingContributors = new List<Contributor>();
        public List<Contributor> noncontributingContributors = new List<Contributor>();
        public ImageButton changeLeaderBtn;
        // List<Contributor> contributorsCurrentlyVisible;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.edit_card_activity);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "Edit Card";
            SetSupportActionBar(toolbar);            

            // Finding our kanbanModel inside the item source while using the intent extra we put from the calling activity
            clickedKanbanModel = UseBoardActivity.kanbanModels.Single(kanbanModel => kanbanModel.ID == this.Intent.GetLongExtra("kanbanModelId", -1));
            
            // Creates a dialog of the non-contributing contributors for adding
            FindViewById<Button>(Resource.Id.addContributorBtn).Click += delegate
            {
                new ContributorListDialog(this, ManipulatingContributorMode.Adding);
            };
            // Creates a dialog of the contributing contributors for removing
            FindViewById<Button>(Resource.Id.removeContributorBtn).Click += delegate
            {
                new ContributorListDialog(this, ManipulatingContributorMode.Removing);
            };


            // Since a leader a doesn't need to be assigned it can be null hence:
            if (clickedKanbanModel.Leader != null)
                FindViewById<TextView>(Resource.Id.cardLeaderTextView).Text = $"{clickedKanbanModel.Leader.Name} | {clickedKanbanModel.Leader.Email}";

            // Setting up the pointers to the TextViews
            cardNameEditText = FindViewById<EditText>(Resource.Id.cardNameEditText);
            cardDescriptionEditText = FindViewById<EditText>(Resource.Id.cardDescriptionEditText);
            // Assigning the textviews the current values of the kanbanModel
            cardNameEditText.Text = clickedKanbanModel.Title;
            cardDescriptionEditText.Text = clickedKanbanModel.Description;

            contributingContributors = AssetManager.contributors.Where(contributor =>
            {
                // We are dividing up the entire list into two groups
                if (clickedKanbanModel.Tags != null)
                {
                    if (clickedKanbanModel.Tags.Contains(contributor.Email))
                    {
                        return true;
                    }
                    else
                    {
                        noncontributingContributors.Add(contributor);
                        return false;
                    }
                }
                else
                {
                    noncontributingContributors.Add(contributor);
                    return false;
                }
            }).ToList();

            // this button will put the listview mode into selecting a leader for the card
            changeLeaderBtn = FindViewById<ImageButton>(Resource.Id.changeCardLeaderBtn);
            changeLeaderBtn.Click += delegate
            {
                // First we need to check to make sure the contributing contributors list isn't empty
                if (contributingContributors == null || contributingContributors.Count == 0)
                {
                    RunOnUiThread(() => Toast.MakeText(this, "Before adding a leader add contributors to your card.", ToastLength.Long).Show());  
                    return;
                }

                // Create our dialog for setting the card's leader
                new ContributorListDialog(this, ManipulatingContributorMode.SettingLeader);                
            };
        }

        /// 
        /// 
        ///     Closes the keyboard when anything that is not the keyboard is pressed
        /// 
        /// 
        public override bool OnTouchEvent(MotionEvent e)
        {
            InputMethodManager inputMM = (InputMethodManager)GetSystemService(Context.InputMethodService);
            inputMM.HideSoftInputFromWindow(cardNameEditText.WindowToken, 0);
            return base.OnTouchEvent(e);
        }


        /// 
        /// 
        ///     Saving the data to the KanbanModel (NOT THE BOARD'S CARD) and returning to the details activity
        /// 
        ///
        private void SaveAndFinishedEditing()
        {
            // Array of all the names to check with
            string[] names = UseBoardActivity.kanbanModels.Where(kanban => !kanban.Equals(this.clickedKanbanModel)).Select(kanban => kanban.Title).ToArray();

            string name = cardNameEditText.Text.Trim();
            // Checks to make sure that the name doesn't already exist and isn't a space filled string
            if (name != "" && names.All(usedName => usedName != name))
            {
                clickedKanbanModel.Title = cardNameEditText.Text;
                clickedKanbanModel.Description = cardDescriptionEditText.Text;
                clickedKanbanModel.Tags = contributingContributors.Select(contributor => contributor.Email).ToArray();
                SetResult(Result.Ok);
                Finish();
            }
            else
            {
                ///
                ///     Added from user feedback
                ///
                Toast.MakeText(this, "This name is already being used or is not allowed.", ToastLength.Long).Show();                
                return;
            }                                           
        }

        ///
        /// 
        ///     Initalizes & Applies our custom toolbar
        /// 
        ///
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.edit_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        ///
        /// 
        ///     Determines which toolbar button was pressed
        /// 
        ///
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // Comparing the title of the toolbar btns to a string to determine which was clicked
            switch (item.ItemId)
            {
                case Resource.Id.saveEdit:
                    SaveAndFinishedEditing();
                    break;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}