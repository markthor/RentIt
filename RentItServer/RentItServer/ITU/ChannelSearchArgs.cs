using System;

namespace RentItServer.ITU
{
    [Serializable]
    public class ChannelSearchArgs
    {
        public const string NameDesc = "nam desc";
        public const string NameAsc = "nam asc";
        public const string AmountPlayedDesc = "ap desc";
        public const string AmountPlayedAsc = "ap asc";
        public const string SubscriptionsDesc = "sub desc";
        public const string SubscriptionsAsc = "sub asc";
        public const string NumberOfCommentsDesc = "com desc";
        public const string NumberOfCommentsAsc = "com asc";
        public const string RatingDesc = "rat desc";
        public const string RatingAsc = "rat asc";

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
        /// The channels rating. Default is -1
        /// </value>
        public double Rating { get; private set; }
        /// <summary>
        /// Gets the start index.
        /// </summary>
        /// <value>
        /// The start index.
        /// </value>
        public int StartIndex { get; private set; }
        /// <summary>
        /// Gets the end index.
        /// </summary>
        /// <value>
        /// The end index.
        /// </value>
        public int EndIndex { get; private set; }

        /// <summary>
        /// Gets the sort option. Must be one of the const fields of this class
        /// </summary>
        /// <value>
        /// The sort option.
        /// </value>
        public string SortOption { get; private set; }

        public ChannelSearchArgs()
        {
            SearchString = "";
            Genres = new string[] { };
            AmountPlayed = -1;
            NumberOfSubscriptions = -1;
            NumberOfComments = -1;
            Rating = -1;
            StartIndex = -1;
            EndIndex = -1;
            SortOption = "";
        }
    }
}