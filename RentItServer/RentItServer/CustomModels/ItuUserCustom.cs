using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer
{
    public partial class User
    {
        public ITU.DataObjects.User GetUser()
        {
            return new ITU.DataObjects.User(Id, Username, Email);
        }
    }
}