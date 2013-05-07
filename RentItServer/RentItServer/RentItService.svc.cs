using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using RentItServer.ITU;
using RentItServer.SMU;

namespace RentItServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
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
            User theUser = _controller.Login(usernameOrEmail, password);
            return theUser.GetUser();
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
            User theUser = _controller.SignUp(usernameOrEmail, email, password);
            return theUser.GetUser();
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
            User theUser = _controller.GetUser(userId);
            return theUser.GetUser();
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
        
        public void DeleteChannel(int userId, int channelId)
        {
            _controller.DeleteChannel(userId, channelId);
        }

        public void UpdateChannel(int channelId, int? ownerId, string channelName, string description, double? hits, double? rating)
        {
            _controller.UpdateChannel(channelId, ownerId, channelName, description, hits, rating);
        }

        public ITU.DatabaseWrapperObjects.Channel GetChannel(int channelId)
        {
            Channel theChannel = _controller.GetChannel(channelId);
            return theChannel.GetChannel();
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
        public void AddTrack(int userId, int channelId, MemoryStream audioStream, Track trackInfo)
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
            return _controller.GetTrackInfo(audioStream).GetTrack();
        }

        /// <summary>
        /// Gets the track info associated with the track.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        /// <param name="trackname">The trackname.</param>
        /// <returns></returns>
        public ITU.DatabaseWrapperObjects.Track GetTrackInfoByTrackname(int channelId, string trackname)
        {
            Track theTrack = _controller.GetTrackInfo(channelId, trackname);
            return theTrack.GetTrack();
        }
        
        public void RemoveTrack(int userId, int trackId)
        {
            _controller.RemoveTrack(userId, trackId);
        }

        public int[] GetTrackIds(int channelId)
        {
            return _controller.GetTrackIds(channelId).ToArray();
        }

        public ITU.DatabaseWrapperObjects.Track[] GetTracks(int channelId, TrackSearchArgs args)
        {
            IEnumerable<Track> theRawTracks = _controller.GetTracks(channelId, args);
            List<ITU.DatabaseWrapperObjects.Track> theTracks = new List<ITU.DatabaseWrapperObjects.Track>();
            foreach (Track track in theRawTracks)
            {
                theTracks.Add(track.GetTrack());
            }
            return theTracks.ToArray();
        }

        public void CreateComment(string comment, int userId, int channelId)
        {
            _controller.CreateComment(comment, userId, channelId);
        }

        public void DeleteComment(int channelId, int userId, DateTime date)
        {
            _controller.DeleteComment(channelId, userId, date);
        }

        public ITU.DatabaseWrapperObjects.Comment GetComment(int channelId, int userId, DateTime date)
        {
            throw new NotImplementedException();
            //CreateComment theComment = _controller.GetComment(channelId, userId, date);
            //return theComment.GetComment();
        }

        public ITU.DatabaseWrapperObjects.Comment[] GetComments(int channelId, int userId, int fromInclusive, int toExclusive)
        {
            throw new NotImplementedException();
            //List<CreateComment> theRawComments = _controller.GetComments(channelId, userId, fromInclusive, toExclusive);
            //List<ITU.DatabaseWrapperObjects.CreateComment> theComments = new List<ITU.DatabaseWrapperObjects.CreateComment>();
            //foreach (CreateComment comment in theRawComments)
            //{
            //    theComments.Add(comment.GetComment());
            //}
            //return theComments.ToArray();
        }

        public ITU.DatabaseWrapperObjects.Comment[] GetChannelComments(int channelId, int fromInclusive, int toExclusive)
        {
            throw new NotImplementedException();
            //List<CreateComment> theRawComments = _controller.GetComments(channelId, null, fromInclusive, toExclusive);
            //List<ITU.DatabaseWrapperObjects.CreateComment> theComments = new List<ITU.DatabaseWrapperObjects.CreateComment>();
            //foreach (CreateComment comment in theRawComments)
            //{
            //    theComments.Add(comment.GetComment());
            //}
            //return theComments.ToArray();
        }

        public ITU.DatabaseWrapperObjects.Comment[] GetUserComments(int userId, int fromInclusive, int toExclusive)
        {
            throw new NotImplementedException();
            //List<CreateComment> theRawComments = _controller.GetComments(null, userId, fromInclusive, toExclusive);
            //List<ITU.DatabaseWrapperObjects.CreateComment> theComments = new List<ITU.DatabaseWrapperObjects.CreateComment>();
            //foreach (CreateComment comment in theRawComments)
            //{
            //    theComments.Add(comment.GetComment());
            //}
            //return theComments.ToArray();
        }

        //public ITU.DatabaseWrapperObjects.Comment GetComment(int commentId)
        //{
        //    Comment theComment = _controller.GetComment(commentId);
        //    return theComment.GetComment();
        //}

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

        public int ListenToChannel(int channelId)
        {
            return _controller.ListenToChannel(channelId);
        }

        /// <summary>
        /// Starts the channel stream.
        /// </summary>
        /// <param name="cId">The id of the channel</param>
        public void startChannel(int cId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Stops the channel stream.
        /// </summary>
        /// <param name="cId">The id of the channel</param>
        public void stopChannel(int cId)
        {
            throw new NotImplementedException();
        }
    }
}
