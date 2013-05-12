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
            int? userId = (int?)Session["userId"];
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
                RentItService.Channel serviceChan = proxy.GetChannel(channelId);
                GuiChannel chan = GuiChannel.GuiChannelFactory(serviceChan);
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
        public ActionResult EditChannel(int channelId, int userId)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                Channel chan = proxy.GetChannel(channelId);
                GuiChannel channel = GuiClassConverter.ConvertChannel(chan);
                return View(channel);
            }
        }

        public ActionResult CreateChannel(string channelName, int userId)
        {
            int channelId;
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                channelId = proxy.CreateChannel(channelName, userId, "", new string[0]);
            }
            int routeChannelId = channelId;
            int routeUserId = userId;
            return RedirectToAction("EditChannel", new { channelId = routeChannelId, userId = routeUserId });
        }

        /// <summary>
        /// Returns a list of most popular or highlighted channels
        /// </summary>
        /// <returns></returns>
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
            return RedirectToAction("ChannelList", new { title = "Featured Channels", theList = channelList });
        }

        /// <summary>
        /// Displays a list of channels
        /// </summary>
        /// <param name="theList"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public ActionResult ChannelList(List<Channel> theList, string title)
        {
            Channel[] channels;
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                ChannelSearchArgs searchArgs = proxy.GetDefaultChannelSearchArgs();
                searchArgs.StartIndex = 0;
                searchArgs.EndIndex = 10;
                channels = proxy.GetChannels(searchArgs);
            }
            List<GuiChannel> GuiChannelList = GuiClassConverter.ConvertChannelList(channels.ToList());
            ViewBag.title = title;
            if (GuiChannelList == null)
                GuiChannelList = new List<GuiChannel>();
            return PartialView(GuiChannelList);
        }

        /// <summary>
        /// The list of channels assossiated with the logged in user
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns></returns>
        public ActionResult MyChannels(int userId)
        {
            Channel[] channels;
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                channels = proxy.GetCreatedChannels(userId);
            }
            List<GuiChannel> guiChannelList = GuiClassConverter.ConvertChannelList(channels.ToList());
            return PartialView(guiChannelList);
        }

        /// <summary>
        /// Deletes a track and reloads the edit channel page
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public ActionResult DeleteTrack(int userId, int channelId)
        {
            return RedirectToAction("EditChannel", new { userId = userId, channelId = channelId });
        }

        /// <summary>
        /// Adds a track to a channel
        /// </summary>
        /// <param name="file"></param>
        /// <param name="modelId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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
