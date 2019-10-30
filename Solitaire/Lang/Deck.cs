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
    public class Deck : SolitaireType
    {
        public Deck(string _name, string _description) : base(_name, _description) { }
        
    }
}