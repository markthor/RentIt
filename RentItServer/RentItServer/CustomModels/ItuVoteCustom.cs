using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer
{
    public partial class Vote
    {
        public ITU.DataObjects.Vote GetVote()
        {
            return new ITU.DataObjects.Vote(Value, Date, User.GetUser(), Track.GetTrack());
        }
    }
}