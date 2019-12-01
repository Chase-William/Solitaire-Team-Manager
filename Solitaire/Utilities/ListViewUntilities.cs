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

namespace Solitaire
{
    /// 
    /// 
    ///     Contains extension methods for interacting with the BoardAdapter
    /// 
    /// 
    public static class ListViewUntilities
    {
        ///
        ///        
        ///     Gets the BoardAdapter from the generic adapter
        /// 
        /// 
        public static BoardAdapter GetBoardAdapter(this ListView _listView)
        {
            return (BoardAdapter)_listView.Adapter;
        }

        /// 
        /// 
        ///     Gets the ContributorAdapter from the generic adapter
        /// 
        /// 
        public static ContributorsAdapter GetContributorAdapter(this ListView _listView)
        {
            return (ContributorsAdapter)_listView.Adapter;
        }
    }
}
