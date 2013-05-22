using System;
using System.Runtime.Serialization;

namespace RentItServer.ITU.DatabaseWrapperObjects
{
    /// <summary>
    /// Wrapper object for the database entity "Vote". Hides some database specific details about the object.
    /// </summary>
    [DataContract]
    public class Vote
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vote"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="voteTime">The vote time.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="trackId">The track id.</param>
        public Vote(int value, DateTime voteTime, int userId, int trackId)
        {
            Value = value;
            VoteTime = voteTime;
            UserId = userId;
            TrackId = trackId;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [DataMember]
        public int Value { get; set; }
        /// <summary>
        /// Gets or sets the vote time.
        /// </summary>
        /// <value>
        /// The vote time.
        /// </value>
        [DataMember]
        public DateTime VoteTime { get; set; }
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>
        /// The user id.
        /// </value>
        [DataMember]
        public int UserId { get; set; }
        /// <summary>
        /// Gets or sets the track id.
        /// </summary>
        /// <value>
        /// The track id.
        /// </value>
        [DataMember]
        public int TrackId { get; set; }
    }
}