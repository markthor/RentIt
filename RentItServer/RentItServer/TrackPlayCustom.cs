using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer
{
    public partial class TrackPlay
    {
        public TrackPlay(int trackId, DateTime playtime)
        {
            this.trackId = trackId;
            this.playtime = playtime;
        }
    }
}