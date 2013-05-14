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
        public Vote(int value, DateTime voteTime, User user, Track track)
        {
            Value = value;
            VoteTime = voteTime;
            UserId = user.Id;
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