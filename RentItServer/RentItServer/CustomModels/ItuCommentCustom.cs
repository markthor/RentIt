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
            return new ITU.DatabaseWrapperObjects.Comment(Id, Date, Content, ChannelId, UserId);
        }

        /// <summary>
        /// Gets the wrappers for these comments.
        /// </summary>
        /// <param name="comments">The comments.</param>
        /// <returns></returns>
        public static List<ITU.DatabaseWrapperObjects.Comment> GetComments(IEnumerable<Comment> comments)
        {
            List<ITU.DatabaseWrapperObjects.Comment> convertedComments = new List<ITU.DatabaseWrapperObjects.Comment>();
            foreach (Comment channel in comments)
            {
                convertedComments.Add(channel.GetComment());
            }
            return convertedComments;
        }
    }
}