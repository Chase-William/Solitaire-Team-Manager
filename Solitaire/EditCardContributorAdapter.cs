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
    public class EditCardContributorAdapter : BaseAdapter<Contributor>
    {
        //private Action<View, int> ListViewElementSelected;
        EditCardActivity callerActivity;
        // Source collection for the adapter
        public List<Contributor> contributors;     
        // List containing all the rows (views) for changing their color when representing a selected contributor
        public List<View> listViewChildren = new List<View>();
        // Contains all the selected contributors that will be used in the ContributorListDialog depending on the dialog's purpose
        public List<Contributor> selectedContributors = new List<Contributor>();
        //private bool HasInitializedAllViews;

        public EditCardContributorAdapter(List<Contributor> _contributors, EditCardActivity _context, ManipulatingContributorMode _mode)
        {
            contributors = _contributors;
            callerActivity = _context;

        }


        public override Contributor this[int position] { get { return contributors[position]; } }
        public override int Count { get { return contributors.Count; } }
        public override long GetItemId(int position) { return position; }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            if (view == null)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.contributor_row, parent, false);

                var avatar = view.FindViewById<Refractored.Controls.CircleImageView>(Resource.Id.avatar);
                var name = view.FindViewById<TextView>(Resource.Id.nameOfContributor);
                var email = view.FindViewById<TextView>(Resource.Id.emailOfContributor);

                view.Tag = new ContributorViewHolder() { Name = name, Email = email, Avatar = avatar };                
            }

            // if (HasInitializedAllViews)
            DetermineViewBG(view, position);

            ContributorViewHolder holder = (ContributorViewHolder)view.Tag;
            holder.Name.Text = contributors[position].Name;
            holder.Email.Text = contributors[position].Email;
            holder.Avatar.SetImageDrawable(AssetManager.GetRandomDrawable(callerActivity));

            // Getting references to all the views
            if (!listViewChildren.Contains(view))
                listViewChildren.Add(view);

            return view;
        }



        private void DetermineViewBG(View view, int position)
        {
            if (selectedContributors.Contains(contributors[position]))
            {
                view.SetBackgroundResource(Resource.Color.deleteColor);
            }
            else
            {
                view.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }
        }
































        //public override bool AreAllItemsEnabled()
        //{
        //    HasInitializedAllViews = true;
        //    return base.AreAllItemsEnabled();            
        //}

        //private void SetLeaderContributorAdapter(View view, int position)
        //{

        //    // If the board is within the list then it is up for deletion
        //    if (selectedContributors.Contains(contributors[position]))
        //    {
        //        view.SetBackgroundResource(Resource.Color.deleteColor);
        //    }
        //    else
        //    {
        //        view.SetBackgroundColor(Android.Graphics.Color.Transparent);
        //    }

        //    // If two of the same contributors are within the list, unselect the current view and clear the list
        //    //if (selectedContributors.FindAll(contributor => contributor == contributors[position]).Count == 2)
        //    //{
        //    //    selectedContributors.Clear();
        //    //    view.SetBackgroundColor(Android.Graphics.Color.Transparent);
        //    //}
        //    //// If the first contributor is different from the 2nd remove the first so that the second becomes the first. 
        //    //// Also unselect the view of the first contributor within the list
        //    //else 
        //    //{                     
        //    //    view.SetBackgroundColor(Android.Graphics.Color.Transparent);
        //    //    selectedContributors.Insert(0, contributors[0]);
        //    //}
        //}
    }




}