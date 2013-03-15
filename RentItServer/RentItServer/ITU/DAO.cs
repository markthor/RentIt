using System.Collections.Generic;
using System.Linq;

namespace RentItServer.ITU
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
            Channel ch;
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var channels = from channel in context.channels.Where(channel => channel.id == channelId)
                              select channel;

                if (channels.Any() == false)
                {   // No channel with matching id
                    return null;
                }
                if (channels.Count() > 1)
                {
                    // Da fuk?
                }
                ch = channels.First();
            }
            return ch;
        }

        public IEnumerable<Channel> GetAllChannels()
        {
            IEnumerable<Channel> allChannels;
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var channels = from channel in context.channels select channel;
                allChannels = channels.ToList();
            }
            return allChannels;
        }

        /// <summary>
        /// Filters the with respect to the filter arguments: 
        ///     filter.SearchString
        ///     filter.AmountPlayed
        ///     filter.Genres
        ///     filter.NumberOfComments
        ///     filter.NumberOfSubscriptions
        ///     filter.SortOptions
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// Channel ids of the channels matching the filter.
        /// </returns>
        public List<Channel> GetChannelsWithFilter(SearchArgs filter)
        {
            List<Channel> filteredChannels;
            using (RENTIT21Entities context = new RENTIT21Entities())
            {   // get all channels that starts with filter.SearchString
                var channels = from channel in context.channels
                               where channel.name.StartsWith(filter.SearchString)
                               select channel;

                if (filter.AmountPlayed > -1)
                {   // Apply amount played filter
                    channels = from channel in channels 
                               where channel.plays >= filter.AmountPlayed 
                               select channel;
                }
                if (filter.Genres.Any() == true)
                {   // Apply genre filter
                    channels = from channel in channels
                               where channel.genres.Any(genre => filter.Genres.Contains(genre.name))
                               select channel;
                }
                if (filter.NumberOfComments > -1)
                {   // Apply comment filter
                    channels = from channel in channels
                               where channel.comments.Count >= filter.NumberOfComments
                               select channel;
                }
                if (filter.NumberOfSubscriptions > -1)
                {   // Apply subscription filter
                    channels = from channel in channels
                               where channel.subscriptions.Count >= filter.NumberOfSubscriptions
                               select channel;
                }
                if (filter.SortOption == -1)
                {   // Descending
                    channels = from channel in channels
                               orderby channel.name descending
                               select channel;

                }else if (filter.SortOption == 1)
                {   // Ascending
                    channels = from channel in channels
                               orderby channel.name ascending
                               select channel;
                }
                filteredChannels = channels.ToList();
            }
            if (filter.startIndex != -1 && filter.endIndex != -1 && filter.startIndex <= filter.endIndex)
            {   // Only get the channels within the specified interval [filter.startIndex, ..., filter.endIndex]
                Channel[] range = new Channel[filter.endIndex - filter.startIndex];
                filteredChannels.CopyTo(filter.startIndex, range, 0, filter.endIndex-filter.startIndex);
                filteredChannels = new List<Channel>(range);
            }
            return filteredChannels;
        }   

        public void DeleteChannel(int userId, int channelId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var channels = from channel in context.channels.Where(channel => channel.userId == userId && channel.id == channelId)
                               select channel;

                if (channels.Any() == false)
                {   // The channel does not exist
                    return;
                }

            }
        }

        public void VoteTrack(int rating, int userId, int trackId)
        {
            
        }

        public void Comment(string comment, int userId, int channelId)
        {
            
        }
    }
}