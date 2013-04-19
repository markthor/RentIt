using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentItMvc.Models;
using RentItMvc.RentItService;

namespace RentItMvc.Controllers
{
    public class AccountController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(string username, string email, string password, string confirmPassword)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                if (password.Equals(confirmPassword))
                {
                    int userId = proxy.CreateUser(username, password, email);
                }
                
            }
            return Redirect("/");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(string email, string password)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                User user = new User();
                user.Id = 1;
                user.Username = "Mikkel";
                Session["userId"] = user.Id;
                Session["username"] = user.Username;
            }
            return Redirect("/");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            Session.RemoveAll();
            return Redirect("/");
        }
    }
}
