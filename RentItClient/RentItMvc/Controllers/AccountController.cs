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
        public ActionResult SignUp(Account account)
        {
            if (ModelState.IsValid)
            {
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    if (account.Password.Equals(account.ConfirmPassword))
                    {
                        try
                        {
                            User user = proxy.SignUp(account.Username, account.Email, account.Password);
                            Session["userId"] = user.Id;
                            Session["username"] = user.Username;
                        }
                        catch (Exception e)
                        {
                            string message = e.Message;
                            if (message.StartsWith("Username"))
                            {
                                ModelState.AddModelError("Username", message);
                            } else if (message.StartsWith("Email"))
                            {
                                ModelState.AddModelError("Email", message);
                            }
                            else
                            {
                                throw e;
                            }
                        }
                    }
                }
            }
            return Redirect("/");
        }

        [HttpPost]
        public ActionResult LogIn(Account account)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                User user = proxy.Login(account.Email, account.Password);
                Session["userId"] = user.Id;
                Session["username"] = user.Username;
            }
            return Redirect("/");
        }

        [HttpPost]
        public ActionResult LogOut()
        {
            Session.RemoveAll();
            return Redirect("/");
        }
    }
}
