using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer
{
    public partial class Vote
    {
        public ITU.DatabaseWrapperObjects.Vote GetVote()
        {
            return new ITU.DatabaseWrapperObjects.Vote(Value, Date, UserId, Track.GetTrack());
        }
    }
}