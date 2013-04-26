using System;
using System.Collections.ObjectModel;
using System.ServiceModel;

namespace RentItServer.ITU
{
    [ServiceContract]
    public interface IRentItService
    {
        /// <summary>
        /// Logins the user with the specified usernameOrEmail and password.
        /// </summary>
        /// <param name="usernameOrEmail">The usernameOrEmail of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns>The User. null if the (usernameOrEmail,password) combination does not exist.</returns>
        [OperationContract]
        DatabaseWrapperObjects.User Login(string usernameOrEmail, string password);

        /// <summary>
        /// Signs up a user
        /// </summary>
        /// <param name="usernameOrEmail">The usernameOrEmail of the user.</param>
        /// <param name="email">The email associated with user.</param>
        /// <param name="password">The password for the user.</param>
        /// <returns>The id of the created user.</returns>
        [OperationContract]
        DatabaseWrapperObjects.User SignUp(string usernameOrEmail, string email, string password);

        /// <summary>
        /// Delete the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <exception cref="System.ArgumentException">No user with user id [+userId+]</exception>
        [OperationContract]
        void DeleteUser(int userId);

        /// <summary>
        /// Gets the user with specified user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>The user</returns>
        /// <exception cref="System.ArgumentException">No user with user id [ + userId + ]</exception>
        [OperationContract]
        DatabaseWrapperObjects.User GetUser(int userId);

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns>The users</returns>
        [OperationContract]
        int[] GetAllUserIds();

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="username">The username. Can be null.</param>
        /// <param name="password">The password. Can be null.</param>
        /// <param name="email">The email. Can be null</param>
        /// <exception cref="System.ArgumentException">No user with user id[ + userId + ]</exception>
        [OperationContract]
        void UpdateUser(int userId, string username, string password, string email);
        
        /// <summary>
        /// Creates a channel.
        /// </summary>
        /// <param name="channelName">SearchString of the channel.</param>
        /// <param name="userId">The id of the user creating the channel.</param>
        /// <param name="description">The description of the channel.</param>
        /// <param name="genres">The genres associated with the channel.</param>
        /// <returns>The id of the created channel. -1 if the channel creation failed.</returns>
        [OperationContract]
        int CreateChannel(string channelName, int userId, string description, string[] genres);

        /// <summary>
        /// Deletes the channel.
        /// </summary>
        /// <param name="userId">The user id making the request, this must correspond to the channel owners id.</param>
        /// <param name="channelId">The channel id.</param>
        [OperationContract]
        void DeleteChannel(int userId, int channelId);

        /// <summary>
        /// Updates the channel.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <param name="ownerId">The owner id. Can be null.</param>
        /// <param name="channelName">SearchString of the channel. Can be null.</param>
        /// <param name="description">The description. Can be null.</param>
        /// <param name="hits">The hits. Can be null.</param>
        /// <param name="rating">The rating. Can be null.</param>
        /// <exception cref="System.ArgumentException">No channel with channel id [ + channelId + ]
        /// or
        /// No user with user id [ + ownerId + ]</exception>
        [OperationContract]
        void UpdateChannel(int channelId, int? ownerId, string channelName, string description, double? hits, double? rating);

        /// <summary>
        /// Gets a channel.
        /// </summary>
        /// <param name="channelId">The channel id for the channel to get.</param>
        /// <returns>The channel matching the given id.</returns>
        [OperationContract]
        DatabaseWrapperObjects.Channel GetChannel(int channelId);

        /// <summary>
        /// Gets all channels.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        int[] GetAllChannelIds();

        /// <summary>
        /// Gets the channel ids matching the given search arguments.
        /// </summary>
        /// <param name="args">The search arguments (used for filtering).</param>
        /// <returns>An array of channel ids matching search criteria. </returns>
        [OperationContract]
        DatabaseWrapperObjects.Channel[] GetChannels(ChannelSearchArgs args);
        
        /// <summary>
        /// Creates a vote.
        /// </summary>
        /// <param name="rating">The rating.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="trackId">The track id.</param>
        /// <exception cref="System.ArgumentException">
        /// no track with track id [+trackId+]
        /// or
        /// no user with user id [+userId+]
        /// </exception>
        [OperationContract]
        void CreateVote(int rating, int userId, int trackId);

        /// <summary>
        /// Gets the track info associated with the track.
        /// </summary>
        /// <param name="trackId">The track id.</param>
        /// <returns></returns>
        [OperationContract]
        DatabaseWrapperObjects.Track GetTrackInfo(int trackId);
        
        /// <summary>
        /// Removes the track.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="trackId">The track id.</param>
        [OperationContract]
        void RemoveTrack(int userId, int trackId);

        /// <summary>
        /// Gets the track ids associated witht he channel.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <returns></returns>
        [OperationContract]
        int[] GetTrackIds(int channelId);

        [OperationContract]
        DatabaseWrapperObjects.Track[] GetTracks(int channelId, TrackSearchArgs args);

        /// <summary>
        /// Comments on the specified channel.
        /// </summary>
        /// <param name="comment">The comment.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="channelId">The channel id.</param>
        /// <exception cref="System.ArgumentException">
        /// No user with user id [+userId+]
        /// or
        /// No channel with channel id [ + channelId + ]
        /// </exception>
        [OperationContract]
        void CreateComment(string comment, int userId, int channelId);

        /// <summary>
        /// Deletes the comment.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="date">The date of the comment.</param>
        /// <exception cref="System.ArgumentException">No comment with channelId [+channelId+] and userId [+userId+] and date [+date+]</exception>
        [OperationContract]
        void DeleteComment(int channelId, int userId, DateTime date);
        
        /// <summary>
        /// Gets a specific comment.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="date">The date of the comment.</param>
        /// <returns>The comment</returns>
        [OperationContract]
        DatabaseWrapperObjects.Comment GetComment(int channelId, int userId, DateTime date);

        /// <summary>
        /// Gets the comments from a specific user in a specific channel.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>All comments from a channel made by a specific user</returns>
        DatabaseWrapperObjects.Comment[] GetComments(int channelId, int userId, int fromInclusive, int toExclusive);
        
        /// <summary>
        /// Gets all comments associated with a channel
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <returns>
        /// All comments from a specific channel
        /// </returns>
        DatabaseWrapperObjects.Comment[] GetChannelComments(int channelId, int fromInclusive, int toExclusive);

        /// <summary>
        /// Gets all comments associated with a user
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>
        /// All comments from a specific user
        /// </returns>
        DatabaseWrapperObjects.Comment[] GetUserComments(int userId, int fromInclusive, int toExclusive);

            /// <summary>
        /// Gets the comment.
        /// </summary>
        /// <param name="commentId">The comment id.</param>
        /// <returns></returns>
        [OperationContract]
        DatabaseWrapperObjects.Comment GetComment(int commentId);

        
        /// <summary>
        /// Determines whether [is email available] [the specified email].
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>
        ///   <c>true</c> if [is email available] [the specified email]; otherwise, <c>false</c>.
        /// </returns>
        [OperationContract]
        bool IsEmailAvailable(string email);

        /// <summary>
        /// Determines whether [is username available] [the specified username].
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        ///   <c>true</c> if [is username available] [the specified username]; otherwise, <c>false</c>.
        /// </returns>
        [OperationContract]
        bool IsUsernameAvailable(string username);

        /// <summary>
        /// Subscribes the specified user id to the specified channel.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="channelId">The channel id.</param>
        [OperationContract]
        void Subscribe(int userId, int channelId);

        /// <summary>
        /// Unsubscribes the specified user id to the specified channel.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="channelId">The channel id.</param>
        [OperationContract]
        void Unsubscribe(int userId, int channelId);

        /// <summary>
        /// Gets a channel port.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        [OperationContract]
        int GetChannelPort(int channelId, int ipAddress, int port);

        /// <summary>
        /// Listens to channel.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <returns>Returns the port which the client should connect to</returns>
        [OperationContract]
        int ListenToChannel(int channelId);
    }
}
