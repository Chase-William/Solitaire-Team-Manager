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

namespace Solitaire
{
    [Activity(Label = "EditCard")]
    public class EditCardActivity : AppCompatActivity
    {
        event Action<int> ManipulateContributor;
        event Action UpdateListViewForMode;

        EditText cardNameEditText, cardDescriptionEditText;
        KanbanModel clickedKanbanModel;
        ListView contributorsListView;
        List<Lang.Contributor> contributingContributors = new List<Lang.Contributor>();
        List<Lang.Contributor> noncontributingContributors = new List<Lang.Contributor>();
        Button toggleModeBtn;
        TextView cardLeaderTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.edit_card_activity);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "Edit Card";
            SetSupportActionBar(toolbar);

            // Setting a default state
            ManipulateContributor = RemoveContributor;
            // Our default starting activity will be to remove contributors, so we need to set our event for refreshing the UI accordingly
            UpdateListViewForMode = SetAdapterToRemoveMode;

            // Finding our kanbanModel inside the item source while using the intent extra we put from the calling activity
            clickedKanbanModel = UseBoardActivity.thisKanban.ItemsSource.Cast<KanbanModel>().Single(kanbanModel => kanbanModel.ID == this.Intent.GetLongExtra("kanbanModelId", -1));
            toggleModeBtn = FindViewById<Button>(Resource.Id.toggleCardContributorBtn);
            toggleModeBtn.Click += (e ,a) =>
            {
                if (toggleModeBtn.Text[0] == 'R')
                {
                    toggleModeBtn.Text = "Adding";
                    UpdateListViewForMode = SetAdapterToAddMode;
                    ManipulateContributor = AddContributor;
                }
                else
                {
                    toggleModeBtn.Text = "Removing";
                    UpdateListViewForMode = SetAdapterToRemoveMode;
                    ManipulateContributor = RemoveContributor;
                }

                UpdateListViewForMode.Invoke();
            };

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


            cardLeaderTextView = FindViewById<TextView>(Resource.Id.cardLeaderTextView);
            // If the card only has 1 contributor, automatically set them as the leader
            if (contributingContributors.Count != 0)
            {
                cardLeaderTextView.Text = contributingContributors[0].Name;
            }

            contributorsListView = FindViewById<ListView>(Resource.Id.contributorsListView);
            // Gets list of contributors using email as our primary key, also null check
            contributorsListView.Adapter = new ContributorsAdapter(contributingContributors, this);
            contributorsListView.ItemClick += (e, a) =>
            {                
                ManipulateContributor?.Invoke(a.Position);
            };
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
            UpdateListViewForMode?.Invoke();
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
            UpdateListViewForMode?.Invoke();
        }

        /// 
        /// 
        ///     Sets the current adapter to display all the contributers that can be added
        /// 
        /// 
        private void SetAdapterToAddMode()
        {
            contributorsListView.Adapter = new ContributorsAdapter(noncontributingContributors, this);
        }

        /// 
        /// 
        ///     Sets the current adapter to display the currently contributing contributors
        /// 
        /// 
        private void SetAdapterToRemoveMode()
        {
            contributorsListView.Adapter = new ContributorsAdapter(contributingContributors, this);
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