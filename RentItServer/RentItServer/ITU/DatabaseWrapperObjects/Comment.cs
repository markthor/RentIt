using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RentItServer.ITU.DatabaseWrapperObjects
{
    /// <summary>
    /// Wrapper object for the database entity "Comment". Hides some database specific details about the object.
    /// </summary>
    [DataContract]
    public class Comment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Comment"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="postTime">The post time.</param>
        /// <param name="content">The content.</param>
        /// <param name="channel">The channel.</param>
        /// <param name="user">The user.</param>
        public Comment(int id, DateTime postTime, string content, Channel channel, User user)
        {
            Id = id;
            PostTime = postTime;
            Content = content;
            ChannelId = channel.Id;
            UserId = user.Id;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the post time.
        /// </summary>
        /// <value>
        /// The post time.
        /// </value>
        [DataMember]
        public DateTime PostTime { get; set; }
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        [DataMember]
        public string Content { get; set; }
        /// <summary>
        /// Gets or sets the channel id.
        /// </summary>
        /// <value>
        /// The channel id.
        /// </value>
        [DataMember]
        public int ChannelId { get; set; }
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>
        /// The user id.
        /// </value>
        [DataMember]
        public int UserId { get; set; }
    }
}