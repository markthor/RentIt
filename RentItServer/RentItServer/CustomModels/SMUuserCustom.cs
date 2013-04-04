using System;
using RentItServer.SMU;
using System.Collections;

namespace RentItServer
{
    public partial class SMUuser
    {
        public SMU.User GetUser()
        {
            return new SMU.User(id, email, username, password, isAdmin, SMUrentals);
        }
    }
}