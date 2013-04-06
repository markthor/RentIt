using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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

        public User GetUserInfo(int userId)
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
                SMUuser result = users.First();
                return result.GetUser();
            }
        }

        public User UpdateUserInfo(int userId, string email, string username, string password, bool? isAdmin)
        {
            if (userId < 0) throw new ArgumentException("userId was below 0");
            if (email != null && email.Equals("")) throw new ArgumentException("email was empty");
            if (username != null && username.Equals("")) throw new ArgumentException("username was empty");
            if (password != null && password.Equals("")) throw new ArgumentException("password was empty");

            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {
                var users = from u in proxy.SMUusers
                            where u.id == userId
                            select u;

                if (users.Any() == false)
                {
                    throw new ArgumentException("No SMUuser with userid/password combination = " + userId + "/" + password);
                }

                SMUuser user = users.First();
                if (email != null)
                    user.email = email;
                if (username != null)
                    user.username = username;
                if (password != null)
                    user.password = password;
                if (isAdmin != null)
                    user.isAdmin = (bool)isAdmin;
                proxy.SaveChanges();

                return user.GetUser();
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

        /// <summary>
        /// Checks if the book is already rented
        /// </summary>
        /// <param name="userId">int</param>
        /// <param name="bookId">int</param>
        /// <returns>
        /// return -1 if not rented
        /// return 0 if rented PDF
        /// return 1 if rented audio
        /// return 2 if rented both
        /// </returns>
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

                List<SMUrental> list = rentals.ToList();
                bool aud = false;
                bool pdf = false;
                foreach (SMUrental rental in list)
                {
                    // tests if the rental is more than 7 days old
                    if (DateTime.UtcNow.Subtract(rental.startDate) < new TimeSpan(7, 0, 0, 0))
                    {
                        //tests for audio and book
                        if (rental.mediaType == 2)
                        {
                            aud = true;
                            pdf = true;
                        }
                        if (rental.mediaType == 1)
                        {
                            aud = true;
                        }
                        if (rental.mediaType == 0)
                        {
                            pdf = true;
                        }
                    }
                }
                // returns 2, 1 or 0 depending on audio and pdf
                if (aud == true && pdf == true)
                {
                    return 2;
                }
                if (aud == true)
                {
                    return 1;
                }
                if (pdf == true)
                {
                    return 0;
                }
                return -1;
            }
        }

        /// <summary>
        /// Gets the rental for the specified user id and book id. The rental with highest mediatype is always chosen
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="bookId">The book id.</param>
        /// <returns>
        /// The rental
        /// </returns>
        /// <exception cref="System.ArgumentException">No rental with specified userid/bookid pair</exception>
        public Rental[] GetRental(int userId, int bookId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var rentals = from rental in context.SMUrentals
                              where rental.SMUuser.id == userId && rental.bookId == bookId
                              orderby rental.mediaType descending
                              select rental;
                if (rentals.Any() == false) throw new ArgumentException("No rental with specified userid/bookid pair");
                List<SMUrental> list = rentals.ToList();
                List<Rental> returnList = new List<Rental>();
                foreach (SMUrental rent in list)
                    returnList.Add(rent.GetRental());
                return returnList.ToArray();
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
                    list.Add(book.GetBook());
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
                int accumulatorValue = 1;
                foreach (SMUbook book in books)
                {
                    list.Add(book.GetBook());
                    accumulatorValue++;
                    if (accumulatorValue >= limit) break;
                }
            }
            return list;
        }

        public List<Book> GetNewBooks()
        {
            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {
                var books = from b in proxy.SMUbooks
                            orderby b.dateAdded ascending
                            select b;

                List<Book> bookList = new List<Book>();
                if (books.Any())
                {
                    int count = 0;
                    foreach (SMUbook book in books)
                    {
                        bookList.Add(book.GetBook());
                        count++;
                        if (count == 30) break;
                    }
                }
                return bookList;
            }

        }

        public List<Book> SearchBooks(string searchString)
        {
            if (searchString == null) throw new ArgumentNullException("searchString");
            List<Book> list = new List<Book>();
            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {
                var books = from book in proxy.SMUbooks
                            where (book.title.Contains(searchString) ||
                                    book.author.Contains(searchString) ||
                                    book.description.Contains(searchString))
                            select book;

                foreach (SMUbook book in books)
                {
                    list.Add(book.GetBook());
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
                    list.Add(book.GetBook());
                }
            }
            return list;
        }

        public SMUbook GetBookInfo(int bookId)
        {
            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {
                var books = from book in proxy.SMUbooks
                            where book.id == bookId
                            select book;

                if (books.Any() == false)
                {
                    throw new ArgumentException("No book with bookId = " + bookId);
                }
                return books.First();
            }
        }

        public Book GetBookRepresentation(SMUbook theBook)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var books = from book in context.SMUbooks
                            where book.id == theBook.id
                            select book;

                if (books.Any() == false)
                {
                    throw new ArgumentException("No book with bookId = " + theBook.id);
                }
                return books.First().GetBook();
            }
        }

        public int RentBook(int userId, int bookId, DateTime time, int mediaType)
        {
            if (userId < 0) throw new ArgumentException("userId < 0");

            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {
                var books = from book in proxy.SMUbooks
                            where book.id == bookId
                            select book;

                if (books.Any() == false) throw new ArgumentException("no book with bookId = " + bookId);

                var us = from user in proxy.SMUusers
                         where user.id == userId
                         select user;

                if (us.Any() == false) throw new ArgumentException("No user with that userid");

                SMUrental theRental = new SMUrental()
                {
                    userId = userId,
                    startDate = time,
                    bookId = bookId,
                    mediaType = mediaType
                };
                SMUbook theBook = books.First();
                SMUuser theUser = us.First();

                if (mediaType == 0 || mediaType == 2)
                {   // Only rent the pdf, throw exception if there's no pdf path
                    if (theBook.PDFFilePath == null) throw new ArgumentException("mediaType parameter specified pdf. The book [" + theBook.title + "] with id [" + theBook.id + "] is not associated with a pdf. ");
                }
                if (mediaType == 1)
                {   // Only rent the audio for the book, throw exception if there's no audio path
                    if (theBook.audioFilePath == null) throw new ArgumentException("mediaType parameter specified audio. The book [" + theBook.title + "] with id [" + theBook.id + "] is not associated with audio. ");
                }
                theRental.SMUbook = theBook;
                theRental.SMUuser = theUser;
                theBook.hit += 1;
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

        public void AddAudio(int bookId, string filePath, string narrator)
        {
            // if (userId < 0) throw new ArgumentException("userId < 0");

            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {
                var books = from book in proxy.SMUbooks
                            where book.id == bookId
                            select book;
                if (books.Any() == false)
                {
                    throw new ArgumentException("No book with bookId = " + bookId);
                }
                SMUbook theBook = books.First();

                theBook.audioNarrator = narrator;
                theBook.audioFilePath = filePath;

                proxy.SaveChanges();
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
                SMUbook theBook = new SMUbook()
                {
                    title = title,
                    author = author,
                    description = description,
                    genre = genre,
                    price = price,
                    dateAdded = dateAdded,
                    SMUrentals = new Collection<SMUrental>(),
                };

                proxy.SMUbooks.Add(theBook);
                proxy.SaveChanges();
                return theBook.id;
            }
        }

        public Book UpdateBook(int bookId, String title, String author, String description, String genre, double? price)
        {
            SMUbook theBook;
            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {
                var books = from book in proxy.SMUbooks
                            where book.id == bookId
                            select book;

                if (books.Any() == false)
                {
                    throw new ArgumentException("No book with bookId = " + bookId);
                }
                theBook = books.First();
                if (title != null) theBook.title = title;
                if (author != null) theBook.author = author;
                if (description != null) theBook.description = description;
                if (genre != null) theBook.genre = genre;
                if (price != null) theBook.price = (double)price;

                proxy.SaveChanges();
            }
            return theBook.GetBook();
        }

        public void DeleteSMUDatabaseData()
        {
            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {
                //Delete all SMUusers
                var users = proxy.SMUusers;
                foreach (SMUuser u in users)
                {
                    proxy.SMUusers.Remove(u);
                }

                //Delete all SMUbooks
                var books = proxy.SMUbooks;
                foreach (SMUbook b in books)
                {
                    proxy.SMUbooks.Remove(b);
                }

                //Delete all rentals
                var rentals = proxy.SMUrentals;
                foreach (SMUrental r in rentals)
                {
                    proxy.SMUrentals.Remove(r);
                }
                proxy.SaveChanges();
            }
        }

        public void AddPdf(int bookId, string pdfFilePath)
        {
            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {
                var books = from b in proxy.SMUbooks
                            where b.id == bookId
                            select b;
                if (books.Any() == false)
                {
                    throw new ArgumentException("No book with bookId = " + bookId);
                }

                SMUbook book = books.First();
                book.PDFFilePath = pdfFilePath;
                proxy.SaveChanges();
            }
        }

        /// <summary>
        /// Check if a user already exist with the specified email
        /// </summary>
        /// <param name="email">The email to check for</param>
        /// <returns>True if a user with the email exists - false otherwise</returns>
        public bool CheckIfEmailExistsInDb(string email)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var users = from u in context.SMUusers
                            where u.email.Equals(email)
                            select u;
                return users.Any();
            }
        }

        public void AddImage(int bookId, string fullPath)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var books = from b in context.SMUbooks
                            where b.id == bookId
                            select b;
                if (books.Any() == false)
                {
                    throw new ArgumentException("No book with bookId = " + bookId);
                }

                SMUbook book = books.First();
                book.imageFilePath = fullPath;
                context.SaveChanges();
            }
        }

        public List<Rental> GetUserRentals(int userId)
        {
            List<Rental> list = new List<Rental>();
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var rentals = from r in context.SMUrentals
                              where r.userId == userId
                              select r;

                foreach (SMUrental r in rentals)
                {
                    list.Add(r.GetRental());
                }
            }
            return list;
        }
    }
}