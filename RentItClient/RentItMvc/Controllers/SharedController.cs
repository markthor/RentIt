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
            audio.Ogg = "'radio.reaper.fm/stream/'";
            audio.Mp3 = "'radio.reaper.fm/stream/'";
            //audio.Ogg = "'ia600302.us.archive.org/17/items/1920sPop/BestThingsInLifeAreFree.ogg'";
            //audio.Mp3 = "'ia600302.us.archive.org/17/items/1920sPop/BestThingsInLifeAreFree_64kb.mp3'";
            return PartialView(audio);
        }
    }
}