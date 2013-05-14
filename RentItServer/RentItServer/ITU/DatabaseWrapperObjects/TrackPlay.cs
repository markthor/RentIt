using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RentItServer.ITU.DatabaseWrapperObjects
{
    [DataContract]
    public class TrackPlay
    {
        public TrackPlay(DateTime timePlayed, Track track)
        {
            TimePlayed = timePlayed;
            TrackId = track.Id;
        }

        [DataMember]
        public DateTime TimePlayed { get; set; }
        [DataMember]
        public int TrackId { get; set; }
    }
}