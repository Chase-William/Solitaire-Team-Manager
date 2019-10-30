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
    public class Card : SolitaireType
    {
        public Card(string _name, string _description, string _parentDeck) : base(_name, _description) { ParentDeck = _parentDeck; }

        private string parentDeck;

        public string ParentDeck
        {
            get { return parentDeck; }
            set { parentDeck = value; }
        }

    }
}