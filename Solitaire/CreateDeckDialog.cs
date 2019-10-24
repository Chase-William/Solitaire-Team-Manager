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
    public class CreateDeckDialog : Dialog
    {
        public CreateDeckDialog(Context _context) : base(_context) { }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.create_deck_dialog);

            Button cancel = (Button)FindViewById(Resource.Id.button_cancel);
            cancel.Click += (e, a) =>
            {
                Dismiss();
            };
        }
    }
}