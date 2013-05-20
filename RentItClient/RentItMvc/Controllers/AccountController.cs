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
        public ActionResult ChangePassword(Account account, int? userId)
        {
            if (userId.HasValue)
            {
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    proxy.UpdateUser(userId.Value, null, account.NewPassword, null);
                }
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ChangeUsername(Account account, int? userId)
        {
            if (userId.HasValue)
            {
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    proxy.UpdateUser(userId.Value, account.NewUsername, null, null);
                }
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ChangeEmail(Account account, int? userId)
        {
            if (userId.HasValue)
            {
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    proxy.UpdateUser(userId.Value, null, null, account.NewEmail);
                }
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Edit(int? userId)
        {
            if (userId.HasValue)
            {
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    User user = proxy.GetUser(userId.Value);
                    if (user != null)
                    {
                        Account acc = new Account();
                        acc.UserId = user.Id;
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
                        return RedirectToAction("PopularChannels", "Channel", new { userId = user.Id });
                    }
                    catch (Exception)
                    {
                    }
                }
                return RedirectToAction("Index", "Home");
            }
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
            return RedirectToAction("PopularChannels", "Channel", new { userId = Session["userId"] });
        }

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
        public ActionResult Subscribe(int channelId, int? userId)
        {
            if (userId.HasValue)
            {
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    proxy.Subscribe(userId.Value, channelId);
                }
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult UnSubscribe(int channelId, int? userId)
        {
            if (userId.HasValue)
            {
                using (RentItServiceClient proxy = new RentItServiceClient())
                {
                    proxy.Unsubscribe(userId.Value, channelId);
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

        public JsonResult IsSubscribedJson(int channelId, int userId)
        {
            Channel[] channels;
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                channels = proxy.GetSubscribedChannels(userId);
            }
            if (channels != null)
            {
                if (channels.Any(channel => channel.Id == channelId))
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public static int GetVote(int userId, int trackId)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                Vote vote = proxy.GetVote(userId, trackId);
                if (vote != null)
                {
                    return vote.Value;
                }
                return 0;
            }
        }

        public JsonResult GetVotes(int userId, int trackId)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                Vote vote = proxy.GetVote(userId, trackId);
                if (vote != null)
                {
                    return Json(vote.Value, JsonRequestBehavior.AllowGet);
                }
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public void CreateUpvote(int userId, int trackId)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                proxy.CreateVote(1, userId, trackId);
            }
        }

        public void CreateDownvote(int userId, int trackId)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                proxy.CreateVote(-1, userId, trackId);
            }
        }

        public void DeleteVote(int userId, int trackId)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                proxy.DeleteVote(userId, trackId);
            }
        }

        public ActionResult DeleteAccount(int userId)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                proxy.DeleteAccount(userId);
            }
            return RedirectToAction("LogOut", "Account");
        }
    }
}
