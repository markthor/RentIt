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

        public PartialViewResult CommentList(int channelId, int startIndex, int endIndex)
        {
            Comment[] comments;
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                comments = proxy.GetChannelComments(channelId, null, null);
            }
            List<GuiComment> guiComments = GuiClassConverter.ConvertComments(comments);
            ViewBag.ChannelId = channelId;
            return PartialView(guiComments);
        }

        public PartialViewResult Comment(GuiComment c)
        {
            return PartialView(c);
        }

        public ActionResult AddComment(Comment comment, int? userId, int? channelId)
        {
            if(userId.HasValue && channelId.HasValue)
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                proxy.CreateComment(comment.Content, userId.Value, channelId.Value);
            }
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        public PartialViewResult AddCommentForm()
        {
            return PartialView();
        }
    }
}
