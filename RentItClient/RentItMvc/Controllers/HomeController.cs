using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentItMvc.Models;
using RentItMvc.RentItService;
using RentItMvc.Utilities;

namespace RentItMvc.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Main()
        {
            return View();
        }

        public ActionResult SelectChannel(int channelId)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                RentItService.Channel serviceChan = proxy.GetChannel(channelId);
                RentItMvc.Models.GuiChannel chan = new RentItMvc.Models.GuiChannel();
                chan.Name = @"Cyperchannel";
                chan.Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris non metus condimentum dolor molestie egestas. Phasellus ac fermentum augue. Fusce sem massa, pharetra quis dictum tempus, tempor ut lectus. Sed quis mauris felis. Vestibulum sed libero turpis, vel sagittis odio. Fusce pharetra purus quis neque aliquet quis tempus diam varius. Donec orci elit, cursus in consequat sed, hendrerit semper libero. Morbi id augue nulla, a blandit ligula. ";
                chan.Hits = 1024;
                chan.Upvotes = 100;
                chan.DownVotes = 12;
                if (chan != null)
                {   
                    return View(chan);
                }
            }
            return Redirect("/");
        }

        public ActionResult ChannelList(List<Channel> theList)
        {
            List<Channel> channelList = new List<Channel>();
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                int[] chanIds = proxy.GetAllChannelIds();
                foreach (int id in chanIds)
                {
                    channelList.Add(proxy.GetChannel(id));
                }
            }
            List<GuiChannel> GuiChannelList = GuiClassConverter.ConvertChannelList(theList);
            /*ChannelList list1 = new ChannelList();
            GuiChannel chan = new GuiChannel();
            chan.Id = 1;
            chan.Description = "A nice channel";
            chan.Name = "SuperChannel";
            chan.Hits = 1000;
            chan.Upvotes = 100;
            chan.DownVotes = 10;
            GuiChannel chan2 = new GuiChannel();
            chan2.Id = 2;
            chan2.Description = "Vesy sfsefsef";
            chan2.Name = "Go Go Go Go";
            chan2.Hits = 123123;
            chan2.Upvotes = 1523;
            chan2.DownVotes = 50;
            GuiChannel chan3 = new GuiChannel();
            chan3.Id = 3;
            chan3.Description = "Bullshit channel rating";
            chan3.Name = "wpeofmwpeomf";
            chan3.Hits = 1928;
            chan3.Upvotes = 5001;
            chan3.DownVotes = 2;
            List<GuiChannel> list2 = new List<GuiChannel>();
            list2.Add(chan);
            list2.Add(chan2);
            list2.Add(chan3);*/
            return PartialView(GuiChannelList);
        }
    }
}
