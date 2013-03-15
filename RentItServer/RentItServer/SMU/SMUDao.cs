using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer.SMU
{
    public class SMUDao
    {
        private static SMUDao _instance;
        public static SMUDao GetInstance()
        {
            
            return _instance ?? (_instance = new SMUDao());
        }
    }
}