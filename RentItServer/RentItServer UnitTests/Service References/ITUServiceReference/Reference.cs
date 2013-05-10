﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RentItServer_UnitTests.ITUServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ITUServiceReference.IRentItService")]
    public interface IRentItService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/Login", ReplyAction="http://tempuri.org/IRentItService/LoginResponse")]
        RentItServer.ITU.DatabaseWrapperObjects.User Login(string usernameOrEmail, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/SignUp", ReplyAction="http://tempuri.org/IRentItService/SignUpResponse")]
        RentItServer.ITU.DatabaseWrapperObjects.User SignUp(string usernameOrEmail, string email, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/DeleteUser", ReplyAction="http://tempuri.org/IRentItService/DeleteUserResponse")]
        void DeleteUser(int userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/GetUser", ReplyAction="http://tempuri.org/IRentItService/GetUserResponse")]
        RentItServer.ITU.DatabaseWrapperObjects.User GetUser(int userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/IsCorrectPassword", ReplyAction="http://tempuri.org/IRentItService/IsCorrectPasswordResponse")]
        bool IsCorrectPassword(int userId, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/GetAllUserIds", ReplyAction="http://tempuri.org/IRentItService/GetAllUserIdsResponse")]
        int[] GetAllUserIds();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/UpdateUser", ReplyAction="http://tempuri.org/IRentItService/UpdateUserResponse")]
        void UpdateUser(int userId, string username, string password, string email);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/CreateChannel", ReplyAction="http://tempuri.org/IRentItService/CreateChannelResponse")]
        int CreateChannel(string channelName, int userId, string description, string[] genres);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/DeleteChannel", ReplyAction="http://tempuri.org/IRentItService/DeleteChannelResponse")]
        void DeleteChannel(int userId, int channelId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/UpdateChannel", ReplyAction="http://tempuri.org/IRentItService/UpdateChannelResponse")]
        void UpdateChannel(int channelId, System.Nullable<int> ownerId, string channelName, string description, System.Nullable<double> hits, System.Nullable<double> rating);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/GetChannel", ReplyAction="http://tempuri.org/IRentItService/GetChannelResponse")]
        RentItServer.ITU.DatabaseWrapperObjects.Channel GetChannel(int channelId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/GetAllChannelIds", ReplyAction="http://tempuri.org/IRentItService/GetAllChannelIdsResponse")]
        int[] GetAllChannelIds();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/GetChannels", ReplyAction="http://tempuri.org/IRentItService/GetChannelsResponse")]
        RentItServer.ITU.DatabaseWrapperObjects.Channel[] GetChannels(RentItServer.ITU.ChannelSearchArgs args);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/CreateVote", ReplyAction="http://tempuri.org/IRentItService/CreateVoteResponse")]
        void CreateVote(int rating, int userId, int trackId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/AddTrack", ReplyAction="http://tempuri.org/IRentItService/AddTrackResponse")]
        void AddTrack(int userId, int channelId, System.IO.MemoryStream audioStream, RentItServer.Track trackInfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/GetTrackInfoByStream", ReplyAction="http://tempuri.org/IRentItService/GetTrackInfoByStreamResponse")]
        RentItServer.ITU.DatabaseWrapperObjects.Track GetTrackInfoByStream(System.IO.MemoryStream audioStream);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/GetTrackInfoByTrackname", ReplyAction="http://tempuri.org/IRentItService/GetTrackInfoByTracknameResponse")]
        RentItServer.ITU.DatabaseWrapperObjects.Track GetTrackInfoByTrackname(int channelId, string trackname);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/RemoveTrack", ReplyAction="http://tempuri.org/IRentItService/RemoveTrackResponse")]
        void RemoveTrack(int userId, int trackId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/GetTrackIds", ReplyAction="http://tempuri.org/IRentItService/GetTrackIdsResponse")]
        int[] GetTrackIds(int channelId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/GetTracks", ReplyAction="http://tempuri.org/IRentItService/GetTracksResponse")]
        RentItServer.ITU.DatabaseWrapperObjects.Track[] GetTracks(int channelId, RentItServer.ITU.TrackSearchArgs args);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/CreateComment", ReplyAction="http://tempuri.org/IRentItService/CreateCommentResponse")]
        void CreateComment(string comment, int userId, int channelId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/DeleteComment", ReplyAction="http://tempuri.org/IRentItService/DeleteCommentResponse")]
        void DeleteComment(int channelId, int userId, System.DateTime date);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/GetComment", ReplyAction="http://tempuri.org/IRentItService/GetCommentResponse")]
        RentItServer.ITU.DatabaseWrapperObjects.Comment GetComment(int channelId, int userId, System.DateTime date);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/IsEmailAvailable", ReplyAction="http://tempuri.org/IRentItService/IsEmailAvailableResponse")]
        bool IsEmailAvailable(string email);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/IsUsernameAvailable", ReplyAction="http://tempuri.org/IRentItService/IsUsernameAvailableResponse")]
        bool IsUsernameAvailable(string username);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/Subscribe", ReplyAction="http://tempuri.org/IRentItService/SubscribeResponse")]
        void Subscribe(int userId, int channelId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/Unsubscribe", ReplyAction="http://tempuri.org/IRentItService/UnsubscribeResponse")]
        void Unsubscribe(int userId, int channelId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/GetChannelPort", ReplyAction="http://tempuri.org/IRentItService/GetChannelPortResponse")]
        int GetChannelPort(int channelId, int ipAddress, int port);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/ListenToChannel", ReplyAction="http://tempuri.org/IRentItService/ListenToChannelResponse")]
        int ListenToChannel(int channelId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/startChannel", ReplyAction="http://tempuri.org/IRentItService/startChannelResponse")]
        void startChannel(int cId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/stopChannel", ReplyAction="http://tempuri.org/IRentItService/stopChannelResponse")]
        void stopChannel(int cId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/GetDefaultChannelSearchArgs", ReplyAction="http://tempuri.org/IRentItService/GetDefaultChannelSearchArgsResponse")]
        RentItServer.ITU.ChannelSearchArgs GetDefaultChannelSearchArgs();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRentItService/GetDefaultTrackSearchArgs", ReplyAction="http://tempuri.org/IRentItService/GetDefaultTrackSearchArgsResponse")]
        RentItServer.ITU.TrackSearchArgs GetDefaultTrackSearchArgs();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IRentItServiceChannel : RentItServer_UnitTests.ITUServiceReference.IRentItService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class RentItServiceClient : System.ServiceModel.ClientBase<RentItServer_UnitTests.ITUServiceReference.IRentItService>, RentItServer_UnitTests.ITUServiceReference.IRentItService {
        
        public RentItServiceClient() {
        }
        
        public RentItServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public RentItServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public RentItServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public RentItServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public RentItServer.ITU.DatabaseWrapperObjects.User Login(string usernameOrEmail, string password) {
            return base.Channel.Login(usernameOrEmail, password);
        }
        
        public RentItServer.ITU.DatabaseWrapperObjects.User SignUp(string usernameOrEmail, string email, string password) {
            return base.Channel.SignUp(usernameOrEmail, email, password);
        }
        
        public void DeleteUser(int userId) {
            base.Channel.DeleteUser(userId);
        }
        
        public RentItServer.ITU.DatabaseWrapperObjects.User GetUser(int userId) {
            return base.Channel.GetUser(userId);
        }
        
        public bool IsCorrectPassword(int userId, string password) {
            return base.Channel.IsCorrectPassword(userId, password);
        }
        
        public int[] GetAllUserIds() {
            return base.Channel.GetAllUserIds();
        }
        
        public void UpdateUser(int userId, string username, string password, string email) {
            base.Channel.UpdateUser(userId, username, password, email);
        }
        
        public int CreateChannel(string channelName, int userId, string description, string[] genres) {
            return base.Channel.CreateChannel(channelName, userId, description, genres);
        }
        
        public void DeleteChannel(int userId, int channelId) {
            base.Channel.DeleteChannel(userId, channelId);
        }
        
        public void UpdateChannel(int channelId, System.Nullable<int> ownerId, string channelName, string description, System.Nullable<double> hits, System.Nullable<double> rating) {
            base.Channel.UpdateChannel(channelId, ownerId, channelName, description, hits, rating);
        }
        
        public RentItServer.ITU.DatabaseWrapperObjects.Channel GetChannel(int channelId) {
            return base.Channel.GetChannel(channelId);
        }
        
        public int[] GetAllChannelIds() {
            return base.Channel.GetAllChannelIds();
        }
        
        public RentItServer.ITU.DatabaseWrapperObjects.Channel[] GetChannels(RentItServer.ITU.ChannelSearchArgs args) {
            return base.Channel.GetChannels(args);
        }
        
        public void CreateVote(int rating, int userId, int trackId) {
            base.Channel.CreateVote(rating, userId, trackId);
        }
        
        public void AddTrack(int userId, int channelId, System.IO.MemoryStream audioStream, RentItServer.Track trackInfo) {
            base.Channel.AddTrack(userId, channelId, audioStream, trackInfo);
        }
        
        public RentItServer.ITU.DatabaseWrapperObjects.Track GetTrackInfoByStream(System.IO.MemoryStream audioStream) {
            return base.Channel.GetTrackInfoByStream(audioStream);
        }
        
        public RentItServer.ITU.DatabaseWrapperObjects.Track GetTrackInfoByTrackname(int channelId, string trackname) {
            return base.Channel.GetTrackInfoByTrackname(channelId, trackname);
        }
        
        public void RemoveTrack(int userId, int trackId) {
            base.Channel.RemoveTrack(userId, trackId);
        }
        
        public int[] GetTrackIds(int channelId) {
            return base.Channel.GetTrackIds(channelId);
        }
        
        public RentItServer.ITU.DatabaseWrapperObjects.Track[] GetTracks(int channelId, RentItServer.ITU.TrackSearchArgs args) {
            return base.Channel.GetTracks(channelId, args);
        }
        
        public void CreateComment(string comment, int userId, int channelId) {
            base.Channel.CreateComment(comment, userId, channelId);
        }
        
        public void DeleteComment(int channelId, int userId, System.DateTime date) {
            base.Channel.DeleteComment(channelId, userId, date);
        }
        
        public RentItServer.ITU.DatabaseWrapperObjects.Comment GetComment(int channelId, int userId, System.DateTime date) {
            return base.Channel.GetComment(channelId, userId, date);
        }
        
        public bool IsEmailAvailable(string email) {
            return base.Channel.IsEmailAvailable(email);
        }
        
        public bool IsUsernameAvailable(string username) {
            return base.Channel.IsUsernameAvailable(username);
        }
        
        public void Subscribe(int userId, int channelId) {
            base.Channel.Subscribe(userId, channelId);
        }
        
        public void Unsubscribe(int userId, int channelId) {
            base.Channel.Unsubscribe(userId, channelId);
        }
        
        public int GetChannelPort(int channelId, int ipAddress, int port) {
            return base.Channel.GetChannelPort(channelId, ipAddress, port);
        }
        
        public int ListenToChannel(int channelId) {
            return base.Channel.ListenToChannel(channelId);
        }
        
        public void startChannel(int cId) {
            base.Channel.startChannel(cId);
        }
        
        public void stopChannel(int cId) {
            base.Channel.stopChannel(cId);
        }
        
        public RentItServer.ITU.ChannelSearchArgs GetDefaultChannelSearchArgs() {
            return base.Channel.GetDefaultChannelSearchArgs();
        }
        
        public RentItServer.ITU.TrackSearchArgs GetDefaultTrackSearchArgs() {
            return base.Channel.GetDefaultTrackSearchArgs();
        }
    }
}