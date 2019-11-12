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

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }

        public Contributor() { }

        public Contributor(string _first, string _last, string _email)
        {
            FirstName = _first;
            LastName = _last;
            Email = _email;
        }
    }
}