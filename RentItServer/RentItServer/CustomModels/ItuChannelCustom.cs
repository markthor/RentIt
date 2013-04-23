using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer
{
    public partial class Channel
    {
        public ITU.DataObjects.Channel GetChannel()
        {
            return new ITU.DataObjects.Channel(Id, Name, Description, Rating, Hits, ChannelOwner.GetUser(),
                                   GetComments(), GetSubcribers(), GetGenres(), GetTracks());
        }

        private List<ITU.DataObjects.Comment> GetComments()
        {
            List<ITU.DataObjects.Comment> comments = new List<ITU.DataObjects.Comment>(Comments.Count);
            foreach (Comment c in Comments)
            {
                comments.Add(c.GetComment());
            }
            return comments;
        }

        private List<ITU.DataObjects.User> GetSubcribers()
        {
            List<ITU.DataObjects.User> subscribers = new List<ITU.DataObjects.User>(Subscribers.Count);
            foreach (User u in Subscribers)
            {
                subscribers.Add(u.GetUser());
            }
            return subscribers;
        }

        private List<ITU.DataObjects.Genre> GetGenres()
        {
            List<ITU.DataObjects.Genre> genres = new List<ITU.DataObjects.Genre>(Genres.Count);
            foreach (Genre g in Genres)
            {
                genres.Add(g.GetGenre());
            }
            return genres;
        }

        private List<ITU.DataObjects.Track> GetTracks()
        {
            List<ITU.DataObjects.Track> tracks = new List<ITU.DataObjects.Track>(Tracks.Count);
            foreach (Track t in Tracks)
            {
                tracks.Add(t.GetTrack());
            }
            return tracks;
        }
    }
}