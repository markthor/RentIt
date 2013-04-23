using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer
{
    public partial class User
    {
        public ITU.DatabaseWrapperObjects.User GetUser()
        {
            return new ITU.DatabaseWrapperObjects.User(Id, Username, Email);
        }
    }
}