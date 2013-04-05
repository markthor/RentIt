using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RentItServer.SMU
{
    [DataContract]
    public class User
    {
        public User(int id, string email, string username, string password, bool isAdmin, ICollection<SMUrental> rentals)
        {
            Id = id;
            Email = email;
            Username = username;
            Password = password;
            IsAdmin = isAdmin;
            Rentals = GetRentals(rentals);
        }

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public bool IsAdmin { get; set; }
        [DataMember]
        public ICollection<Rental> Rentals { get; set; }

        private ICollection<Rental> GetRentals(ICollection<SMUrental> smuRentals)
        {
            ICollection<Rental> r = new List<Rental>(smuRentals.Count);
            foreach (SMUrental smur in smuRentals)
            {
                r.Add(smur.GetRental());
            }
            return r;
        }
    }
}