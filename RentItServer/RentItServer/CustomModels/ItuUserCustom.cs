using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RentItServer.ITU.DatabaseWrapperObjects;

namespace RentItServer
{
    public partial class User
    {
        public ITU.DatabaseWrapperObjects.User GetUser()
        {
            return new ITU.DatabaseWrapperObjects.User(Id, Username, Email);
        }

        public static List<ITU.DatabaseWrapperObjects.User> GetUsers(List<User> users)
        {
            List<ITU.DatabaseWrapperObjects.User> convertedUsers = new List<ITU.DatabaseWrapperObjects.User>(users.Count);
            foreach (User u in users)
            {
                convertedUsers.Add(u.GetUser());
            }
            return convertedUsers;
        }
    }
}