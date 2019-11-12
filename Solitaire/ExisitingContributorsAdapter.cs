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
    class ExisitingContributorsAdapter : BaseAdapter<Contributor>
    {

        List<Contributor> contributors;

        public ExisitingContributorsAdapter(List<Contributor> _contributors)
        {
            contributors = _contributors;
        }

        public override Contributor this[int position] { get { return contributors[position]; } }
        public override int Count { get { return contributors.Count; } }
        public override long GetItemId(int position) { return position; }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            string[] images = { "images/avatar_red.png", "images/avatar_orange.png", "images/avatar_blue.png", "images/avatar_purple.png", "images/avatar_green.png" };

            View view = convertView;

            if (view == null)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.existing_contributor_adapter, parent, false);

                var avatar = view.FindViewById<Refractored.Controls.CircleImageView>(Resource.Id.avatar);
                var name = view.FindViewById<TextView>(Resource.Id.nameOfContributor);
                var email = view.FindViewById<TextView>(Resource.Id.emailOfContributor);

                name.Text = contributors[position].FirstName + " " + contributors[position].LastName;
                email.Text = contributors[position].Email;
                Random rand = new Random();
                string imageSelected = images[rand.Next(0, 4)];
                avatar.SetImageDrawable(Drawable.CreateFromStream(parent.Context.Assets.Open(imageSelected), null));
            }

            

            return view;
        }
    }
}