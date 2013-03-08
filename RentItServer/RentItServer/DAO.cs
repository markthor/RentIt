using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer
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
        /// <returns>The id of the created channel. -1 if the channel creation failed.</returns>
        public int CreateChannel(string channelName, int userId, string description, string[] genres)
        {
            return -1;
        }

        public int CreateUser(string username, string password, string email)
        {
            return -1;
        }

        public Channel GetChannel(int channelId)
        {
            return new Channel();
        }

        public void DeleteChannel(int userId, int channelId)
        {
            
        }

        public void VoteTrack(int rating, int userId, int trackId)
        {
            
        }

        public void Comment(string comment, int userId, int channelId)
        {
            
        }
    }
}