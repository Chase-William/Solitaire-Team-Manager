using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Solitaire.Lang;

namespace Solitaire
{
    public class ListContributorsFragment : Android.Support.V4.App.Fragment
    {
        MainActivity parentActivityPtr;
        ListView contributorListView;

        public ListContributorsFragment(MainActivity _context) { parentActivityPtr = _context; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.list_contributors_fragment, container, false);

            contributorListView = view.FindViewById<ListView>(Resource.Id.listViewContributors);

            //Fill up assetmanagers contributors with starter contributors for testing.
            List<Contributor> ree = new List<Contributor>();
            ree.Add(new Contributor("Kyle", "Murphy", "ksm3091@rit.edu"));
            ree.Add(new Contributor("re", "goddamn", "fuuuuck"));
            AssetManager.contributors = ree;

            contributorListView.Adapter = new ExisitingContributorsAdapter(AssetManager.contributors);

            return view;
        }

        public override void OnResume()
        {
            base.OnResume();
            //contributorListView.Adapter = new ExisitingContributorsAdapter(AssetManager.contributors);
        }

    }
}