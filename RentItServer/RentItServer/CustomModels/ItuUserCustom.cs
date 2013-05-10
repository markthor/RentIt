using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RentItServer.ITU.DatabaseWrapperObjects;

namespace RentItServer
{
    public partial class User
    {
        public ITU.DatabaseWrapperObjects.User GetUser()
        {
            return new ITU.DatabaseWrapperObjects.User(Id, Username, Email, ITU.DatabaseWrapperObjects.Channel.GetChannels(Channels), ITU.DatabaseWrapperObjects.Channel.GetChannels(SubscribedChannels));
        }
    }
}