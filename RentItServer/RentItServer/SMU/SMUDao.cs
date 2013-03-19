using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer.SMU
{
    public class SMUDao
    {
        private static SMUDao _instance;
        public static SMUDao GetInstance()
        {

            return _instance ?? (_instance = new SMUDao());
        }

        public int LogIn(string username, string password)
        {
            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {
                var u = from user in proxy.SMUusers
                        where user.username == username && user.password == password
                        select user;
                if (u.Any())
                {
                    return u.First().id;
                }
                throw new ArgumentException("No SMUuser with username/password combination = " + username + "/" + password);
            }
        }

        public int SignUp(string email, string name, string password)
        {
            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {
                SMUuser user = new SMUuser();
                user.email = email;
                user.username = name;
                user.password = password;
                proxy.SMUusers.Add(user);
                proxy.SaveChanges();
                return user.id;
            }
        }

        public SMUuser GetUser(int userId)
        {
            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {
                var users = from user in proxy.SMUusers
                            where user.id == userId
                            select user;

                if (users.Any() == false)
                {
                    throw new ArgumentException("No SMUuser with userId = " + userId);
                }
                return users.First();
            }
        }

        public bool UpdateUserInfo(int userId, string email, string username, string password)
        {
            if (userId < 0) throw new ArgumentException("userId was below 0");
            if (email == null) throw new ArgumentNullException("email");
            if (email.Equals("")) throw new ArgumentException("email was empty");
            if (username == null) throw new ArgumentNullException("username");
            if (username.Equals("")) throw new ArgumentException("username was empty");
            if (password == null) throw new ArgumentNullException("password");
            if (password.Equals("")) throw new ArgumentException("password was empty");

            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {
                var users = from user in proxy.SMUusers
                            where user.id == userId
                            select user;

                if (users.Any() == false)
                {
                    throw new ArgumentException("No SMUuser with userid/password combination = " + userId + "/" + password);
                }

                SMUuser theUser = users.First();
                theUser.email = email;
                theUser.username = username;
                theUser.password = password;
                proxy.SaveChanges();

                var updatedUsers = from user in proxy.SMUusers
                                   where user.id == userId && user.email.Equals(email) && user.username.Equals(username) &&
                                       user.password.Equals(password)
                                   select user;

                return updatedUsers.Any();
            }
        }

        public bool DeleteAccount(int userId)
        {
            if (userId < 0) throw new ArgumentException("userId < 0");

            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {
                var users = from user in proxy.SMUusers
                            where user.id == userId
                            select user;

                if (users.Any() == false)
                {
                    throw new ArgumentException("No SMUuser with userId = " + userId);
                }

                SMUuser theUser = users.First();
                proxy.SMUusers.Remove(theUser);
                proxy.SaveChanges();

                users = from user in proxy.SMUusers
                        where user.id == userId
                        select user;

                return users.Any() == false;
            }
        }

        //return -1 if not rented
        //return 0 if rented PDF
        //return 1 if rented audio
        //return 2 if rented both
        public int HasRental(int userId, int bookId)
        {
            if(userId < 0) throw new ArgumentException("userId < 0");

            using (RENTIT21Entities proxy = new RENTIT21Entities()){
                var rentals = from rental in proxy.SMUrentals
                              where rental.bookId == bookId && rental.SMUuser.id == userId
                              select rental;

                if (rentals.Any() == false){
                    return -1;
                }
                SMUrental theRental = rentals.First();
                if (theRental.audioId != null && theRental.bookId != null){
                    return 2;
                }
                if (theRental.audioId != null){
                    return 1;
                }
                if (theRental.bookId != null){
                    return 0;
                }
                throw new Exception("There was a rental, but bookId and audioId was null. Dayum");
            }
        }
    }
}