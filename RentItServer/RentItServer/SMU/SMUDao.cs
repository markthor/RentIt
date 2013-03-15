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

        public int LogIn(string username, string password)
        {
            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {
                var u = from user in proxy.users
                           where user.username == username && user.password == password
                           select user;
                if (u.Any())
                    return u.First().id;
                else throw new ArgumentException("user not existing");
            }
        }

        public int SignUp(string email, string name, string password)
        {
            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {
                User user = new User();
                user.username = name;
                user.password = password;
                return proxy.users.Add(user).id;
            }
        }
    }
}