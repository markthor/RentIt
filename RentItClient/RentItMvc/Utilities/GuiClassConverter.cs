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
        public static List<GuiChannel> ConvertChannelList(Channel[] channels)
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

        public static GuiChannel ConvertChannel(Channel c)
        {
            GuiChannel chan = new GuiChannel()
            {
                Id = c.Id,
                Description = c.Description,
                Plays = c.Hits != null ? c.Hits.Value : 0,
                Name = c.Name,
                StreamUri = c.StreamUri,
                Tracks = new List<GuiTrack>(),
                OwnerId = c.OwnerId,
            };
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                //Get number of subscribers
                chan.Subscribers = proxy.GetSubscriberCount(chan.Id);
                //Calls the webservice and recieves an array of the tracks asossiated with the channel
                Track[] tracks = proxy.GetTrackByChannelId(c.Id);
                foreach (Track t in tracks)
                    chan.Tracks.Add(ConvertTrack(t));
            }
            return chan;
        }

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

        public static List<GuiTrack> ConvertTrackList(Track[] tracks)
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
    }
}