using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentItMvc.Models;

namespace RentItMvc.Controllers
{
    public class AccountController : Controller
    {
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(Account account)
        {
            return Redirect("Narrow");
        }
        */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(string email, string password)
        {
            return Redirect("Narrow");
        }

    }
}
