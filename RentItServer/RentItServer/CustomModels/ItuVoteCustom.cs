namespace RentItServer
{
    /// <summary>
    /// Partial class of the database entity "Vote". Used to add functionality to the class.
    /// </summary>
    public partial class Vote
    {
        /// <summary>
        /// Gets the wrapper for this vote.
        /// </summary>
        /// <returns></returns>
        public ITU.DatabaseWrapperObjects.Vote GetVote()
        {
            return new ITU.DatabaseWrapperObjects.Vote(Value, Date, UserId, TrackId);
        }
    }
}