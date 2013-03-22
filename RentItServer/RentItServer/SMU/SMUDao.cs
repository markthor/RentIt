using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public int SignUp(string email, string name, string password, bool isAdmin)
        {
            SMUuser user = new SMUuser();
            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {          
                user.email = email;
                user.username = name;
                user.password = password;
                user.isAdmin = isAdmin;
                proxy.SMUusers.Add(user);
                proxy.SaveChanges();         
            }
            return user.id;
        }

        public int LogIn(string email, string password)
        {
            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {
                var u = from user in proxy.SMUusers
                        where user.email == email && user.password == password
                        select user;
                if (u.Any())
                {
                    return u.First().id;
                }
                throw new ArgumentException("No SMUuser with email/password combination = " + email + "/" + password);
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

        public SMUuser UpdateUserInfo(int userId, string email, string username, string password, bool isAdmin)
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
                theUser.isAdmin = isAdmin;
                proxy.SaveChanges();

                return theUser;
            }
        }

        public void DeleteAccount(int userId)
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
            }
        }

        //return -1 if not rented
        //return 0 if rented PDF
        //return 1 if rented audio
        //return 2 if rented both
        public int HasRental(int userId, int bookId)
        {
            if (userId < 0) throw new ArgumentException("userId < 0");

            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {
                var rentals = from rental in proxy.SMUrentals
                              where rental.bookId == bookId && rental.SMUuser.id == userId
                              select rental;

                if (rentals.Any() == false)
                {
                    return -1;
                }
                SMUrental theRental = rentals.First();

                if (DateTime.Now.Subtract(theRental.startDate) > new TimeSpan(7, 0, 0, 0))
                {   // The rental has passed 7 days - it will be removed from the database
                    proxy.SMUrentals.Remove(theRental);
                    proxy.SaveChanges();
                    return -1;
                }

                if (theRental.audioId != null && theRental.bookId != null)
                {
                    return 2;
                }
                if (theRental.audioId != null)
                {
                    return 1;
                }
                if (theRental.bookId != null)
                {
                    return 0;
                }
                throw new Exception("There was a rental, but bookId and audioId was null. Dayum");
            }
        }

        public List<Book> GetAllBooks()
        {
            List<Book> list = new List<Book>();
            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {
                var books = from book in proxy.SMUbooks
                            select book;

                foreach (SMUbook book in books)
                {
                    list.Add(book.getBook());
                }
            }
            return list;
        }

        public List<Book> GetPopularBooks()
        {
            List<Book> list = new List<Book>();
            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {
                var books = from book in proxy.SMUbooks
                            orderby book.hit descending
                            select book;

                int limit = 30;
                foreach (SMUbook book in books)
                {
                    list.Add(book.getBook());
                    limit++;
                    if (limit >= 30) break;
                }
            }
            return list;
        }

        public List<Book> SearhBooks(string searchString)
        {
            if (searchString == null) throw new ArgumentNullException("searchString");
            List<Book> list = new List<Book>();
            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {
                var books = from book in proxy.SMUbooks
                            where book.title.StartsWith(searchString)
                            select book;

                foreach (SMUbook book in books)
                {
                    list.Add(book.getBook());
                }
            }
            return list;
        }

        public List<Book> GetBooksByGenre(String genre)
        {
            if (genre == null) throw new ArgumentNullException("genre");
            List<Book> list = new List<Book>();
            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {
                var books = from book in proxy.SMUbooks
                            where book.genre.Contains(genre)
                            select book;

                foreach (SMUbook book in books)
                {
                    list.Add(book.getBook());
                }
            }
            return list;
        }

        public Book GetBookInfo(int bookId)
        {
            Book theBook;
            using (RENTIT21Entities proxy = new RENTIT21Entities()){
                var books = from book in proxy.SMUbooks
                            where book.id == bookId
                            select book;

                if (books.Any() == false){
                    throw new ArgumentException("No book with bookId = " + bookId);
                }
                theBook = books.First().getBook();
            }
            return theBook;
        }

        public int RentBook(int userId, int bookId, int mediaType)
        {
            if (userId < 0) throw new ArgumentException("userId < 0");

            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {
                var books = from book in proxy.SMUbooks
                            where book.id == bookId
                            select book;

                if (books.Any() == false)
                {
                    throw new ArgumentException("no book with bookId = " + bookId);
                }
                SMUrental theRental = new SMUrental()
                {
                    userId = userId,
                    startDate = DateTime.Now
                };
                SMUbook theBook = books.First();
                theBook.hit += 1;
                if (mediaType == 0)
                {   // Only rent the book
                    theRental.bookId = bookId;
                }
                if (mediaType == 1)
                {   // Only rent the audio for the book
                    if (theBook.audioId == null)
                    {
                        throw new ArgumentException("mediaType parameter specified audio. The book [" + theBook.title + "] with id [" + theBook.id + "] is not associated with audio. ");
                    }
                    theRental.audioId = theBook.audioId;
                }
                if (mediaType == 2)
                {   // Rent both the book and the audio for the book
                    theRental.bookId = bookId;
                    theRental.audioId = theBook.audioId;
                }

                proxy.SMUrentals.Add(theRental);
                proxy.SaveChanges();
                return theRental.id;
            }
        }

        public bool DeleteBook(int bookId)
        {
            //if (userId < 0) throw new ArgumentException("userId < 0");

            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {
                //var users = from user in proxy.SMUusers
                //            where user.id == userId
                //            select user;
                //if (users.Any() == false)
                //{
                //    throw new ArgumentException("No user with userId = " + userId);
                //}
                //SMUuser theUser = users.First();
                //if (theUser.isAdmin == false)
                //{
                //    //throw new ArgumentException("User with userId = " + userId + " is not administrator");
                //}
                var books = from book in proxy.SMUbooks
                            where book.id == bookId
                            select book;

                if (books.Any() == false)
                {
                    throw new ArgumentException("No book with bookId = " + bookId);
                }

                SMUbook theBook = books.First();
                proxy.SMUbooks.Remove(theBook);
                proxy.SaveChanges();

                books = from book in proxy.SMUbooks
                        where book.id == bookId
                        select book;

                return books.Any() == false;
            }
        }

        public int AddAudio(int bookId, string narrator, string filePath)
        {
           // if (userId < 0) throw new ArgumentException("userId < 0");

            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {
                // TODO: Move the admin check to a separate method
                // Check if the user is an admin
                //var users = from user in proxy.SMUusers
                //            where user.id == userId
                //            select user;
                //if (users.Any() == false)
                //{
                //    throw new ArgumentException("No user with userId = " + userId);
                //}
                //SMUuser theUser = users.First();
                //if (theUser.isAdmin == false)
                //{
                //    throw new ArgumentException("User with userId = " + userId + " is not administrator");
                //}

                var books = from book in proxy.SMUbooks
                            where book.id == bookId
                            select book;
                if (books.Any() == false)
                {
                    throw new ArgumentException("No book with bookId = " + bookId);
                }
                SMUbook theBook = books.First();

                SMUaudio theAudio = new SMUaudio()
                {
                    narrator = narrator,
                    filePath = filePath,
                    SMUbooks = new Collection<SMUbook>(),
                    SMUrentals = new Collection<SMUrental>()
                };

                proxy.SMUaudios.Add(theAudio);
                proxy.SaveChanges();

                theBook.audioId = theAudio.id;
                theBook.SMUaudio = theAudio;
                proxy.SaveChanges();
                return theAudio.id;
            }
        }

        public int AddBook(string title, string author, string description, string genre, DateTime dateAdded, double price)
        {
            if (title == null) throw new ArgumentNullException("title");
            if (author == null) throw new ArgumentNullException("author");
            if (description == null) throw new ArgumentNullException("description");
            if (genre == null) throw new ArgumentNullException("genre");
           // if (userId < 0) throw new ArgumentException("userId < 0");
            if (price < 0.0) throw new ArgumentException("price < 0.0");
            if (title.Equals("")) throw new ArgumentException("title was empty");
            if (author.Equals("")) throw new ArgumentException("author was empty");
            if (genre.Equals("")) throw new ArgumentException("genre was empty");

            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {
                // Check if the user is an admin
                //var users = from user in proxy.SMUusers
                //            where user.id == userId
                //            select user;
                //if (users.Any() == false)
                //{
                //    throw new ArgumentException("No user with userId = " + userId);
                //}
                //SMUuser theUser = users.First();
                //if (theUser.isAdmin == false)
                //{
                //    throw new ArgumentException("User with userId = " + userId + " is not administrator");
                //}

                SMUbook theBook = new SMUbook()
                {
                    title = title,
                    author = author,
                    description = description,
                    genre = genre,
                    price = price,
                    audioId = null,
                    dateAdded = dateAdded,
                    SMUrentals = new Collection<SMUrental>(),
                    SMUaudio = null
                };

                proxy.SMUbooks.Add(theBook);
                proxy.SaveChanges();
                return theBook.id;
            }
        }

        public void UpdateBook(int bookId, String title, String author, String description, String genre,
                               DateTime dateAdded, double price, string pdfFilePath, string imageFilePath)
        {
            if (title == null) throw new ArgumentNullException("title");
            if (author == null) throw new ArgumentNullException("author");
            if (description == null) throw new ArgumentNullException("description");
            if (genre == null) throw new ArgumentNullException("genre");
            if (pdfFilePath == null) throw new ArgumentNullException("pdfFilePath");
            if (imageFilePath == null) throw new ArgumentNullException("imageFilePath");
           // if (userId < 0) throw new ArgumentException("userId < 0");
            if (price < 0.0) throw new ArgumentException("price < 0.0");
            if (title.Equals("")) throw new ArgumentException("title was empty");
            if (author.Equals("")) throw new ArgumentException("author was empty");
            if (genre.Equals("")) throw new ArgumentException("genre was empty");
            if (pdfFilePath.Equals("")) throw new ArgumentException("pdfFilePath was empty");

            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {
                // Check if the user is an admin
                //var users = from user in proxy.SMUusers
                //            where user.id == userId
                //            select user;
                //if (users.Any() == false)
                //{
                //    throw new ArgumentException("No user with userId = " + userId);
                //}
                //SMUuser theUser = users.First();
                //if (theUser.isAdmin == false)
                //{
                //    throw new ArgumentException("User with userId = " + userId + " is not administrator");
                //}

                var books = from book in proxy.SMUbooks
                            where book.id == bookId
                            select book;

                if (books.Any() == false){
                    throw new ArgumentException("No book with bookId = " + bookId);
                }

                SMUbook theBook = books.First();
                theBook.title = title;
                theBook.author = author;
                theBook.description = description;
                theBook.genre = genre;
                theBook.price = price;
                theBook.PDFFilePath = pdfFilePath;
                theBook.imageFilePath = imageFilePath;
                theBook.dateAdded = dateAdded;
                
                proxy.SaveChanges();
            }
        }
    }
}