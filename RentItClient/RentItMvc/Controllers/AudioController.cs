using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using RentItMvc.Models;
using RentItMvc.RentItService;

namespace RentItMvc.Controllers
{
    public class AudioController : Controller
    {
        public ActionResult AudioPlayer(int channelId)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                proxy.IncrementChannelPlays(channelId);
            }
            Audio audio = new Audio();
            audio.StreamUri = "http://rentit.itu.dk:27000/" + channelId.ToString();
            return View(audio);
        }
    }
}