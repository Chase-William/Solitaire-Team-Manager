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

namespace Solitaire.Lang
{
    public static class IdManager
    {
        private static int Id = 0;

        public static int GenerateId() => Id++;
    }
}