using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.SfKanban.Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Solitaire.Lang
{
    /// 
    /// 
    ///     Class to be used as a wrapper for KanbanModel so we can keep track of the leaders
    /// 
    /// 
    public class KanbanModelWrapper : KanbanModel
    {
        private Contributor leader;

        public Contributor Leader  { get { return leader; } set { leader = value; } }
    }
}