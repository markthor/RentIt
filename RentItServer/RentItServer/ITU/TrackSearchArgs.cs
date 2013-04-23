using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer.ITU
{
    [Serializable]
    public class TrackSearchArgs
    {
        public const string NameDesc = "nam desc";
        public const string NameAsc = "nam asc";
        public const string ArtistDesc = "art desc";
        public const string ArtistAsc = "art asc";
        public const string UpvotesDesc = "upvot desc";
        public const string UpvotesAsc = "upvot asc";
        public const string DownvotesDesc = "downvot desc";
        public const string DownvotesAsc = "downvot asc";

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the artist.
        /// </summary>
        /// <value>
        /// The artist.
        /// </value>
        public string Artist { get; private set; }

        /// <summary>
        /// Gets the upvotes.
        /// </summary>
        /// <value>
        /// The upvotes.
        /// </value>
        public int Upvotes { get; private set; }

        /// <summary>
        /// Gets the downvotes.
        /// </summary>
        /// <value>
        /// The downvotes.
        /// </value>
        public int Downvotes { get; private set; }

        /// <summary>
        /// Gets the sort option. Must be one of the const fields of this class
        /// </summary>
        /// <value>
        /// The sort option.
        /// </value>
        public string SortOption { get; private set; }

        public TrackSearchArgs()
        {
            Name = "";
            Artist = "";
            Upvotes = -1;
            Downvotes = -1;
        }
    }
}