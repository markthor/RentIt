using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RentItServer.ITU.DatabaseWrapperObjects
{
    [DataContract]
    public class User
    {
        public User(int id, string username, string email, ICollection<Channel> channels, ICollection<Channel> subscribedChannels)
        {
            Id = id;
            Username = username;
            Email = email;
            Channels = channels;
            SubscribedChannels = subscribedChannels;
        }

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public ICollection<Channel> Channels = new HashSet<Channel>();
        [DataMember]
        public ICollection<Channel> SubscribedChannels = new HashSet<Channel>();
    }
}