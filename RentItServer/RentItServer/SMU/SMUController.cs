using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer.SMU
{
    public class SMUController
    {
        //Singleton instance of the class
        private static SMUController _instance;
        //Data access object for database IO
        private readonly SMUDao _dao = SMUDao.GetInstance();
        //The logger
        private readonly SMULogger _logger = SMULogger.GetInstance();

        /// <summary>
        /// Accessor method to access the only instance of the class
        /// </summary>
        /// <returns>The singleton instance of the class</returns>
        public static SMUController GetInstance()
        {
            return _instance ?? (_instance = new SMUController());
        }

        public int LogIn(string username, string password)
        {
            return _dao.LogIn(username, password);
        }

        public int SignUp(string username, string password, string email)
        {
            return _dao.SignUp(username, password, email);
        }


    }
}