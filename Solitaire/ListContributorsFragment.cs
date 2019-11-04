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

namespace Solitaire
{
    public class ListContributorsFragment : Android.Support.V4.App.Fragment
    {
        MainActivity parentActivityPtr;
        ListView contributorListView;

        public ListContributorsFragment(MainActivity _context) { parentActivityPtr = _context; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.list_boards_fragment, container, false);

            contributorListView = view.FindViewById<ListView>(Resource.Id.);






            return view;
        }
    }
}