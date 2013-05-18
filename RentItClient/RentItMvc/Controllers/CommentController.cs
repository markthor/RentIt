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
            Comment[] comments;
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                comments = proxy.GetChannelComments(channelId, null, null);
            }
            List<GuiComment> guiComments = GuiClassConverter.ConvertComments(comments);
            return PartialView(guiComments);
        }

        public PartialViewResult Comment(GuiComment c)
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
