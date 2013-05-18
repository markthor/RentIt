using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RentItServer.ITU.DatabaseWrapperObjects
{
    [DataContract]
    public class Vote
    {
        public Vote(int value, DateTime voteTime, int userId, Track track)
        {
            Value = value;
            VoteTime = voteTime;
            UserId = userId;
            TrackId = track.Id;
        }

        [DataMember]
        public int Value { get; set; }
        [DataMember]
        public DateTime VoteTime { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public int TrackId { get; set; }
    }
}