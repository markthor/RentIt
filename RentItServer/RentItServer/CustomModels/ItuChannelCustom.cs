using System.Collections.Generic;

namespace RentItServer
{
    /// <summary>
    /// Partial class of the database entity "Channel". Used to add functionality to the class.
    /// </summary>
    public partial class Channel
    {
        /// <summary>
        /// Gets the wrapper for this channel.
        /// </summary>
        /// <returns></returns>
        public ITU.DatabaseWrapperObjects.Channel GetChannel()
        {
            return new ITU.DatabaseWrapperObjects.Channel(Id, Name, Description, Rating, Hits, UserId, StreamUri);
        }

        /// <summary>
        /// Gets the wrappers for these channels.
        /// </summary>
        /// <param name="channels">The channels.</param>
        /// <returns></returns>
        public static List<ITU.DatabaseWrapperObjects.Channel> GetChannels(IEnumerable<Channel> channels)
        {
            List<ITU.DatabaseWrapperObjects.Channel> convertedChannels = new List<ITU.DatabaseWrapperObjects.Channel>();
            foreach(Channel channel in channels)
            {
                convertedChannels.Add(channel.GetChannel());
            }
            return convertedChannels;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override bool Equals(object obj)
        {
            var item = obj as Channel;

            if (item == null)
            {
                return false;
            }
            return this.Id == item.Id;
        }
    }
}