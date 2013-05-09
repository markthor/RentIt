﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RentItServer.ITU.DatabaseWrapperObjects
{
    [DataContract]
    public class Channel
    {
        public Channel(int id, string name, string description, double? rating, int? hits, User owner,
                       List<Comment> comments, List<User> subscribers, List<Genre> genres, List<Track> tracks, string streamUri)
        {
            Id = id;
            Name = name;
            Description = description;
            Rating = rating;
            Hits = hits;
            Owner = owner;
            Comments = comments;
            Subscribers = subscribers;
            Genres = genres;
            Tracks = tracks;
            StreamUri = streamUri;
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
        public User Owner { get; set; }
        [DataMember]
        public List<Comment> Comments { get; set; }
        [DataMember]
        public List<User> Subscribers { get; set; }
        [DataMember]
        public List<Genre> Genres { get; set; }
        [DataMember]
        public List<Track> Tracks { get; set; }
        [DataMember]
        public string StreamUri { get; set; }

        public static Channel[] GetChannels(IEnumerable<RentItServer.Channel> channels)
        {
            if (channels == null) return null;
            List<Channel> convertedChannels = new List<Channel>();
            foreach (RentItServer.Channel channel in channels)
            {
                convertedChannels.Add(channel.GetChannel());
            }
            return convertedChannels.ToArray();
        }
    }
}