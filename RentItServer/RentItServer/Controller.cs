using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RentItServer.Search;

namespace RentItServer
{
    public class Controller
    {
        //Singleton instance of the class
        private static Controller _instance;
        //Data access object for database IO
        private readonly DAO _dao = DAO.GetInstance();
        //Responsible for choosing the next trackStream
        private readonly TrackPrioritizer _trackPrioritizer = TrackPrioritizer.GetInstance();
        //Data access object for file system IO
        private readonly FileSystemHandler _fileSystemHandler = FileSystemHandler.GetInstance();
        //The logger
        private readonly Logger _logger = Logger.GetInstance();
        //The ternary search trie for channels. Each channel has its id as value
        private TernarySearchTrie<int> _channelSearch;
        //The ternary search trie for users. Each user has his/her password as value
        private TernarySearchTrie<String> _userSearch;

        /// <summary>
        /// Private to ensure local instantiation.
        /// </summary>
        private Controller()
        {
            _channelSearch = new TernarySearchTrie<int>();
            _userSearch = new TernarySearchTrie<string>();
            // Initialize channel search trie
            IEnumerable<Channel> allChannels = _dao.GetAllChannels();
            foreach (Channel channel in allChannels)
            {
                _channelSearch.Put(channel.name, channel.id);
            }
            // Initialize user search trie
            // TODO
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
            if (channelName == null)    LogAndThrowException(new ArgumentNullException("channelName"), "CreateChannel");
            if (channelName.Equals("")) LogAndThrowException(new ArgumentException("channelName was empty"), "CreateChannel");
            if (userId < 0)             LogAndThrowException(new ArgumentException("userId was below 0"), "CreateChannel");
            if (description == null)    LogAndThrowException(new ArgumentException("description"), "CreateChannel");
            if (genres == null)         LogAndThrowException(new ArgumentNullException("genres"), "CreateChannel");

            int channelId;
            string logEntry = "User id [" + userId + "] want to create the channel [" + channelName + "] with description [" + description + "] and genres ["+genres+"]. ";
            try
            {
                channelId = _dao.CreateChannel(channelName, userId, description, genres);
                _channelSearch.Put(channelName, channelId);
                _logger.AddEntry(logEntry + "Channel creation succeeded.");
            }
            catch(Exception e)
            {
                _logger.AddEntry(logEntry + "Channel creation failed with exception ["+e+"].");
                throw;
            }
            return channelId;
        }

        /// <summary>
        /// Gets the channel ids matching the given search arguments.
        /// </summary>
        /// <param name="args">The search arguments (used for filtering).</param>
        /// <returns>An array of channel ids matching search criteria. If there are no matches, will return an empty array. </returns>
        public int[] GetChannelIds(SearchArgs args)
        {
            IEnumerable<Channel> channels = _dao.GetChannelsWithFilter(args);
            IEnumerable<string> channelMatches = _channelSearch.PrefixMatch(args.SearchString).ToArray();
            
            return new int[]{};
        }

        /// <summary>
        /// Gets a channel.
        /// </summary>
        /// <param name="channelId">The channel id for the channel to get.</param>
        /// <returns>The channel matching the given id.</returns>
        public Channel GetChannel(int channelId)
        {
            if(channelId < 0)   LogAndThrowException(new ArgumentException("channelId was below 0"), "GetChannel");

            return _dao.GetChannel(channelId);
        }

        public Channel ModifyChannel(int userId, int channelId)
        {
            //TODO .... waaht?
            return new Channel();
        }

        /// <summary>
        /// Deletes the channel.
        /// </summary>
        /// <param name="userId">The user id making the request, this must correspond to the channel owners id.</param>
        /// <param name="channelId">The channel id.</param>
        public void DeleteChannel(int userId, int channelId)
        {
            if(userId < 0)  LogAndThrowException(new ArgumentException("userId is below 0"), "DeleteChannel");
            if(channelId < 0)   LogAndThrowException(new ArgumentException("channelId was below 0"), "DeleteChannel");

            try
            {
                Channel channel = _dao.GetChannel(channelId);
                string logEntry = "User id [" + userId + "] want to delete the channel [" + channel.name + "]. ";
                if (channel.userId == userId)
                {
                    _dao.DeleteChannel(userId, channelId);
                    _logger.AddEntry(logEntry + "Deletion successful.");
                }
                else
                {
                    _logger.AddEntry(logEntry + "Deletion failed. Request comes from a user other than channel owner.");
                }
            }
            catch (Exception e)
            {
                _logger.AddEntry("Channel deletion failed with exception [" + e + "].");
                throw;
            }
        }

        /// <summary>
        /// Login the specified user.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password for the user.</param>
        /// <returns>The id of the user, or -1 if the (username,password) is not found.</returns>
        public int Login(string username, string password)
        {
            if (_userSearch.Get(username).Equals(password))
            {
                // TODO: why should the id be returned? we can find the id from the username...
            }
            // TODO
            return 0;
        }

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password for the user.</param>
        /// <param name="email">The email associated with user.</param>
        /// <returns>The id of the created user.</returns>
        public int CreateUser(string username, string password, string email)
        {
            if (username == null) LogAndThrowException(new ArgumentNullException("username"), "CreateUser");
            if (username.Equals("")) LogAndThrowException(new ArgumentException("username was empty"), "CreateUser");
            if (password == null) LogAndThrowException(new ArgumentException("password"), "CreateUser");
            if (password.Equals("")) LogAndThrowException(new ArgumentException("password was empty"), "CreateUser");
            if (email == null) LogAndThrowException(new ArgumentNullException("email"), "CreateUser");
            if (email.Equals("")) LogAndThrowException(new ArgumentException("email was empty"), "CreateUser");
            // TODO use regex to better check mail validity

            int userId;
            try
            {
                userId = _dao.CreateUser(username, password, email);
                _logger.AddEntry("User created with username ["+username+"] and e-mail ["+email+"].");
            }
            catch (Exception e)
            {
                _logger.AddEntry("User creation failed with exception [" + e + "].");
                throw;
            }
            return userId;
        }

        public void UploadTrack(Track track, int userId, int channelId)
        {
            // TODO possibly better with MemoryStream instead of Track
        }

        public void RemoveTrack(int userId, int trackId)
        {
            // TODO
        }

        public void VoteTrack(int rating, int userId, int trackId)
        {
            // TODO parameter validation
            try
            {
                _dao.VoteTrack(rating, userId, trackId);
                _logger.AddEntry("User with user id ["+userId+"] rated track with track id ["+trackId+"] with the rating ["+rating+"].");
            }
            catch (Exception e)
            {
                _logger.AddEntry("Voting failed with exception ["+e+"].");
                throw;
            }
        }

        public int[] GetTrackIds(int channelId)
        {
            return new[] { 0 };
        }

        public TrackInfo GetTrackInfo(int trackId)
        {
            return new TrackInfo();
        }

        /// <summary>
        /// Comments on the specified channel.
        /// </summary>
        /// <param name="comment">The comment.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="channelId">The channel id.</param>
        public void Comment(string comment, int userId, int channelId)
        {
            _dao.Comment(comment, userId, channelId);
            _logger.AddEntry("User id ["+userId+"] commented on the channel ["+channelId+"] with the comment ["+comment + "].");
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
            _logger.AddEntry("[" + e + "] raised in [" + operationName + "] with message [" + e.Message + "].");
            throw e;
        }
    }
}