using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentItMvc.Models;
using RentItMvc.RentItService;
using RentItMvc.Utilities;
using System.Threading;

namespace RentItMvc.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult ChangePassword(Account account)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                proxy.UpdateUser((int) Session["userId"], null, account.NewPassword, null);
            }
            return Redirect("/");
        }

        public ActionResult Edit()
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
            return Redirect("/");
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
            return Redirect("/BlobfishRadio");
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
                    ModelState.AddModelError("", "Wrong username or password");
                }
            }
            return Redirect("/BlobfishRadio");
        }

        [HttpPost]
        public ActionResult LogOut()
        {
            Session.RemoveAll();
            return Redirect("/BlobfishRadio");
        }

        /// <summary>
        /// Subscribes the user to a channel
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public ActionResult Subscribe(int channelId )
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                proxy.Subscribe((int)Session["UserId"], channelId);              
            }
            string previousPage = Request.UrlReferrer.AbsolutePath;
            return Redirect(previousPage);
        }

        public ActionResult UnSubscribe(int channelId)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {           
                proxy.Unsubscribe((int)Session["UserId"], channelId);
            }
            string previousPage = Request.UrlReferrer.AbsolutePath;
            return Redirect(previousPage);
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
    }
}
