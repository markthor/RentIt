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
            if (Session["userId"] != null)
            {
                int userId = (int) Session["userId"];
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    proxy.UpdateUser(userId, null, account.NewPassword, null);
                }
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ChangeUsername(Account account)
        {
            if (Session["userId"] != null)
            {
                int userId = (int)Session["userId"];
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    proxy.UpdateUser(userId, account.NewUsername, null, null);
                }
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ChangeEmail(Account account)
        {
            if (Session["userId"] != null)
            {
                int userId = (int)Session["userId"];
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    proxy.UpdateUser(userId, null, null, account.NewEmail);
                }
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Edit()
        {
            if (Session["userId"] != null)
            {
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    User user = proxy.GetUser((int)Session["userId"]);
                    if (user != null)
                    {
                        Account acc = new Account();
                        acc.CurrentEmail = user.Email;
                        acc.CurrentUsername = user.Username;
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
                        User user = proxy.SignUp(account.NewUsername, account.NewEmail, account.NewPassword);
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
            if (Session["userId"] != null)
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
            if (Session["userId"] != null)
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
            Channel[] channels;
            try
            {
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    channels = proxy.GetSubscribedChannels(userId);
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
    }
}
