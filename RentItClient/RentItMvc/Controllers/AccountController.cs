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
        public ActionResult ChangePassword(Account account)
        {
            if (UserIsLoggedIn())
            {
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    proxy.UpdateUser((int)Session["userId"], null, account.NewPassword, null);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Edit()
        {
            if (UserIsLoggedIn())
            {
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    User user = proxy.GetUser((int)Session["userId"]);
                    if (user != null)
                    {
                        Account acc = new Account();
                        acc.Email = user.Email;
                        acc.Username = user.Username;
                        return View(acc);
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult SignUp(Account account)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                if (account.NewPassword.Equals(account.ConfirmPassword))
                {
                    try
                    {
                        User user = proxy.SignUp(account.Username, account.Email, account.NewPassword);
                        Session["userId"] = user.Id;
                        Session["username"] = user.Username;
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult LogIn(string usernameOrEmail, string currentPassword)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                try
                {
                    User user = proxy.Login(usernameOrEmail, currentPassword);
                    Session["userId"] = user.Id;
                    Session["username"] = user.Username;
                }
                catch (Exception)
                {
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult LogOut()
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Subscribes the user to a channel
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public ActionResult Subscribe(int channelId)
        {
            if (UserIsLoggedIn())
            {
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    proxy.Subscribe((int)Session["UserId"], channelId);
                }
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult UnSubscribe(int channelId)
        {
            if (UserIsLoggedIn())
            {
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    proxy.Unsubscribe((int)Session["UserId"], channelId);
                }
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Method called to determine if a user has subscribed to a given channel
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool IsSubscribed(int channelId, int userId)
        {
            int uId = userId;
            Channel[] channels;
            try
            {
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    channels = proxy.GetSubscribedChannels(uId);
                }
            }
            catch (Exception)
            {
                return false;
            }

            foreach (Channel c in channels)
            {
                if (c.Id == channelId)
                    return true;
            }
            return false;
        }

        public bool UserIsLoggedIn()
        {
            return Session["userId"] != null;
        }
    }
}
