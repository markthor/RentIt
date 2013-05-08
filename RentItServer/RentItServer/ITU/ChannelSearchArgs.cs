using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace RentItServer.ITU
{
    [Serializable]
    [DataContract]
    public class ChannelSearchArgs
    {
        [DataMember]
        public const string NameDesc = "nam desc";
        [DataMember]
        public const string NameAsc = "nam asc";
        [DataMember]
        public const string HitsDesc = "ap desc";
        [DataMember]
        public const string HitsAsc = "ap asc";
        [DataMember]
        public const string SubscriptionsDesc = "sub desc";
        [DataMember]
        public const string SubscriptionsAsc = "sub asc";
        [DataMember]
        public const string NumberOfCommentsDesc = "com desc";
        [DataMember]
        public const string NumberOfCommentsAsc = "com asc";
        [DataMember]
        public const string RatingDesc = "rat desc";
        [DataMember]
        public const string RatingAsc = "rat asc";

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
        /// Gets the sort option. Must be one of the const fields of this class
        /// </summary>
        /// <value>
        /// The sort option.
        /// </value>
        [DataMember]
        [DefaultValueAttribute("")]
        public string SortOption { get; set; }

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