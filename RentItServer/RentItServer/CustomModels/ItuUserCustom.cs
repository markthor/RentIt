using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer
{
    public partial class User
    {
        public ITU.User GetUser()
        {
            return new ITU.User(Id, Email, Username);
        }
    }
}