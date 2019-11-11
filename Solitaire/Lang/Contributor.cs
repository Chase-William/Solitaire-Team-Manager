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
    /// 
    /// 
    ///     Class to house all the data about the contributors
    /// 
    /// 
    public class Contributor
    {       
        private string email;
        private string firstName;
        private string lastName;

        public string LastName { get { return lastName; } set { lastName = value; } }
        public string FirstName { get { return firstName; } set { firstName = value; } }
        public string Email { get { return email; } set { email = value; } }

        public Contributor()
        {

        }

        public Contributor(string _first,string _last,string _email)
        {
            FirstName = _first;
            LastName = _last;
            Email = _email;

        }
    }
}