using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        //The channel organizer
        private readonly ChannelOrganizer _cOrganizer;
        // The dictionary for channel, mapping the id to the object. This is to ease database load as the "GetChannel(int channelId)" will be used very frequently.
        private readonly Dictionary<int, Channel> _channelCache;
        //The ternary search trie for users. Each username has his/her password as value
        private TernarySearchTrie<String> _userSearch;

        /// <summary>
        /// Private to ensure local instantiation.
        /// </summary>
        private Controller()
        {
            _channelCache = new Dictionary<int, Channel>();
            _userSearch = new TernarySearchTrie<string>();
            // Initialize channel search trie
            IEnumerable<Channel> allChannels = _dao.GetAllChannels();
            foreach (Channel channel in allChannels)
            {
                _channelCache[channel.Id] = channel;
            }
            // Initialize user search trie
            _logger = new Logger(FilePath.ITULogPath.GetPath() + LogFileName, ref _handler);
            _cOrganizer = ChannelOrganizer.GetInstance();
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
        /// Creates a channel.
        /// </summary>
        /// <param name="channelName">Name of the channel.</param>
        /// <param name="userId">The id of the user creating the channel.</param>
        /// <param name="description">The description of the channel.</param>
        /// <param name="genres">The genres associated with the channel.</param>
        /// <returns>The id of the created channel. -1 if the channel creation failed.</returns>
        public int CreateChannel(string channelName, int userId, string description, string[] genres)
        {
            if (channelName == null) LogAndThrowException(new ArgumentNullException("channelName"), "CreateChannel");
            if (channelName.Equals("")) LogAndThrowException(new ArgumentException("channelName was empty"), "CreateChannel");
            if (userId < 0) LogAndThrowException(new ArgumentException("userId was below 0"), "CreateChannel");
            if (description == null) LogAndThrowException(new ArgumentException("description"), "CreateChannel");
            if (genres == null) LogAndThrowException(new ArgumentNullException("genres"), "CreateChannel");

            Channel channel;
            string logEntry = "User id [" + userId + "] want to create the channel [" + channelName + "] with description [" + description + "] and genres [" + genres + "]. ";
            try
            {
                channel = _dao.CreateChannel(channelName, userId, description, genres);
                _channelCache[channel.Id] = channel;
                //_logger.AddEntry(logEntry + "Channel creation succeeded.");
            }
            catch (Exception e)
            {
               if(_handler != null)
                    _handler(this, new RentItEventArgs(logEntry + "Channel creation failed with exception [" + e + "]."));
                throw;
            }
            return channel.Id;
        }

        /// <summary>
        /// Gets the channel ids matching the given search arguments.
        /// </summary>
        /// <param name="args">The search arguments (used for filtering).</param>
        /// <returns>An array of channel ids matching search criteria. If there are no matches, will return an empty array. </returns>
        public int[] GetChannelIds(SearchArgs args)
        {
            // Get channels that match all filters
            List<Channel> channels = _dao.GetChannelsWithFilter(args);

            // Extract all ids
            List<int> filteredChannelIds = new List<int>();
            for (int i = 0; i < channels.Count(); i++)
            {
                filteredChannelIds.Add(channels[i].Id);
            }

            return filteredChannelIds.ToArray();
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

            // cache might be outdated, query the databse to be sure.
            Channel channel = _dao.GetChannel(channelId);
            if (channel != null){
                // channel was found in the database, adding to cache
                _channelCache[channelId] = channel;
            }
            else
            {   // A channel with id = channelId does not exist in cache or in database nigga
                LogAndThrowException(new ArgumentException("No channel with channelId = " + channelId + " exist."), "GetChannel");
            }

            return channel;
        }

        public Channel ModifyChannel(int userId, int channelId)
        {
            throw new NotImplementedException();
            //TODO .... waaht?
            //return new Channel();
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
                Channel channel = _dao.GetChannel(channelId);
                string logEntry = "User id [" + userId + "] want to delete the channel [" + channel.Name + "]. ";
                if (channel.UserId == userId)
                {
                    _dao.DeleteChannel(userId, channel);
                    //_logger.AddEntry(logEntry + "Deletion successful.");
                }
                else
                {
                    if(_handler != null)
                    _handler(this, new RentItEventArgs(logEntry + "Deletion failed. Request comes from a user other than channel owner."));
                }
            }
            catch (Exception e)
            {
                if(_handler != null)
                    _handler(this, new RentItEventArgs("Channel deletion failed with exception [" + e + "]."));
                throw;
            }
        }

        /// <summary>
        /// Login the specified user.
        /// </summary>
        /// <param name="usernameOrEmail">The username or email of the user.</param>
        /// <param name="password">The password for the user.</param>
        /// <returns>The id of the user, or -1 if the (username,password) is not found.</returns>
        public User Login(string usernameOrEmail, string password)
        {
            return _dao.Login(usernameOrEmail, password);
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
                return _dao.SignUp(username, password, email);
                //_logger.AddEntry("User created with username [" + username + "] and e-mail [" + email + "].");
            }
            catch (Exception e)
            {
               if(_handler != null)
                    _handler(this, new RentItEventArgs("User creation failed with exception [" + e + "]."));
                throw;
            }
            return null;
        }

        public void UploadTrack(Track track, int userId, int channelId)
        {
            // TODO possibly better with MemoryStream instead of Track
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
                if(_handler != null)
                    _handler(this, new RentItEventArgs("Track deletion failed with exception [" + e + "]."));
                throw;
            }
        }

        public void VoteTrack(int rating, int userId, int trackId)
        {
            // TODO parameter validation
            try
            {
                //_dao.VoteTrack(rating, userId, trackId);
                //_logger.AddEntry("User with user id [" + userId + "] rated track with track id [" + trackId + "] with the rating [" + rating + "].");
            }
            catch (Exception e)
            {
                if(_handler != null)
                    _handler(this, new RentItEventArgs("Voting failed with exception [" + e + "]."));
                throw;
            }
        }

        public int[] GetTrackIds(int channelId)
        {
            return new[] { 0 };
        }

        public Track GetTrackInfo(int trackId)
        {
            return new Track();
        }

        /// <summary>
        /// Comments on the specified channel.
        /// </summary>
        /// <param name="comment">The comment.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="channelId">The channel id.</param>
        public void Comment(string comment, int userId, int channelId)
        {
            //_dao.Comment(comment, userId, channelId);
            //_logger.AddEntry("User id [" + userId + "] commented on the channel [" + channelId + "] with the comment [" + comment + "].");
            if(_handler != null)
                    _handler(this, new RentItEventArgs("User id [" + userId + "] commented on the channel [" + channelId + "] with the comment [" + comment + "]."));
        }

        public int[] GetCommentIds(int channelId)
        {
            return new[] { 0 };
        }

        public Comment GetComment(int commentId)
        {
            return new Comment();
        }

        public void Subscribe(int userId, int channelId)
        {

        }

        public void UnSubscribe(int userId, int channelId)
        {

        }

        /// <summary>
        /// Logs and throws the exception e.
        /// </summary>
        /// <param name="e">The exception to throw.</param>
        /// <param name="operationName">Name of the operation.</param>
        private void LogAndThrowException(Exception e, String operationName)
        {
            //_logger.AddEntry("[" + e + "] raised in [" + operationName + "] with message [" + e.Message + "].");
            if(_handler != null)
                    _handler(this, new RentItEventArgs("[" + e + "] raised in [" + operationName + "] with message [" + e.Message + "]."));
            throw e;
        }

        public int ListenToChannel(int channelId)
        {
            _cOrganizer.StartChannel(channelId);
            return _cOrganizer.GetChannelPortNumber(channelId);
        }
    }
}