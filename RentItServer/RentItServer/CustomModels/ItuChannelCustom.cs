using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer
{
    public partial class Channel
    {
        public ITU.DatabaseWrapperObjects.Channel GetChannel()
        {
            return new ITU.DatabaseWrapperObjects.Channel(Id, Name, Description, Rating, Hits, UserId, StreamUri, GetGenresAsStrings().ToArray());
        }

        private List<ITU.DatabaseWrapperObjects.Comment> GetComments()
        {
            List<ITU.DatabaseWrapperObjects.Comment> comments = new List<ITU.DatabaseWrapperObjects.Comment>(Comments.Count);
            foreach (Comment c in Comments)
            {
                comments.Add(c.GetComment());
            }
            return comments;
        }

        private List<string> GetGenresAsStrings()
        {
            List<string> genres = new List<string>(Genres.Count);
            foreach (Genre g in Genres)
            {
                genres.Add(g.Name);
            }
            return genres;
        } 

        private List<ITU.DatabaseWrapperObjects.User> GetSubcribers()
        {
            List<ITU.DatabaseWrapperObjects.User> subscribers = new List<ITU.DatabaseWrapperObjects.User>(Subscribers.Count);
            foreach (User u in Subscribers)
            {
                subscribers.Add(u.GetUser());
            }
            return subscribers;
        }

        private List<ITU.DatabaseWrapperObjects.Genre> GetGenres()
        {
            List<ITU.DatabaseWrapperObjects.Genre> genres = new List<ITU.DatabaseWrapperObjects.Genre>(Genres.Count);
            foreach (Genre g in Genres)
            {
                genres.Add(g.GetGenre());
            }
            return genres;
        }

        private List<ITU.DatabaseWrapperObjects.Track> GetTracks()
        {
            List<ITU.DatabaseWrapperObjects.Track> tracks = new List<ITU.DatabaseWrapperObjects.Track>(Tracks.Count);
            foreach (Track t in Tracks)
            {
                tracks.Add(t.GetTrack());
            }
            return tracks;
        }

        public static List<ITU.DatabaseWrapperObjects.Channel> GetChannels(IEnumerable<Channel> channels)
        {
            List<ITU.DatabaseWrapperObjects.Channel> convertedChannels = new List<ITU.DatabaseWrapperObjects.Channel>();
            foreach(Channel channel in channels)
            {
                convertedChannels.Add(channel.GetChannel());
            }
            return convertedChannels;
        }
    }
}