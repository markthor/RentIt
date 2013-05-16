using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentItMvc.Models;
using RentItMvc.RentItService;
using RentItMvc.Utilities;

namespace RentItMvc.Controllers
{
    public class CommentController : Controller
    {
        //
        // GET: /Comment/

        public PartialViewResult CommentList()
        {
            int channelId = (int) Session["channelId"];
            Comment[] comments = new Comment[0];
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                comments = proxy.GetChannelComments(channelId, 0, 10);
            }
            List<GuiComment> guiComments = GuiClassConverter.ConvertComments(comments);
            GuiComment c = new GuiComment();
            c.UserName = "TokelOke";
            c.Content = "I AM A VAMPIRE I AM A VAMPIRE";
            c.Date = DateTime.UtcNow;
            GuiComment c2 = new GuiComment();
            c2.UserName = "Tokel0ke";
            c2.Content = "Biome 4﻿ ever, sound fucking my brain!!!";
            c2.Date = DateTime.UtcNow.AddDays(2);
            GuiComment c3 = new GuiComment();
            c3.UserName = "PETER ";
            c3.Content = "I AM A VAMPIRE I AM A VAMASDASDASDASDPIRE";
            c3.Date = DateTime.UtcNow.AddDays(-2);
            List<GuiComment> list = new List<GuiComment>();
            list.Add(c);
            list.Add(c2);
            list.Add(c3);
            return PartialView(guiComments);
        }

        public PartialViewResult Comment(Comment c)
        {
            return PartialView(c);
        }

        public ActionResult AddComment(Comment comment)
        {
            int userId = (int) Session["userId"];
            int channelId = (int) Session["channelId"];
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                proxy.CreateComment(comment.Content, userId, channelId);
            }
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        public PartialViewResult AddCommentForm()
        {
            return PartialView();
        }
    }
}
