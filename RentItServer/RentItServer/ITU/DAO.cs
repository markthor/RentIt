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
                var someGenres = from genre in context.Genres.Where(genre => genres.Contains(genre.Name))
                                 select genre;

                var aUser = from user in context.Users.Where(user => user.Id == userId)
                            select user;

                // Create the channel object
                ch = new Channel()
                {
                    Comments = new Collection<Comment>(),
                    Description = description,
                    Genres = someGenres.ToList(),
                    UserId = userId,
                    Name = channelName,
                    ChannelOwner = aUser.First(),  
                    Subscribers = new Collection<User>(),
                    Hits = null,
                    Rating = null,
                    Tracks = new Collection<Track>()
                };

                // I'm not sure this will work, i have not specified the channels id as the databse should generate it itself. 
                context.Channels.Add(ch);
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

                var theChannel = from channel in context.Channels.Where(channel => channel.Name == channelName && channel.UserId == userId)
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
                var channels = from channel in context.Channels.Where(channel => channel.Id == channelId)
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
                var channels = from channel in context.Channels select channel;

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
                var channels = from channel in context.Channels
                               where channel.Name.StartsWith(filter.SearchString)
                               select channel;

                if (filter.AmountPlayed > -1)
                {   // Apply amount played filter
                    channels = from channel in channels
                               where channel.Hits >= filter.AmountPlayed
                               select channel;
                }
                if (filter.Genres.Any() == true)
                {   // Apply genre filter
                    channels = from channel in channels
                               where channel.Genres.Any(genre => filter.Genres.Contains(genre.Name))
                               select channel;
                }
                if (filter.NumberOfComments > -1)
                {   // Apply comment filter
                    channels = from channel in channels
                               where channel.Comments.Count >= filter.NumberOfComments
                               select channel;
                }
                if (filter.NumberOfSubscriptions > -1)
                {   // Apply subscription filter
                    channels = from channel in channels
                               where channel.Subscribers.Count >= filter.NumberOfSubscriptions
                               select channel;
                }
                if (filter.SortOption == -1)
                {   // Descending
                    channels = from channel in channels
                               orderby channel.Name descending
                               select channel;

                }
                else if (filter.SortOption == 1)
                {   // Ascending
                    channels = from channel in channels
                               orderby channel.Name ascending
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
                var channels = from channel in context.Channels.Where(channel => channel.UserId == userId && channel.Id == channelId)
                               select channel;

                if (channels.Any() == false)
                {   // The channel does not exist
                    return;
                }
                // Delete the channel entry from the database
                context.Channels.Remove(channels.First());
                context.SaveChanges();
            }
        }

        public void VoteTrack(int rating, int userId, int trackId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {   // TODO: Will only work if the vote table doesn't have "tracks" and "users" associated...
                Vote vote = new Vote()
                    {
                        TrackId = trackId,
                        UserId = userId,
                        Value = rating,
                        Date = DateTime.UtcNow
                    };
                context.Votes.Add(vote);
                context.SaveChanges();
            }
        }

        public Track GetTrack(int trackId)
        {
            Track theTrack;
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var tracks = from track in context.Tracks.Where(track => track.Id == trackId)
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
                context.Tracks.Remove(track);
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
                        ChannelId = channelId,
                        Content = comment,
                        UserId = userId,
                        Date = DateTime.UtcNow
                    };
                context.Comments.Add(theComment);
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