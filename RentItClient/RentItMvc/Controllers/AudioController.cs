using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentItMvc.Models;

namespace RentItMvc.Controllers
{
    public class AudioController : Controller
    {
        public ActionResult AudioPlayer()
        {
            Audio audio = new Audio()
                {
                    Ogg = "'rentit.itu.dk:27000/stream'",
                    Mp3 = "'rentit.itu.dk:27000/stream'"
                    //Ogg = "'radio.reaper.fm/stream/'",
                    //Mp3 = "'radio.reaper.fm/stream/'"
                };
            return View(audio);
        }
    }
}