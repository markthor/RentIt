using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RentItMvc.Models;
using RentItMvc.RentItService;

namespace RentItMvc.Utilities
{
    public static class GuiClassConverter
    {
        #region Comment
        public static GuiComment ConvertComment(Comment c)
        {
            GuiComment comment = new GuiComment()
            {
                UserId = c.UserId,
                Content = c.Content,
                Date = c.PostTime,
                ChannelId = c.ChannelId
            };
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                comment.UserName = proxy.GetUser(comment.UserId).Username;
            }
            return comment;
        }

        public static List<GuiComment> ConvertComments(Comment[] comments)
        {
            List<GuiComment> guiComments = new List<GuiComment>();
            if (comments != null)
            {
                foreach (Comment comment in comments)
                {
                    guiComments.Add(ConvertComment(comment));
                }
            }
            return guiComments;
        }
        #endregion

        #region Channel
        public static GuiChannel ConvertChannel(Channel c)
        {
            GuiChannel chan = new GuiChannel()
            {
                Id = c.Id,
                Description = c.Description,
                Plays = c.Hits != null ? c.Hits.Value : 0,
                Name = c.Name,
                StreamUri = c.StreamUri,
                OwnerId = c.OwnerId,
            };
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                //Get number of subscribers
                chan.Subscribers = proxy.GetSubscriberCount(chan.Id);
                //Get the channels
                chan.Tracks = ConvertTracks(proxy.GetTrackByChannelId(c.Id));
                //Get the genres
                chan.Genres = ConvertGenres(proxy.GetGenresForChannel(c.Id));
            }
            return chan;
        }

        public static List<GuiChannel> ConvertChannels(Channel[] channels)
        {
            List<GuiChannel> returnList = new List<GuiChannel>();
            if (channels != null)
            {
                foreach (Channel c in channels)
                {
                    returnList.Add(ConvertChannel(c));
                }
            }
            return returnList;
        }
        #endregion

        #region Track
        public static GuiTrack ConvertTrack(Track t)
        {
            GuiTrack track = new GuiTrack()
            {
                TrackName = t.Name,
                Id = t.Id,
                ArtistName = t.Artist,
                ChannelId = t.ChannelId,
            };
            return track;
        }

        public static List<GuiTrack> ConvertTracks(Track[] tracks)
        {
            List<GuiTrack> convertedTracks = new List<GuiTrack>();
            if (tracks != null)
            {
                foreach (Track t in tracks)
                {
                    convertedTracks.Add(ConvertTrack(t));
                }
            }
            return convertedTracks;
        }
        #endregion

        #region Genre
        public static GuiGenre ConvertGenre(Genre genre)
        {
            GuiGenre guiGenre = new GuiGenre
            {
                Id = genre.Id,
                Name = genre.Name
            };
            return guiGenre;
        }

        public static List<GuiGenre> ConvertGenres(Genre[] genres)
        {
            List<GuiGenre> convertedGenres = new List<GuiGenre>();
            if (genres != null)
            {
                foreach (Genre genre in genres)
                {
                    convertedGenres.Add(ConvertGenre(genre));
                }
            }
            return convertedGenres;
        }
    }
        #endregion
}