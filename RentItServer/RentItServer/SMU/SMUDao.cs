﻿using System;
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
                var u = from user in proxy.SMUusers
                           where user.username == username
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
                SMUuser user = new SMUuser();
                user.email = email;
                user.username = name;
                user.password = password;
                user = proxy.SMUusers.Add(user);
                proxy.SaveChanges();
                return user.id;
            }
        }
    }
}