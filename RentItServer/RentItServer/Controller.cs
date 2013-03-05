using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer
{
    public class Controller
    {
        //Singleton instance of the class
        public static Controller _instance;
        //Data access object for database IO
        public static DAO _dao = DAO.GetInstance();
        //Responsible for choosing the next trackStream
        public static TrackPrioritizer _trackPrioritizer = TrackPrioritizer.GetInstance();
        //Data access object for file system IO
        public static FileSystemHandler _fileSystemHandler = FileSystemHandler.GetInstance();
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
        /// Comments on the specified channel.
        /// </summary>
        /// <param name="comment">The comment.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="channelId">The channel id.</param>
        public void Comment(string comment, int userId, int channelId)
        {
            DAO.GetInstance().Comment(comment, userId, channelId);
            _logger.AddEntry(  @"User id ["+userId+"] commented on the channel ["+channelId+"]."+
                                "Comment content = "+comment + ".");
        }

        
        public int GetChannelPort(int channelId, int ipAddress, int port)
        {
            


            return -1;
        }
    }
}