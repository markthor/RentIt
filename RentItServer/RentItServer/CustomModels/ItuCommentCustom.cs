using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer
{
    public partial class Comment
    {
        public ITU.DataObjects.Comment GetComment()
        {
            return new ITU.DataObjects.Comment(ChannelId, Date, Content, Channel.GetChannel(), User.GetUser());
        }
    }
}