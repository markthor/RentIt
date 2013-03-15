using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using RentItServer.SMU;

namespace RentItServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SMURentItService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SMURentItService.svc or SMURentItService.svc.cs at the Solution Explorer and start debugging.
    public class SMURentItService : ISMURentItService
    {
        public int SignUp(string email, string name, string password)
        {
            throw new NotImplementedException("SingUp not implemented");
        }

        public int LogIn(string email, string password)
        {
            throw new NotImplementedException("LogIn not implemented");
        }

        public User GetUserInfo(int userId)
        {
            throw new NotImplementedException("GetUserInfo not implemented");
        }

        public bool UpdateUserInfo(int userId, string email, string name, string password)
        {
            throw new NotImplementedException("UpdateUserInfo not implemented");
        }

        public bool DeleteAccount(int userId)
        {
            throw new NotImplementedException("DeleteAccount not implemented");
        }

        public int HasRental(int userId, int bookId)
        {
            throw new NotImplementedException("HasRental not implemented");
        }

        public int RentBook(int userId, int bookId, DateTime startDate, int mediaType)
        {
            throw new NotImplementedException("RentBook not implemented");
        }

        public bool DeleteBook(int bookId)
        {
            throw new NotImplementedException("DeleteBook not implemented");
        }
    }
}
