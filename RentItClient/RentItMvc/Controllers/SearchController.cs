using System.Collections.Generic;
using System.Web.Mvc;
using RentItMvc.Models;
using RentItMvc.RentItService;

namespace RentItMvc.Controllers
{
    public class SearchController : Controller
    {
        public ActionResult Search(string searchArgs)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                ChannelSearchArgs channelSearchArgs = proxy.GetDefaultChannelSearchArgs();
                channelSearchArgs.SearchString = searchArgs;
                TempData["ChannelArray"] = proxy.GetChannels(channelSearchArgs);
                return RedirectToAction("ChannelList", "Home", new { title = "Results" });
            }
        }
    }
}