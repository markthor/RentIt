using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RentItServer.ITU
{
    public class DAO
    {
        //Singleton instance of the class
        private static DAO _instance;

        /// <summary>
        /// Private to ensure local instantiation.
        /// </summary>
        private DAO()
        {
        }

        /// <summary>
        /// Accessor method to access the only instance of the class
        /// </summary>
        /// <returns>The singleton instance of the class</returns>
        public static DAO GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DAO();
            }
            return _instance;
        }

        /// <summary>
        /// Creates a channel.
        /// </summary>
        /// <param name="channelName">Name of the channel.</param>
        /// <param name="userId">The id of the user creating the channel.</param>
        /// <param name="description">The description of the channel.</param>
        /// <param name="genres">The genres associated with the channel.</param>
        /// <returns>The created channel.</returns>
        public Channel CreateChannel(string channelName, int userId, string description, string[] genres)
        {
            // int channelId;
            Channel ch;
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var someGenres = from genre in context.genres.Where(genre => genres.Contains(genre.name))
                                 select genre;

                var aUser = from user in context.users.Where(user => user.id == userId)
                            select user;

                // Create the channel object
                ch = new Channel()
                {
                    comments = new Collection<Comment>(),
                    description = description,
                    genres = someGenres.ToList(),
                    userId = userId,
                    name = channelName,
                    users = aUser.First(),  // what if aUser is null/empty?... btw i assume that this is the channels owner. just as userId.... for some reason it is both a foreign key and a "navigational property"...
                    subscriptions = new Collection<User>(),
                    plays = null,
                    rating = null,
                    tracks = new Collection<Track>()
                };

                // I'm not sure this will work, i have not specified the channels id as the databse should generate it itself. 
                context.channels.Add(ch);
                context.SaveChanges();

                //// Seems a bit weird way around this, but for now it's the deal :)
                //var thelId = from channel in context.channels.Where(channel => channel.name == channelName && channel.userId == userId)
                //                select channel.id;

                //if(thelId.Any() == true){
                //    channelId = thelId.First();
                //}
                //else
                //{   // The channel does not have an id in the database, either something fucked up or another thread removed it already.
                //    throw new Exception("Channel got created and saved in the database but has no id.... O.ô.");
                //}

                var theChannel = from channel in context.channels.Where(channel => channel.name == channelName && channel.userId == userId)
                                 select channel;

                if (theChannel.Any() == true){
                    ch = theChannel.First();
                }
                else
                {   // The channel does not exist in the database, either something fucked up or another thread removed it already.
                   throw new Exception("Channel got created and saved in the database but is not in the database.... O.ô.");     
                }
            }

            return ch;
        }

        public int CreateUser(string username, string password, string email)
        {
            return -1;
        }

        public Channel GetChannel(int channelId)
        {
            Channel ch;
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var channels = from channel in context.channels.Where(channel => channel.id == channelId)
                               select channel;

                if (channels.Any() == false)
                {   // No channel with matching id
                    return null;
                }
                if (channels.Count() > 1)
                {
                    // Da fuk?
                }
                ch = channels.First();
            }
            return ch;
        }

        public IEnumerable<Channel> GetAllChannels()
        {
            IEnumerable<Channel> allChannels;
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var channels = from channel in context.channels select channel;

                // Execute the query before leaving "using" block
                allChannels = channels.ToList();
            }
            return allChannels;
        }

        /// <summary>
        /// Filters the with respect to the filter arguments: 
        ///     filter.SearchString
        ///     filter.AmountPlayed
        ///     filter.Genres
        ///     filter.NumberOfComments
        ///     filter.NumberOfSubscriptions
        ///     filter.SortOptions
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// Channel ids of the channels matching the filter.
        /// </returns>
        public List<Channel> GetChannelsWithFilter(SearchArgs filter)
        {
            List<Channel> filteredChannels;
            using (RENTIT21Entities context = new RENTIT21Entities())
            {   // get all channels that starts with filter.SearchString
                var channels = from channel in context.channels
                               where channel.name.StartsWith(filter.SearchString)
                               select channel;

                if (filter.AmountPlayed > -1)
                {   // Apply amount played filter
                    channels = from channel in channels
                               where channel.plays >= filter.AmountPlayed
                               select channel;
                }
                if (filter.Genres.Any() == true)
                {   // Apply genre filter
                    channels = from channel in channels
                               where channel.genres.Any(genre => filter.Genres.Contains(genre.name))
                               select channel;
                }
                if (filter.NumberOfComments > -1)
                {   // Apply comment filter
                    channels = from channel in channels
                               where channel.comments.Count >= filter.NumberOfComments
                               select channel;
                }
                if (filter.NumberOfSubscriptions > -1)
                {   // Apply subscription filter
                    channels = from channel in channels
                               where channel.subscriptions.Count >= filter.NumberOfSubscriptions
                               select channel;
                }
                if (filter.SortOption == -1)
                {   // Descending
                    channels = from channel in channels
                               orderby channel.name descending
                               select channel;

                }
                else if (filter.SortOption == 1)
                {   // Ascending
                    channels = from channel in channels
                               orderby channel.name ascending
                               select channel;
                }
                // Execute the query before leaving "using" block
                filteredChannels = channels.ToList();
            }

            if (filter.StartIndex != -1 && filter.EndIndex != -1 && filter.StartIndex <= filter.EndIndex)
            {   // Only get the channels within the specified interval [filter.startIndex, ..., filter.endIndex]
                Channel[] range = new Channel[filter.EndIndex - filter.StartIndex];
                filteredChannels.CopyTo(filter.StartIndex, range, 0, filter.EndIndex - filter.StartIndex);
                filteredChannels = new List<Channel>(range);
            }
            return filteredChannels;
        }

        public void DeleteChannel(int userId, int channelId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var channels = from channel in context.channels.Where(channel => channel.userId == userId && channel.id == channelId)
                               select channel;

                if (channels.Any() == false)
                {   // The channel does not exist
                    return;
                }
                // Delete the channel entry from the database
                context.channels.Remove(channels.First());
                context.SaveChanges();
            }
        }

        public void VoteTrack(int rating, int userId, int trackId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {   // TODO: Will only work if the vote table doesn't have "tracks" and "users" associated...
                Vote vote = new Vote()
                    {
                        trackId = trackId,
                        userId = userId,
                        value = rating,
                        date = DateTime.UtcNow
                    };
                context.votes.Add(vote);
                context.SaveChanges();
            }
        }

        public Track GetTrack(int trackId)
        {
            Track theTrack;
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var tracks = from track in context.tracks.Where(track => track.id == trackId)
                             select track;

                if (tracks.Any() == false)
                {
                    throw new Exception("No track found with id = " + trackId);
                }
                theTrack = tracks.First();
            }
            return theTrack;
        }

        public void RemoveTrack(Track track)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                context.tracks.Remove(track);
                context.SaveChanges();
            }
        }

        public void Comment(string comment, int userId, int channelId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                // TODO: Will only work if the comment table doesn't have "channels" and "users" associated..
                Comment theComment = new Comment()
                    {
                        channelId = channelId,
                        content = comment,
                        userId = userId,
                        date = DateTime.UtcNow
                    };
                context.comments.Add(theComment);
                context.SaveChanges();
            }
        }

        internal List<Track> GetTrackList(int ChannelId)
        {
            throw new NotImplementedException();
        }

        internal List<TrackPlay> GetTrackPlays(int ChannelId)
        {
            throw new NotImplementedException();
        }
    }
}