using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer
{
    public partial class TrackPlay
    {
        public ITU.DataObjects.TrackPlay GetTrackPlay()
        {
            return new ITU.DataObjects.TrackPlay(TimePlayed, Track.GetTrack());
        }
    }
}