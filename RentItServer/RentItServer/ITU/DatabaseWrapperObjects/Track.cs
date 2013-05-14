﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RentItServer.ITU.DatabaseWrapperObjects
{
    [DataContract]
    public class Track
    {
        public Track()
        {
        }

        public Track(int id, string path, string name, string artist, int length,
                     int upVotes, int downVotes, int channelId)
        {
            Id = id;
            Path = path;
            Name = name;
            Artist = artist;
            Length = length;
            UpVotes = upVotes;
            DownVotes = downVotes;
            ChannelId = channelId;
        }

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Path { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Artist { get; set; }
        [DataMember]
        public int Length { get; set; }
        [DataMember]
        public int UpVotes { get; set; }
        [DataMember]
        public int DownVotes { get; set; }

        [DataMember]
        public int ChannelId { get; set; }
    
        public static Track[] GetTracks(IEnumerable<RentItServer.Track> tracks)
        {
            if (tracks == null) return null;
            List<Track> convertedTracks = new List<Track>();
            foreach (RentItServer.Track track in tracks)
            {
                convertedTracks.Add(track.GetTrack());
            }
            return convertedTracks.ToArray();
        }
    }
}