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
    ///     Contains functions for interacting with the BoardAdapter
    /// 
    /// 
    public static class ListViewUntilities
    {
        ///
        ///        
        ///     Gets the BoardAdapter from the Adapter
        /// 
        /// 
        public static BoardAdapter GetBoardAdapter(this ListView _listView)
        {
            return (BoardAdapter)_listView.Adapter;
        }
    }
}
