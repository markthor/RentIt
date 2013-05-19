using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer
{
    /// <summary>
    /// Partial class of the database entity "TrackPlay". Used to add functionality to the class.
    /// </summary>
    public partial class TrackPlay
    {
        /// <summary>
        /// Gets the wrapper for this trackplay.
        /// </summary>
        /// <returns></returns>
        public ITU.DatabaseWrapperObjects.TrackPlay GetTrackPlay()
        {
            return new ITU.DatabaseWrapperObjects.TrackPlay(TimePlayed, Track.GetTrack());
        }
    }
}