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
    public class ChannelController : Controller
    {
        /// <summary>
        /// Displays a list of channels
        /// </summary>
        /// <param name="channels">The list of channels to display</param>
        /// <returns></returns>
        public ActionResult ChannelList(List<GuiChannel> channels)
        {
            if (Session["userId"] != null)
            {
                if (channels == null)
                    channels = new List<GuiChannel>();
                return View(channels);
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Creates a new channel in the database. Is called from CreateChannel window.
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public ActionResult CreateNewChannel(GuiChannel channel)
        {
            if (Session["userId"] != null)
            {
                int channelId;
                int userId = (int)Session["userId"];
                string description = channel.Description;
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    channelId = proxy.CreateChannel(channel.Name, userId, description, new string[0]);
                }
                int routeChannelId = channelId;
                int routeUserId = userId;
                return RedirectToAction("SelectChannel", new { channelId = routeChannelId });
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Open site that enables user to edit a channel. Is also used for creating a channel
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public ActionResult EditChannel(int channelId)
        {
            if (Session["userId"] != null)
            {
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    Channel chan = proxy.GetChannel(channelId);
                    GuiChannel channel = GuiClassConverter.ConvertChannel(chan);
                    return View(channel);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Returns a list of most popular or highlighted channels
        /// </summary>
        /// <returns></returns>
        public ActionResult PopularChannels()
        {
            if (Session["userId"] != null)
            {
                Channel[] channels;
                try
                {
                    using (RentItServiceClient proxy = new RentItServiceClient())
                    {
                        ChannelSearchArgs searchArgs = proxy.GetDefaultChannelSearchArgs();
                        searchArgs.StartIndex = 0;
                        searchArgs.EndIndex = 10;
                        channels = proxy.GetChannels(searchArgs);
                    }
                }
                catch (Exception)
                {
                    channels = new Channel[0];
                }
                ViewBag.Title = "Featured channels";
                List<GuiChannel> guiChannels = GuiClassConverter.ConvertChannelList(channels);
                return View("ChannelList", guiChannels);
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// The list of channels assossiated with the logged in user
        /// </summary>
        /// <returns></returns>
        public ActionResult MyChannels()
        {
            if (Session["userId"] != null)
            {
                int userId = (int)Session["userId"];
                Channel[] channels;
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    User user = proxy.GetUser(userId);
                    channels = proxy.GetCreatedChannels(userId);
                }
                List<GuiChannel> guiChannelList = GuiClassConverter.ConvertChannelList(channels);
                ViewBag.Title = "My channels";
                return View("ChannelList", guiChannelList);
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Returns a view of the subscriptions assosiated with a User
        /// </summary>
        /// <returns></returns>
        public ActionResult MySubscriptions()
        {
            if (Session["userId"] != null)
            {
                int userId = (int)Session["UserId"];
                Channel[] channels;
                try
                {
                    using (RentItServiceClient proxy = new RentItServiceClient())
                    {
                        channels = proxy.GetSubscribedChannels(userId);
                    }
                }
                catch (Exception)
                {
                    channels = new Channel[0];
                }
                List<GuiChannel> guiChannels = GuiClassConverter.ConvertChannelList(channels);
                ViewBag.Title = "My subscriptions";
                return View("ChannelList", guiChannels);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SaveEditChannel(GuiChannel channel, int channelId)
        {
            if (Session["userId"] != null)
            {
                //channelId = Model.Id, ownerId = Model.OwnerId, name = Model.Name, description = Model.Description
                int userId = (int)Session["userId"];
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    proxy.UpdateChannel(channelId, userId, channel.Name, channel.Description, 0.0, 0.0);
                }
                return RedirectToAction("MyChannels", "Channel");
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SearchResult(string searchArgs)
        {
            if (Session["userId"] != null)
            {
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    ChannelSearchArgs channelSearchArgs = proxy.GetDefaultChannelSearchArgs();
                    channelSearchArgs.SearchString = searchArgs;
                    Channel[] channels = proxy.GetChannels(channelSearchArgs);
                    List<GuiChannel> guiChannels = GuiClassConverter.ConvertChannelList(channels);
                    ViewBag.Title = "Search results";
                    return View("ChannelList", guiChannels);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SelectChannel(int channelId)
        {
            if (Session["userId"] != null)
            {
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    Channel serviceChan = proxy.GetChannel(channelId);
                    GuiChannel chan = GuiClassConverter.ConvertChannel(serviceChan);
                    if (chan != null)
                    {
                        Session["channelId"] = chan.Id;
                        return View(chan);
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult CreateChannel()
        {
            if (Session["userId"] != null)
            {
                return View(new GuiChannel());
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
