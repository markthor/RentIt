using System.Collections.Generic;

namespace RentItServer
{
    /// <summary>
    /// Partial class of the database entity "User". Used to add functionality to the class.
    /// </summary>
    public partial class User
    {
        /// <summary>
        /// Gets the wrapper for this user.
        /// </summary>
        /// <returns></returns>
        public ITU.DatabaseWrapperObjects.User GetUser()
        {
            return new ITU.DatabaseWrapperObjects.User(Id, Username, Email);
        }

        /// <summary>
        /// Gets the wrappers for these users.
        /// </summary>
        /// <param name="users">The users.</param>
        /// <returns></returns>
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