using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RentItServer
{
    [DataContract]
    public class Track
    {
        [DataMember]
        public int Upvotes
        {
            get;
            set;
        }
        [DataMember]
        public int Downvotes
        {
            get;
            set;
        }
        [DataMember]
        public string Name
        {
            get;
            set;
        }
        [DataMember]
        public int Id
        {
            get;
            set;
        }

        public Track(string name, int id)
        {
            Name = name;
            Id = id;
        }
    }
}