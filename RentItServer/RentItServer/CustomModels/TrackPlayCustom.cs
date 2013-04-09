using System;
using RentItServer;

namespace RentItServer
{
    [Serializable]
    public partial class TrackPlay
    {
        public TrackPlay(int trackId, DateTime timePlayed)
        {
            TrackId = trackId;
            TimePlayed = timePlayed;
        }
    }
}