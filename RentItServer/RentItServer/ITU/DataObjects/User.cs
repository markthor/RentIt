using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RentItServer.ITU.DataObjects
{
    [DataContract]
    public class User
    {
        public User(int id, string username, string email)
        {
            Id = id;
            Username = username;
            Email = email;
        }

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Email { get; set; }
    }
}