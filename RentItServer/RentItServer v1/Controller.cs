using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer_v1
{
    public class Controller
    {
        //Singleton instance of the class
        private static Controller _instance;
        //Data access object for database IO
        private DAO _dao = DAO.GetInstance();
        //Responsible for choosing the next trackStream
        private TrackPrioritizer _trackPrioritizer = TrackPrioritizer.GetInstance();
        //Data access object for file system IO
        private FileSystemHandler _fileSystemHandler = FileSystemHandler.GetInstance();
        //The logger
        private readonly Logger _logger = Logger.GetInstance();

        /// <summary>
        /// Private to ensure local instantiation.
        /// </summary>
        private Controller()
        {
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
        public int CreateChannel(string channelName, int userId, string description, int[] genres)
        {
            if (channelName == null)    LogAndThrowException(new ArgumentNullException("channelName"), "CreateChannel");
            if (channelName.Equals("")) LogAndThrowException(new ArgumentException("channelName was empty"), "CreateChannel");
            if (userId < 0)             LogAndThrowException(new ArgumentException("userId was below 0"), "CreateChannel");
            if (description == null)    LogAndThrowException(new ArgumentException("description"), "CreateChannel");
            if (genres == null)         LogAndThrowException(new ArgumentNullException("genres"), "CreateChannel");
            
            int channelId = _dao.CreateChannel(channelName, userId, description, genres);
            _logger.AddEntry(   @"User id [" + userId + "] want to create the channel [" + channelName + "]." +
                                 "Channel description = " + description + "." +
                                 "Channel genres = " + genres + "." +
                                 (channelId == -1 ?
                                 "Channel creation failed." : 
                                 "Channel creation succeeded."));
            return channelId;
        }

        /// <summary>
        /// Gets the channel ids matching the given search string and search arguments.
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <param name="args">The search arguments (used for filtering).</param>
        /// <returns>An array of channel ids matching search criteria. </returns>
        public int[] GetChannelIds(string searchString, SearchArgs args)
        {
            if (searchString == null)    LogAndThrowException(new ArgumentNullException("searchString"), "GetChannelIds");
            if (searchString.Equals("")) LogAndThrowException(new ArgumentException("searchString was empty"), "GetChannelIds");
            
            //String[] searchMatches = TernarySearchTrie.WildcardMatch(searchString)
            //List<int> idList = new List<int>();
            //foreach ....
            return new int[]{};
        }

        public Channel GetChannel(int channelId)
        {
            if(channelId < 0)   LogAndThrowException(new ArgumentException("channelId was below 0"), "GetChannel");
            return _dao.GetChannel(channelId);
        }

        public Channel ModifyChannel(int channelId)
        {
            return new Channel();
        }

        public void DeleteChannel(int channelId)
        {
        }

        public int Login(string username, string password)
        {
            return 0;
        }

        public int CreateUser(string username, string password)
        {
            return 0;
        }

        public void UploadTrack(Track track, int channelId)
        {
        }

        public void RemoveTrack(int trackId)
        {
        }

        public void VoteTrack(int rating, int userId, int trackId)
        {
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
            _logger.AddEntry(  @"User id ["+userId+"] commented on the channel ["+channelId+"]."+
                                "Comment content = "+comment + ".");
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
            _logger.AddEntry("[" + e + "] raised in [" + operationName + "] with message [" + e.Message + "]");
            throw e;
        }
    }
}