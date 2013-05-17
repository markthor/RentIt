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
        public ActionResult Index()
        {
            int? userId = (int?)Session["userId"];
            if (userId != null && userId > 0)
            {
                //User is logged in
                return RedirectToAction("PopularChannels", "Channel");
            }
            //User is not logged in
            return View();
        }
    }
}