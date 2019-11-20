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
    /// 
    /// 
    ///     Contains extension methods for querying board data
    /// 
    ///
    public static class QueryUtilities
    {
        /// 
        /// 
        ///     Queries a board for all distinct emails and then returns the distinct emails as List<string>
        /// 
        /// 
        public static List<string> QueryBoardAllDistinctContributorsForEmail(this Lang.Board _board)
        {
            List<string> emails = new List<string>();
            _board.Cards.ForEach(card =>
            {
                // If no contributors were added it will be the value null, which we need to check for
                if (card.ContributorEmails != null)
                {
                    card.ContributorEmails.ForEach(email =>
                    {
                        if (!emails.Contains(email))
                            emails.Add(email);
                    });
                }
            });
            return emails;
        }

        ///
        ///        
        ///     Queries a board for all distinct emails and then gets references to those contributors using their emails
        ///         then it returns the List<Contributor>
        /// 
        /// 
        public static List<Lang.Contributor> QueryBoardDistinctContributorsForInstance(this Lang.Board _board)
        {
            // First aquire list of distinct emails for the board
            List<string> emails = QueryBoardAllDistinctContributorsForEmail(_board);

            // Then use the emails to get references to the contributor instances
            return AssetManager.contributors.Where(contributor => emails.Contains(contributor.Email)).ToList();
        }
    }
}