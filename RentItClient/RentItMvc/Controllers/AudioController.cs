using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using RentItMvc.Models;
using RentItMvc.RentItService;
using System.Runtime.Serialization.Json;
using RentItMvc.Utilities;

namespace RentItMvc.Controllers
{
    public class AudioController : Controller
    {
        public ActionResult AudioPlayer(int channelId, int userId)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                proxy.IncrementChannelPlays(channelId);
            }
            Audio audio = new Audio();
            audio.StreamUri = "http://rentit.itu.dk:27000/" + channelId.ToString();
            audio.ChannelId = channelId;
            return View(new Tuple<Audio, int>(audio, userId));
        }

        public PartialViewResult Last5Tracks(int channelId, int userId)
        {
            List<GuiTrack> guiTracks;
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                Track[] tracks = proxy.GetRecentlyPlayedTracks(channelId, 5);
                guiTracks = GuiClassConverter.ConvertTracks(tracks);
            }
            return PartialView(new Tuple<List<GuiTrack>, int>(guiTracks, userId));
        }

        public JsonResult Last5TracksJson(int channelId, int userId)
        {
            List<GuiTrack> guiTracks;
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                Track[] tracks = proxy.GetRecentlyPlayedTracks(channelId, 5);
                guiTracks = GuiClassConverter.ConvertTracks(tracks);
            }
            return Json(guiTracks, JsonRequestBehavior.AllowGet);
        }
    }
}