using System;
using RentItServer.SMU;
using System.Collections;

namespace RentItServer
{
    /// <summary>
    /// This partial class extends functionality of the SMUuser entity used by the entity framework
    /// </summary>
    public partial class SMUuser
    {
        /// <summary>
        /// Gets the user representation of this object.
        /// </summary>
        /// <returns></returns>
        public SMU.User GetUser()
        {
            return new SMU.User(id, email, username, password, isAdmin, SMUrentals);
        }
    }
}