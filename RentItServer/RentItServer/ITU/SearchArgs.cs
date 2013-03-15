using System;

namespace RentItServer.ITU
{
    [Serializable]
    public class SearchArgs
    {
        /// <summary>
        /// Gets the search string.
        /// </summary>
        /// <value>
        /// The search string. Default is an empty string
        /// </value>
        public string SearchString { get; private set; }
        /// <summary>
        /// Gets the genres to include in search.
        /// </summary>
        /// <value>
        /// The genres to include in search. Default is an emoty string array
        /// </value>
        public string[] Genres { get; private set; }
        /// <summary>
        /// Gets the amount played filter.
        /// </summary>
        /// <value>
        /// The amount played. Default is -1
        /// </value>
        public int AmountPlayed { get; private set; }
        /// <summary>
        /// Gets the number of subscriptions filter.
        /// </summary>
        /// <value>
        /// The number of subscriptions. Default is -1
        /// </value>
        public int NumberOfSubscriptions { get; private set; }
        /// <summary>
        /// Gets the number of comments to filter.
        /// </summary>
        /// <value>
        /// The number of comments. Default is -1
        /// </value>
        public int NumberOfComments { get; private set; }
        /// <summary>
        /// Gets the sort option filter.
        /// </summary>
        /// <value>
        /// The sort option. Default is 0, -1 is descending, 1 is ascending
        /// </value>
        public int SortOption { get; private set; }
        /// <summary>
        /// Gets the start index.
        /// </summary>
        /// <value>
        /// The start index.
        /// </value>
        public int startIndex { get; private set; }
        /// <summary>
        /// Gets the end index.
        /// </summary>
        /// <value>
        /// The end index.
        /// </value>
        public int endIndex { get; private set; }

        public SearchArgs()
        {
            SearchString = "";
            Genres = new string[] { };
            AmountPlayed = -1;
            NumberOfSubscriptions = -1;
            NumberOfComments = -1;
            SortOption = 0;
            startIndex = -1;
            endIndex = -1;
        }
    }
}