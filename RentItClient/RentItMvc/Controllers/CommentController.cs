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

        public PartialViewResult CommentListRange(int channelId, int startIndex, int endIndex)
        {
            if (startIndex <= 0)
            {
                startIndex = 0;
                endIndex = 10;
            }

            Comment[] comments;
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                comments = proxy.GetChannelComments(channelId, startIndex, endIndex);
            }
            List<GuiComment> guiComments = GuiClassConverter.ConvertComments(comments);
            ViewBag.ChannelId = channelId;
            return PartialView("CommentList", new Tuple<List<GuiComment>, int, int>(guiComments, startIndex, endIndex));
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

        public static int GetCountChannelComments(int channelId)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                return proxy.GetCountChannelComments(channelId);
            }
        }
    }
}
