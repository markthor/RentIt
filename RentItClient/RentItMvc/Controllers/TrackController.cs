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
        public ActionResult AddTrack(HttpPostedFileBase file, int channelId, string trackName, string artistName)
        {
            if (Session["userId"] != null)
            {
                // Verify that the user selected a file
                if (file != null && file.ContentLength > 0)
                {
                    int userId = (int)Session["userId"];
                    Stream stream = file.InputStream;
                    MemoryStream memory = new MemoryStream();
                    stream.CopyTo(memory);
                    Track track = new Track();
                    track.Artist = artistName;
                    track.Name = trackName;
                    using (RentItServiceClient proxy = new RentItServiceClient())
                    {
                        proxy.AddTrack(userId, channelId, memory, track);
                    }
                }
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Returns a for thats enables adding uploading a track
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
        public ActionResult DeleteTrack(int trackId)
        {
            if (Session["userId"] != null)
            {
                int userId = (int)Session["userId"];
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    proxy.RemoveTrack(trackId);
                }
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult EditTracks(int channelId)
        {
            if (Session["userId"] != null)
            {
                List<GuiTrack> guiTracks;
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    Track[] tracks = proxy.GetTrackByChannelId(channelId);
                    guiTracks = Utilities.GuiClassConverter.ConvertTrackList(tracks);
                }
                return View("TrackList", guiTracks);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult TrackList(List<GuiTrack> tracks)
        {
            if (Session["userId"] != null)
            {
                return View(tracks);
            }
            return RedirectToAction("Index", "Home");
        }

        
    }
}
