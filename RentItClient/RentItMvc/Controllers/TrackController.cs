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
            // redirect back to the index action to show the form once again
            return RedirectToAction("EditChannel", "Channel", new { channelId = channelId });
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
        /// <param name="channelId"></param>
        /// <param name="trackId"></param>
        /// <returns></returns>
        public ActionResult DeleteTrack(int channelId, int trackId)
        {
            int userId = (int)Session["userId"];
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                proxy.RemoveTrack(userId, trackId);
            }
            return RedirectToAction("EditChannel", "Channel", new { channelId = channelId });
        }
    }
}
