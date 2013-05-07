using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using RentItMvc.Models;
using RentItMvc.RentItService;
using RentItMvc.Utilities;

namespace RentItMvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            int? userId = (int?) Session["userId"];
            if (userId != null && userId > 0)
            {
                //User is logged in
                return Redirect("/Home/Main");
            }
            else
            {
                //User is not logged in
                return View();    
            }
        }

        public ActionResult Main()
        {
            if (Session["userId"] != null)
            {
                return View();    
            }
            return Redirect("/");
        }


        public ActionResult SelectChannel(int channelId)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                //RentItService.Channel serviceChan = proxy.GetChannel(channelId);
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

        /// <summary>
        /// Open site that enables user to edit a channel. Is also used for creating a channel
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public ActionResult EditChannel(int channelId)
        {
            //using (RentItServiceClient proxy = new RentItServiceClient())
            //{
                //RentItService.Channel serviceChan = proxy.GetChannel(channelId);
            RentItMvc.Models.GuiChannel chan = new RentItMvc.Models.GuiChannel();
            chan.Name = @"Cyperchannel";
            chan.Id = 1;
            chan.Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris non metus condimentum dolor molestie egestas. Phasellus ac fermentum augue. Fusce sem massa, pharetra quis dictum tempus, tempor ut lectus. Sed quis mauris felis. Vestibulum sed libero turpis, vel sagittis odio. Fusce pharetra purus quis neque aliquet quis tempus diam varius. Donec orci elit, cursus in consequat sed, hendrerit semper libero. Morbi id augue nulla, a blandit ligula. ";
            chan.Hits = 1024;
            chan.Upvotes = 100;
            chan.DownVotes = 12;
            chan.Tracks = new List<GuiTrack>();
            GuiTrack t1 = new GuiTrack();
            t1.TrackName = "track1";
            t1.Id = 1;
            GuiTrack t2 = new GuiTrack();
            t2.TrackName = "track2";
            t1.Id = 2;
            chan.Tracks.Add(t1);
            chan.Tracks.Add(t2);
            if (chan != null)
            {
                return View(chan);
            }
            //}
            return Redirect("/");
        }

        public ActionResult GetFeaturedChannels()
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
            return ChannelList(channelList, "Featured Channels");
        }

        public ActionResult ChannelList(List<Channel> theList, string title)
        {
            RentItServer.ITU.DatabaseWrapperObjects.Channel[] channels;
            RentItServer.ITU.Controller controller = RentItServer.ITU.Controller.GetInstance();
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                RentItServer.ITU.ChannelSearchArgs searchArgs = new RentItServer.ITU.ChannelSearchArgs()
                    {
                        StartIndex = 0,
                        EndIndex = 1
                    };
                channels = controller.GetChannels(searchArgs);
            }
            List<GuiChannel> GuiChannelList = null;// GuiClassConverter.ConvertChannelList(channels.ToList());
            ViewBag.title = title;
            //List<GuiChannel> GuiChannelList = GuiClassConverter.ConvertChannelList(theList);
            if(GuiChannelList == null)
                GuiChannelList = new List<GuiChannel>();
            //ChannelList list1 = new ChannelList();
            /*
            GuiChannel chan = new GuiChannel();
            chan.Id = 1;
            chan.Description = "A nice channel";
            chan.Name = "SuperChannel";
            chan.Hits = 1000;
            chan.Upvotes = 100;
            chan.DownVotes = 10;
            GuiChannelList.Add(chan);*/
            /*GuiChannel chan2 = new GuiChannel();
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

        public ActionResult MyChannels(int userId)
        {
            List<GuiChannel> GuiChannelList = new List<GuiChannel>();
            //ChannelList list1 = new ChannelList();
            GuiChannel chan = new GuiChannel();
            chan.Id = 1;
            chan.Description = "My private Channel";
            chan.Name = "My channel";
            chan.Hits = 1000;
            chan.Upvotes = 100;
            chan.DownVotes = 10;
            GuiChannelList.Add(chan);
            return PartialView(GuiChannelList);
        }

        public ActionResult DeleteTrack(int trackId)
        {
            return EditChannel(trackId);
        }

        public ActionResult AddTrack(HttpPostedFileBase file, int modelId, int userId)
        {
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                // extract only the fielname
                var fileName = Path.GetFileName(file.FileName);
                // store the file inside ~/App_Data/uploads folder             
                Stream stream = file.InputStream;
                MemoryStream memory = new MemoryStream();
                stream.CopyTo(memory);
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    Track chan = proxy.GetTrackInfoByStream(memory);
                }
            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("Index"); 
        }
    }
}
