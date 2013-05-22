using System;

namespace RentItServer
{
    /// <summary>
    /// Partial class of the database entity "TrackPlay". Used to add functionality to the class.
    /// </summary>
    [Serializable]
    public partial class TrackPlay
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrackPlay"/> class.
        /// </summary>
        /// <param name="trackId">The track id.</param>
        /// <param name="timePlayed">The time played.</param>
        public TrackPlay(int trackId, DateTime timePlayed)
        {
            TrackId = trackId;
            TimePlayed = timePlayed;
        }
    }
}