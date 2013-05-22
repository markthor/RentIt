using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer
{
    /// <summary>
    /// Partial class of the database entity "Channel". Used to add functionality to the class.
    /// </summary>
    public partial class Channel
    {
        /// <summary>
        /// Gets the wrapper for this channel.
        /// </summary>
        /// <returns></returns>
        public ITU.DatabaseWrapperObjects.Channel GetChannel()
        {
            return new ITU.DatabaseWrapperObjects.Channel(Id, Name, Description, Rating, Hits, UserId, StreamUri);
        }

        /// <summary>
        /// Gets the wrappers of comments.
        /// </summary>
        /// <returns></returns>
        private List<ITU.DatabaseWrapperObjects.Comment> GetComments()
        {
            List<ITU.DatabaseWrapperObjects.Comment> comments = new List<ITU.DatabaseWrapperObjects.Comment>(Comments.Count);
            foreach (Comment c in Comments)
            {
                comments.Add(c.GetComment());
            }
            return comments;
        }

        /// <summary>
        /// Gets the wrappers of these subcribers.
        /// </summary>
        /// <returns></returns>
        private List<ITU.DatabaseWrapperObjects.User> GetSubcribers()
        {
            List<ITU.DatabaseWrapperObjects.User> subscribers = new List<ITU.DatabaseWrapperObjects.User>(Subscribers.Count);
            foreach (User u in Subscribers)
            {
                subscribers.Add(u.GetUser());
            }
            return subscribers;
        }

        /// <summary>
        /// Gets the wrappers of these genres.
        /// </summary>
        /// <returns></returns>
        private List<ITU.DatabaseWrapperObjects.Genre> GetGenres()
        {
            List<ITU.DatabaseWrapperObjects.Genre> genres = new List<ITU.DatabaseWrapperObjects.Genre>(Genres.Count);
            foreach (Genre g in Genres)
            {
                genres.Add(g.GetGenre());
            }
            return genres;
        }

        /// <summary>
        /// Gets wrappers for these tracks.
        /// </summary>
        /// <returns></returns>
        private List<ITU.DatabaseWrapperObjects.Track> GetTracks()
        {
            List<ITU.DatabaseWrapperObjects.Track> tracks = new List<ITU.DatabaseWrapperObjects.Track>(Tracks.Count);
            foreach (Track t in Tracks)
            {
                tracks.Add(t.GetTrack());
            }
            return tracks;
        }

        /// <summary>
        /// Gets the wrappers for these channels.
        /// </summary>
        /// <param name="channels">The channels.</param>
        /// <returns></returns>
        public static List<ITU.DatabaseWrapperObjects.Channel> GetChannels(IEnumerable<Channel> channels)
        {
            List<ITU.DatabaseWrapperObjects.Channel> convertedChannels = new List<ITU.DatabaseWrapperObjects.Channel>();
            foreach(Channel channel in channels)
            {
                convertedChannels.Add(channel.GetChannel());
            }
            return convertedChannels;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override bool Equals(object obj)
        {
            var item = obj as Channel;

            if (item == null)
            {
                return false;
            }
            return this.Id == item.Id;
        }
    }
}