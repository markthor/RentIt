using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer
{
    public partial class TrackPlay
    {
        public ITU.DatabaseWrapperObjects.TrackPlay GetTrackPlay()
        {
            return new ITU.DatabaseWrapperObjects.TrackPlay(TimePlayed, Track.GetTrack());
        }
    }
}