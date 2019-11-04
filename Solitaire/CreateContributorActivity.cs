using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Solitaire.Lang;

namespace Solitaire
{
    [Activity(Label = "CreateContactActivity")]
    public class CreateContributorActivity : AppCompatActivity
    {
        EditText firstNameEditText, lastNameEditText, emailEditText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.create_contributor_activity);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "Create Contributor";
            SetSupportActionBar(toolbar);

            firstNameEditText = FindViewById<EditText>(Resource.Id.firstNameEditText);
            lastNameEditText = FindViewById<EditText>(Resource.Id.lastNameEditText);
            emailEditText = FindViewById<EditText>(Resource.Id.emailEditText);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.create_contributor_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.TitleFormatted.ToString())
            {
                case "Create Contributor":
                    if (!AssetManager.IsEmailAlreadyUsed(emailEditText.Text))
                    {
                        AssetManager.contributors.Add(new Contributor()
                        {
                            FirstName = firstNameEditText.Text,
                            LastName = lastNameEditText.Text,
                            Email = emailEditText.Text
                        });
                        Finish();
                    }
                    else
                    {
                        Toast.MakeText(this, "This email has already been used.", ToastLength.Short);
                    }
                    
                    break;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }        
    }
}