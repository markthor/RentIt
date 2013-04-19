using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentItMvc.RentItService;

namespace RentItMvc.Controllers
{
    public class ValidationController : Controller
    {
        public JsonResult IsEmailAvailable(string email)
        {
            using (RentItServiceClient proxy = new RentItServiceClient())
            {
                if (proxy.IsEmailAvailable(email))
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}
