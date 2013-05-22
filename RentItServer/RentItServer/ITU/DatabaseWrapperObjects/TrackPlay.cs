using System;
using System.Runtime.Serialization;

namespace RentItServer.ITU.DatabaseWrapperObjects
{
    /// <summary>
    /// Wrapper object for the database entity "TrackPlay". Hides some database specific details about the object.
    /// </summary>
    [DataContract]
    public class TrackPlay
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrackPlay"/> class.
        /// </summary>
        /// <param name="timePlayed">The time played.</param>
        /// <param name="track">The track.</param>
        public TrackPlay(DateTime timePlayed, Track track)
        {
            TimePlayed = timePlayed;
            TrackId = track.Id;
        }

        /// <summary>
        /// Gets or sets the time played.
        /// </summary>
        /// <value>
        /// The time played.
        /// </value>
        [DataMember]
        public DateTime TimePlayed { get; set; }
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