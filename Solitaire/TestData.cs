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
using Syncfusion.SfKanban.Android;

namespace Solitaire
{
    public static class TestData
    {
        public static List<Board> boards = new List<Board>();
        static TestData()
        {
            boards.Add(new Board("Ma Lady", "Jesus Brother"));
            boards.Add(new Board("Some Board", "Wowwy"));
            boards.Add(new Board("Apple Board", "Little Pepe"));
            boards.Add(new Board("Ringo", "I'm Outty"));
            boards.Add(new Board("Pasta With No Cheese", "Yikers"));
            boards.Add(new Board("Momma Had a Little Lamb", "Brother"));
        }
    }
}