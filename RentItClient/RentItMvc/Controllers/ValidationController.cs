﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using RentItMvc.RentItService;

namespace RentItMvc.Controllers
{
    //The OutputCacheAttribute attribute is required in order to prevent ASP.NET MVC from caching the results of the validation methods.
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class ValidationController : Controller
    {
        public JsonResult IsEmailAvailable(string email)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                if (proxy.IsEmailAvailable(email))
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                return Json("Email is already in use.", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult IsUsernameAvailable(string username)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                if (proxy.IsUsernameAvailable(username))
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                return Json("Username is already in use.", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
