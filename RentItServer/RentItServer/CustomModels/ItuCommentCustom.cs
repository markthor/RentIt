using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer
{
    /// <summary>
    /// Partial class of the database entity "Comment". Used to add functionality to the class.
    /// </summary>
    public partial class Comment
    {
        /// <summary>
        /// Gets the wrapper for this comment.
        /// </summary>
        /// <returns></returns>
        public ITU.DatabaseWrapperObjects.Comment GetComment()
        {
            return new ITU.DatabaseWrapperObjects.Comment(ChannelId, Date, Content, Channel.GetChannel(), User.GetUser());
        }
    }
}