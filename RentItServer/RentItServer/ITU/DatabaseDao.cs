using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using RentItServer.ITU.Exceptions;

namespace RentItServer.ITU
{
    /// <summary>
    /// The Data Access Object for all communication from the program to the database.
    /// </summary>
    public class DatabaseDao
    {
        //Singleton instance of the class
        private static DatabaseDao _instance;

        /// <summary>
        /// Private to ensure local instantiation.
        /// </summary>
        private DatabaseDao() { }

        /// <summary>
        /// Accessor method to access the only instance of the class
        /// </summary>
        /// <returns>The singleton instance of the class</returns>
        public static DatabaseDao GetInstance()
        {
            return _instance ?? (_instance = new DatabaseDao());
        }

        /// <summary>
        /// Logins the specified username or email.
        /// </summary>
        /// <param name="usernameOrEmail">The username or email.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public User Login(string usernameOrEmail, string password)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var users = from u in context.Users
                            where (u.Username.Equals(usernameOrEmail) || u.Email.Equals(usernameOrEmail)) &&
                                   u.Password.Equals(password)
                            select u;
                if (!users.Any()) return null;
                return users.First();
            }
        }

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns>The id of the user.</returns>
        public User SignUp(string username, string email, string password)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var users = from u in context.Users
                            where u.Username.Equals(username)
                            select u;
                if (users.Any())
                {
                    throw new ArgumentException("Username is already taken.");
                }
                users = from u in context.Users
                        where u.Email.Equals(email)
                        select u;
                if (users.Any())
                {
                    throw new ArgumentException("Email is already in use.");
                }

                User user = new User()
                    {
                        Username = username,
                        Email = email,
                        Password = password,
                        Comments = new Collection<Comment>(),
                        Channels = new Collection<Channel>(),
                        SubscribedChannels = new Collection<Channel>(),
                        Votes = new Collection<Vote>()
                    };
                context.Users.Add(user);
                context.SaveChanges();
                return user;
            }
        }

        /// <summary>
        /// Determines whether [is correct password] [the specified user id].
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        ///   <c>true</c> if [is correct password] [the specified user id]; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentException"></exception>
        public bool IsCorrectPassword(int userId, string password)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var users = from u in context.Users
                            where u.Id == userId
                            select u;
                if (users.Any() == false)
                {
                    throw new ArgumentException(string.Format("No user with userid {0}", userId));
                }
                return users.First().Password.Equals(password);
            }
        }

        /// <summary>
        /// Subscribes the specified user id to the channel.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="channelId">The channel id.</param>
        /// <exception cref="System.ArgumentException">
        /// No user with userId [ + userId + ]
        /// or
        /// No channel with channelId [ + channelId + ]
        /// </exception>
        public void Subscribe(int userId, int channelId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var users = from user in context.Users
                            where user.Id == userId
                            select user;
                if (users.Any() == false) throw new ArgumentException("No user with userId [" + userId + "]");

                var channels = from channel in context.Channels
                               where channel.Id == channelId
                               select channel;

                if (channels.Any() == false) throw new ArgumentException("No channel with channelId [" + channelId + "]");

                User theUser = users.First();
                Channel theChannel = channels.First();
                theChannel.Subscribers.Add(theUser);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Unsubscribes the user form the channel.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="channelId">The channel id.</param>
        /// <exception cref="System.ArgumentException">
        /// No user with userId [ + userId + ]
        /// or
        /// No channel with channelId [ + channelId + ]
        /// </exception>
        public void UnSubscribe(int userId, int channelId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var users = from user in context.Users
                            where user.Id == userId
                            select user;
                if (users.Any() == false) throw new ArgumentException("No user with userId [" + userId + "]");

                var channels = from channel in context.Channels
                               where channel.Id == channelId
                               select channel;

                if (channels.Any() == false) throw new ArgumentException("No channel with channelId [" + channelId + "]");

                User theUser = users.First();
                Channel theChannel = channels.First();
                theChannel.Subscribers.Remove(theUser);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Removes the user from the database. This method also removed all votes by the user
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

                // Remove all votes from the user
                var votes = from vote in context.Votes
                            where vote.UserId == theUser.Id
                            select vote;

                foreach (Vote vote in votes)
                {
                    context.Votes.Remove(vote);
                }
                context.Users.Remove(theUser);

                context.SaveChanges();

                // Verify deletion succeeded
                users = from user in context.Users
                        where user.Id == theUser.Id
                        select user;

                if (users.Any() == true) throw new Exception("End of \"RemoveUser\". User was not get removed from the database.");
            }
        }

        /// <summary>
        /// Gets the user with specified user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>The user</returns>
        /// <exception cref="System.ArgumentException">No user with user id [ + userId + ]</exception>
        public User GetUser(int userId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var users = from user in context.Users
                            where user.Id == userId
                            select user;
                if (users.Any() == false) throw new ArgumentException("No user with user id [" + userId + "]");

                return users.First();
            }
        }

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="username">The username. Can be null.</param>
        /// <param name="password">The password. Can be null.</param>
        /// <param name="email">The email. Can be null</param>
        /// <exception cref="System.ArgumentException">No user with user id[ + userId + ]</exception>
        public void UpdateUser(int userId, string username, string password, string email)
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
                if (email != null) theUser.Email = email;
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
        public Channel CreateChannel(string channelName, int userId, string description, int[] genreIds)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                // Check that no channel with channelName already exists
                var channels = from channel in context.Channels
                               where channel.Name.ToLower().Equals(channelName.ToLower())
                               select channel;
                if (channels.Any() == true) throw new ArgumentException("Channel with channelname [" + channelName + "] already exists");

                var users = from user in context.Users
                            where user.Id == userId
                            select user;

                if (users.Any() == false) throw new ArgumentException("No user with userId [" + userId + "]");

                //Set the genres of the channel
                var genres = from genre in context.Genres
                             where genreIds.Contains(genre.Id)
                             select genre;

                // Create the channel object
                Channel theChannel = new Channel()
                {
                    Comments = new Collection<Comment>(),
                    Description = description,
                    Genres = genres.Any() ? genres.ToList() : new List<Genre>(),
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

                channels = from channel in context.Channels.Where(channel => channel.Name == channelName && channel.UserId == userId)
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
        /// <param name="channel">The channel.</param>
        /// <exception cref="System.ArgumentException">No channel with channel found</exception>
        /// <exception cref="System.Exception">End of DeleteChannel, but channel entry is still in database</exception>
        public void DeleteChannel(DatabaseWrapperObjects.Channel channel)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                int channelId = channel.Id;
                var channels = from c in context.Channels
                               where c.Id == channel.Id
                               select c;
                if (channels.Any() == false) throw new ArgumentException("No channel with channel found");
                Channel dbChannel = channels.First();
                dbChannel.Subscribers.Clear();
                dbChannel.Genres.Clear();
                context.Channels.Remove(dbChannel);
                context.SaveChanges();

                channels = from c in context.Channels
                           where c.Id == channelId
                           select c;
                if (channels.Any() == true) throw new Exception("End of DeleteChannel, but channel entry is still in database");
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
        /// <param name="streamUri">The stream URI.</param>
        /// <exception cref="System.ArgumentException">No channel with channel id [ + channelId + ]
        /// or
        /// No user with user id [ + ownerId + ]</exception>
        public void UpdateChannel(int channelId, int? ownerId, string channelName, string description, double? hits, double? rating, string streamUri, int[] genreIds)
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
                if (rating != null) theChannel.Rating = rating;
                if (streamUri != null) theChannel.StreamUri = streamUri;
                if (genreIds != null)
                {   //Set the genres of the channel
                    theChannel.Genres.Clear();
                    if (genreIds.Length > 0)
                    {
                        var genres = from genre in context.Genres
                                     where genreIds.Contains(genre.Id)
                                     select genre;
                        foreach (Genre g in genres)
                        {
                            theChannel.Genres.Add(g);
                        }
                    }
                }
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
                var channels = from c in context.Channels
                               where c.Id == channelId
                               select c;

                if (channels.Any() == false)
                {   // No channel with matching id
                    return null;
                }
                if (channels.Count() > 1)
                {
                    // Won't happen
                }
                return channels.First();
            }
        }

        /// <summary>
        /// Filters the with respect to the filter arguments:
        /// filter.Name
        /// filter.AmountPlayed
        /// filter.Genres
        /// filter.NumberOfComments
        /// filter.NumberOfSubscriptions
        /// filter.SortOptions
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// Channel ids of the channels matching the filter.
        /// </returns>
        public List<Channel> GetChannelsWithFilter(ChannelSearchArgs filter)
        {
            List<Channel> filteredChannels;
            using (RENTIT21Entities context = new RENTIT21Entities())
            {   // get all channels that starts with filter.Name

                var channels = from c in context.Channels
                               where c.Name.Contains(filter.SearchString) || c.Description.Contains(filter.SearchString)
                               select c;

                if (filter.MinAmountPlayed > -1)
                {   // Apply amount played filter
                    channels = from channel in channels where channel.Hits >= filter.MinAmountPlayed select channel;
                }
                if (filter.MaxAmountPlayed < Int32.MaxValue)
                {   // Apply amount played filter
                    channels = from channel in channels where channel.Hits <= filter.MaxAmountPlayed select channel;
                }
                if (filter.MinNumberOfComments > -1)
                {   // Apply comment filter
                    channels = from channel in channels where channel.Comments.Count >= filter.MinNumberOfComments select channel;
                }
                if (filter.MaxNumberOfComments < Int32.MaxValue)
                {   // Apply comment filter
                    channels = from channel in channels where channel.Comments.Count <= filter.MaxNumberOfComments select channel;
                }
                if (filter.MinNumberOfSubscriptions > -1)
                {   // Apply subscription filter
                    channels = from channel in channels where channel.Subscribers.Count >= filter.MinNumberOfSubscriptions select channel;
                }
                if (filter.MaxNumberOfSubscriptions < Int32.MaxValue)
                {   // Apply subscription filter
                    channels = from channel in channels where channel.Subscribers.Count <= filter.MaxNumberOfSubscriptions select channel;
                }
                if (filter.MinTotalVotes > -1)
                {   // Apply votes filter
                    IQueryable<Channel> noTracksChannels = null;
                    if (filter.MinTotalVotes == 0)
                    {
                        noTracksChannels = from channel in channels
                                           where channel.Tracks.Count == 0
                                           select channel;
                    }

                    channels = from channel in channels
                               where (from track in channel.Tracks
                                      select track.UpVotes + track.DownVotes).Sum() >= filter.MinTotalVotes
                               select channel;

                    if (filter.MinTotalVotes == 0 && noTracksChannels != null)
                    {
                        channels = channels.Concat(noTracksChannels);
                    }
                    channels = channels.Distinct();
                }
                if (filter.Genres != null && filter.Genres.Length > 0)
                {
                    channels = from channel in channels
                               where (from g in channel.Genres
                                      where channel.Genres.Contains(g)
                                      select g).Any()
                               select channel;
                }
                if (filter.MaxTotalVotes < Int32.MaxValue)
                {   // Apply votes filter
                    IQueryable<Channel> noTracksChannels = null;
                    if (filter.MinTotalVotes <= 0)
                    {
                        noTracksChannels = from channel in channels
                                           where channel.Tracks.Count == 0
                                           select channel;
                    }

                    channels = from channel in channels
                               where (from track in channel.Tracks
                                      select track.UpVotes + track.DownVotes).Sum() <= filter.MaxTotalVotes
                               select channel;

                    if (filter.MinTotalVotes <= 0 && noTracksChannels != null)
                    {
                        channels = channels.Concat(noTracksChannels);
                    }
                    channels = channels.Distinct();
                }
                if (!filter.SortOption.Equals(""))
                {   // Apply specific sort order
                    if (filter.SortOption.Equals(filter.HitsAsc))
                    {
                        channels = from channel in channels orderby channel.Hits ascending select channel;
                    }
                    else if (filter.SortOption.Equals(filter.HitsDesc))
                    {
                        channels = from channel in channels orderby channel.Hits descending select channel;
                    }
                    else if (filter.SortOption.Equals(filter.NameAsc))
                    {
                        channels = from channel in channels orderby channel.Name ascending select channel;
                    }
                    else if (filter.SortOption.Equals(filter.NameDesc))
                    {
                        channels = from channel in channels orderby channel.Name descending select channel;
                    }
                    else if (filter.SortOption.Equals(filter.NumberOfCommentsAsc))
                    {
                        channels = from channel in channels orderby channel.Comments.Count ascending select channel;
                    }
                    else if (filter.SortOption.Equals(filter.NumberOfCommentsDesc))
                    {
                        channels = from channel in channels orderby channel.Comments.Count descending select channel;
                    }
                    else if (filter.SortOption.Equals(filter.SubscriptionsAsc))
                    {
                        channels = from channel in channels orderby channel.Subscribers.Count ascending select channel;
                    }
                    else if (filter.SortOption.Equals(filter.SubscriptionsDesc))
                    {
                        channels = from channel in channels orderby channel.Subscribers.Count descending select channel;
                    }
                    else if (filter.SortOption.Equals(filter.NumberOfVotesAsc))
                    {
                        channels = from channel in channels
                                   orderby (from track in channel.Tracks
                                            select track.UpVotes + track.DownVotes).Sum() ascending
                                   select channel;
                    }
                    else if (filter.SortOption.Equals(filter.NumberOfVotesDesc))
                    {
                        channels = from channel in channels
                                   orderby (from track in channel.Tracks
                                            select track.UpVotes + track.DownVotes).Sum() descending
                                   select channel;
                    }
                }
                else
                {
                    // Apply default sort order
                    channels = from channel in channels orderby channel.Name ascending select channel;
                }
                filteredChannels = channels.Any() == false ? new List<Channel>() : channels.ToList();
            }
            if (filter.StartIndex != -1 && filter.EndIndex != -1 && filter.StartIndex <= filter.EndIndex)
            {   // Only get the channels within the specified interval [filter.startIndex, ..., filter.endIndex-1]

                // The amount of channels
                int count;

                if (filter.StartIndex < -1)
                {   // If start index is negative, start from 0
                    count = filter.EndIndex;
                    filter.StartIndex = 0;
                }
                else
                {   // If both start and endindex are positive, 
                    count = filter.EndIndex - filter.StartIndex;
                }

                if (filteredChannels.Count < filter.EndIndex) //Check if endindex is higher than the amount of channels found
                {
                    //Set endindex to the amount of channels found
                    filter.EndIndex = filteredChannels.Count;
                }
                if (filteredChannels.Count < filter.StartIndex) //Check if startindex is higher than the amount of channels found
                {
                    //Set startindex to the amount of channels found minus the amount of channels which should be retreived
                    filter.StartIndex = (filteredChannels.Count - count);
                    //Set endindex to the amount of channels found
                    filter.EndIndex = filteredChannels.Count;
                }

                //Create array to contain the result channels
                Channel[] result = new Channel[filter.EndIndex - filter.StartIndex];
                //Copy the channels to the result array
                filteredChannels.CopyTo(filter.StartIndex, result, 0, (filter.EndIndex - filter.StartIndex));
                return result.ToList();
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

                if (tracks.Any() == false) throw new ArgumentException("no track with track id [" + trackId + "]");
                Track theTrack = tracks.First();

                var users = from user in context.Users
                            where user.Id == userId
                            select user;

                if (users.Any() == false) throw new ArgumentException("no user with user id [" + userId + "]");
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

                // Add Vote to database and assign it an id
                context.Votes.Add(vote);
                context.SaveChanges();

                // Update the user with the new vote. 
                theUser.Votes.Add(vote);

                //Update the track
                if (rating < 0)
                {
                    theTrack.DownVotes += 1;
                }
                else if (rating > 0)
                {
                    theTrack.UpVotes += 1;
                }

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes all votes for a specific track.
        /// </summary>
        /// <param name="trackId">The track id</param>
        public void DeleteVotesForTrack(int trackId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var votes = from v in context.Votes
                            where v.TrackId == trackId
                            select v;
                foreach (Vote v in votes)
                {
                    // remove vote from the track
                    v.Track.Votes.Remove(v);
                    if (v.Value < 0)
                    {
                        v.Track.DownVotes -= 1;
                    }
                    else if (v.Value > 0)
                    {
                        v.Track.UpVotes -= 1;
                    }

                    //remove the vote itself
                    context.Votes.Remove(v);
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes all votes for a specific user.
        /// </summary>
        /// <param name="trackId">The user id</param>
        public void DeleteVotesForUser(int userId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var votes = from v in context.Votes
                            where v.UserId == userId
                            select v;
                foreach (Vote v in votes)
                {
                    //remove the vote from the track
                    v.Track.Votes.Remove(v);
                    if (v.Value < 0)
                    {
                        v.Track.DownVotes -= 1;
                    }
                    else if (v.Value > 0)
                    {
                        v.Track.UpVotes -= 1;
                    }
                    // remove the vote itself
                    context.Votes.Remove(v);
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Creates the track entry.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <param name="track">The track.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">No channel with channel id [ + channelId + ].</exception>
        public Track CreateTrackEntry(int channelId, Track track)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var channels = from c in context.Channels
                               where c.Id == channelId
                               select c;
                if (channels.Any() == false) throw new ArgumentException("No channel with channel id [" + channelId + "].");
                track.Channel = channels.First();
                context.Tracks.Add(track);
                context.SaveChanges();
            }

            return track;
        }

        /// <summary>
        /// Updates the track.
        /// </summary>
        /// <param name="track">The track.</param>
        /// <exception cref="System.ArgumentException">No track with id [ + track.Id + ].</exception>
        public void UpdateTrack(Track track)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var tracks = from t in context.Tracks
                             where t.Id == track.Id
                             select t;
                if (tracks.Any() == false) throw new ArgumentException("No track with id [" + track.Id + "].");

                Track tdb = tracks.First();
                tdb.Artist = track.Artist;
                tdb.DownVotes = track.DownVotes;
                tdb.Length = track.Length;
                tdb.Name = track.Name;
                tdb.Path = track.Path;
                tdb.UpVotes = track.UpVotes;

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Gets the track.
        /// </summary>
        /// <param name="trackId">The track id.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">No track with trackId [ + trackId + ]</exception>
        public Track GetTrack(int trackId)
        {
            Track theTrack;
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var tracks = from track in context.Tracks
                             where track.Id == trackId
                             select track;

                if (tracks.Any() == false) throw new ArgumentException("No track with trackId [" + trackId + "]");

                theTrack = tracks.First();
            }
            return theTrack;
        }

        /// <summary>
        /// Removes the track.
        /// </summary>
        /// <param name="track">The track.</param>
        public void DeleteTrackEntry(ITU.DatabaseWrapperObjects.Track track)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var channels = from channel in context.Channels
                               where channel.Id == track.ChannelId
                               select channel;

                var tracks = from atrack in context.Tracks
                             where atrack.Id == track.Id
                             select atrack;

                if (tracks.Any() == false) throw new ArgumentException("The track entry is not in the database");

                Track theTrack = tracks.First();

                // If the track is associated with a channel, remove it from the channel as well
                if (channels.Any() == true)
                {
                    Channel theChannel = channels.First();
                    theChannel.Tracks.Remove(theTrack);
                }

                context.Tracks.Remove(theTrack);
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
                if (users.Any() == false) throw new ArgumentException("No user with user id [" + userId + "]");
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

                // Update the channel with the comment
                theChannel.Comments.Add(theComment);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Creates a genre with the name.
        /// </summary>
        /// <param name="genreName">The name of the genre.</param>
        /// <returns>The id of the genre</returns>
        public int CreateGenre(string genreName)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var genres = from g in context.Genres
                             where g.Name == genreName
                             select g;

                if (genres.Any()) throw new ArgumentException("A genre with the name already exists");

                Genre genre = new Genre();
                genre.Name = genreName;
                context.Genres.Add(genre);
                context.SaveChanges();
                return genre.Id;
            }
        }

        /// <summary>
        /// Gets the channel genres.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">No channel with channelId [ + channelId + ]</exception>
        public IEnumerable<string> GetChannelGenres(int channelId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var channels = from channel in context.Channels select channel;
                if (channels.Any() == false)
                    throw new ArgumentException("No channel with channelId [" + channelId + "]");

                Channel theChannel = channels.First();
                List<string> genres = new List<string>();
                foreach (Genre genre in theChannel.Genres)
                {
                    genres.Add(genre.Name);
                }
                return genres;
            }
        }

        /// <summary>
        /// Deletes all comments from a specific user.
        /// </summary>
        /// <param name="userId">The id of the user</param>
        public void DeleteUserComments(int userId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var comments = from c in context.Comments
                               where c.UserId == userId
                               select c;

                foreach (Comment c in comments)
                {
                    context.Comments.Remove(c);
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Gets comments associated with a channel within the specified range. Ranges outside the size of the comment collection is interpreted as extremes.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// /// <param name="fromInclusive">The start index to retrieve comments from inclusive.</param>
        /// /// <param name="toExclusive">The end index to retieve comments from exclusive.</param>
        /// <returns>
        /// Comments from a specific channel in the specified range.
        /// </returns>
        public List<Comment> GetChannelComments(int channelId, int fromInclusive, int toExclusive)
        {
            if (fromInclusive >= toExclusive) throw new ArgumentException("fromInclusive has to be lesser than to toExclusive.");
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var comments = from comment in context.Comments
                               where comment.ChannelId == channelId
                               orderby comment.Date descending
                               select comment;
                //Set variables relevant for getting list range
                if (fromInclusive < 0) fromInclusive = 0;
                if (toExclusive > comments.Count()) toExclusive = comments.Count();
                int sizeOfRange = toExclusive - fromInclusive;
                //Gets the specified range
                List<Comment> listComments = comments.ToList().GetRange(fromInclusive, sizeOfRange);
                return listComments;
            }
        }

        /// <summary>
        /// Gets comments associated with a user within the specified range. Ranges outside the size of the comment collection is interpreted as extremes.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// /// <param name="fromInclusive">The start index to retrieve comments from inclusive.</param>
        /// /// <param name="toExclusive">The end index to retieve comments from exclusive.</param>
        /// <returns>
        /// Comments from a specific user in the specified range.
        /// </returns>
        public List<Comment> GetUserComments(int userId, int fromInclusive, int toExclusive)
        {
            if (fromInclusive >= toExclusive) throw new ArgumentException("fromInclusive has to be lesser than to toExclusive.");
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var comments = from comment in context.Comments
                               where comment.UserId == userId
                               select comment;
                //Set variables relevant for getting list range
                if (fromInclusive < 0) fromInclusive = 0;
                if (toExclusive > comments.Count()) toExclusive = comments.Count();
                int sizeOfRange = toExclusive - fromInclusive;
                //Gets the specified range
                List<Comment> listComments = comments.ToList().GetRange(fromInclusive, sizeOfRange);
                return listComments;
            }
        }

        /// <summary>
        /// Gets the track list.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <returns></returns>
        public List<Track> GetTrackList(int channelId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var tracks = from track in context.Tracks
                             where track.ChannelId == channelId
                             select track;

                // It doesn't matter if the list is empty
                return tracks.ToList();
            }
        }

        /// <summary>
        /// Gets the trackplays for a channel.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <returns>All TrackPlays associated with the channel</returns>
        /// <exception cref="System.ArgumentException">No channel with channelId [+channelId+]</exception>
        public List<TrackPlay> GetTrackPlays(int channelId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var channels = from channel in context.Channels
                               where channel.Id == channelId
                               select channel;

                if (channels.Any() == false) throw new ArgumentException("No channel with channelId [" + channelId + "]");

                Channel theChannel = channels.First();
                var tracks = theChannel.Tracks;

                List<TrackPlay> trackPlays = new List<TrackPlay>();
                foreach (Track track in tracks)
                {
                    if (track.TrackPlays.Any() == true)
                    {
                        trackPlays.AddRange(track.TrackPlays);
                    }
                }
                return trackPlays;
            }
        }

        /// <summary>
        /// Retreives trackplays for a channel which has a playtime after the given datetime
        /// </summary>
        /// <param name="channelId">Id of the channel</param>
        /// <param name="dateTime">The lower-bound datetime</param>
        /// <returns>All TrackPlays associated with the channel after the given datetime</returns>
        public List<TrackPlay> GetTrackPlays(int channelId, DateTime dateTime)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var trackPlays = from tp in context.TrackPlays
                                 where tp.TimePlayed > dateTime && tp.Track.ChannelId == channelId
                                 select tp;

                return trackPlays.ToList();
            }
        }

        /// <summary>
        /// Determines whether [is email available] [the specified email].
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>
        ///   <c>true</c> if [is email available] [the specified email]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsEmailAvailable(string email)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var users = from u in context.Users
                            where u.Email == email
                            select u;
                return !users.Any();
            }
        }

        /// <summary>
        /// Determines whether [is username available] [the specified username].
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        ///   <c>true</c> if [is username available] [the specified username]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsUsernameAvailable(string username)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var users = from u in context.Users
                            where u.Username == username
                            select u;
                return !users.Any();
            }
        }

        /// <summary>
        /// Deletes the database data.
        /// </summary>
        public void DeleteDatabaseData()
        {
            const string deleteAllDataSql = "EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL' " +
                                            "EXEC sp_MSForEachTable 'ALTER TABLE ? DISABLE TRIGGER ALL' " +
                                            "EXEC sp_MSForEachTable 'DELETE FROM ?' " +
                                            "EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL' " +
                                            "EXEC sp_MSForEachTable 'ALTER TABLE ? ENABLE TRIGGER ALL'";
            
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                context.Database.ExecuteSqlCommand(deleteAllDataSql);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Adds the track play.
        /// </summary>
        /// <param name="track">The track.</param>
        public void AddTrackPlay(Track track)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                context.TrackPlays.Add(new TrackPlay(track.Id, DateTime.UtcNow));

            }
        }

        /// <summary>
        /// Gets the created channels.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public List<Channel> GetCreatedChannels(int userId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var channels = from c in context.Channels
                               where c.ChannelOwner.Id == userId
                               select c;
                return channels.ToList();
            }
        }

        /// <summary>
        /// Gets the subscribed channels.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public List<Channel> GetSubscribedChannels(int userId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var users = from u in context.Users
                            where u.Id == userId
                            select u;
                if (!users.Any())
                    return new List<Channel>();

                var channels = from c in context.Channels
                               where c.Subscribers.Where(u => u.Id == userId).Any()
                               select c;

                return channels.ToList();
            }
        }

        /// <summary>
        /// Gets the tracks by channel id.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <returns></returns>
        public List<Track> GetTracksByChannelId(int channelId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var tracks = from t in context.Tracks
                             where t.ChannelId == channelId
                             select t;
                if (!tracks.Any())
                    return new List<Track>();
                return tracks.ToList();
            }
        }

        /// <summary>
        /// Determines whether [is channel name available] [the specified channel id].
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <param name="channelName">Name of the channel.</param>
        /// <returns>
        ///   <c>true</c> if [is channel name available] [the specified channel id]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsChannelNameAvailable(int channelId, string channelName)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var channels = from c in context.Channels
                               where c.Id != channelId && c.Name.ToLower().Equals(channelName.ToLower())
                               select c;
                return !channels.Any();
            }
        }

        /// <summary>
        /// Gets the subscriber count.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <returns></returns>
        public int GetSubscriberCount(int channelId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var channel = from c in context.Channels
                              where c.Id == channelId
                              select c;
                if (!channel.Any()) return 0;
                return channel.First().Subscribers.Count;
            }
        }

        /// <summary>
        /// Increments the channel plays.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void IncrementChannelPlays(int channelId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var channels = from c in context.Channels
                               where c.Id == channelId
                               select c;
                if (channels.Any())
                {
                    Channel channel = channels.First();
                    if (channel.Hits.HasValue)
                    {
                        channel.Hits = channel.Hits + 1;
                    }
                    else
                    {
                        channel.Hits = 1;
                    }
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Gets the channels with tracks.
        /// </summary>
        /// <returns></returns>
        public List<Channel> GetChannelsWithTracks()
        {
            List<Channel> channelList = new List<Channel>();
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var channels = from c in context.Channels
                               where c.Tracks.Count > 0
                               select c;
                if (channels.Any())
                {
                    channelList = channels.ToList();
                }
            }

            return channelList;
        }

        /// <summary>
        /// Checks if the channel has tracks.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <returns></returns>
        public bool ChannelHasTracks(int channelId)
        {
            bool result = false;
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var channels = from c in context.Channels
                               where c.Id == channelId && c.Tracks.Count > 0
                               select c;
                if (channels.Any())
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Adds the track playlist.
        /// </summary>
        /// <param name="trackPlayList">The track playlist.</param>
        public void AddTrackPlayList(List<TrackPlay> trackPlayList)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                foreach (TrackPlay tp in trackPlayList)
                {
                    context.TrackPlays.Add(tp);
                }
                context.SaveChanges();
            }
        }


        /// <summary>
        /// Gets the recently played tracks.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <param name="numberOfTracks">The number of tracks.</param>
        /// <returns></returns>
        public List<Track> GetRecentlyPlayedTracks(int channelId, int numberOfTracks)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var trackPlays = from tp in context.TrackPlays
                                 from t in context.Tracks
                                 where tp.TimePlayed < DateTime.Now
                                 where tp.TrackId == t.Id
                                 where t.ChannelId == channelId
                                 orderby tp.TimePlayed descending
                                 select tp;

                List<TrackPlay> trackPlaysList = trackPlays.ToList();
                if (trackPlaysList.Count > numberOfTracks)
                {
                    trackPlaysList = trackPlaysList.GetRange(0, numberOfTracks);
                }
                List<Track> result = new List<Track>();

                foreach (TrackPlay tp in trackPlaysList)
                {
                    result.Add(GetTrack(tp.TrackId));
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the vote.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="trackId">The track id.</param>
        /// <returns></returns>
        public Vote GetVote(int userId, int trackId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var votes = from v in context.Votes
                            where v.UserId == userId && v.TrackId == trackId
                            select v;
                if (votes.Any())
                {
                    return votes.First();
                }
                return null;
            }
        }

        /// <summary>
        /// Deletes the vote.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="trackId">The track id.</param>
        public void DeleteVote(int userId, int trackId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var votes = from v in context.Votes
                            where v.UserId == userId && v.TrackId == trackId
                            select v;
                if (votes.Any() == false) throw new ArgumentException("No vote with userId [" + userId + "] and trackId [" + trackId + "]");

                Vote vote = votes.First();

                //remove the vote from the user
                vote.User.Votes.Remove(vote);

                //remove the vote from the track
                if (vote.Value < 0)
                {
                    vote.Track.DownVotes -= 1;
                }
                else if (vote.Value > 0)
                {
                    vote.Track.UpVotes -= 1;
                }
                vote.Track.Votes.Remove(vote);

                //remove the vote itself
                context.Votes.Remove(vote);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes the track plays associated with channel id.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <param name="datetime">The datetime.</param>
        public void DeleteTrackPlaysByChannelId(int channelId, DateTime datetime)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var trackplays = from tp in context.TrackPlays
                                 where tp.Track.ChannelId == channelId && tp.TimePlayed > datetime
                                 select tp;

                foreach (TrackPlay tp in trackplays)
                {
                    context.TrackPlays.Remove(tp);
                }

                context.SaveChanges();
            }
        }

        public void DeleteOlderTrackPlays(DateTime datetime)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var trackplays = from tp in context.TrackPlays
                                 where tp.TimePlayed > datetime
                                 select tp;

                foreach (TrackPlay tp in trackplays)
                {
                    context.TrackPlays.Remove(tp);
                }

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes the trackplays associated with track id.
        /// </summary>
        /// <param name="trackId">The track id.</param>
        public void DeleteTrackPlaysByTrackId(int trackId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var trackplays = from tp in context.TrackPlays
                                 where tp.TrackId == trackId
                                 select tp;

                foreach (TrackPlay tp in trackplays)
                {
                    context.TrackPlays.Remove(tp);
                }

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes all the users votes on a given channel
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="channelId">The id of the channel</param>
        public void DeleteVotesForUser(int userId, int channelId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var votes = from v in context.Votes
                            where v.UserId == userId && v.Track.ChannelId == channelId
                            select v;
                foreach (Vote v in votes)
                {
                    context.Votes.Remove(v);
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes the track entry.
        /// </summary>
        /// <param name="trackId">The track id.</param>
        public void DeleteTrackEntry(int trackId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var track = from t in context.Tracks
                            where t.Id == trackId
                            select t;
                if (track.Any())
                {
                    context.Tracks.Remove(track.First());
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Counts the track votes.
        /// </summary>
        /// <param name="trackId">The track id.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public int CountTrackVotes(int trackId, int value)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var votes = from v in context.Votes
                            where v.TrackId == trackId && v.Value == value
                            select v;

                return votes.Count();
            }
        }

        /// <summary>
        /// Deletes all comments.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        public void DeleteAllComments(int channelId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var comments = from c in context.Comments
                               where c.ChannelId == channelId
                               select c;
                foreach (Comment c in comments)
                {
                    context.Comments.Remove(c);
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Gets all genres.
        /// </summary>
        /// <returns></returns>
        public List<Genre> GetAllGenres()
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var genres = from g in context.Genres
                             orderby g.Name ascending
                             select g;
                return genres.ToList();
            }
        }

        /// <summary>
        /// Gets the genres for channel.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <returns></returns>
        public List<Genre> GetGenresForChannel(int channelId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var genres = from c in context.Channels
                             where c.Id == channelId
                             select c.Genres;

                if (!genres.Any())
                {
                    return new List<Genre>();
                }
                else
                {
                    return genres.First().ToList();
                }
            }
        }

        /// <summary>
        /// Gets the users votes.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public List<Vote> GetUserVotes(int userId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var votes = from v in context.Votes
                            where v.UserId == userId
                            select v;
                if(votes.Any() == false)    return new List<Vote>();
                return votes.ToList();
            }
        } 
    }
}