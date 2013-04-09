using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RentItServer.SMU
{
    /// <summary>
    /// Class representing a user object as it lies in the database. 
    /// Fields not directly related to the user object but contained in the database has been omitted.
    /// This object is immutable.
    /// </summary>
    [DataContract]
    public class User
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <param name="email">The email of the user.</param>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="isAdmin">if set to <c>true</c> [is admin].</param>
        /// <param name="rentals">The rentals of the user.</param>
        public User(int id, string email, string username, string password, bool isAdmin, ICollection<SMUrental> rentals)
        {
            Id = id;
            Email = email;
            Username = username;
            Password = password;
            IsAdmin = isAdmin;
            Rentals = GetRentals(rentals);
        }

        /// <summary>
        /// Gets the id of the user.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [DataMember]
        public int Id { get; private set; }
        /// <summary>
        /// Gets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [DataMember]
        public string Email { get; private set; }
        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        [DataMember]
        public string Username { get; private set; }
        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [DataMember]
        public string Password { get; private set; }
        /// <summary>
        /// Gets a value indicating whether this instance is admin.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is admin; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsAdmin { get; private set; }
        /// <summary>
        /// Gets the rentals.
        /// </summary>
        /// <value>
        /// The rentals.
        /// </value>
        [DataMember]
        public ICollection<Rental> Rentals { get; private set; }

        /// <summary>
        /// Gets the rentals.
        /// </summary>
        /// <param name="smuRentals">The smu rentals.</param>
        /// <returns>The rentals</returns>
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