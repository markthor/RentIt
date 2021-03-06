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
    //The OutputCacheAttribute attribute is required in order to prevent ASP.NET MVC from caching the results of the validation methods.
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class ValidationController : Controller
    {
        public JsonResult IsEmailAvailable(string newEmail)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                try
                {
                    if (proxy.IsEmailAvailable(newEmail))
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                }
                catch
                {
                    return Json("Email is already in use.", JsonRequestBehavior.AllowGet);
                }
                return Json("Email is already in use.", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult IsUsernameAvailable(string newUsername)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                if (proxy.IsUsernameAvailable(newUsername))
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                return Json("Username is already in use.", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult IsCurrentPasswordCorrect(string currentPassword, int userId)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                if (proxy.IsCorrectPassword(userId, currentPassword))
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                return Json("The current password is wrong.", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult IsChannelNameAvailable(string name, int id)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                if (proxy.IsChannelNameAvailable(id, name))
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                return Json("The channel name is already in use.", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
