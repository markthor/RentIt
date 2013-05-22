using System.IO;
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
        /// Gets the user with specified user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>The user</returns>
        /// <exception cref="System.ArgumentException">No user with user id [ + userId + ]</exception>
        [OperationContract]
        DatabaseWrapperObjects.User GetUser(int userId);

        [OperationContract]
        bool IsCorrectPassword(int userId, string password);

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
        /// <param name="channelName">Name of the channel.</param>
        /// <param name="userId">The id of the user creating the channel.</param>
        /// <param name="description">The description of the channel.</param>
        /// <param name="genreIds">The genre ids.</param>
        /// <returns>
        /// The id of the created channel. -1 if the channel creation failed.
        /// </returns>
        [OperationContract]
        int CreateChannel(string channelName, int userId, string description, int[] genreIds);

        /// <summary>
        /// Deletes the channel.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        [OperationContract]
        void DeleteChannel(int channelId);

        /// <summary>
        /// Updates the channel.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <param name="ownerId">The owner id. Can be null.</param>
        /// <param name="channelName">Name of the channel. Can be null.</param>
        /// <param name="description">The description. Can be null.</param>
        /// <param name="hits">The hits. Can be null.</param>
        /// <param name="rating">The rating. Can be null.</param>
        /// <param name="genreIds">The genre ids.</param>
        /// <exception cref="System.ArgumentException">No channel with channel id [ + channelId + ]
        /// or
        /// No user with user id [ + ownerId + ]</exception>
        [OperationContract]
        void UpdateChannel(int channelId, int? ownerId, string channelName, string description, double? hits, double? rating, int[] genreIds);

        /// <summary>
        /// Gets a channel.
        /// </summary>
        /// <param name="channelId">The channel id for the channel to get.</param>
        /// <returns>The channel matching the given id.</returns>
        [OperationContract]
        DatabaseWrapperObjects.Channel GetChannel(int channelId);

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
        /// <param name="rating">The rating. -1 for downvote, 1 for upvote. Any other value than -1 or 1 will be have no effect</param>
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
        /// Adds the track.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="channelId">The channel id.</param>
        /// <param name="audioStream">The audio stream.</param>
        [OperationContract]
        void AddTrack(int userId, int channelId, MemoryStream audioStream);

        /// <summary>
        /// Removes the track.
        /// </summary>
        /// <param name="trackId">The track id.</param>
        [OperationContract]
        void RemoveTrack(int trackId);

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
        /// Gets all comments associated with a channel
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <param name="fromInclusive">From inclusive.</param>
        /// <param name="toExclusive">To exclusive.</param>
        /// <returns>
        /// All comments from a specific channel
        /// </returns>
        [OperationContract]
        DatabaseWrapperObjects.Comment[] GetChannelComments(int channelId, int? fromInclusive, int? toExclusive);

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
        /// Starts the channel stream.
        /// </summary>
        /// <param name="cId">The id of the channel</param>
        [OperationContract]
        void StartChannelStream(int cId);

        /// <summary>
        /// Gets a channel search args object with all fields having default values.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ChannelSearchArgs GetDefaultChannelSearchArgs();

        /// <summary>
        /// Gets the channels created by the user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [OperationContract]
        DatabaseWrapperObjects.Channel[] GetCreatedChannels(int userId);

        /// <summary>
        /// Gets the channels this user is subscribed to
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [OperationContract]
        DatabaseWrapperObjects.Channel[] GetSubscribedChannels(int userId);

        /// <summary>
        /// Gets the track assosiated with a channel
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        [OperationContract]
        DatabaseWrapperObjects.Track[] GetTrackByChannelId(int channelId);

        /// <summary>
        /// Gets the specified number of most recently played tracks
        /// </summary>
        /// <param name="channelId">The id of the channel</param>
        /// <param name="numberOfTracks">The number of tracks to retrieve</param>
        /// <returns>The most recently played tracks</returns>
        [OperationContract]
        DatabaseWrapperObjects.Track[] GetRecentlyPlayedTracks(int channelId, int numberOfTracks);
        
        /// <summary>
        /// Check if the given channelname is already used
        /// </summary>
        /// <param name="channelId">Id of a channel which name should not be taken into account when checking</param>
        /// <param name="channelName">The name to lookup</param>
        /// <returns></returns>
        [OperationContract]
        bool IsChannelNameAvailable(int channelId, string channelName);

        /// <summary>
        /// Retreives the amount of subscribers to the given channel
        /// </summary>
        /// <param name="channelId">The id of the channel to get count for</param>
        /// <returns>The amout of subscribers</returns>
        [OperationContract]
        int GetSubscriberCount(int channelId);

        /// <summary>
        /// Increments the channel's hit property with 1
        /// </summary>
        /// <param name="channelId">The id of the channel which should have it's property incremented</param>
        [OperationContract]
        void IncrementChannelPlays(int channelId);

        /// <summary>
        /// Checks if a channel is currently streaming
        /// </summary>
        /// <param name="channelId">Id of the channel to check</param>
        /// <returns>Whether or not the channel is streaming</returns>
        [OperationContract]
        bool IsChannelPlaying(int channelId);

        /// <summary>
        /// Stops the running ezstream for the the given channelid
        /// </summary>
        /// <param name="channelId">The id of the channel which stream should be stopped</param>
        [OperationContract]
        void StopChannelStream(int channelId);

        /// <summary>
        /// Retreives the vote from a user on a track
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="trackId">The id of the track</param>
        /// <returns></returns>
        [OperationContract]
        DatabaseWrapperObjects.Vote GetVote(int userId, int trackId);

        /// <summary>
        /// Deletes the vote from a user on a track
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="trackId">The id of the track</param>
        [OperationContract]
        void DeleteVote(int userId, int trackId);

        /// <summary>
        /// Count all channels which pass the given filter
        /// </summary>
        /// <param name="filter">The filter which should be applied on the channels</param>
        /// <returns>The number of channels which passes</returns>
        [OperationContract]
        int CountAllChannelsWithFilter(ChannelSearchArgs filter);

        /// <summary>
        /// Counts the total amount of upvotes given to a track
        /// </summary>
        /// <param name="trackId">The id of the track</param>
        /// <returns>The total amount of upvotes</returns>
        [OperationContract]
        int CountAllUpvotes(int trackId);

        /// <summary>
        /// Counts the total amount of downvotes given to a track
        /// </summary>
        /// <param name="trackId">The id of the track</param>
        /// <returns>The total amount of dwonvotes</returns>
        [OperationContract]
        int CountAllDownvotes(int trackId);

        /// <summary>
        /// Retreives all genres
        /// </summary>
        /// <returns>
        /// An array of all genres in the database
        /// </returns>
        [OperationContract]
        ITU.DatabaseWrapperObjects.Genre[] GetAllGenres();

        /// <summary>
        /// Retreives all genres associated with the given channel
        /// </summary>
        /// <param name="channelId">The id of the channel</param>
        /// <returns>Array of asociated genres</returns>
        [OperationContract]
        ITU.DatabaseWrapperObjects.Genre[] GetGenresForChannel(int channelId);

        /// <summary>
        /// Counts the total amount of comments for a channel
        /// </summary>
        /// <param name="channelId">The id of the channel</param>
        /// <returns>The total amount of comments</returns>
        [OperationContract]
        int GetCountChannelComments(int channelId);

        /// <summary>
        /// Counts the amount of channels which pass the given ChannelSearchArgs filter
        /// </summary>
        /// <param name="filter">ChannelSearchArgs filter to apply to all channels</param>
        /// <returns>The amount of channels passing the filter</returns>
        [OperationContract]
        int CountChannelsPassingFilter(ChannelSearchArgs filter);

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        [OperationContract]
        void DeleteAccount(int userId);
    }
}