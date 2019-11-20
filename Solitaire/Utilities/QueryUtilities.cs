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
    public static class QueryUtilities
    {
        public static List<string> QueryBoardLevelDistinctContributorsForEmail(Lang.Board _board)
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

        // TODO Make these extention methods?

        public static List<Lang.Contributor> QueryBoardLevelDistinctContributorsForInstance(Lang.Board _board)
        {
            List<string> emails = QueryBoardLevelDistinctContributorsForEmail(_board);


            // TODO: for this method we need to lookup all the contributors once we have a list of their primary keys


            
        }
    }
}