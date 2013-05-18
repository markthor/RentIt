using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using RentItMvc.Models;
using RentItMvc.RentItService;
using RentItMvc.Utilities;

namespace RentItMvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int? userId)
        {
            if (userId.HasValue)
            {
                //User is logged in
                return RedirectToAction("PopularChannels", "Channel", new { userId = userId.Value });
            }
            //User is not logged in
            return View();
        }
    }
}