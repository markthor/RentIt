using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentItMvc.Models;

namespace RentItMvc.Controllers
{
    public class SharedController : Controller
    {
        public ActionResult Topbar()
        {
            return PartialView();
        }

        public ActionResult AudioPlayer()
        {
            Audio audio = new Audio();
            //audio.Ogg = "'radio.reaper.fm/stream/'";
            //audio.Mp3 = "'radio.reaper.fm/stream/'";
            audio.Ogg = "'rentit.itu.dk:27000/stream'";
            audio.Mp3 = "'rentit.itu.dk:27000/stream'";
            return PartialView(audio);
        }
    }
}