using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using RentItServer.ITU;

namespace RentItServer
{
    public class RentItService : IRentItService
    {
        private static readonly Controller _controller = Controller.GetInstance();


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
            return _controller.Login(usernameOrEmail, password);
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
            return _controller.SignUp(usernameOrEmail, email, password);
        }

        /// <summary>
        /// Delete the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        public void DeleteUser(int userId)
        {
            _controller.DeleteUser(userId);
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
            return _controller.GetUser(userId);
        }

        public bool IsCorrectPassword(int userId, string password)
        {
            return _controller.IsCorrectPassword(userId, password);
        }

        public int[] GetAllUserIds()
        {
            IEnumerable<int> theUsers = _controller.GetAllUserIds();
            return theUsers.ToArray();
        }

        public void UpdateUser(int userId, string username, string password, string email)
        {
            _controller.UpdateUser(userId, username, password, email);
        }

        public int CreateChannel(string channelName, int userId, string description, string[] genres)
        {
            return _controller.CreateChannel(channelName, userId, description, genres);
        }

        public void DeleteChannel(int channelId)
        {
            _controller.DeleteChannel(channelId);
        }

        public void UpdateChannel(int channelId, int? ownerId, string channelName, string description, double? hits, double? rating)
        {
            _controller.UpdateChannel(channelId, ownerId, channelName, description, hits, rating);
        }

        public void IncrementHitsForChannel(int channelId)
        {
            _controller.IncrementHitsForChannel(channelId);
        }

        public ITU.DatabaseWrapperObjects.Channel GetChannel(int channelId)
        {
            return _controller.GetChannel(channelId);
        }

        public int[] GetAllChannelIds()
        {
            return _controller.GetAllChannelIds().ToArray();
        }

        public ITU.DatabaseWrapperObjects.Channel[] GetChannels(ChannelSearchArgs args)
        {
            return _controller.GetChannels(args);
        }

        public void CreateVote(int rating, int userId, int trackId)
        {
            _controller.CreateVote(rating, userId, trackId);
        }

        /// <summary>
        /// Adds the track.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="channelId">The channel id.</param>
        /// <param name="audioStream">The audio stream.</param>
        /// <param name="trackInfo">The track info. Get this by calling GetTrackInfroByStream.</param>
        public void AddTrack(int userId, int channelId, MemoryStream audioStream, ITU.DatabaseWrapperObjects.Track trackInfo)
        {
            _controller.AddTrack(userId, channelId, audioStream, trackInfo);
        }

        /// <summary>
        /// Gets the track info associated with the track stream.
        /// </summary>
        /// <param name="audioStream">The audio stream.</param>
        /// <returns></returns>
        public ITU.DatabaseWrapperObjects.Track GetTrackInfoByStream(MemoryStream audioStream)
        {
            return _controller.GetTrackInfo(audioStream);
        }

        /// <summary>
        /// Gets the track info associated with the track.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <param name="trackname">The trackname.</param>
        /// <returns></returns>
        public ITU.DatabaseWrapperObjects.Track GetTrackInfoByTrackname(int channelId, string trackname)
        {
            return _controller.GetTrackInfo(channelId, trackname);
        }

        public void RemoveTrack(int trackId)
        {
            _controller.RemoveTrack(trackId);
        }

        public int[] GetTrackIds(int channelId)
        {
            return _controller.GetTrackIds(channelId).ToArray();
        }

        public ITU.DatabaseWrapperObjects.Track[] GetTracks(int channelId, TrackSearchArgs args)
        {
            return _controller.GetTracks(channelId, args).ToArray();
        }

        public void CreateComment(string comment, int userId, int channelId)
        {
            _controller.CreateComment(comment, userId, channelId);
        }

        public void DeleteComment(int channelId, int userId, DateTime date)
        {
            _controller.DeleteComment(channelId, userId, date);
        }

        public ITU.DatabaseWrapperObjects.Comment[] GetChannelComments(int channelId, int? fromInclusive, int? toExclusive)
        {
            return _controller.GetChannelComments(channelId, fromInclusive, toExclusive);
        }

        public ITU.DatabaseWrapperObjects.Comment[] GetUserComments(int userId, int fromInclusive, int toExclusive)
        {
            return _controller.GetUserComments(userId, fromInclusive, toExclusive);
        }

        public bool IsEmailAvailable(string email)
        {
            return _controller.IsEmailAvailable(email);
        }

        public bool IsUsernameAvailable(string username)
        {
            return _controller.IsUsernameAvailable(username);
        }

        public void Subscribe(int userId, int channelId)
        {
            _controller.Subscribe(userId, channelId);
        }

        public void Unsubscribe(int userId, int channelId)
        {
            _controller.UnSubscribe(userId, channelId);
        }

        public int GetChannelPort(int channelId, int ipAddress, int port)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Starts the channel stream.
        /// </summary>
        /// <param name="cId">The id of the channel</param>
        public void StartChannelStream(int cId)
        {
            _controller.StartChannelStream(cId);
        }

        /// <summary>
        /// Stops the channel stream.
        /// </summary>
        /// <param name="cId">The id of the channel</param>
        public void StopChannel(int cId)
        {
            _controller.StopChannelStream(cId);
        }

        public ChannelSearchArgs GetDefaultChannelSearchArgs()
        {
            return _controller.GetDefaultChannelSearchArgs();
        }

        public TrackSearchArgs GetDefaultTrackSearchArgs()
        {
            return _controller.GetDefaultTrackSearchArgs();
        }

        public ITU.DatabaseWrapperObjects.Channel[] GetCreatedChannels(int userId)
        {
            return Channel.GetChannels(_controller.GetCreatedChannels(userId)).ToArray();
        }

        public ITU.DatabaseWrapperObjects.Channel[] GetSubscribedChannels(int userId)
        {
            return Channel.GetChannels(_controller.GetSubscribedChannels(userId)).ToArray();
        }

        public ITU.DatabaseWrapperObjects.Track[] GetTrackByChannelId(int channelId)
        {
            return Track.GetTracks(_controller.GetTracksByChannelId(channelId)).ToArray();
        }

        public bool IsChannelNameAvailable(string channelName)
        {
            return _controller.IsChannelNameAvailable(channelName);
        }

        public int GetSubscriberCount(int channelId)
        {
            return _controller.GetSubscriberCount(channelId);
        }

        public void IncrementChannelPlays(int channelId)
        {
            _controller.IncrementChannelPlays(channelId);
        }
    }
}
