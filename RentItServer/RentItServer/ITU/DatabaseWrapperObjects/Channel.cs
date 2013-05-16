using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RentItServer.ITU.DatabaseWrapperObjects
{
    [DataContract]
    public class Channel
    {
        public Channel(int id, string name, string description, double? rating, int? hits, int ownerId, string streamUri, int subscribers)
        {
            Id = id;
            Name = name;
            Description = description;
            Rating = rating;
            Hits = hits;
            OwnerId = ownerId;
            StreamUri = streamUri;
            Subscribers = subscribers;
        }

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public double? Rating { get; set; }
        [DataMember]
        public int? Hits { get; set; }
        [DataMember]
        public int OwnerId { get; set; }
        [DataMember]
        public string StreamUri { get; set; }
        [DataMember]
        public int Subscribers { get; set; }
    }
}