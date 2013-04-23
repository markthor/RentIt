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
            Channel = channel;
            User = user;
        }

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public DateTime PostTime { get; set; }
        [DataMember]
        public string Content { get; set; }
        [DataMember]
        public Channel Channel { get; set; }
        [DataMember]
        public User User { get; set; }
    }
}