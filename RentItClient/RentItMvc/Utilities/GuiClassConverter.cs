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
        public static List<GuiChannel> ConvertChannelList(List<Channel> list)
        {
            List<GuiChannel> returnList = new List<GuiChannel>();
            if (list != null)
            {
                foreach (Channel c in list)
                {
                    GuiChannel chan = new GuiChannel();
                    chan.Id = c.Id;
                    if(chan.Description != null)
                        chan.Description = c.Description;
                    if(c.Hits != null)
                        chan.Hits = c.Hits.Value;
                    if (c.Name != null)
                        chan.Name = c.Name;
                }

                return returnList;
            }
            return null;
        }
    }
}