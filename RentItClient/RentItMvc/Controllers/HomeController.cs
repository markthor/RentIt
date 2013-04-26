using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentItMvc.Models;

namespace RentItMvc.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Main()
        {
            return View();
        }

        public ActionResult SelectChannel(int? channelId)
        {
            //using (RentItServiceClient proxy = new RentItServiceClient())
            //{
                //RentItService.Channel chan = proxy.GetChannel((int)channelId);
                RentItMvc.Models.Channel chan = new RentItMvc.Models.Channel();
                chan.Name = @"Cyperchannel";
                chan.Description = "The greatest channel ever made";
                if (chan != null)
                {   
                    return View(chan);
                }
            //}
            return Redirect("/");
        }

        public ActionResult ChannelList()
        {
            ChannelList list1 = new ChannelList();
            Channel chan = new Channel();
            chan.Description = "A nice channel";
            chan.Name = "SuperChannel";
            chan.Hits = 1000;
            chan.Rating = 100.8;
            Channel chan2 = new Channel();
            chan2.Description = "Vesy sfsefsef";
            chan2.Name = "Go Go Go Go";
            chan2.Hits = 123123;
            chan2.Rating = 1200.5;
            Channel chan3 = new Channel();
            chan3.Description = "Bullshit channel rating";
            chan3.Name = "wpeofmwpeomf";
            chan3.Hits = 1928;
            chan3.Rating = 5000.5656;
            List<Channel> list2 = new List<Channel>();
            list2.Add(chan);
            list2.Add(chan2);
            list2.Add(chan3);
            return PartialView(list2);
        }
    }
}
