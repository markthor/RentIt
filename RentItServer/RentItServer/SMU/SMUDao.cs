using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RentItServer.SMU
{
    /// <summary>
    /// Class responsible for communication with the database
    /// Primary author: Morten
    /// </summary>
    public class SMUDao
    {
        private static SMUDao _instance;
        /// <summary>
        /// SMUDao is a singleton
        /// </summary>
        /// <returns></returns>
        public static SMUDao GetInstance()
        {

            return _instance ?? (_instance = new SMUDao());
        }

        /// <summary>
        /// Creates a user in the database
        /// </summary>
        /// <param name="email">string email</param>
        /// <param name="name"> string name</param>
        /// <param name="password">string password</param>
        /// <param name="isAdmin">boolean indicating admin status</param>
        /// <returns>user Id</returns>
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

        /// <summary>
        /// Checks if the user is in the database
        /// </summary>
        /// <param name="email">string email</param>
        /// <param name="password">string password</param>
        /// <returns>user Id</returns>
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

        /// <summary>
        /// Returns Ifo about a User
        /// </summary>
        /// <param name="userId">int userId</param>
        /// <returns>User object</returns>
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

        /// <summary>
        /// Updates the database with info about a User
        /// </summary>
        /// <param name="userId">int userId</param>
        /// <param name="email">string email</param>
        /// <param name="username">string username</param>
        /// <param name="password">string password</param>
        /// <param name="isAdmin">boolean indicating admin status</param>
        /// <returns>The updated user</returns>
        public User UpdateUserInfo(int userId, string email, string username, string password, bool? isAdmin)
        {
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

        /// <summary>
        /// Deletes a User in the database
        /// </summary>
        /// <param name="userId">int UserId</param>
        public void DeleteAccount(int userId)
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

        /// <summary>
        /// Returns a List of all books in the database
        /// </summary>
        /// <returns>A List containing the Books</returns>
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

        /// <summary>
        /// Returns a List of the 30 most popular books
        /// </summary>
        /// <returns>List of Books</returns>
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

        /// <summary>
        /// Returns a List of the 30 newest books
        /// </summary>
        /// <returns>List of Books</returns>
        public List<Book> GetNewBooks()
        {
            using (RENTIT21Entities proxy = new RENTIT21Entities())
            {
                var books = from b in proxy.SMUbooks
                            orderby b.dateAdded descending
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

        /// <summary>
        /// Searches for a Book using a search string
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>List of Books that matches the search string</returns>
        public List<Book> SearchBooks(string searchString)
        {
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

        /// <summary>
        /// Returns a List of Books matching a specific genre
        /// </summary>
        /// <param name="genre">name of the genre</param>
        /// <returns>A List of Books</returns>
        public List<Book> GetBooksByGenre(String genre)
        {
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

        /// <summary>
        /// Returns info about a specific Book
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns>SMUBook</returns>
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

        /// <summary>
        /// Returns a Book object 
        /// </summary>
        /// <param name="theBook">A SMUBook object</param>
        /// <returns></returns>
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

        /// <summary>
        /// Makes a Rent Entry in the Database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bookId"></param>
        /// <param name="time"></param>
        /// <param name="mediaType">Type of media the User wants to Rent</param>
        /// <returns>The Rent Id</returns>
        public int RentBook(int userId, int bookId, DateTime time, int mediaType)
        {
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

        /// <summary>
        /// Deletes a Book
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns>true if Book is Deleted</returns>
        public bool DeleteBook(int bookId)
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

                SMUbook theBook = books.First();
                proxy.SMUbooks.Remove(theBook);
                proxy.SaveChanges();

                books = from book in proxy.SMUbooks
                        where book.id == bookId
                        select book;

                return books.Any() == false;
            }
        }

        /// <summary>
        /// Adds a MP3 File to a Book
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="filePath">Path of the File</param>
        /// <param name="narrator"></param>
        public void AddAudio(int bookId, string filePath, string narrator)
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
                SMUbook theBook = books.First();

                theBook.audioNarrator = narrator;
                theBook.audioFilePath = filePath;

                proxy.SaveChanges();
            }
        }

        /// <summary>
        /// Adds a Book object to the database
        /// </summary>
        /// <param name="title"></param>
        /// <param name="author"></param>
        /// <param name="description"></param>
        /// <param name="genre"></param>
        /// <param name="dateAdded"></param>
        /// <param name="price"></param>
        /// <returns>Book Id</returns>
        public int AddBook(string title, string author, string description, string genre, DateTime dateAdded, double price)
        {
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

        /// <summary>
        /// Updates Info about a Book
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="title"></param>
        /// <param name="author"></param>
        /// <param name="description"></param>
        /// <param name="genre"></param>
        /// <param name="price"></param>
        /// <returns>Book object</returns>
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

        /// <summary>
        /// Deletes Data in the database
        /// </summary>
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

        /// <summary>
        /// Adds a PDF file to a Book
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="pdfFilePath">Path of the PDF</param>
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

        /// <summary>
        /// Adds an image to the Book
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="fullPath"></param>
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

        /// <summary>
        /// Returns all rentals made by a User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List of Rentals</returns>
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

        /// <summary>
        /// Gets the audio path for the book.
        /// </summary>
        /// <param name="bookId">The book id.</param>
        /// <returns></returns>
        public string GetAudioPath(int bookId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var audioPaths = from book in context.SMUbooks
                                 where book.id == bookId
                                 select book.audioFilePath;
                if (audioPaths.Any() == true)
                {
                    return audioPaths.First();
                }
                return "";
            }
        }

        /// <summary>
        /// Gets the PDF path for the book.
        /// </summary>
        /// <param name="bookId">The book id.</param>
        /// <returns></returns>
        public string GetPdfPath(int bookId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var pdfPaths = from book in context.SMUbooks
                               where book.id == bookId
                               select book.PDFFilePath;
                if (pdfPaths.Any() == true)
                {
                    return pdfPaths.First();
                }
                return "";
            }
        }

        /// <summary>
        /// Gets the image path for the book.
        /// </summary>
        /// <param name="bookId">The book id.</param>
        /// <returns></returns>
        public string GetImagePath(int bookId)
        {
            using (RENTIT21Entities context = new RENTIT21Entities())
            {
                var imagePaths = from book in context.SMUbooks
                                 where book.id == bookId
                                 select book.imageFilePath;
                if (imagePaths.Any() == true)
                {
                    return imagePaths.First();
                }
                return "";
            }
        }
    }
}