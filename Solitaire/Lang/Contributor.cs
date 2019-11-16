using System;

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
        private string imageUrl;
        private string name;

        public string Name { get { return name; } set { name = value; } }
        public string Email { get { return email; } set { email = value; } }

        public string ImageUrl { get => imageUrl; set => imageUrl = value; }

        public Contributor() { }

        public Contributor(string _name, string _email)
        {
            Name = _name;
            Email = _email;

            Random rand = new Random();
            string[] images = { "images/avatar_red.png", "images/avatar_orange.png", "images/avatar_blue.png", "images/avatar_purple.png", "images/avatar_green.png" };
            string imageSelected = images[rand.Next(0, 4)];
            ImageUrl = imageSelected;
        }

        public Contributor(string name, string email, string imageUrl)
        {
            Name = name;
            Email = email;
            ImageUrl = imageUrl;
        }
    }
}