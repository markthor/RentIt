using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentItMvc.Controllers
{
    public class DownloadLogController : Controller
    {
        public FileResult Download()
        {
            string logFilePath = "C:\\RentItServices\\Rentit21Files\\ITU\\Log\\ItuLogs.txt";
            string contentType = "text/plain";
            DateTime now = DateTime.Now;
            string downloadFileName = "ItuLogFile - " + now.ToLongDateString() + " " + now.ToLongTimeString();
            return File(logFilePath, contentType, downloadFileName);
        }
    }
}