using System;
using RentItServer;

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