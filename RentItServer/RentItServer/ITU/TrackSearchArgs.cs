using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RentItServer.ITU
{
    /// <summary>
    /// This class holds sorting information for search queries on tracks.
    /// </summary>
    [Serializable]
    [DataContract]
    public class TrackSearchArgs
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
        /// Assign this value to SortOption in order to get the query sorted by artist descending
        /// </summary>
        [DataMember]
        public readonly string ArtistDesc = "art desc";
        /// <summary>
        /// Assign this value to SortOption in order to get the query sorted by artist ascending
        /// </summary>
        [DataMember]
        public readonly string ArtistAsc = "art asc";
        /// <summary>
        /// Assign this value to SortOption in order to get the query sorted by upvotes descending
        /// </summary>
        [DataMember]
        public readonly string UpvotesDesc = "upvot desc";
        /// <summary>
        /// Assign this value to SortOption in order to get the query sorted by upvotes ascending
        /// </summary>
        [DataMember]
        public readonly string UpvotesAsc = "upvot asc";
        /// <summary>
        /// Assign this value to SortOption in order to get the query sorted by downvotes descending
        /// </summary>
        [DataMember]
        public readonly string DownvotesDesc = "downvot desc";
        /// <summary>
        /// Assign this value to SortOption in order to get the query sorted by downvotes ascending
        /// </summary>
        [DataMember]
        public readonly string DownvotesAsc = "downvot asc";

        /// <summary>
        /// Gets the search string.
        /// </summary>
        /// <value>
        /// The search string.
        /// </value>
        [DataMember]
        public string SearchString { get; set; }

        /// <summary>
        /// Gets the artist.
        /// </summary>
        /// <value>
        /// The artist.
        /// </value>
        [DataMember]
        public string Artist { get; set; }

        /// <summary>
        /// Gets the upvotes.
        /// </summary>
        /// <value>
        /// The upvotes.
        /// </value>
        [DataMember]
        public int Upvotes { get; set; }

        /// <summary>
        /// Gets the downvotes.
        /// </summary>
        /// <value>
        /// The downvotes.
        /// </value>
        [DataMember]
        public int Downvotes { get; set; }

        /// <summary>
        /// Gets the start index.
        /// </summary>
        /// <value>
        /// The start index.
        /// </value>
        [DataMember]
        public int StartIndex { get; set; }
        
        /// <summary>
        /// Gets the end index.
        /// </summary>
        /// <value>
        /// The end index.
        /// </value>
        [DataMember]
        public int EndIndex { get; set; }
        
        /// <summary>
        /// Gets the sort option. Must be one of the readonly fields of this class
        /// </summary>
        /// <value>
        /// The sort option.
        /// </value>
        [DataMember]
        public string SortOption { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackSearchArgs"/> class.
        /// </summary>
        public TrackSearchArgs()
        {
            SearchString = "";
            Artist = "";
            Upvotes = -1;
            Downvotes = -1;
            StartIndex = -1;
            EndIndex = -1;
        }
    }
}