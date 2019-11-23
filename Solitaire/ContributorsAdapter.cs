using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Solitaire.Lang;

namespace Solitaire
{
    class ContributorsAdapter : BaseAdapter<Contributor>
    {

        List<Contributor> contributors;
        Context callerActivity;

        //public ContributorsAdapter(List<Contributor> _contributors)
        //{
        //    contributors = _contributors;
        //}

        public ContributorsAdapter(List<Contributor> _contributors, Context _context)
        {
            contributors = _contributors;
            callerActivity = _context;
        }


        public override Contributor this[int position] { get { return contributors[position]; } }
        public override int Count { get { return contributors.Count; } }
        public override long GetItemId(int position) { return position; }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            // string[] images = { "images/avatar_red.png", "images/avatar_orange.png", "images/avatar_blue.png", "images/avatar_purple.png", "images/avatar_green.png" };

            View view = convertView;

            if (view == null)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.contributor_row, parent, false);

                var avatar = view.FindViewById<Refractored.Controls.CircleImageView>(Resource.Id.avatar);
                var name = view.FindViewById<TextView>(Resource.Id.nameOfContributor);
                var email = view.FindViewById<TextView>(Resource.Id.emailOfContributor);

                //avatar.SetImageDrawable(Drawable.CreateFromStream(parent.Context.Assets.Open(contributors[position].ImageUrl), null));
                //name.Text = contributors[position].Name;
                //email.Text = contributors[position].Email;
                //// Random rand = new Random();
                //string imageSelected = images[new Random().Next(0, 4)];
                // avatar.SetImageDrawable(Drawable.CreateFromStream(callingActivity.Context.Assets.Open(imageSelected), null));

                view.Tag = new ContributorViewHolder() { Name = name, Email = email, Avatar = avatar };
            }

            ContributorViewHolder holder = (ContributorViewHolder)view.Tag;
            holder.Name.Text = contributors[position].Name;
            holder.Email.Text = contributors[position].Email;
            holder.Avatar.SetImageDrawable(AssetManager.GetRandomDrawable(callerActivity));

            return view;
        }
    }

    public class ContributorViewHolder : Java.Lang.Object
    {
        public TextView Name { get; set; }
        public TextView Email { get; set; }
        public Refractored.Controls.CircleImageView Avatar { get; set; }
    }
}