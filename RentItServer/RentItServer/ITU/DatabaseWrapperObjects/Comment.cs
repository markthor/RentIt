using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RentItServer.ITU.DatabaseWrapperObjects
{
    [DataContract]
    public class Comment
    {
        public Comment(int id, DateTime postTime, string content, Channel channel, User user)
        {
            Id = id;
            PostTime = postTime;
            Content = content;
            ChannelId = channel.Id;
            UserId = user.Id;
        }

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public DateTime PostTime { get; set; }
        [DataMember]
        public string Content { get; set; }
        [DataMember]
        public int ChannelId { get; set; }
        [DataMember]
        public int UserId { get; set; }
    }
}