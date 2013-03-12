using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RentItServer
{
    [ServiceContract]
    public interface IRentItService
    {
        /// <summary>
        /// Creates a channel.
        /// </summary>
        /// <param name="channelName">Name of the channel.</param>
        /// <param name="userId">The id of the user creating the channel.</param>
        /// <param name="description">The description of the channel.</param>
        /// <param name="genres">The genres associated with the channel.</param>
        /// <returns>The id of the created channel. -1 if the channel creation failed.</returns>
        [OperationContract]
        int CreateChannel(string channelName, int userId, string description, string[] genres);

        /// <summary>
        /// Gets the channel ids matching the given search arguments.
        /// </summary>
        /// <param name="args">The search arguments (used for filtering).</param>
        /// <returns>An array of channel ids matching search criteria. </returns>
        [OperationContract]
        int[] GetChannelIds(SearchArgs args);


        /// <summary>
        /// Gets a channel.
        /// </summary>
        /// <param name="channelId">The channel id for the channel to get.</param>
        /// <returns>The channel matching the given id.</returns>
        [OperationContract]
        Channel GetChannel( int channelId);

        [OperationContract]
        Channel ModifyChannel(int userId, int channelId);

        /// <summary>
        /// Deletes the channel.
        /// </summary>
        /// <param name="userId">The user id making the request, this must correspond to the channel owners id.</param>
        /// <param name="channelId">The channel id.</param>
        [OperationContract]
        void DeleteChannel(int userId, int channelId);

        /// <summary>
        /// Logins the user with the specified username and password.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>The id of the user. -1 if the (username,password) combination does not exist.</returns>
        [OperationContract]
        int Login(string username, string password);

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password for the user.</param>
        /// <param name="email">The email associated with user.</param>
        /// <returns>The id of the created user.</returns>
        [OperationContract]
        int CreateUser(string username, string password, string email);

        [OperationContract]
        void UploadTrack(Track track, int userId, int channelId);

        [OperationContract]
        void RemoveTrack(int userId, int trackId);

        [OperationContract]
        void VoteTrack(int rating, int userId, int trackId);

        [OperationContract]
        int[] GetTrackIds(int channelId);

        [OperationContract]
        TrackInfo GetTrackInfo(int trackId);

        /// <summary>
        /// Comments on the specified channel.
        /// </summary>
        /// <param name="comment">The comment.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="channelId">The channel id.</param>
        [OperationContract]
        void Comment(string comment, int userId, int channelId);

        [OperationContract]
        int[] GetCommentIds(int channelId);

        [OperationContract]
        Comment GetComment(int commentId);

        [OperationContract]
        void Subscribe(int userId, int channelId);

        [OperationContract]
        void UnSubscribe(int userId, int channelId);

        [OperationContract]
        int GetChannelPort(int channelId, int ipAddress, int port);

    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
