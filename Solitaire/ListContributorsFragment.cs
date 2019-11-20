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
using Android.Support.V4.App;

namespace Solitaire
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class ListContributorsFragment : Android.Support.V4.App.Fragment
    {
        MainActivity callerActivity;
        ListView listViewContributors;
        //List<Contributor> ree = new List<Contributor>();
        public ListContributorsFragment(MainActivity _context) { callerActivity = _context; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.all_contributors_fragment, container, false);

            listViewContributors = view.FindViewById<ListView>(Resource.Id.listViewContributors);
            listViewContributors.Adapter = new ContributorsAdapter(AssetManager.contributors, callerActivity);

            view.FindViewById<Button>(Resource.Id.addNewContributorBtn).Click += delegate
            {
                new CreateContributorDialog(callerActivity);
            };
            
            return view;
        }
    }
}