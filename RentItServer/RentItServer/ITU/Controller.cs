using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using RentItServer.ITU.Exceptions;
using RentItServer.ITU.Search;
using RentItServer.Utilities;

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
        // The dictionary for channel, mapping the id to the object. This is to ease database load as the "GetChannel(int channelId)" will be used very frequently.
        private readonly Dictionary<int, Channel> _channelCache;
        //The streamhandler
        private readonly StreamHandler _streamHandler;
        //The ternary search trie for users. Each username or email has an associated password as value
        private TernarySearchTrie<User> _userCache;
        //The url properties of the stream
        public static int _defaultPort = 27000;
        public static string _defaultUri = "http://rentit.itu.dk";
        public static string _defaultStreamExtension = "";
        public static string _defaultUrl = _defaultUri + ":" + _defaultPort + "/";
        
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
            //_logger = new Logger(FilePath.ITULogPath + LogFileName);

            //Initialize the streamhandler
            _streamHandler = StreamHandler.GetInstance();
            _streamHandler.AddLogger(_logger);
            _streamHandler.InitTimer();
        }

        /// <summary>
        /// Accessor method to access the only instance of the class
        /// </summary>
        /// <returns>The singleton instance of the class</returns>
        public static Controller GetInstance()
        {
            return _instance ?? (_instance = new Controller());
        }


        public void StartChannelStream(int channelId)
        {
            _streamHandler.ManualStreamStart(channelId);
        }

        /*public void StopChannelStream(int channelId)
        {
            _streamHandler.StopStream(channelId);
        }*/

        /// <summary>
        /// Login the specified user.
        /// </summary>
        /// <param name="usernameOrEmail">The username or email of the user.</param>
        /// <param name="password">The password for the user.</param>
        /// <returns>The id of the user, or -1 if the (username,password) is not found.</returns>
        public DatabaseWrapperObjects.User Login(string usernameOrEmail, string password)
        {
            if(usernameOrEmail == null) LogAndThrowException(new ArgumentNullException("usernameOrEmail"), "Login");
            if(usernameOrEmail.Equals("")) LogAndThrowException(new ArgumentException("usernameOrEmail was empty"), "Login");
            if(password == null)    LogAndThrowException(new ArgumentNullException("password"), "Login");
            if(password.Equals("")) LogAndThrowException(new ArgumentException("password was empty"), "Login");

            User user = null;
            try
            {
                /*try
                {
                    user = _userCache.Get(usernameOrEmail).GetUser();
                }
                catch (NullValueException)
                {
                    lock (_dbLock)
                    {
                        user = _dao.Login(usernameOrEmail, password);
                    }
                }*/
                user = _dao.Login(usernameOrEmail, password);
                _logger.AddEntry("Login succeeded. Local variables: usernameOrEmail = " + usernameOrEmail + ", password = " + password);
                return user.GetUser();
            }
            catch (Exception e)
            {
                _logger.AddEntry("Login failed with exception [" + e + "]. Local variables: usernameOrEmail = " + usernameOrEmail + ", password = " + password + ", user = " + user);
                throw;
            }
        }


        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password for the user.</param>
        /// <param name="email">The email associated with user.</param>
        /// <returns>The id of the created user.</returns>
        public DatabaseWrapperObjects.User SignUp(string username, string email, string password)
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
                User user = null;
                lock (_dbLock)
                {
                    user = _dao.SignUp(username, email, password);
                    _userCache.Put(user.Username, user);
                    _userCache.Put(user.Email, user);
                    _logger.AddEntry("User created with username [" + username + "] and e-mail [" + email + "].");
                }
                return user.GetUser();
            }
            catch (Exception e)
            {
                //if (_handler != null)
                //    _handler(this, new RentItEventArgs("User creation failed with exception [" + e + "]."));
                _logger.AddEntry(string.Format("User creation failed with exception [{0}]. Local variables: username = {1}, email = {2}, password = {3}", e, username, email, password));
                throw;
            }
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        public void DeleteUser(int userId)
        {
            User user = null;
            List<Channel> userCreatedChannels = GetCreatedChannels(userId);
            foreach (Channel c in userCreatedChannels)
            {
                DeleteChannel(c.Id);
            }
            try
            {
                lock (_dbLock)
                {
                    user = _dao.GetUser(userId);
                    _dao.DeleteUser(userId);
                    _dao.DeleteVotesForUser(userId);
                    _userCache.Put(user.Username, null);
                    _userCache.Put(user.Email, null);
                    _logger.AddEntry(string.Format("User successfully deleted. Local variables: userId = {0}, theUser = {1}", userId, user));
                }
            }
            catch (Exception e)
            {
                //if (_handler != null)
                //    _handler(this, new RentItEventArgs("User deletion failed with exception [" + e + "]."));
                _logger.AddEntry(
                    string.Format(
                        "User deletion failed with exception [{0}]. Local variables: userId = {1}, theUser = {2}.", e,
                        userId, user));
                throw;
            }
        }

        public DatabaseWrapperObjects.User GetUser(int userId)
        {
            try
            {
                return _dao.GetUser(userId).GetUser();
            }
            catch (Exception e)
            {
                //if (_handler != null)
                //    _handler(this, new RentItEventArgs("Get user failed with exception [" + e + "]."));
                _logger.AddEntry(string.Format("GetUser failed with exception [{0}]. Local variables: userId = {1}.", e, userId));
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
                _logger.AddEntry("GetAllUserIds succeeded.");
                return userIds;
            }
            catch (Exception e)
            {
                //if (_handler != null)
                //    _handler(this, new RentItEventArgs("GetAllUserIds failed with exception [" + e + "]."));
                _logger.AddEntry(string.Format("GetAllUserIds failed with exception [{0}].", e));
                throw;
            }
        }

        public void UpdateUser(int userId, string username, string password, string email)
        {
            User user = null;
            User updatedUser = null;
            try
            {
                user = _dao.GetUser(userId);
                _dao.UpdateUser(userId, username, password, email);
                if (user.Username != null) _userCache.Put(user.Username, null);
                if (user.Email != null) _userCache.Put(user.Email, null);
                updatedUser = _dao.GetUser(userId);
                if (username != null) _userCache.Put(username, updatedUser);
                if (email != null) _userCache.Put(email, updatedUser);
            }
            catch (Exception e)
            {
                //if (_handler != null)
                //    _handler(this, new RentItEventArgs("UpdateUser failed with exception [" + e + "]."));
                _logger.AddEntry(string.Format("UpdateUser failed with exception [{0}]. Local variables: userId = {1}, username = {2}, password = {3}, email = {4}, user = {5}, updatedUser = {6}", userId, username, password, email, user, updatedUser));
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

            Channel channel = null;
            string logEntry = "User id [" + userId + "] want to create the channel [" + channelName + "] with description [" + description + "] and genres [" + genres + "]. ";
            try
            {
                lock (_dbLock)
                {
                    channel = _dao.CreateChannel(channelName, userId, description, genres);
                    _dao.UpdateChannel(channel.Id, null, null, null, null, null, _defaultUrl + channel.Id);
                    _channelCache[channel.Id] = channel;
                    _logger.AddEntry(logEntry + "Channel creation succeeded.");
                }
            }
            catch (Exception e)
            {
                //if (_handler != null)
                //    _handler(this, new RentItEventArgs(logEntry + "Channel creation failed with exception [" + e + "]."));
                _logger.AddEntry("ChannelCreation failed with exception [{0}]. logEntry = " + logEntry + ". Local variable: channel = " + channel+".");
                throw;
            }
            return channel.Id;
        }

        /// <summary>
        /// Creates a genre with the name.
        /// </summary>
        /// <param name="genreName">The name of the genre.</param>
        public void CreateGenre(string genreName)
        {
            if (genreName == null) LogAndThrowException(new ArgumentNullException("genreName"), "CreateGenre");
            string logEntry = "Genre with name: " + " [" + genreName + "] has been created." ;
            
            try
            {
                lock (_dbLock)
                {
                    _dao.CreateGenre(genreName);
                    _logger.AddEntry(logEntry + "Genre creation succeeded.");
                }
            }
            catch (Exception e)
            {
                _logger.AddEntry("GenreCreation failed with exception [{0}]. logEntry = " + logEntry + ".");
                throw;
            }
        }

        /// <summary>
        /// Deletes the channel.
        /// </summary>
        /// <param name="userId">The user id making the request, this must correspond to the channel owners id.</param>
        /// <param name="channelId">The channel id.</param>
        public void DeleteChannel(int channelId)
        {
            if (channelId < 0) LogAndThrowException(new ArgumentException("channelId was below 0"), "DeleteChannel");
            List<Track> associatedTracks = GetTracksByChannelId(channelId);
            foreach (Track t in associatedTracks)
            {
                RemoveTrack(t.Id);
            }

            Channel channel = null;
            try
            {
                lock (_dbLock)
                {
                    channel = _dao.GetChannel(channelId);
                    string logEntry = "[" + channel.Name + "] with id [" + channelId + "] is being deleted.";
                    _dao.DeleteChannel(channel.GetChannel());
                    _channelCache[channelId] = null;
                    _logger.AddEntry(logEntry + "Deletion successful.");
                }
            }
            catch (Exception e)
            {
                //if (_handler != null)
                //    _handler(this, new RentItEventArgs("Channel deletion failed with exception [" + e + "]."));
                _logger.AddEntry(string.Format("DeleteChannel failed with exception [{0}]. Local variables: channelId = {1}, channel = {2}", e, channelId, channel));
                throw;
            }
        }

        public void UpdateChannel(int channelId, int? ownerId, string channelName, string description, double? hits,
                                  double? rating)
        {
            try
            {
                _dao.UpdateChannel(channelId, ownerId, channelName, description, hits, rating, null);
            }
            catch (Exception e)
            {
                //if (_handler != null)
                //    _handler(this, new RentItEventArgs("Update channel failed with exception [" + e + "]."));
                _logger.AddEntry(string.Format("Update channel failed with exception [{0}]. Local variables: channelId = {1}, ownerId = {2}, channelName = {3}, description = {4}, hits = {5}, rating = {6}.", e, channelId, ownerId, channelName, description, hits, rating));
                throw;
            }
        }

        /// <summary>
        /// Adds one to the hits of a channel.
        /// </summary>
        /// <param name="channelId">The id of the channel.</param>
        public void IncrementHitsForChannel(int channelId)
        {
            _dao.IncrementHitsForChannel(channelId);
        }

        /// <summary>
        /// Gets a channel.
        /// </summary>
        /// <param name="channelId">The channel id for the channel to get.</param>
        /// <returns>The channel matching the given id.</returns>
        public DatabaseWrapperObjects.Channel GetChannel(int channelId)
        {
            if (channelId < 0) LogAndThrowException(new ArgumentException("channelId was below 0"), "GetChannel");

            /*if (_channelCache[channelId] != null)
            {   // Attempt to use cache first
                return _channelCache[channelId].GetChannel();
            }*/

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

            return channel.GetChannel();
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
                //if (_handler != null)
                //    _handler(this, new RentItEventArgs("GetChannelIds failed with exception [" + e + "]."));
                LogAndThrowException(e, "GetAllChannelIds");
                throw;
            }
        }

        public DatabaseWrapperObjects.Channel[] GetChannels(ChannelSearchArgs args)
        {
            try
            {
                IEnumerable<Channel> channels = _dao.GetChannelsWithFilter(args);
                return Channel.GetChannels(channels).ToArray();
            }
            catch (Exception e)
            {
                //if (_handler != null)
                //    _handler(this, new RentItEventArgs("GetChannels failed with exception [" + e + "]."));
                string entry = string.Format("GetChannels failed with exception [{0}]. Local variables: args = {1}", e, args);
                if (args != null)
                    entry =
                        string.Format(
                            "{0}, args.AmountPlayed = {1}, args.Genres = {2}, args.NumberOfComments = {3}, args.NumberOfSubscriptions = {4}, args.Rating = {5}, args.SearchString = {6}, args.SortOption = {7}, args.StartIndex = {8}, args.EndIndex = {9}",
                            entry, args.AmountPlayed, args.Genres, args.NumberOfComments, args.NumberOfSubscriptions,
                            args.Rating, args.SearchString.Equals("") ? "\"\"" : args.SearchString, args.SortOption.Equals("") ? "\"\"" : args.SortOption, args.StartIndex, args.EndIndex);
                _logger.AddEntry(entry);
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
                //if (_handler != null)
                //    _handler(this, new RentItEventArgs("CreateVote failed with exception [" + e + "]."));
                _logger.AddEntry(string.Format("CreateVote failed with exception [{0}]. Local variables: rating = {1}, userId = {2}, trackId = {3}.", e, rating, userId, trackId));
                throw;
            }
        }

        public void AddTrack(int userId, int channelId, MemoryStream audioStream, DatabaseWrapperObjects.Track trackInfo)
        {
            try
            {
                _logger.AddEntry("[Controller-AddTrack]: Gathering track information");
                trackInfo = GetTrackInfo(audioStream);
                _logger.AddEntry("Trackinfo: Name: " + trackInfo.Name + " - Artist: " + trackInfo.Artist + " - length: " + trackInfo.Length);
                _dao.CreateTrackEntry(channelId, "", trackInfo.Name, trackInfo.Artist, trackInfo.Length, trackInfo.UpVotes, trackInfo.DownVotes);
                string relativePath = FileName.ItuGenerateAudioFileName(_dao.GetTrack(channelId, trackInfo.Name).Id);
                try
                {
                    _logger.AddEntry("Size of audioStream: " + audioStream.Length);
                    _fileSystemHandler.WriteFile(FilePath.ITUTrackPath, relativePath, audioStream);
                }
                catch
                {
                    _dao.DeleteTrackEntry(trackInfo);
                    throw new Exception("Exception occured when trying to write file to filesystem.");
                }
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("AddTrack failed with exception [" + e + "]."));
                throw;
            }
        }

        public DatabaseWrapperObjects.Track GetTrackInfo(MemoryStream audioStream)
        {
            Track theTrack = new Track();
            theTrack.Artist = "";
            try
            {
                int counter = tempCounter++;
                _fileSystemHandler.WriteFile(FilePath.ITUTempPath, FileName.ItuGenerateAudioFileName(counter), audioStream);
                // Use external library
                TagLib.File audioFile = TagLib.File.Create(FilePath.ITUTempPath.GetPath() + FileName.ItuGenerateAudioFileName(counter));
                
                string[] artists = audioFile.Tag.AlbumArtists;
                foreach (string artist in artists)
                {
                    theTrack.Artist += artist + ", ";
                }
                theTrack.Artist = theTrack.Artist.Substring(0, theTrack.Artist.Count() - 2);
                theTrack.DownVotes = 0;
                theTrack.UpVotes = 0;
                theTrack.Name = audioFile.Tag.Title;
                theTrack.TrackPlays = new List<TrackPlay>();
                theTrack.Votes = new List<Vote>();
                theTrack.Length = audioFile.Properties.Duration.Milliseconds;
                try
                {
                    _fileSystemHandler.DeleteFile(FilePath.ITUTempPath.GetPath() + FileName.ItuGenerateAudioFileName(counter));
                }
                catch
                {
                    // It doesn't matter much
                }
                return theTrack.GetTrack();
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("GetTrackInfo failed with exception [" + e + "]."));
                throw;
            }
        }

        public DatabaseWrapperObjects.Track GetTrackInfo(int channelId, string trackname)
        {
            try
            {
                return _dao.GetTrack(channelId, trackname).GetTrack();
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("GetTrackInfoByTrackname deletion failed with exception [" + e + "]."));
                throw;
            }
        }

        public void RemoveTrack(int trackId)
        {
            if (trackId < 0) LogAndThrowException(new ArgumentException("trackId was below 0"), "RemoveTrack");

            try
            {
                Track track = _dao.GetTrack(trackId);
                string logEntry = "[" + track.Name + "] with id [" + trackId + "] is being deleted.";
                if (_fileSystemHandler.Exists(track.Path))
                    _fileSystemHandler.DeleteFile(track.Path);
                _dao.DeleteTrackEntry(track.GetTrack());
                _dao.DeleteVotesForTrack(trackId);
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

        public IEnumerable<ITU.DatabaseWrapperObjects.Track> GetTracks(int channelId, TrackSearchArgs args)
        {
            try
            {
                IEnumerable<Track> tracks = _dao.GetTracksWithFilter(channelId, args);
                return ITU.DatabaseWrapperObjects.Track.GetTracks(tracks);
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

        /// <summary>
        /// Gets comments associated with a user within the specified range. Ranges outside the size of the comment collection or null is interpreted as extremes.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// /// <param name="fromInclusive">The start index to retrieve comments from inclusive.</param>
        /// /// <param name="toExclusive">The end index to retieve comments from exclusive.</param>
        /// <returns>
        /// Comments from a specific user in the specified range.
        /// </returns>
        public DatabaseWrapperObjects.Comment[] GetUserComments(int userId, int? fromInclusive, int? toExclusive)
        {
            if (fromInclusive == null) fromInclusive = 0;
            if (toExclusive == null) toExclusive = int.MaxValue;
            try
            {
                List<DatabaseWrapperObjects.Comment> comments = _dao.GetUserComments(userId, fromInclusive.Value, toExclusive.Value);
                return comments.ToArray();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets comments associated with a channel within the specified range. Ranges outside the size of the comment collection or null is interpreted as extremes.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// /// <param name="fromInclusive">The start index to retrieve comments from inclusive.</param>
        /// /// <param name="toExclusive">The end index to retieve comments from exclusive.</param>
        /// <returns>
        /// Comments from a specific channel in the specified range.
        /// </returns>
        public DatabaseWrapperObjects.Comment[] GetChannelComments(int channelId, int? fromInclusive, int? toExclusive)
        {
            if (fromInclusive == null) fromInclusive = 0;
            if (toExclusive == null) toExclusive = int.MaxValue; 
            try
            {
                List<DatabaseWrapperObjects.Comment> comments = _dao.GetChannelComments(channelId, fromInclusive.Value, toExclusive.Value);
                return comments.ToArray();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public DatabaseWrapperObjects.Comment GetComment(int commentId)
        {
            throw new NotImplementedException();
            //return new DatabaseWrapperObjects.Comment();
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
                //if (_handler != null)
                //    _handler(this, new RentItEventArgs("Subscribe failed with exception [" + e + "]."));
                _logger.AddEntry(string.Format("Subscribe failed with exception [{0}]. Local variables: userId = {1}, channelId = {2}", e, userId, channelId));
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
                //if (_handler != null)
                //    _handler(this, new RentItEventArgs("UnSubscribe failed with exception [" + e + "]."));
                _logger.AddEntry(string.Format("Unsubscribe failed with exception [{0}]. Local variables: userId = {1}, channelId = {2}", e, userId, channelId));
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
            _logger.AddEntry("[" + e + "] raised in [" + operationName + "] with message [" + e.Message + "].");
            //if (_handler != null)
            //    _handler(this, new RentItEventArgs("[" + e + "] raised in [" + operationName + "] with message [" + e.Message + "]."));
            throw e;
        }

        public bool IsEmailAvailable(string email)
        {
            if(email == null)   LogAndThrowException(new ArgumentException("email"), "IsEmailEavailable");
            return _dao.IsEmailAvailable(email);
        }

        public bool IsUsernameAvailable(string username)
        {
            if (username == null) LogAndThrowException(new ArgumentException("username"), "IsUsernameAvailable");
            if (username.Equals("")) LogAndThrowException(new ArgumentException("username was empty"), "IsUsernameAvailable");
            return _dao.IsUsernameAvailable(username);
        }

        public ChannelSearchArgs GetDefaultChannelSearchArgs()
        {
            return new ChannelSearchArgs();
        }

        public TrackSearchArgs GetDefaultTrackSearchArgs()
        {
            return new TrackSearchArgs();
        }

        public List<Channel> GetCreatedChannels(int userId)
        {
            return _dao.GetCreatedChannels(userId);
        }

        public List<Channel> GetSubscribedChannels(int userId)
        {
            return _dao.GetSubscribedChannels(userId);
        }

        public List<Track> GetTracksByChannelId(int channelId)
        {
            return _dao.GetTracksByChannelId(channelId);
        }

        public bool IsChannelNameAvailable(int channelId, string channelName)
        {
            if (channelName == null) LogAndThrowException(new ArgumentException("channelName"), "IsChannelNameAvailable");
            return _dao.IsChannelNameAvailable(channelId, channelName);
        }

        public int GetSubscriberCount(int channelId)
        {
            return _dao.GetSubscriberCount(channelId);
        }

        public void IncrementChannelPlays(int channelId)
        {
            _dao.IncrementChannelPlays(channelId);
        }
    }
}