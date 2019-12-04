using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Graphics;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Solitaire.Lang;

namespace Solitaire
{
    /// 
    /// 
    ///     Will present the user with a list of contributors inside a dialog to interact with.
    ///         How the user interacts with the items depends on the base parameter passed in from the base class
    /// 
    ///
    public class ContributorListDialog : Dialog
    {
        // Event used for getting the staging variables
        event Action<int, View> SelectContributorEvent;
        // Event for committing / finalizing staging variables into card -- like github -gitbash... commit == git push basically
        event Action CommitChanges;
        EditCardActivity callerActivity;
        ListView contributorListView;
        ManipulatingContributorMode mode;
        TextView cardLeaderTextView;

        // Temp storage variables awaiting cancel, or commit clicks
        // contributor to be set as leader
        // Contributor cardLeader;
        // contributors to be added to the contributing contributors in the callerActivity
        // List<Contributor> newContributingContributors = new List<Contributor>();
        // contributors to be removed from the contributing contributors in the callerActivity
        // List<Contributor> newNoncontributingContributors = new List<Contributor>();


        public ContributorListDialog(EditCardActivity context, ManipulatingContributorMode _mode) : base (context) { callerActivity = context; mode = _mode; this.Show(); }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.contributor_list_dialog);

            cardLeaderTextView = callerActivity.FindViewById<TextView>(Resource.Id.cardLeaderTextView);

            // When clicked will commit any changes that are going to be made
            FindViewById<Button>(Resource.Id.commitContributorDialogBtn).Click += delegate
            {
                // Committing changes to be made
                CommitChanges?.Invoke();
                this.Dismiss();
            };
            // When clicked will cancel any changes that were going to be made
            FindViewById<Button>(Resource.Id.cancelContributorDialogBtn).Click += delegate
            {                
                this.Dismiss();
            };
            contributorListView = FindViewById<ListView>(Resource.Id.contributorDialogListView);
            contributorListView.ItemClick += (e, a) =>
            {
                // Depending on the dialogs purpose the event will fire different bounded methods
                SelectContributorEvent?.Invoke(a.Position, a.View);
            };


            // Users are only allowed to make the leader of the card a contributor that is already added to the contributing contributors list
            switch (mode)
            {
                // The dialog was created to select a single contributing contributor as it's leader
                //      Will be set as the leader of the card, set eventhandler and render contributing contributors
                case ManipulatingContributorMode.SettingLeader:
                    SelectContributorEvent = SelectCardLeader;
                    CommitChanges = CommitCardLeader;
                    contributorListView.Adapter = new EditCardContributorAdapter(callerActivity.contributingContributors, callerActivity, mode);
                    break;
                // The dialog was created to select & add as many contributing contributors as desired.
                //      Will be swapped to non-contributing contributors list, set eventhandler and render contributing contributors
                case ManipulatingContributorMode.Removing:
                    SelectContributorEvent = SelectContributingContributor;
                    CommitChanges = CommitToBeRemovedContributors;
                    contributorListView.Adapter = new EditCardContributorAdapter(callerActivity.contributingContributors, callerActivity, mode);
                    break;
                // The dialog was created to select & add as many non-contributing contributors as desired.
                //      Will be swapped to contributing contributors list, set eventhandler and render non-contributing contributors
                case ManipulatingContributorMode.Adding:
                    SelectContributorEvent = SelectNoncontributingContributor;
                    CommitChanges = CommitToBeAddedContributors;
                    contributorListView.Adapter = new EditCardContributorAdapter(callerActivity.noncontributingContributors, callerActivity, mode);
                    break;
            }
        }

        /// 
        /// 
        ///     Gets a reference the clicked contributor, before changes are comitted the user must either cancel or commit
        /// 
        /// 
        private void SelectCardLeader(int _pos, View _view)
        {
            var contributorAdapter = contributorListView.GetEditCardContributorAdapter();           
            var selectedContributors = contributorAdapter.selectedContributors;
            var contributors = contributorAdapter.contributors;

            // If the user hasn't selected this board before add it and to the list for deletion
            if (!selectedContributors.Contains(contributors[_pos]))
            {
                // if a contributor is already selected, remove it from the list since the user has selected a new contributor
                if (selectedContributors.Count == 1)
                {
                    // Which ever view contains the contributor which is about to be removed needs have it background set to transparent to show its been deselected
                    contributorAdapter.listViewChildren.First(view => ((ContributorViewHolder)view.Tag).Email.Text == selectedContributors[0].Email).SetBackgroundColor(Color.Transparent);
                    selectedContributors.RemoveAt(0);
                }

                _view.SetBackgroundResource(Resource.Color.deleteColor);
                selectedContributors.Add(contributors[_pos]);
            }
            // If the user has selected this board and now clicks it again, remove it from the list
            else
            {
                _view.SetBackgroundColor(Color.Transparent);
                selectedContributors.Remove(contributors[_pos]);
            }   
        }

        /// 
        /// 
        ///     Adds the selected row's underlying viewholder's contributor to a list for being removed from contributing contributor list
        /// 
        /// 
        private void SelectNoncontributingContributor(int _pos, View _view)
        {
            ProcessSelection(_pos, _view, callerActivity.noncontributingContributors);            
        }

        ///
        /// 
        ///     Adds the selected row's underlying viewholder's contributor to a list for being added to contributing contributors
        /// 
        /// 
        private void SelectContributingContributor(int _pos, View _view)
        {
            ProcessSelection(_pos, _view, callerActivity.contributingContributors);
        }

        ///     
        /// 
        ///     Processes the item selection and assigns the correct contributor to the selected contributors adapter's list.       
        /// 
        /// 
        private void ProcessSelection(int _pos, View _view ,List<Contributor> _callerActivitycontributors)
        {
            var selectedContributors = contributorListView.GetEditCardContributorAdapter().selectedContributors;
            if (!selectedContributors.Contains(_callerActivitycontributors[_pos]))
            {                
                selectedContributors.Add(_callerActivitycontributors[_pos]);
                _view.SetBackgroundResource(Resource.Color.deleteColor);
            }
            else
            {
                selectedContributors.Remove(_callerActivitycontributors[_pos]);
                _view.SetBackgroundColor(Color.Transparent);
            }
        }


        /// 
        /// 
        ///     Commits the changes from the staging variables to the original variables inside the callerActivity
        ///         Assigns the callerActivity's' variables for managing the card leader the selected contributor
        /// 
        /// 
        private void CommitCardLeader()
        {

            // TODO: Make this safe when a user pressed commit and no contributor is selected            
            var selectedContributors = contributorListView.GetEditCardContributorAdapter().selectedContributors;

            // If nothing was selected then set the values to default
            if (selectedContributors == null || selectedContributors.Count == 0)
            {
                cardLeaderTextView.Text = "N/A";
                // Also include the leader into the wrapper for the kanbanModel
                callerActivity.clickedKanbanModel.Leader = null;
                return;
            }

            Contributor cardLeader = selectedContributors[0];
            // Based off the position provided from the ItemClicker handler, we assign the values of the selected contributors to the leader UI view components
            cardLeaderTextView.Text = $"{cardLeader.Name} | {cardLeader.Email}";
            // Also include the leader into the wrapper for the kanbanModel
            callerActivity.clickedKanbanModel.Leader = cardLeader;            
        }

        /// 
        /// 
        ///     Sets up the ProcessCommit method to add all selected contributors to noncontributingContributes    
        /// 
        /// 
        private void CommitToBeAddedContributors()
        {
            ProcessCommit(callerActivity.contributingContributors, callerActivity.noncontributingContributors);
        }

        /// 
        /// 
        ///     Sets up the ProcessCommit method to add all selected contributors to contributingContributes
        ///
        /// 
        private void CommitToBeRemovedContributors()
        {            
            var selectedContributors = contributorListView.GetEditCardContributorAdapter().selectedContributors;
            
            // If the user commits the removal of an empty list just return
            if (selectedContributors == null || selectedContributors.Count == 0) return;

            // If any of the contributor's emails match the leaders contributor.
            // The leader needs to be set to a default value because the contributor that was the leader is going to be removed          
            if (selectedContributors.Any(contributor => contributor.Email == callerActivity.clickedKanbanModel.Leader?.Email))
            {
                callerActivity.clickedKanbanModel.Leader = null;
                cardLeaderTextView.Text = "N/A";
            }

            ProcessCommit(callerActivity.noncontributingContributors, callerActivity.contributingContributors);
        }

        ///
        /// 
        ///     Processes the commit based off the calling method
        /// 
        /// 
        private void ProcessCommit(List<Contributor> contributorsAdd, List<Contributor> contributorsRemove)
        {
            var selectedContributors = contributorListView.GetEditCardContributorAdapter().selectedContributors;
            contributorsAdd.AddRange(selectedContributors);
            foreach (Contributor contributor in selectedContributors)
            {
                contributorsRemove.Remove(contributor);
            }            
        }
    }
}