using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RentItServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RentItService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RentItService.svc or RentItService.svc.cs at the Solution Explorer and start debugging.
    public class RentItService : IRentItService
    {

        public int CreateChannel(string channelName, int userId, string description, int[] genres)
        {
            throw new NotImplementedException();
        }

        public int[] GetChannelIds(string channelName, SearchArgs args)
        {
            throw new NotImplementedException();
        }

        public Channel GetChannel(int channelId)
        {
            throw new NotImplementedException();
        }

        public Channel ModifyChannel(int channelId)
        {
            throw new NotImplementedException();
        }

        public void DeleteChannel(int channelId)
        {
            throw new NotImplementedException();
        }

        public int Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public int CreateUser(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void UploadTrack(Track track, int channelId)
        {
            throw new NotImplementedException();
        }

        public void RemoveTrack(int trackId)
        {
            throw new NotImplementedException();
        }

        public void VoteTrack(int rating, int userId, int trackId)
        {
            throw new NotImplementedException();
        }

        public int[] GetTrackIds(int channelId)
        {
            throw new NotImplementedException();
        }

        public TrackInfo GetTrackInfo(int trackId)
        {
            throw new NotImplementedException();
        }

        public void Comment(string comment, int userId, int channelId)
        {
            throw new NotImplementedException();
        }

        public int[] GetCommentIds(int channelId)
        {
            throw new NotImplementedException();
        }

        public Comment GetComment(int commentId)
        {
            throw new NotImplementedException();
        }

        public void Subscribe(int userId, int channelId)
        {
            throw new NotImplementedException();
        }

        public void UnSubscribe(int userId, int channelId)
        {
            throw new NotImplementedException();
        }
    }
}
