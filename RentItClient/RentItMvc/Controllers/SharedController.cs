using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentItMvc.Models;

namespace RentItMvc.Controllers
{
    public class SharedController : Controller
    {
        public PartialViewResult Topbar()
        {
            return PartialView();
        }

        public PartialViewResult Sidebar()
        {
            return PartialView();
        }

        public PartialViewResult Tracks(List<GuiTrack> tracks)
        {
            return PartialView(tracks);
        }
    }
}