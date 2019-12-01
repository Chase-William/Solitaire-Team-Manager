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
    [Activity(Label = "EditCard")]
    public class EditCardActivity : AppCompatActivity
    {        
        event Action<int> ManipulateContributor;
        Action<int> LastContributorManipulatingMethodCalled;

        enum ManiplutingContributorMode { Adding, Removing }

        EditText cardNameEditText, cardDescriptionEditText;
        KanbanModelWrapper clickedKanbanModel;
        ListView contributorsListView;
        List<Contributor> contributingContributors = new List<Contributor>();
        List<Contributor> noncontributingContributors = new List<Contributor>();
        ImageButton changeLeaderBtn;
        // Detemines if the changeLeaderBtn is pressed
        bool changeLeaderFlag = false;
        List<Contributor> contributorsCurrentlyVisible;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.edit_card_activity);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "Edit Card";
            SetSupportActionBar(toolbar);            

            // Finding our kanbanModel inside the item source while using the intent extra we put from the calling activity
            clickedKanbanModel = UseBoardActivity.kanbanModels.Single(kanbanModel => kanbanModel.ID == this.Intent.GetLongExtra("kanbanModelId", -1));
            
            // Getting references to the "Add" and the "Remove" btns for controlling contributors
            FindViewById<Button>(Resource.Id.addContributorBtn).Click += delegate
            {
                ManipulateContributor = AddContributor;
                SetUIToMode(ManiplutingContributorMode.Adding);
            };
            FindViewById<Button>(Resource.Id.removeContributorBtn).Click += delegate
            {
                ManipulateContributor = RemoveContributor;
                SetUIToMode(ManiplutingContributorMode.Removing);
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
                changeLeaderFlag = !changeLeaderFlag;

                if (changeLeaderFlag) changeLeaderBtn.SetImageResource(Resource.Drawable.change_leader_icon_pressed);
                else changeLeaderBtn.SetImageResource(Resource.Drawable.change_leader_icon);


                // TODO when we press the change the contributor btn and then change like add or removing list it wipes the ManipulateContributor event


                // Getting a reference to what the meth
                LastContributorManipulatingMethodCalled = ManipulateContributor;

                // Replaces the eventhandler to now add a leader contributor
                ManipulateContributor = (_pos) =>
                {
                    if (contributorsCurrentlyVisible != null)
                    {
                        FindViewById<TextView>(Resource.Id.cardLeaderTextView).Text = $"{contributorsCurrentlyVisible[_pos].Name} | {contributorsCurrentlyVisible[_pos].Email}";
                        clickedKanbanModel.Leader = contributorsCurrentlyVisible[_pos];
                    }                    
                    changeLeaderBtn.SetImageResource(Resource.Drawable.change_leader_icon);
                    
                    
                };
            };



            contributorsListView = FindViewById<ListView>(Resource.Id.contributorsListView);
            // Gets list of contributors using email as our primary key, also null check
            contributorsListView.Adapter = new ContributorsAdapter(contributingContributors, this);
            contributorsListView.ItemClick += (e, a) =>
            {                
                ManipulateContributor?.Invoke(a.Position);
            };


            // Setting a default state
            ManipulateContributor = AddContributor;
            SetUIToMode(ManiplutingContributorMode.Adding);
        }

        /// 
        /// 
        ///     Updates the program based off the status of the UI     
        ///
        ///
        private void SetUIToMode(ManiplutingContributorMode _mode)
        {
            switch (_mode)
            {                
                case ManiplutingContributorMode.Adding:                    
                    SetAdapterToAddMode();                    
                    break;
                case ManiplutingContributorMode.Removing:
                    SetAdapterToRemoveMode();
                    break;             
            }
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
        ///     Handles the setup required for removing contributors
        /// 
        ///
        private void RemoveContributor(int _pos)
        {
            noncontributingContributors.Add(contributingContributors.ElementAt(_pos));
            contributingContributors.RemoveAt(_pos);
            contributorsListView.GetContributorAdapter().NotifyDataSetChanged();
        }

        /// 
        /// 
        ///     Handles the setup required for add contributors
        /// 
        ///
        private void AddContributor(int _pos)
        {
            contributingContributors.Add(noncontributingContributors.ElementAt(_pos));
            noncontributingContributors.RemoveAt(_pos);
            contributorsListView.GetContributorAdapter().NotifyDataSetChanged();
            // SetUIToMode(ManiplutingContributorMode.Adding);
        }

        /// 
        /// 
        ///     Sets the current adapter to display all the contributers that can be added
        /// 
        /// 
        private void SetAdapterToAddMode()
        {
            contributorsListView.Adapter = new ContributorsAdapter(noncontributingContributors, this);
            contributorsCurrentlyVisible = noncontributingContributors;
        }

        /// 
        /// 
        ///     Sets the current adapter to display the currently contributing contributors
        /// 
        /// 
        private void SetAdapterToRemoveMode()
        {
            contributorsListView.Adapter = new ContributorsAdapter(contributingContributors, this);
            contributorsCurrentlyVisible = contributingContributors;
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
                Toast.MakeText(this, "This name is already being used.", ToastLength.Long).Show();                
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