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
        public ActionResult ChannelList(List<GuiChannel> channels, int? userId)
        {
            if (userId.HasValue)
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
        public ActionResult CreateNewChannel(GuiChannel channel, int? userId)
        {
            if (userId.HasValue)
            {
                channel.OwnerId = userId.Value;
                int channelId;
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    channelId = proxy.CreateChannel(channel.Name, userId.Value, channel.Description, new string[0]);
                }
                return RedirectToAction("SelectChannel", new { channelId = channelId, userId = userId });
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Open site that enables user to edit a channel. Is also used for creating a channel
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public ActionResult EditChannel(int channelId, int? userId)
        {
            if (userId.HasValue)
            {
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    Channel chan = proxy.GetChannel(channelId);
                    GuiChannel channel = GuiClassConverter.ConvertChannel(chan);
                    if (channel.OwnerId == userId.Value)
                    {
                        return View(channel);
                    }
                    return Redirect(Request.UrlReferrer.PathAndQuery);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Returns a list of most popular or highlighted channels
        /// </summary>
        /// <returns></returns>
        public ActionResult PopularChannels(int? userId)
        {
            if (userId.HasValue)
            {
                Channel[] channels;
                try
                {
                    using (RentItServiceClient proxy = new RentItServiceClient())
                    {
                        ChannelSearchArgs searchArgs = proxy.GetDefaultChannelSearchArgs();
                        searchArgs.StartIndex = 0;
                        searchArgs.EndIndex = 10;
                        searchArgs.SortOption = searchArgs.SubscriptionsDesc;
                        channels = proxy.GetChannels(searchArgs);
                    }
                }
                catch (Exception)
                {
                    channels = new Channel[0];
                }
                ViewBag.Title = "Popular channels";
                List<GuiChannel> guiChannels = GuiClassConverter.ConvertChannelList(channels);
                return View("ChannelList", guiChannels);
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// The list of channels assossiated with the logged in user
        /// </summary>
        /// <returns></returns>
        public ActionResult MyChannels(int? userId)
        {
            if (userId.HasValue)
            {
                Channel[] channels;
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    channels = proxy.GetCreatedChannels(userId.Value);
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
        public ActionResult MySubscriptions(int? userId)
        {
            if (userId.HasValue)
            {
                Channel[] channels;
                try
                {
                    using (RentItServiceClient proxy = new RentItServiceClient())
                    {
                        channels = proxy.GetSubscribedChannels(userId.Value);
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

        public ActionResult SaveEditChannel(GuiChannel channel, int channelId, int? userId)
        {
            if (userId.HasValue)
            {
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    proxy.UpdateChannel(channelId, userId.Value, channel.Name, channel.Description, 0.0, 0.0);
                }
                return RedirectToAction("SelectChannel", "Channel", new { channelId = channelId, userId = userId });
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SearchResult(string searchArgs, int? userId)
        {
            if (userId.HasValue)
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

        public ActionResult SelectChannel(int channelId, int? userId)
        {
            if (userId.HasValue)
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

        public ActionResult CreateChannel(int? userId)
        {
            if (userId.HasValue)
            {
                return View(new GuiChannel());
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult StartChannel(int channelId)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                proxy.StartChannelStream(channelId);
            }
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        public ActionResult StopChannel(int channelId)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                proxy.StopChannelStream(channelId);
            }
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        public static bool IsChannelPlaying(int channelId)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                return proxy.IsChannelPlaying(channelId);
            }
        }
    }
}