using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentItMvc.Models;
using RentItMvc.RentItService;

namespace RentItMvc.Controllers
{
    public class TrackController : Controller
    {
        /// <summary>
        /// Adds a track to a channel
        /// </summary>
        /// <param name="file"></param>
        /// <param name="channelId"></param>
        /// <param name="trackName"></param>
        /// <param name="artistName"></param>
        /// <returns></returns>
        public ActionResult AddTrack(HttpPostedFileBase file, int channelId, int? userId, string trackName, string artistName)
        {
            if (userId.HasValue)
            {
                try
                {
                    // Verify that the user selected a file
                    if (file != null && file.ContentLength > 0)
                    {
                        Stream stream = file.InputStream;
                        MemoryStream memory = new MemoryStream();
                        stream.CopyTo(memory);
                        Track track = new Track();
                        track.Artist = artistName;
                        track.Name = trackName;
                        using (RentItServiceClient proxy = new RentItServiceClient())
                        {
                            proxy.AddTrack(userId.Value, channelId, memory);
                        }
                    }
                }
                catch (Exception)
                {
                }
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Returns a form thats enables adding uploading a track
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public PartialViewResult AddTrackForm(int channelId)
        {
            Session["channelId"] = channelId;
            return PartialView(new GuiTrack());
        }

        /// <summary>
        /// Deletes a track and reloads the edit channel page
        /// </summary>
        /// <param name="trackId"></param>
        /// <returns></returns>
        public ActionResult DeleteTrack(int trackId, int? userId)
        {
            if (userId.HasValue)
            {
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    proxy.RemoveTrack(trackId);
                }
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult EditTracks(int channelId, int? userId)
        {
            if (userId.HasValue)
            {
                List<GuiTrack> guiTracks;
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    Track[] tracks = proxy.GetTrackByChannelId(channelId);
                    guiTracks = Utilities.GuiClassConverter.ConvertTracks(tracks);
                }
                return View("TrackList", new Tuple<List<GuiTrack>, int, int>(guiTracks, channelId, userId.Value));
            }
            return RedirectToAction("Index", "Home");
        }

        public static int GetUpvotes(int trackId)
        {
            int upvotes;
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                upvotes = proxy.CountAllUpvotes(trackId);
            }
            return upvotes;
        }

        public static int GetDownvotes(int trackId)
        {
            int downvotes;
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                downvotes = proxy.CountAllDownvotes(trackId);
            }
            return downvotes;
        }
    }
}
