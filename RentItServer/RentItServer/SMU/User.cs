using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer.SMU
{
    [Serializable]
    public class User
    {
    
        public User(int id, string email, string username, string password, bool isAdmin, ICollection<SMUrental> rentals)
        {
            this.id = id;
            this.email = email;
            this.username = username;
            this.password = password;
            this.isAdmin = isAdmin;
            this.rentals = rentals;
        }

        public int id { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public bool isAdmin { get; set; }
    
        public virtual ICollection<SMUrental> rentals { get; set; }
    }
}