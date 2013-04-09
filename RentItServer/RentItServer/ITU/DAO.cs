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
        /// Creates the user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="email">The email.</param>
        /// <returns>The id of the user.</returns>
        public int CreateUser(string username, string password, string email)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                User theUser = new User()
                    {
                        Username = username,
                        Password = password,
                        //TODO: add email to entity
                        Comments = new Collection<Comment>(),
                        Channels = new Collection<Channel>(),
                        SubscribedChannels = new Collection<Channel>(),
                        Votes = new Collection<Vote>()
                    };
                context.Users.Add(theUser);
                context.SaveChanges();

                var users = from user in context.Users
                            where user.Username.Equals(username) && user.Password.Equals(password)
                            select user;
                if (users.Any() == false)
                {   // The user does not exist in the database, either something fucked up or another thread removed it already.
                    throw new Exception("User got created and saved in the database but is not in the database.... O.ô.");
                }
                return users.First().Id;
            }
        }

        /// <summary>
        /// Removes the user from the database.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <exception cref="System.ArgumentException">No user with user id [+userId+]</exception>
        /// <exception cref="System.Exception">End of \RemoveUser\. User was not get removed from the database.</exception>
        public void DeleteUser(int userId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var users = from user in context.Users
                            where user.Id == userId
                            select user;
                if (users.Any() == false) throw new ArgumentException("No user with user id [" + userId + "]");

                User theUser = users.First();
                context.Users.Remove(theUser);
                context.SaveChanges();

                users = from user in context.Users
                        where user.Id == userId
                        select user;

                if (users.Any() == true) throw new Exception("End of \"RemoveUser\". User was not get removed from the database.");
            }
        }

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="username">The username. Can be null.</param>
        /// <param name="password">The password. Can be null.</param>
        /// <param name="channels">The channels. Can be null.</param>
        /// <param name="comments">The comments. Can be null.</param>
        /// <param name="subscribedChannels">The subscribed channels. Can be null.</param>
        /// <param name="votes">The votes. Can be null.</param>
        /// <exception cref="System.ArgumentException">No user with user id[ + userId + ]</exception>
        public void UpdateUser(int userId, string username, string password, Collection<Channel> channels, Collection<Comment> comments, Collection<Channel> subscribedChannels, Collection<Vote> votes)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var users = from user in context.Users
                            where user.Id == userId
                            select user;
                if (users.Any() == false) throw new ArgumentException("No user with user id[" + userId + "]");

                User theUser = users.First();
                if (username != null) theUser.Username = username;
                if (password != null) theUser.Password = password;
                if (channels != null) theUser.Channels = channels;
                if (comments != null) theUser.Comments = comments;
                if (subscribedChannels != null) theUser.SubscribedChannels = subscribedChannels;
                if (votes != null) theUser.Votes = votes;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Creates a channel.
        /// </summary>
        /// <param name="channelName">Name of the channel.</param>
        /// <param name="userId">The id of the user creating the channel.</param>
        /// <param name="description">The description of the channel.</param>
        /// <param name="genres">The genres associated with the channel.</param>
        /// <returns>The created channel.</returns>
        public Channel CreateChannel(string channelName, int userId, string description, IEnumerable<string> genres)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var users = from user in context.Users
                            where user.Id == userId
                            select user;
                if (users.Any() == false)
                {
                    // throw new ArgumentException("No user with userId ["+userId+"]");
                    throw new Exception();
                }

                var someGenres = from genre in context.Genres.Where(genre => genres.Contains(genre.Name))
                                 select genre;
                if (someGenres.Any() == false)
                {
                    // throw new EmptyTableException("Genres");
                    throw new Exception();
                }

                // Create the channel object
                Channel theChannel = new Channel()
                {
                    Comments = new Collection<Comment>(),
                    Description = description,
                    Genres = someGenres.ToList(),
                    UserId = userId,
                    Name = channelName,
                    ChannelOwner = users.First(),
                    Subscribers = new Collection<User>(),
                    Hits = null,
                    Rating = null,
                    Tracks = new Collection<Track>()
                };

                context.Channels.Add(theChannel);
                context.SaveChanges();

                var channels = from channel in context.Channels.Where(channel => channel.Name == channelName && channel.UserId == userId)
                               select channel;

                if (channels.Any() == true)
                {
                    theChannel = channels.First();
                }
                else
                {   // The channel does not exist in the database, either something fucked up or another thread removed it already.
                    throw new Exception("Channel got created and saved in the database but is not in the database.... O.ô.");
                }
                return theChannel;
            }
        }

        /// <summary>
        /// Removes the channel from the database.
        /// </summary>
        /// <param name="ownerId">The owner id.</param>
        /// <param name="theChannel">The channel.</param>
        public void DeleteChannel(int ownerId, Channel theChannel)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                int channelId = theChannel.Id;
                context.Channels.Remove(theChannel);
                context.SaveChanges();

                var channels = from channel in context.Channels
                           where channel.ChannelOwner.Id == ownerId && channel.Id == channelId
                           select channel;
                //if(channels.Any() == true)  //TODO: throw exception here
            }
        }

        /// <summary>
        /// Updates the channel.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <param name="ownerId">The owner id. Can be null.</param>
        /// <param name="channelName">Name of the channel. Can be null.</param>
        /// <param name="description">The description. Can be null.</param>
        /// <param name="hits">The hits. Can be null.</param>
        /// <param name="rating">The rating. Can be null.</param>
        /// <param name="comments">The comments. Can be null.</param>
        /// <param name="genres">The genres. Can be null.</param>
        /// <param name="tracks">The tracks. Can be null.</param>
        /// <exception cref="System.ArgumentException">
        /// No channel with channel id [ + channelId + ]
        /// or
        /// No user with user id [ + ownerId + ]
        /// </exception>
        public void UpdateChannel(int channelId, int? ownerId, string channelName, string description, double? hits, double? rating, Collection<Comment> comments, Collection<Genre> genres, Collection<Track> tracks)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var channels = from channel in context.Channels
                               where channel.Id == channelId
                               select channel;
                if (channels.Any() == false) throw new ArgumentException("No channel with channel id [" + channelId + "]");

                Channel theChannel = channels.First();
                if (ownerId != null)
                {
                    var users = from user in context.Users
                                where user.Id == ownerId
                                select user;
                    if (users.Any() == false) throw new ArgumentException("No user with user id [" + ownerId + "]");

                    theChannel.UserId = (int)ownerId;
                    theChannel.ChannelOwner = users.First();
                }
                if (channelName != null) theChannel.Name = channelName;
                if (description != null) theChannel.Description = description;
                if (hits != null) theChannel.Hits = (int)hits;
                if (rating != null) theChannel.Rating = (double)rating;
                if (comments != null) theChannel.Comments = comments;
                if (genres != null) theChannel.Genres = genres;
                if (tracks != null) theChannel.Tracks = tracks;

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Gets the channel.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <returns>The channel</returns>
        public Channel GetChannel(int channelId)
        {
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
                return channels.First();
            }
        }

        /// <summary>
        /// Gets all channels.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Creates a vote.
        /// </summary>
        /// <param name="rating">The rating.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="trackId">The track id.</param>
        /// <exception cref="System.ArgumentException">
        /// no track with track id [+trackId+]
        /// or
        /// no user with user id [+userId+]
        /// </exception>
        public void CreateVote(int rating, int userId, int trackId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var tracks = from track in context.Tracks
                             where track.Id == trackId
                             select track;

                if(tracks.Any() == false)   throw new ArgumentException("no track with track id ["+trackId+"]");
                Track theTrack = tracks.First();

                var users = from user in context.Users
                            where user.Id == userId
                            select user;

                if(users.Any() == false)    throw new ArgumentException("no user with user id ["+userId+"]");
                User theUser = users.First();

                Vote vote = new Vote()
                    {
                        TrackId = trackId,
                        UserId = userId,
                        Value = rating,
                        Date = DateTime.UtcNow,
                        User = theUser,
                        Track = theTrack
                    };
                context.Votes.Add(vote);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Gets the track.
        /// </summary>
        /// <param name="trackId">The track id.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">No track found with id =  + trackId</exception>
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

        /// <summary>
        /// Removes the track.
        /// </summary>
        /// <param name="track">The track.</param>
        public void RemoveTrack(Track track)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                context.Tracks.Remove(track);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Creates a comment.
        /// </summary>
        /// <param name="comment">The comment.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="channelId">The channel id.</param>
        /// <exception cref="System.ArgumentException">
        /// No user with user id [+userId+]
        /// or
        /// No channel with channel id [ + channelId + ]
        /// </exception>
        public void CreateComment(string comment, int userId, int channelId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var users = from user in context.Users
                            where user.Id == userId
                            select user;
                if(users.Any() == false)    throw new ArgumentException("No user with user id ["+userId+"]");
                User theUser = users.First();

                var channels = from channel in context.Channels
                               where channel.Id == channelId
                               select channel;
                if (channels.Any() == false) throw new ArgumentException("No channel with channel id [" + channelId + "]");
                Channel theChannel = channels.First();

                Comment theComment = new Comment()
                    {
                        ChannelId = channelId,
                        Content = comment,
                        UserId = userId,
                        Date = DateTime.UtcNow,
                        User = theUser,
                        Channel = theChannel
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