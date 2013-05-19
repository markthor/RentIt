using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace RentItServer.ITU
{
    /// <summary>
    /// This class holds sorting information for search queries on channels.
    /// </summary>
    [Serializable]
    [DataContract]
    public class ChannelSearchArgs
    {
        /// <summary>
        /// Assign this value to SortOption in order to get the query sorted by name descending
        /// </summary>
        [DataMember]
        public readonly string NameDesc = "nam desc";
        /// <summary>
        /// Assign this value to SortOption in order to get the query sorted by name ascending
        /// </summary>
        [DataMember]
        public readonly string NameAsc = "nam asc";
        /// <summary>
        /// Assign this value to SortOption in order to get the query sorted by hits descending
        /// </summary>
        [DataMember]
        public readonly string HitsDesc = "ap desc";
        /// <summary>
        /// Assign this value to SortOption in order to get the query sorted by hits ascending
        /// </summary>
        [DataMember]
        public readonly string HitsAsc = "ap asc";
        /// <summary>
        /// Assign this value to SortOption in order to get the query sorted by subscriptions descending
        /// </summary>
        [DataMember]
        public readonly string SubscriptionsDesc = "sub desc";
        /// <summary>
        /// Assign this value to SortOption in order to get the query sorted by subscriptions ascending
        /// </summary>
        [DataMember]
        public readonly string SubscriptionsAsc = "sub asc";
        /// <summary>
        /// Assign this value to SortOption in order to get the query sorted by number of comments descending
        /// </summary>
        [DataMember]
        public readonly string NumberOfCommentsDesc = "com desc";
        /// <summary>
        /// Assign this value to SortOption in order to get the query sorted by number of comments ascending
        /// </summary>
        [DataMember]
        public readonly string NumberOfCommentsAsc = "com asc";
        /// <summary>
        /// Assign this value to SortOption in order to get the query sorted by rating descending
        /// </summary>
        [DataMember]
        public readonly string RatingDesc = "rat desc";
        /// <summary>
        /// Assign this value to SortOption in order to get the query sorted by rating ascending
        /// </summary>
        [DataMember]
        public readonly string RatingAsc = "rat asc";

        /// <summary>
        /// Gets the search string.
        /// </summary>
        /// <value>
        /// The search string. Default is an empty string
        /// </value>
        [DataMember]
        [DefaultValueAttribute("")]
        public string SearchString { get; set; }
        /// <summary>
        /// Gets the genres to include in search.
        /// </summary>
        /// <value>
        /// The genres to include in search. Default is an emoty string array
        /// </value>
        [DataMember]
        [DefaultValueAttribute(new string[] { "jazz" })]
        public string[] Genres { get; set; }
        /// <summary>
        /// Gets the amount played filter.
        /// </summary>
        /// <value>
        /// The amount played. Default is -1
        /// </value>
        [DataMember]
        [DefaultValueAttribute(-1)]
        public int AmountPlayed { get; set; }
        /// <summary>
        /// Gets the number of subscriptions filter.
        /// </summary>
        /// <value>
        /// The number of subscriptions. Default is -1
        /// </value>
        [DataMember]
        [DefaultValueAttribute(-1)]
        public int NumberOfSubscriptions { get; set; }
        /// <summary>
        /// Gets the number of comments to filter.
        /// </summary>
        /// <value>
        /// The number of comments. Default is -1
        /// </value>
        [DataMember]
        [DefaultValueAttribute(-1)]
        public int NumberOfComments { get; set; }
        /// <summary>
        /// Gets the sort option filter.
        /// </summary>
        /// <value>
        /// The channels rating. Default is -1
        /// </value>
        [DataMember]
        [DefaultValueAttribute(-1)]
        public double Rating { get; set; }
        /// <summary>
        /// Gets the start index.
        /// </summary>
        /// <value>
        /// The start index.
        /// </value>
        [DataMember]
        [DefaultValueAttribute(-1)]
        public int StartIndex { get; set; }
        /// <summary>
        /// Gets the end index.
        /// </summary>
        /// <value>
        /// The end index.
        /// </value>
        [DataMember]
        [DefaultValueAttribute(-1)]
        public int EndIndex { get; set; }

        /// <summary>
        /// Gets the sort option. Must be one of the readonly fields of this class
        /// </summary>
        /// <value>
        /// The sort option.
        /// </value>
        [DataMember]
        [DefaultValueAttribute("")]
        public string SortOption { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelSearchArgs"/> class.
        /// </summary>
        public ChannelSearchArgs()
        {
            SearchString = "";
            Genres = new string[] {};
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