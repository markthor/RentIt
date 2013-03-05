using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RentItServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRentItService" in both code and config file together.
    [ServiceContract]
    public interface IRentItService
    {
        [OperationContract]
        int CreateChannel(string channelName, int userId, string description, int[] genres);

        [OperationContract]
        int[] GetChannelIds(String channelName, SearchArgs args);
        
        [OperationContract]
        Channel GetChannel(int channelId);
        
        [OperationContract]
        Channel ModifyChannel(int channelId);
        
        [OperationContract]
        void DeleteChannel(int channelId);
        
        [OperationContract]
        int Login(string username, string password);
        
        [OperationContract]
        int CreateUser(string username, string password);
        
        [OperationContract]
        void UploadTrack(Track track, int channelId);
        
        [OperationContract]
        void RemoveTrack(int trackId);
        
        [OperationContract]
        void VoteTrack(int rating, int userId, int trackId);
        
        [OperationContract]
        int[] GetTrackIds(int channelId);
        
        [OperationContract]
        TrackInfo GetTrackInfo(int trackId);
        
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

}