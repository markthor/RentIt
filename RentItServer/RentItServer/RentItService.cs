﻿using System;
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
            return 0;
        }

        public int[] GetChannelIds(string channelName, SearchArgs args)
        {
            return new []{0};
        }

        public Channel GetChannel(int channelId)
        {
            return new Channel();
        }

        public Channel ModifyChannel(int channelId)
        {
            return new Channel();
        }

        public void DeleteChannel(int channelId)
        {            
        }

        public int Login(string username, string password)
        {
            return 0;
        }

        public int CreateUser(string username, string password)
        {
            return 0;
        }

        public void UploadTrack(Track track, int channelId)
        {
        }

        public void RemoveTrack(int trackId)
        {
        }

        public void VoteTrack(int rating, int userId, int trackId)
        {
        }

        public int[] GetTrackIds(int channelId)
        {
            return new[] { 0 };
        }

        public TrackInfo GetTrackInfo(int trackId)
        {
            return new TrackInfo();
        }

        public void Comment(string comment, int userId, int channelId)
        {
        }

        public int[] GetCommentIds(int channelId)
        {
            return new[] { 0 };
        }

        public Comment GetComment(int commentId)
        {
            return new Comment();
        }

        public void Subscribe(int userId, int channelId)
        {
        }

        public void UnSubscribe(int userId, int channelId)
        {
        }
    }
}