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
        [OperationContract]
        int CreateChannel(string channelName, int userId, string description, int[] genres);

        [OperationContract]
        int[] GetChannelIds(String channelName, SearchArgs args);

        [OperationContract]
        Channel GetChannel( int channelId);

        [OperationContract]
        Channel ModifyChannel(int userId, int channelId);

        [OperationContract]
        void DeleteChannel(int userId, int channelId);

        [OperationContract]
        int Login(string username, string password);

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
