using System;
namespace RentItServer.ITU
{
    interface IRentItServiceasdasdasdasd
    {
        void Comment(string comment, int userId, int channelId);
        int CreateChannel(string channelName, int userId, string description, string[] genres);
        int CreateUser(string username, string password, string email);
        void DeleteChannel(int userId, int channelId);
        RentItServer.Channel GetChannel(int channelId);
        int[] GetChannelIds(SearchArgs args);
        int GetChannelPort(int channelId, int ipAddress, int port);
        RentItServer.Comment GetComment(int commentId);
        int[] GetCommentIds(int channelId);
        int[] GetTrackIds(int channelId);
        TrackInfo GetTrackInfo(int trackId);
        int ListenToChannel(int channelId);
        int Login(string username, string password);
        RentItServer.Channel ModifyChannel(int userId, int channelId);
        void RemoveTrack(int userId, int trackId);
        void Subscribe(int userId, int channelId);
        void UnSubscribe(int userId, int channelId);
        void UploadTrack(RentItServer.Track track, int userId, int channelId);
        void VoteTrack(int rating, int userId, int trackId);
    }
}
