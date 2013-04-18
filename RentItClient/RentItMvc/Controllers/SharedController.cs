using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentItMvc.Controllers
{
    public class SharedController : Controller
    {
        public ActionResult LogIn(string email, string password)
        {
            return Redirect("index");
        }
    }
}
