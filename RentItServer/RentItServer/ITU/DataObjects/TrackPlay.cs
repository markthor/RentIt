using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RentItServer.ITU.DataObjects
{
    [DataContract]
    public class TrackPlay
    {
        public TrackPlay(DateTime timePlayed, Track track)
        {
            TimePlayed = timePlayed;
            Track = track;
        }

        [DataMember]
        public DateTime TimePlayed { get; set; }
        [DataMember]
        public Track Track { get; set; }
    }
}