namespace RentItServer
{
    /// <summary>
    /// Partial class of the database entity "Track". Used to add functionality to the class.
    /// </summary>
    public partial class Track
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Track"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="upvotes">The upvotes.</param>
        /// <param name="downvotes">The downvotes.</param>
        public Track(int id, int upvotes, int downvotes)
        {
            Id = id;
            UpVotes = upvotes;
            DownVotes = downvotes;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Track"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="upvotes">The upvotes.</param>
        /// <param name="downvotes">The downvotes.</param>
        /// <param name="length">The length.</param>
        public Track(int id, int upvotes, int downvotes, int length)
        {
            Id = id;
            UpVotes = upvotes;
            DownVotes = downvotes;
            Length = length;
        }
    }
}
