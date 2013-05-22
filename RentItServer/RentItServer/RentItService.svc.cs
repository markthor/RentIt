using System.IO;
using RentItServer.ITU;

namespace RentItServer
{
    public class RentItService : IRentItService
    {
        private static readonly Controller Controller = Controller.GetInstance();
        private static readonly StreamHandler StreamHandler = StreamHandler.GetInstance();

        /// <summary>
        /// Logins the user with the specified usernameOrEmail and password.
        /// </summary>
        /// <param name="usernameOrEmail">The usernameOrEmail of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// The User. null if the (usernameOrEmail,password) combination does not exist.
        /// </returns>
        public ITU.DatabaseWrapperObjects.User Login(string usernameOrEmail, string password)
        {
            return Controller.Login(usernameOrEmail, password);
        }

        /// <summary>
        /// Signs up a user
        /// </summary>
        /// <param name="usernameOrEmail">The usernameOrEmail of the user.</param>
        /// <param name="email">The email associated with user.</param>
        /// <param name="password">The password for the user.</param>
        /// <returns>
        /// The id of the created user.
        /// </returns>
        public ITU.DatabaseWrapperObjects.User SignUp(string usernameOrEmail, string email, string password)
        {
            return Controller.SignUp(usernameOrEmail, email, password);
        }

        /// <summary>
        /// Gets the user with specified user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>
        /// The user
        /// </returns>
        public ITU.DatabaseWrapperObjects.User GetUser(int userId)
        {
            return Controller.GetUser(userId);
        }

        /// <summary>
        /// Determines whether [is correct password] [the specified user id].
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        ///   <c>true</c> if [is correct password] [the specified user id]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsCorrectPassword(int userId, string password)
        {
            return Controller.IsCorrectPassword(userId, password);
        }

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="username">The username. Can be null.</param>
        /// <param name="password">The password. Can be null.</param>
        /// <param name="email">The email. Can be null</param>
        public void UpdateUser(int userId, string username, string password, string email)
        {
            Controller.UpdateUser(userId, username, password, email);
        }

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
        public int CreateChannel(string channelName, int userId, string description, int[] genreIds)
        {
            return Controller.CreateChannel(channelName, userId, description, genreIds);
        }

        /// <summary>
        /// Deletes the channel.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        public void DeleteChannel(int channelId)
        {
            Controller.DeleteChannel(channelId);
        }

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
        public void UpdateChannel(int channelId, int? ownerId, string channelName, string description, double? hits, double? rating, int[] genreIds)
        {
            Controller.UpdateChannel(channelId, ownerId, channelName, description, hits, rating, genreIds);
        }

        /// <summary>
        /// Gets a channel.
        /// </summary>
        /// <param name="channelId">The channel id for the channel to get.</param>
        /// <returns>
        /// The channel matching the given id.
        /// </returns>
        public ITU.DatabaseWrapperObjects.Channel GetChannel(int channelId)
        {
            return Controller.GetChannel(channelId);
        }

        /// <summary>
        /// Gets the channel ids matching the given search arguments.
        /// </summary>
        /// <param name="args">The search arguments (used for filtering).</param>
        /// <returns>
        /// An array of channel ids matching search criteria.
        /// </returns>
        public ITU.DatabaseWrapperObjects.Channel[] GetChannels(ChannelSearchArgs args)
        {
            return Controller.GetChannels(args);
        }

        /// <summary>
        /// Creates a vote.
        /// </summary>
        /// <param name="rating">The rating.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="trackId">The track id.</param>
        public void CreateVote(int rating, int userId, int trackId)
        {
            Controller.CreateVote(rating, userId, trackId);
        }

        /// <summary>
        /// Adds the track.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="channelId">The channel id.</param>
        /// <param name="audioStream">The audio stream.</param>
        public void AddTrack(int userId, int channelId, MemoryStream audioStream)
        {
            Controller.AddTrack(userId, channelId, audioStream);
        }

        /// <summary>
        /// Removes the track.
        /// </summary>
        /// <param name="trackId">The track id.</param>
        public void RemoveTrack(int trackId)
        {
            Controller.RemoveTrack(trackId);
        }

        /// <summary>
        /// Gets the specified number of most recently played tracks
        /// </summary>
        /// <param name="channelId">The id of the channel</param>
        /// <param name="numberOfTracks">The number of tracks to retrieve</param>
        /// <returns>
        /// The most recently played tracks
        /// </returns>
        public ITU.DatabaseWrapperObjects.Track[] GetRecentlyPlayedTracks(int channelId, int numberOfTracks)
        {
            return Controller.GetRecentlyPlayedTracks(channelId, numberOfTracks).ToArray();
        }

        /// <summary>
        /// Comments on the specified channel.
        /// </summary>
        /// <param name="comment">The comment.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="channelId">The channel id.</param>
        public void CreateComment(string comment, int userId, int channelId)
        {
            Controller.CreateComment(comment, userId, channelId);
        }

        /// <summary>
        /// Gets all comments associated with a channel
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <param name="fromInclusive"></param>
        /// <param name="toExclusive"></param>
        /// <returns>
        /// All comments from a specific channel
        /// </returns>
        public ITU.DatabaseWrapperObjects.Comment[] GetChannelComments(int channelId, int? fromInclusive, int? toExclusive)
        {
            return Controller.GetChannelComments(channelId, fromInclusive, toExclusive);
        }

        /// <summary>
        /// Determines whether [is email available] [the specified email].
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>
        ///   <c>true</c> if [is email available] [the specified email]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsEmailAvailable(string email)
        {
            return Controller.IsEmailAvailable(email);
        }

        /// <summary>
        /// Determines whether [is username available] [the specified username].
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        ///   <c>true</c> if [is username available] [the specified username]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsUsernameAvailable(string username)
        {
            return Controller.IsUsernameAvailable(username);
        }

        /// <summary>
        /// Subscribes the specified user id to the specified channel.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="channelId">The channel id.</param>
        public void Subscribe(int userId, int channelId)
        {
            Controller.Subscribe(userId, channelId);
        }

        /// <summary>
        /// Unsubscribes the specified user id to the specified channel.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="channelId">The channel id.</param>
        public void Unsubscribe(int userId, int channelId)
        {
            Controller.UnSubscribe(userId, channelId);
        }

        /// <summary>
        /// Starts the channel stream.
        /// </summary>
        /// <param name="cId">The id of the channel</param>
        public void StartChannelStream(int cId)
        {
            StreamHandler.ManualStreamStart(cId);
        }

        /// <summary>
        /// Stops the running ezstream for the the given channelid
        /// </summary>
        /// <param name="channelId">The id of the channel which stream should be stopped</param>
        public void StopChannelStream(int channelId)
        {
            StreamHandler.StopChannelStream(channelId);
        }

        /// <summary>
        /// Gets a channel search args object with all fields having default values.
        /// </summary>
        /// <returns></returns>
        public ChannelSearchArgs GetDefaultChannelSearchArgs()
        {
            return Controller.GetDefaultChannelSearchArgs();
        }

        /// <summary>
        /// Gets the channels created by the user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ITU.DatabaseWrapperObjects.Channel[] GetCreatedChannels(int userId)
        {
            return Channel.GetChannels(Controller.GetCreatedChannels(userId)).ToArray();
        }

        /// <summary>
        /// Gets the channels this user is subscribed to
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ITU.DatabaseWrapperObjects.Channel[] GetSubscribedChannels(int userId)
        {
            return Channel.GetChannels(Controller.GetSubscribedChannels(userId)).ToArray();
        }

        /// <summary>
        /// Gets the track assosiated with a channel
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public ITU.DatabaseWrapperObjects.Track[] GetTrackByChannelId(int channelId)
        {
            return Track.GetTracks(Controller.GetTracksByChannelId(channelId)).ToArray();
        }

        /// <summary>
        /// Check if the given channelname is already used
        /// </summary>
        /// <param name="channelId">Id of a channel which name should not be taken into account when checking</param>
        /// <param name="channelName">The name to lookup</param>
        /// <returns></returns>
        public bool IsChannelNameAvailable(int channelId, string channelName)
        {
            return Controller.IsChannelNameAvailable(channelId, channelName);
        }

        /// <summary>
        /// Retreives the amount of subscribers to the given channel
        /// </summary>
        /// <param name="channelId">The id of the channel to get count for</param>
        /// <returns>
        /// The amout of subscribers
        /// </returns>
        public int GetSubscriberCount(int channelId)
        {
            return Controller.GetSubscriberCount(channelId);
        }

        /// <summary>
        /// Increments the channel's hit property with 1
        /// </summary>
        /// <param name="channelId">The id of the channel which should have it's property incremented</param>
        public void IncrementChannelPlays(int channelId)
        {
            Controller.IncrementChannelPlays(channelId);
        }

        /// <summary>
        /// Checks if a channel is currently streaming
        /// </summary>
        /// <param name="channelId">Id of the channel to check</param>
        /// <returns>
        /// Whether or not the channel is streaming
        /// </returns>
        public bool IsChannelPlaying(int channelId)
        {
            return StreamHandler.IsChannelStreamRunning(channelId);
        }

        /// <summary>
        /// Retreives the vote from a user on a track
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="trackId">The id of the track</param>
        /// <returns></returns>
        public ITU.DatabaseWrapperObjects.Vote GetVote(int userId, int trackId)
        {
            return Controller.GetVote(userId, trackId);
        }

        /// <summary>
        /// Deletes the vote from a user on a track
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="trackId">The id of the track</param>
        public void DeleteVote(int userId, int trackId)
        {
            Controller.DeleteVote(userId, trackId);
        }

        public int CountAllChannelsWithFilter(ChannelSearchArgs filter)
        {
            return Controller.CountAllChannelsWithFilter(filter);
        }

        /// <summary>
        /// Counts the total amount of upvotes given to a track
        /// </summary>
        /// <param name="trackId">The id of the track</param>
        /// <returns>The total amount of upvotes</returns>
        public int CountAllUpvotes(int trackId)
        {
            return Controller.CountAllUpvotes(trackId);
        }

        /// <summary>
        /// Counts the total amount of downvotes given to a track
        /// </summary>
        /// <param name="trackId">The id of the track</param>
        /// <returns>The total amount of dwonvotes</returns>
        public int CountAllDownvotes(int trackId)
        {
            return Controller.CountAllDownvotes(trackId);
        }

        /// <summary>
        /// Retreives all genres
        /// </summary>
        /// <returns>An array of all genres in the database</returns>
        public ITU.DatabaseWrapperObjects.Genre[] GetAllGenres()
        {
            return Genre.GetTracks(Controller.GetAllGenres()).ToArray();
        }

        /// <summary>
        /// Retreives all genres associated with the given channel
        /// </summary>
        /// <param name="channelId">The id of the channel</param>
        /// <returns>Array of asociated genres</returns>
        public ITU.DatabaseWrapperObjects.Genre[] GetGenresForChannel(int channelId)
        {
            return Genre.GetTracks(Controller.GetGenresForChannel(channelId)).ToArray();
        }

        /// <summary>
        /// Counts the total amount of comments for a channel
        /// </summary>
        /// <param name="channelId">The id of the channel</param>
        /// <returns>The total amount of comments</returns>
        public int GetCountChannelComments(int channelId)
        {
            return Controller.GetCountChannelComments(channelId);
        }

        /// <summary>
        /// Counts the amount of channels which pass the given ChannelSearchArgs filter
        /// </summary>
        /// <param name="filter">ChannelSearchArgs filter to apply to all channels</param>
        /// <returns>The amount of channels passing the filter</returns>
        public int CountChannelsPassingFilter(ChannelSearchArgs filter)
        {
            return Controller.CountChannelsPassingFilter(filter);
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        public void DeleteAccount(int userId)
        {
            Controller.DeleteUser(userId);
        }
    }
}