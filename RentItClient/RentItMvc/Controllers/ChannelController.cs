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
        public ActionResult AdvancedSearch(int startIndex, int endIndex)
        {
            return View(new Tuple<int, int>(startIndex, endIndex));
        }

        public ActionResult SearchResults(Tuple<List<GuiChannel>, AdvancedSearchModel> model)
        {
            return View(model);
        }

        public ActionResult SearchAdv(string channelName, int? minAmountOfSubscribers, int? maxAmountOfSubscribers, int? minAmountOfComments,
                                      int? maxAmountOfComments, int? minAmountOfPlays, int? maxAmountOfPlays, int? minAmountOfVotes,
                                      int? maxAmountOfVotes, string sortingKey, string sortingBy, int startIndex, int endIndex)
        {
            Channel[] channels;
            ChannelSearchArgs searchArgs;
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                searchArgs = proxy.GetDefaultChannelSearchArgs();
                if (!channelName.Equals(""))
                    searchArgs.SearchString = channelName;
                searchArgs.StartIndex = startIndex;
                searchArgs.EndIndex = endIndex;
                //Subscribers
                searchArgs.MinNumberOfSubscriptions = minAmountOfSubscribers != null ? minAmountOfSubscribers.Value : -1;
                searchArgs.MaxNumberOfSubscriptions = maxAmountOfSubscribers != null ? maxAmountOfSubscribers.Value : int.MaxValue;
                //Comments
                searchArgs.MinNumberOfComments = minAmountOfComments != null ? minAmountOfComments.Value : -1;
                searchArgs.MaxNumberOfComments = maxAmountOfComments != null ? maxAmountOfComments.Value : int.MaxValue;
                //Plays
                searchArgs.MinAmountPlayed = minAmountOfPlays != null ? minAmountOfPlays.Value : -1;
                searchArgs.MaxAmountPlayed = maxAmountOfPlays != null ? maxAmountOfPlays.Value : int.MaxValue;
                //Votes
                searchArgs.MinTotalVotes = minAmountOfVotes != null ? minAmountOfVotes.Value : -1;
                searchArgs.MaxTotalVotes = maxAmountOfVotes != null ? maxAmountOfVotes.Value : int.MaxValue;
                //Sorting
                searchArgs.SortOption = sortingKey + sortingBy;

                channels = proxy.GetChannels(searchArgs);
            }
            List<GuiChannel> guiChannels = GuiClassConverter.ConvertChannels(channels);
            Tuple<List<GuiChannel>, AdvancedSearchModel> model = new Tuple<List<GuiChannel>, AdvancedSearchModel>(guiChannels, (AdvancedSearchModel) searchArgs);
            return View("SearchResults", model);
        }

        public SelectGenreModel GetGenreModel(int channelId)
        {
            List<GuiGenre> chosenGenres;
            List<GuiGenre> availableGenres;
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                chosenGenres = GuiClassConverter.ConvertGenres(proxy.GetGenresForChannel(channelId));
                availableGenres = GuiClassConverter.ConvertGenres(proxy.GetAllGenres()).Except(chosenGenres).ToList();
            }
            SelectGenreModel model = new SelectGenreModel
            {
                AvailableGenres = availableGenres,
                ChosenGenres = chosenGenres,
                ChannelId = channelId,
            };
            return model;
        }

        public PartialViewResult SelectGenre(int channelId)
        {
            return PartialView(GetGenreModel(channelId));
        }

        public PartialViewResult AddGenres(List<GuiGenre> availableGenres, List<GuiGenre> chosenGenres, int channelId)
        {
            return PartialView("SelectGenre", GetGenreModel(channelId));
        }

        public ActionResult BrowsableChannels(int? userId, int startIndex, int endIndex)
        {
            if (userId.HasValue)
            {
                if (startIndex < 0)
                {
                    startIndex = 0;
                    endIndex = 10;
                }
                Channel[] channels;
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    ChannelSearchArgs searchArgs = proxy.GetDefaultChannelSearchArgs();
                    searchArgs.StartIndex = startIndex;
                    searchArgs.EndIndex = endIndex;
                    searchArgs.SortOption = searchArgs.SubscriptionsDesc;
                    channels = proxy.GetChannels(searchArgs);
                }
                return View(new Tuple<List<GuiChannel>, int, int>(GuiClassConverter.ConvertChannels(channels), startIndex, endIndex));
            }
            return RedirectToAction("Index", "Home");
        }

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
        public ActionResult CreateNewChannel(GuiChannel channel, int? userId, SelectGenreModel model, FormCollection form)
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
                List<GuiChannel> guiChannels = GuiClassConverter.ConvertChannels(channels);
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
                List<GuiChannel> guiChannelList = GuiClassConverter.ConvertChannels(channels);
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
                List<GuiChannel> guiChannels = GuiClassConverter.ConvertChannels(channels);
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
                    List<GuiChannel> guiChannels = GuiClassConverter.ConvertChannels(channels);
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

        public static bool HasChannelTracks(int channelId)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                return proxy.GetTrackByChannelId(channelId).Length > 0;
            }
        }

        public static int TotalChannels()
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                ChannelSearchArgs searchArgs = proxy.GetDefaultChannelSearchArgs();
                searchArgs.SortOption = searchArgs.SubscriptionsDesc;
                return proxy.CountAllChannelsWithFilter(searchArgs);
            }
        }

        public ActionResult DeleteChannel(int channelId, int userId)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                proxy.DeleteChannel(channelId);
            }
            return RedirectToAction("MyChannels", new { userId = userId });
        }
    }
}