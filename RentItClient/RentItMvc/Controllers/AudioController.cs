﻿using System;
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

            }
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }
    }
}