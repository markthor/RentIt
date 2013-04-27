using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentItMvc.Models;
using RentItMvc.RentItService;
using System.Threading;

namespace RentItMvc.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Details()
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                User user = proxy.GetUser((int)Session["userId"]);
                //User user = new User();
                //user.Email = @"andreas.p.poulsen@gmail.com";
                //user.Username = "Prechtig";
                if (user != null)
                {
                    Account acc = new Account();
                    acc.Email = user.Email;
                    acc.Username = user.Username;
                    return View(acc);
                }
            }
            return Redirect("/");
        }

        public ActionResult Edit()
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                User user = proxy.GetUser((int)Session["userId"]);
                //User user = new User();
                //user.Email = @"andreas.p.poulsen@gmail.com";
                //user.Username = "Prechtig";
                if (user != null)
                {
                    Account acc = new Account();
                    acc.Email = user.Email;
                    acc.Username = user.Username;
                    return View(acc);
                }
            }
            return Redirect("/");
        }

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
                            }
                            else if (message.StartsWith("Email"))
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
        [ValidateInput(false)]
        public ActionResult LogIn(string email, string password)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                User user = proxy.Login(email, password);
                Session["userId"] = user.Id;
                Session["username"] = user.Username;
            }
            return Redirect("/Home/Main");
        }

        [HttpPost]
        public ActionResult LogOut()
        {
            Session.RemoveAll();
            return Redirect("/");
        }
    }
}
