﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using RentItServer.ITU.Search;
using RentItServer.Utilities;
using TagLib;

namespace RentItServer.ITU
{
    /// <summary>
    /// Reviewed by Toke
    /// </summary>
    public class Controller
    {
        private static readonly string LogFileName = "ItuLogs.txt";
        //Singleton instance of the class
        private static Controller _instance;
        //Data access object for database IO
        private readonly DatabaseDao _dao = DatabaseDao.GetInstance();
        //Responsible for choosing the next trackStream
        private readonly TrackPrioritizer _trackPrioritizer = TrackPrioritizer.GetInstance();
        //Data access object for file system IO
        private readonly FileSystemDao _fileSystemHandler = FileSystemDao.GetInstance();
        //Event cast when log must make an _handler
        private static EventHandler _handler;
        //The logger
        private readonly Logger _logger;
        //The channel organizer
        private readonly ChannelOrganizer _channelOrganizer;
        // The dictionary for channel, mapping the id to the object. This is to ease database load as the "GetChannel(int channelId)" will be used very frequently.
        private readonly Dictionary<int, Channel> _channelCache;
        //The ternary search trie for users. Each username or email has an associated password as value
        private TernarySearchTrie<User> _userCache;

        private int tempCounter;
        private readonly object _dbLock = new object();

        /// <summary>
        /// Private to ensure local instantiation.
        /// </summary>
        private Controller()
        {
            _channelCache = new Dictionary<int, Channel>();
            _userCache = new TernarySearchTrie<User>();
            // Initialize channel search trie
            IEnumerable<Channel> allChannels = _dao.GetAllChannels();
            foreach (Channel channel in allChannels)
            {
                _channelCache[channel.Id] = channel;
            }

            // Initialize user search tries
            IEnumerable<User> allUsers = _dao.GetAllUsers();
            foreach (User user in allUsers)
            {
                _userCache.Put(user.Email, user);
                _userCache.Put(user.Email, user);
            }

            // Initialize the logger
            _logger = new Logger(FilePath.ITULogPath.GetPath() + LogFileName, ref _handler);

            // Initialize the channel organizer
            _channelOrganizer = ChannelOrganizer.GetInstance();
        }

        /// <summary>
        /// Accessor method to access the only instance of the class
        /// </summary>
        /// <returns>The singleton instance of the class</returns>
        public static Controller GetInstance()
        {
            return _instance ?? (_instance = new Controller());
        }

        /// <summary>
        /// Login the specified user.
        /// </summary>
        /// <param name="usernameOrEmail">The username or email of the user.</param>
        /// <param name="password">The password for the user.</param>
        /// <returns>The id of the user, or -1 if the (username,password) is not found.</returns>
        public User Login(string usernameOrEmail, string password)
        {
            User theUser = _userCache.Get(usernameOrEmail);
            if (theUser == null)
            {
                lock (_dbLock)
                {
                    theUser = _dao.Login(usernameOrEmail, password);
                }
            }
            return theUser;
        }

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password for the user.</param>
        /// <param name="email">The email associated with user.</param>
        /// <returns>The id of the created user.</returns>
        public User SignUp(string username, string email, string password)
        {
            if (username == null) LogAndThrowException(new ArgumentNullException("username"), "CreateUser");
            if (username.Equals("")) LogAndThrowException(new ArgumentException("username was empty"), "CreateUser");
            if (password == null) LogAndThrowException(new ArgumentException("password"), "CreateUser");
            if (password.Equals("")) LogAndThrowException(new ArgumentException("password was empty"), "CreateUser");
            if (email == null) LogAndThrowException(new ArgumentNullException("email"), "CreateUser");
            if (email.Equals("")) LogAndThrowException(new ArgumentException("email was empty"), "CreateUser");
            // TODO use regex to better check mail validity

            try
            {
                lock (_dbLock)
                {
                    User theUser = _dao.SignUp(username, password, email);
                    _userCache.Put(theUser.Username, theUser);
                    //_logger.AddEntry("User created with username [" + username + "] and e-mail [" + email + "].");
                }
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("User creation failed with exception [" + e + "]."));
                throw;
            }
            return null;
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        public void DeleteUser(int userId)
        {
            try
            {
                lock (_dbLock)
                {
                    User theUser = _dao.GetUser(userId);
                    _dao.DeleteUser(userId);
                    _userCache.Put(theUser.Username, null);
                    _userCache.Put(theUser.Email, null);
                }
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("User deletion failed with exception [" + e + "]."));
                throw;
            }
        }

        public User GetUser(int userId)
        {
            try
            {
                return _dao.GetUser(userId);
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("Get user failed with exception [" + e + "]."));
                throw;
            }
        }

        public bool IsCorrectPassword(int userId, string password)
        {
            return _dao.IsCorrectPassword(userId, password);
        }

        public IEnumerable<int> GetAllUserIds()
        {
            try
            {
                List<int> userIds = new List<int>();
                var allUsers = _dao.GetAllUsers();
                foreach (User user in allUsers)
                {
                    userIds.Add(user.Id);
                }
                return userIds;
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("GetAllUserIds failed with exception [" + e + "]."));
                throw;
            }
        }

        public void UpdateUser(int userId, string username, string password, string email)
        {
            try
            {
                User theUser = _dao.GetUser(userId);
                _dao.UpdateUser(userId, username, password);
                if(theUser.Username != null) _userCache.Put(theUser.Username, null);
                if(theUser.Email != null) _userCache.Put(theUser.Email, null);
                User theUpdatedUser = _dao.GetUser(userId);
                if(username != null) _userCache.Put(username, theUpdatedUser);
                if(email != null) _userCache.Put(email, theUpdatedUser);
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("GetAllUserIds failed with exception [" + e + "]."));
                throw;
            }
        }

        /// <summary>
        /// Creates a channel.
        /// </summary>
        /// <param name="channelName">Name of the channel.</param>
        /// <param name="userId">The id of the user creating the channel.</param>
        /// <param name="description">The description of the channel.</param>
        /// <param name="genres">The genres associated with the channel.</param>
        /// <returns>The id of the created channel. -1 if the channel creation failed.</returns>
        public int CreateChannel(string channelName, int userId, string description, IEnumerable<string> genres)
        {
            if (channelName == null) LogAndThrowException(new ArgumentNullException("channelName"), "CreateChannel");
            if (channelName.Equals("")) LogAndThrowException(new ArgumentException("channelName was empty"), "CreateChannel");
            if (userId < 0) LogAndThrowException(new ArgumentException("userId was below 0"), "CreateChannel");
            if (description == null) LogAndThrowException(new ArgumentException("description"), "CreateChannel");
            //if (genres == null) LogAndThrowException(new ArgumentNullException("genres"), "CreateChannel");

            Channel channel;
            string logEntry = "User id [" + userId + "] want to create the channel [" + channelName + "] with description [" + description + "] and genres [" + genres + "]. ";
            try
            {
                lock (_dbLock)
                {
                    channel = _dao.CreateChannel(channelName, userId, description, genres);
                    _channelCache[channel.Id] = channel;
                    //_logger.AddEntry(logEntry + "Channel creation succeeded.");
                }
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs(logEntry + "Channel creation failed with exception [" + e + "]."));
                throw;
            }
            return channel.Id;
        }

        /// <summary>
        /// Deletes the channel.
        /// </summary>
        /// <param name="userId">The user id making the request, this must correspond to the channel owners id.</param>
        /// <param name="channelId">The channel id.</param>
        public void DeleteChannel(int userId, int channelId)
        {
            if (userId < 0) LogAndThrowException(new ArgumentException("userId is below 0"), "DeleteChannel");
            if (channelId < 0) LogAndThrowException(new ArgumentException("channelId was below 0"), "DeleteChannel");

            try
            {
                lock (_dbLock)
                {
                    Channel channel = _dao.GetChannel(channelId);

                    string logEntry = "User id [" + userId + "] want to delete the channel [" + channel.Name + "]. ";
                    if (channel.UserId == userId)
                    {
                        _dao.DeleteChannel(userId, channel);
                        _channelCache[channelId] = null;
                        //_logger.AddEntry(logEntry + "Deletion successful.");
                    }
                    else
                    {
                        if (_handler != null)
                            _handler(this, new RentItEventArgs(logEntry + "Deletion failed. Request comes from a user other than channel owner."));
                    }
                }
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("Channel deletion failed with exception [" + e + "]."));
                throw;
            }
        }

        public void UpdateChannel(int channelId, int? ownerId, string channelName, string description, double? hits,
                                  double? rating)
        {
            try
            {
                _dao.UpdateChannel(channelId, ownerId, channelName, description, hits, rating);
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("Update channel failed with exception [" + e + "]."));
                throw;
            }
        }

        /// <summary>
        /// Gets a channel.
        /// </summary>
        /// <param name="channelId">The channel id for the channel to get.</param>
        /// <returns>The channel matching the given id.</returns>
        public Channel GetChannel(int channelId)
        {
            if (channelId < 0) LogAndThrowException(new ArgumentException("channelId was below 0"), "GetChannel");

            if (_channelCache[channelId] != null)
            {   // Attempt to use cache first
                return _channelCache[channelId];
            }

            // cache might be outdated, query the database to be sure.
            Channel channel = _dao.GetChannel(channelId);
            if (channel != null)
            {
                // channel was found in the database, adding to cache
                _channelCache[channelId] = channel;
            }
            else
            {   // A channel with id = channelId does not exist in cache or in database nigga
                LogAndThrowException(new ArgumentException("No channel with channelId = " + channelId + " exist."), "GetChannel");
            }

            return channel;
        }

        /// <summary>
        /// Gets the channel ids matching the given search arguments.
        /// </summary>
        /// <param name="args">The search arguments (used for filtering).</param>
        /// <returns>An array of channel ids matching search criteria. If there are no matches, will return an empty array. </returns>
        public IEnumerable<int> GetAllChannelIds()
        {
            try
            {
                List<int> allChannelIds = new List<int>();
                IEnumerable<Channel> channels = _dao.GetAllChannels();
                foreach (Channel channel in channels)
                {
                    allChannelIds.Add(channel.Id);
                }
                return allChannelIds;
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("GetChannelIds failed with exception [" + e + "]."));
                throw;
            }
        }

        public IEnumerable<Channel> GetChannels(ChannelSearchArgs args)
        {
            try
            {
                return _dao.GetChannelsWithFilter(args);
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("GetChannels failed with exception [" + e + "]."));
                throw;
            }
        }

        public void CreateVote(int rating, int userId, int trackId)
        {
            try
            {
                _dao.CreateVote(rating, userId, trackId);
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("CreateVote failed with exception [" + e + "]."));
                throw;
            }
        }

        public void AddTrack(int userId, int channelId, MemoryStream audioStream, Track trackInfo)
        {
            // TODO possibly better with MemoryStream instead of Track
        }

        public DatabaseWrapperObjects.Track GetTrackInfo(MemoryStream audioStream)
        {
            DatabaseWrapperObjects.Track theTrack = new DatabaseWrapperObjects.Track();
            theTrack.Artist = "";
            try
            {
                int counter = tempCounter++;
                _fileSystemHandler.WriteFile(FilePath.ITUTempPath, FileName.GenerateAudioFileName(counter), audioStream);
                // Use external library
                TagLib.File audioFile =
                    TagLib.File.Create(FilePath.ITUTempPath + FileName.GenerateAudioFileName(counter));
                string[] artists = audioFile.Tag.AlbumArtists;
                foreach (string artist in artists)
                {
                    theTrack.Artist += artist + ", ";
                }
                theTrack.Artist = theTrack.Artist.Substring(0, theTrack.Artist.Count() - 3);
                theTrack.DownVotes = 0;
                theTrack.UpVotes = 0;
                theTrack.Name = audioFile.Tag.Title;
                theTrack.TrackPlays = new List<DatabaseWrapperObjects.TrackPlay>();
                theTrack.Votes = new List<DatabaseWrapperObjects.Vote>();
                theTrack.Length = audioFile.Properties.Duration.Milliseconds;
                try
                {
                    _fileSystemHandler.DeleteFile(FilePath.ITUTempPath + FileName.GenerateAudioFileName(counter));
                }
                catch
                {
                    // It doesn't matter much
                }
                return theTrack;
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("GetTrackInfo failed with exception [" + e + "]."));
                throw;
            }
        }

        public Track GetTrackInfo(int channelId, string trackname)
        {
            try
            {
                return _dao.GetTrack(channelId, trackname);
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("GetTrackInfoByTrackname deletion failed with exception [" + e + "]."));
                throw;
            }
        }

        public void RemoveTrack(int userId, int trackId)
        {
            if (userId < 0) LogAndThrowException(new ArgumentException("userId is below 0"), "RemoveTrack");
            if (trackId < 0) LogAndThrowException(new ArgumentException("trackId was below 0"), "RemoveTrack");

            try
            {
                Track track = _dao.GetTrack(trackId);
                string logEntry = "User id [" + userId + "] want to delete the track [" + track.Name + "]. ";

                _dao.DeleteTrackEntry(track);
                //_logger.AddEntry(logEntry + "Deletion successful.");
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("Track deletion failed with exception [" + e + "]."));
                throw;
            }
        }

        public IEnumerable<int> GetTrackIds(int channelId)
        {
            try
            {
                List<int> theTrackIds = new List<int>();
                IEnumerable<Track> theTracks = _dao.GetTrackList(channelId);
                foreach (Track track in theTracks)
                {
                    theTrackIds.Add(track.Id);
                }
                return theTrackIds;
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("GetTrackIds failed with exception [" + e + "]."));
                throw;
            }
        }

        public IEnumerable<Track> GetTracks(int channelId, TrackSearchArgs args)
        {
            try
            {
                return _dao.GetTracksWithFilter(channelId, args);
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("GetTracks failed with exception [" + e + "]."));
                throw;
            }
        }

        /// <summary>
        /// Comments on the specified channel.
        /// </summary>
        /// <param name="comment">The comment.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="channelId">The channel id.</param>
        public void CreateComment(string comment, int userId, int channelId)
        {
            lock (_dbLock)
            {
                _dao.CreateComment(comment, userId, channelId);
                //_logger.AddEntry("User id [" + userId + "] commented on the channel [" + channelId + "] with the comment [" + comment + "].");
            }
            if (_handler != null)
                _handler(this, new RentItEventArgs("User id [" + userId + "] commented on the channel [" + channelId + "] with the comment [" + comment + "]."));
        }

        public void DeleteComment(int userId, int channelId, DateTime date)
        {
            try
            {
                _dao.DeleteComment(channelId, userId, date);
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("DeleteComment failed with exception [" + e + "]."));
                throw;
            }
        }

        public Comment GetComment(int channelId, int userId, DateTime date)
        {
            throw new NotImplementedException();
        }

        public Comment[] GetComments(int? channelId, int? userId, int fromInclusive, int toExclusive)
        {
            throw new NotImplementedException();
        }

        public Comment GetComment(int commentId)
        {
            return new Comment();
        }

        public void Subscribe(int userId, int channelId)
        {
            try
            {
                lock (_dbLock)
                {
                    _dao.Subscribe(userId, channelId);
                }
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("Subscribe failed with exception [" + e + "]."));
                throw;
            }
        }

        public void UnSubscribe(int userId, int channelId)
        {
            try
            {
                lock (_dbLock)
                {
                    _dao.UnSubscribe(userId, channelId);
                }
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("UnSubscribe failed with exception [" + e + "]."));
                throw;
            }
        }

        /// <summary>
        /// Logs and throws the exception e.
        /// </summary>
        /// <param name="e">The exception to throw.</param>
        /// <param name="operationName">Name of the operation.</param>
        private void LogAndThrowException(Exception e, String operationName)
        {
            //_logger.AddEntry("[" + e + "] raised in [" + operationName + "] with message [" + e.Message + "].");
            if (_handler != null)
                _handler(this, new RentItEventArgs("[" + e + "] raised in [" + operationName + "] with message [" + e.Message + "]."));
            throw e;
        }

        public int ListenToChannel(int channelId)
        {
            _channelOrganizer.StartChannel(channelId);
            return _channelOrganizer.GetChannelPortNumber(channelId);
        }

        public bool IsEmailAvailable(string email)
        {
            return _dao.IsEmailAvailable(email);
        }

        public bool IsUsernameAvailable(string username)
        {
            return _dao.IsUsernameAvailable(username);
        }
    }
}