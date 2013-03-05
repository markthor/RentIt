using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RentItServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class RentItService : IRentItService
    {
        private static readonly Controller _controller = Controller.GetInstance();

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
            return 0;
            return _controller.CreateChannel(channelName, userId, description, genres);
        }

        /// <summary>
        /// Gets the channel ids matching the given search string and search arguments.
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <param name="args">The search arguments (used for filtering).</param>
        /// <returns>An array of channel ids matching search criteria. </returns>
        public int[] GetChannelIds(string searchString, SearchArgs args)
        {
            return new int[] { 0 };
            return _controller.GetChannelIds(searchString, args);
        }

        public Channel GetChannel(int channelId)
        {
            return new Channel();
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

        public int CreateUser(string username, string password, string email)
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

        public void Comment(string comment, int userId, int channelId)
        {
            _controller.Comment(comment, userId, channelId);
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

        public int GetChannelPort(int channelId, int ipAddress, int port)
        {
            //int i = Controller.GetInstance().GetChannelPort(channelId,ipAddress,port);

            return -1;
        }
    }
}
