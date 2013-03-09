using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.ServiceModel;

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
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var channel = from channels in context.channels.Where(channels => channels.id == channelId)
                              select channels;
                if (channel.Any() == false)
                {   // No channel with matching id
                    return null;
                }
                channels ch = channel.First();
                // TODO not finished... consider renaming tables
            }
            return new Channel();
        }

        public IEnumerable<Channel> GetAllChannels()
        {
            return new List<Channel>();
        }

        /// <summary>
        /// Filters the channels with respect to the filter argument.
        /// </summary>
        /// <param name="channelIds">The channel ids.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>Channel ids of the channels matching the filter.</returns>
        public List<int> FilterChannels(List<int> channelIds, SearchArgs filter)
        {
            return new List<int>();
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