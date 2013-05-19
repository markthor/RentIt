using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using RentItServer.ITU.Exceptions;

namespace RentItServer.ITU
{
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
        /// Gets all users in the database
        /// </summary>
        /// <returns>The users</returns>
        public List<User> GetAllUsers()
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var users = from user in context.Users
                            select user;

                return users.ToList();
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
        /// Persists the modifications to this user object in the database.
        /// </summary>
        /// <param name="theUser">The user object to persist.</param>
        /// <exception cref="System.ArgumentException">No user with user id[ + userId + ]</exception>
        public void UpdateUser(User theUser)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var users = from user in context.Users
                            where user.Id == theUser.Id
                            select user;
                if (users.Any() == false) throw new ArgumentException("No user with user id[" + theUser.Id + "]");

                User theUpdatedUser = users.First();
                theUpdatedUser.Username = theUser.Username;
                theUpdatedUser.Password = theUser.Password;
                theUpdatedUser.Channels = theUser.Channels;
                theUpdatedUser.Comments = theUser.Comments;
                theUpdatedUser.SubscribedChannels = theUser.SubscribedChannels;
                theUpdatedUser.Votes = theUser.Votes;
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
                // Check that no channel with channelName already exists
                var channels = from channel in context.Channels
                               where channel.Name.ToLower().Equals(channelName.ToLower())
                               select channel;
                if (channels.Any() == true) throw new ArgumentException("Channel with channelname [" + channelName + "] already exists");

                var users = from user in context.Users
                            where user.Id == userId
                            select user;

                if (users.Any() == false) throw new ArgumentException("No user with userId [" + userId + "]");

                var someGenres = from genre in context.Genres.Where(genre => genres.Contains(genre.Name))
                                 select genre;

                //if (someGenres.Any() == false && genres != null) throw new EmptyTableException("Genres");

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
        /// <param name="ownerId">The owner id.</param>
        /// <param name="theChannel">The channel.</param>
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
        /// <param name="comments">The comments. Can be null.</param>
        /// <param name="genres">The genres. Can be null.</param>
        /// <param name="tracks">The tracks. Can be null.</param>
        /// <exception cref="System.ArgumentException">
        /// No channel with channel id [ + channelId + ]
        /// or
        /// No user with user id [ + ownerId + ]
        /// </exception>
        public void UpdateChannel(int channelId, int? ownerId, string channelName, string description, double? hits, double? rating, string streamUri)
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

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Adds one to the hits of a channel.
        /// </summary>
        /// <param name="channelId">The id of the channel.</param>
        public void IncrementHitsForChannel(int channelId)
        {
            Channel channel = GetChannel(channelId);
            UpdateChannel(channelId, null, null, null, channel.Hits + 1, null, null);
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

                if (channels.Any() == false)
                {
                    return new List<Channel>();
                }
                return channels.ToList();
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
                //var channels = from channel in context.Channels where channel.Name.StartsWith(filter.SearchString) select channel;

                var channels = from c in context.Channels
                               where c.Name.Contains(filter.SearchString)
                               select c;

                if (filter.AmountPlayed > -1)
                {   // Apply amount played filter
                    channels = from channel in channels where channel.Hits >= filter.AmountPlayed select channel;
                }
                if (filter.Genres.Any() == true)
                {   // Apply genre filter
                    channels = from channel in channels where channel.Genres.Any(genre => filter.Genres.Contains(genre.Name)) select channel;
                }
                if (filter.NumberOfComments > -1)
                {   // Apply comment filter
                    channels = from channel in channels where channel.Comments.Count >= filter.NumberOfComments select channel;
                }
                if (filter.NumberOfSubscriptions > -1)
                {   // Apply subscription filter
                    channels = from channel in channels where channel.Subscribers.Count >= filter.NumberOfSubscriptions select channel;
                }
                if (filter.Rating > -1)
                {   // Apply rating filter
                    channels = from channel in channels where channel.Rating >= filter.Rating select channel;
                }
                if (filter.SortOption.Equals(""))
                {   // Apply default sort order
                    channels = from channel in channels orderby channel.Name select channel;
                }
                else
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
                    else if (filter.SortOption.Equals(filter.RatingAsc))
                    {
                        channels = from channel in channels orderby channel.Rating ascending select channel;
                    }
                    else if (filter.SortOption.Equals(filter.RatingDesc))
                    {
                        channels = from channel in channels orderby channel.Rating descending select channel;
                    }
                    else if (filter.SortOption.Equals(filter.SubscriptionsAsc))
                    {
                        channels = from channel in channels orderby channel.Subscribers.Count ascending select channel;
                    }
                    else if (filter.SortOption.Equals(filter.SubscriptionsDesc))
                    {
                        channels = from channel in channels orderby channel.Subscribers.Count descending select channel;
                    }
                }
                filteredChannels = channels.Any() == false ? new List<Channel>() : channels.ToList();
            }
            if (filter.StartIndex != -1 && filter.EndIndex != -1 && filter.StartIndex <= filter.EndIndex)
            {   // Only get the channels within the specified interval [filter.startIndex, ..., filter.endIndex-1]
                Channel[] range = new Channel[filter.EndIndex - filter.StartIndex];
                if (filter.StartIndex < 0)
                {   // Avoid OutOfBoundsException
                    filter.StartIndex = 0;
                }
                if (filter.EndIndex < filteredChannels.Count)
                {
                    filteredChannels.CopyTo(filter.StartIndex, range, filter.StartIndex, filter.EndIndex);
                }
                else
                {
                    filteredChannels.CopyTo(filter.StartIndex, range, filter.StartIndex, filteredChannels.Count - filter.StartIndex);
                }
                filteredChannels = new List<Channel>(range);
            }
            filteredChannels = filteredChannels.Where(channel => channel != null).ToList();
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
                    context.Votes.Remove(v);
                }
                context.SaveChanges();
            }
        }

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
        /// Gets the track associated with a channel.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <param name="trackname">The trackname.</param>
        /// <returns>
        /// The track, or null if no track with trackname exists in channel
        /// </returns>
        /// <exception cref="System.ArgumentException">No channel with channelId [+channelId+]</exception>
        public Track GetTrack(int channelId, string trackname)
        {
            Track theTrack;
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var channels = from channel in context.Channels
                               where channel.Id == channelId
                               select channel;

                if (channels.Any() == false) throw new ArgumentException("No channel with channelId [" + channelId + "]");

                Channel theChannel = channels.First();

                theTrack = theChannel.Tracks.SingleOrDefault(t => t.Name.Equals(trackname));
            }
            return theTrack;
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

        public List<Track> GetTracksWithFilter(int channelId, TrackSearchArgs filter)
        {
            List<Track> filteredTracks;
            using (RENTIT21Entities context = new RENTIT21Entities())
            {   // get all tracks that starts with filter.Name
                var tracks = from track in context.Tracks where track.Name.StartsWith(filter.SearchString) select track;

                if (string.IsNullOrEmpty(filter.Artist) == false)
                {   // Apply artist filter
                    tracks = from track in tracks where track.Artist.StartsWith(filter.Artist) select track;
                }
                if (string.IsNullOrEmpty(filter.SearchString) == false)
                {   // Apply name filter
                    tracks = from track in tracks where track.Name.StartsWith(filter.SearchString) select track;
                }
                if (filter.Downvotes > -1)
                {   // Apply downvotes filter
                    tracks = from track in tracks where track.DownVotes >= filter.Downvotes select track;
                }
                if (filter.Upvotes > -1)
                {   // Apply upvotes filter
                    tracks = from track in tracks where track.UpVotes >= filter.Upvotes select track;
                }
                if (filter.SortOption.Equals(""))
                {   // Apply default sort order
                    tracks = from track in tracks orderby track.Name select track;
                }
                else
                {   // Apply specific sort order
                    if (filter.SortOption.Equals(filter.ArtistAsc))
                    {
                        tracks = from track in tracks orderby track.Artist ascending select track;
                    }
                    else if (filter.SortOption.Equals(filter.ArtistDesc))
                    {
                        tracks = from track in tracks orderby track.Artist descending select track;
                    }
                    else if (filter.SortOption.Equals(filter.DownvotesAsc))
                    {
                        tracks = from track in tracks orderby track.DownVotes ascending select track;
                    }
                    else if (filter.SortOption.Equals(filter.DownvotesDesc))
                    {
                        tracks = from track in tracks orderby track.DownVotes descending select track;
                    }
                    else if (filter.SortOption.Equals(filter.NameAsc))
                    {
                        tracks = from track in tracks orderby track.Name ascending select track;
                    }
                    else if (filter.SortOption.Equals(filter.NameDesc))
                    {
                        tracks = from track in tracks orderby track.Name descending select track;
                    }
                    else if (filter.SortOption.Equals(filter.UpvotesAsc))
                    {
                        tracks = from track in tracks orderby track.UpVotes ascending select track;
                    }
                    else if (filter.SortOption.Equals(filter.UpvotesDesc))
                    {
                        tracks = from track in tracks orderby track.UpVotes descending select track;
                    }
                }

                // Execute the query before leaving "using" block
                filteredTracks = tracks.ToList();
            }

            if (filter.StartIndex != -1 && filter.EndIndex != -1 && filter.StartIndex <= filter.EndIndex)
            {   // Only get the tracks within the specified interval [filter.startIndex, ..., filter.endIndex]
                Track[] range = new Track[filter.EndIndex - filter.StartIndex];
                filteredTracks.CopyTo(filter.StartIndex, range, 0, filter.EndIndex - filter.StartIndex);
                filteredTracks = new List<Track>(range);
            }
            return filteredTracks;

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
        public void CreateGenre(string genreName)
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
            }
        }

        /// <summary>
        /// Deletes the comment.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="date">The date of the comment.</param>
        /// <exception cref="System.ArgumentException">No comment with channelId [+channelId+] and userId [+userId+] and date [+date+]</exception>
        public void DeleteComment(int channelId, int userId, DateTime date)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var comments = from comment in context.Comments
                               where comment.ChannelId == channelId && comment.UserId == userId && comment.Date == date
                               select comment;
                if (comments.Any() == false) throw new ArgumentException("No comment with channelId [" + channelId + "] and userId [" + userId + "] and date [" + date + "]");

                var channels = from channel in context.Channels
                               where channel.Id == channelId
                               select channel;

                if (channels.Any() == false) throw new ArgumentException("No channel with channelId [" + channelId + "]");

                var users = from user in context.Users
                            where user.Id == userId
                            select user;

                if (users.Any() == false) throw new ArgumentException("No user with userId [" + userId + "]");

                Comment theComment = comments.First();
                context.Comments.Remove(theComment);
                Channel theChannel = channels.First();
                theChannel.Comments.Remove(theComment);
                User theUser = users.First();
                theUser.Comments.Remove(theComment);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Gets a specific comment.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="date">The date of the comment.</param>
        /// <returns>The comment</returns>
        public Comment GetComment(int channelId, int userId, DateTime date)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var comments = from comment in context.Comments
                               where comment.ChannelId == channelId && comment.UserId == userId && comment.Date == date
                               select comment;
                if (comments.Any() == false) throw new ArgumentException("No comment with channelId [" + channelId + "] and userId [" + userId + "] and date [" + date + "]");

                return comments.First();
            }
        }

        /// <summary>
        /// Gets the comments from a specific user in a specific channel.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>All comments from a channel made by a specific user</returns>
        public List<Comment> GetComments(int channelId, int userId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var comments = from comment in context.Comments
                               where comment.ChannelId == channelId && comment.UserId == userId
                               select comment;

                // It doesn't matter if there are no comments
                return comments.ToList();
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
        public List<DatabaseWrapperObjects.Comment> GetChannelComments(int channelId, int fromInclusive, int toExclusive)
        {
            if (fromInclusive >= toExclusive) throw new ArgumentException("fromInclusive has to be lesser than to toExclusive.");
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var comments = from comment in context.Comments
                               where comment.ChannelId == channelId
                               select comment;
                //Set variables relevant for getting list range
                if (fromInclusive < 0) fromInclusive = 0;
                if (toExclusive > comments.Count()) toExclusive = comments.Count();
                int sizeOfRange = toExclusive - fromInclusive;
                //Gets the specified range
                List<Comment> listComments = comments.ToList().GetRange(fromInclusive, sizeOfRange);
                //Converts from entity object to wrapper object
                List<DatabaseWrapperObjects.Comment> result = new List<DatabaseWrapperObjects.Comment>(listComments.Count);
                foreach (Comment c in listComments)
                {
                    result.Add(c.GetComment());
                }
                return result;
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
        public List<DatabaseWrapperObjects.Comment> GetUserComments(int userId, int fromInclusive, int toExclusive)
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
                //Converts from entity object to wrapper object
                List<DatabaseWrapperObjects.Comment> result = new List<DatabaseWrapperObjects.Comment>(listComments.Count);
                foreach (Comment c in listComments)
                {
                    result.Add(c.GetComment());
                }
                return result;
            }
        }

        internal List<Track> GetTrackList(int channelId)
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

        public void DeleteDatabaseData()
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                //Delete all users
                var users = context.Users;
                foreach (User u in users)
                {
                    context.Users.Remove(u);
                }

                //Delete all channels
                var channels = context.Channels;
                foreach (Channel c in channels)
                {
                    c.Subscribers.Clear();
                    c.Genres.Clear();
                    context.Channels.Remove(c);
                }

                //Delete all genres
                var genres = context.Genres;
                foreach (Genre g in genres)
                {
                    context.Genres.Remove(g);
                }

                //Delete all tracks
                var tracks = context.Tracks;
                foreach (Track t in tracks)
                {
                    context.Tracks.Remove(t);
                }

                //Delete all trackPlays
                var trackPlays = context.TrackPlays;
                if (trackPlays.Any())
                {
                    foreach (TrackPlay tp in trackPlays)
                    {
                        context.TrackPlays.Remove(tp);
                    }
                }
                //Delete all comments
                var comments = context.Comments;
                foreach (Comment c in comments)
                {
                    context.Comments.Remove(c);
                }

                //Delete all votes
                var votes = context.Votes;
                foreach (Vote v in votes)
                {
                    context.Votes.Remove(v);
                }

                context.SaveChanges();
            }
        }

        public void AddTrackPlay(Track track)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                context.TrackPlays.Add(new TrackPlay(track.Id, DateTime.UtcNow));

            }
        }

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

        public List<Channel> GetSubscribedChannels(int userId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                User user;
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
                trackPlaysList = trackPlaysList.GetRange(0, numberOfTracks);
                List<Track> result = new List<Track>();

                foreach(TrackPlay tp in trackPlaysList)
                {
                    result.Add(GetTrack(tp.TrackId));
                }

                return result;
            }
        }

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

        public void DeleteVote(int userId, int trackId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var votes = from v in context.Votes
                            where v.UserId == userId && v.TrackId == trackId
                            select v;
                if (votes.Any())
                {
                    context.Votes.Remove(votes.First());
                }
                context.SaveChanges();
            }
        }

        public void DeleteTrackPlays(int channelId, DateTime datetime)
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
    }
}