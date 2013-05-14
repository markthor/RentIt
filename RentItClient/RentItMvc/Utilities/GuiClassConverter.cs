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
                    GuiChannel chan = new GuiChannel();
                    chan.Id = c.Id;
                    chan.Description = c.Description;
                    if(c.Hits != null)
                        chan.Hits = c.Hits.Value;
                    chan.Name = c.Name;
                    chan.StreamUri = c.StreamUri;
                    chan.OwnerId = c.OwnerId;
                    returnList.Add(chan);
                }
            }
            return returnList;
        }

        public static GuiChannel ConvertChannel(Channel c)
        {
            GuiChannel chan = new GuiChannel();
            chan.Id = c.Id;
            chan.Description = c.Description;
            if (c.Hits != null)
                chan.Hits = c.Hits.Value;
            chan.Name = c.Name;
            chan.StreamUri = c.StreamUri;
            chan.Tracks = new List<GuiTrack>();
            chan.OwnerId = c.OwnerId;
            //Calls the webservice and recieves an array of the tracks asossiated with the channel
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                Track[] tracks = proxy.GetTrackByChannelId(c.Id);
                foreach (Track t in tracks)
                    chan.Tracks.Add(ConvertTrack(t));
            }
            return chan;
        }

        public static GuiTrack ConvertTrack(Track t)
        {
            GuiTrack track = new GuiTrack();
            track.TrackName = t.Name;
            track.Id = t.Id;
            return track;
        }
    }
}