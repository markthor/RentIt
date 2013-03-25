using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using RentItServer.ITU;
using RentItServer.Utilities;

namespace RentItServer.SMU
{
    public class SMUController
    {
        private static readonly string DirectoryPath = "C:" + Path.DirectorySeparatorChar +
                                                        "Users" + Path.DirectorySeparatorChar +
                                                        "Mr.Green" + Path.DirectorySeparatorChar +
                                                        "Documents" + Path.DirectorySeparatorChar +
                                                        "SMU" + Path.DirectorySeparatorChar;

        private static readonly string LogFileName = "SmuLogs.txt";
        //Singleton instance of the class
        private static SMUController _instance;
        //Data access object for database IO
        private readonly SMUDao _dao = SMUDao.GetInstance();
        //The logger
        private readonly Logger _logger;
        //Event cast when log must make an _handler
        private static EventHandler _handler;
        //Data access object for file system IO
        private readonly FileSystemHandler _fileSystemHandler = FileSystemHandler.GetInstance();
        /// <summary>
        /// Accessor method to access the only instance of the class
        /// </summary>
        /// <returns>The singleton instance of the class</returns>
        public static SMUController GetInstance()
        {
            return _instance ?? (_instance = new SMUController());
        }

        private SMUController()
        {
            _logger = new Logger(FilePath.SMULogPath.GetPath() + Path.DirectorySeparatorChar + LogFileName, ref _handler);
        }

        public int LogIn(string email, string password)
        {
            int id;
            try
            {
                id = _dao.LogIn(email, password);
                if (_handler != null)
                    _handler(this, new RentItEventArgs("LogIn: " + email + "-" + password));
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("LogIn failed with exception [" + e + "]"));
                throw;
            }
            return id;
        }

        public int SignUp(string email, string username, string password, bool isAdmin)
        {
            int id;
            try
            {
                id = _dao.SignUp(email, username, password, isAdmin);
                if (_handler != null)
                    _handler(this, new RentItEventArgs("SignUp: " + email + "-" + username + "-" + password));
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("SignUp failed with exception [" + e + "]"));
                throw;
            }

            return id;
        }

        public User GetUser(int id)
        {
            User user;
            try
            {
                user = _dao.GetUser(id);
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("GetUser failed with exception [" + e + "]"));
                throw;
            }
            return user;
        }

        public User UpdateUserInfo(int userId, string email, string username, string password, bool isAdmin)
        {
            User user;
            try
            {
                user = _dao.UpdateUserInfo(userId, email, username, password, isAdmin);
                if (_handler != null)
                    _handler(this, new RentItEventArgs("UpdateUserInfo. User id [" + userId + "]'s new attributes: email [" + email + "] username [" + username + "] password [" + password + "] isAdmin [" + isAdmin + "]"));
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("UpdateUserInfo failed with exception [" + e + "]"));
                throw;
            }
            return user;
        }

        public void DeleteAccount(int userId)
        {
            try
            {
                _dao.DeleteAccount(userId);
                if (_handler != null)
                    _handler(this, new RentItEventArgs("DeleteAccount succeeded for userId [" + userId + "]"));
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("DeleteAccount failed with exception [" + e + "]"));
                throw;
            }
        }

        public int HasRental(int userId, int bookId)
        {
            int rentalId;
            try
            {
                rentalId = _dao.HasRental(userId, bookId);
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("HasRental failed with exception [" + e + "]"));
                throw;
            }
            return rentalId;
        }

        public int AddBook(string title, string author, string description, string genre, DateTime dateAdded, double price)
        {
            int bookId;
            try
            {
                bookId = _dao.AddBook(title, author, description, genre, dateAdded, price);
                if (_handler != null)
                    _handler(this, new RentItEventArgs("AddBook succeeded. Title [" + title + "] Author [" + author + "] Description [" + description + "] Genre [" + genre + "] Price [" + price + "]"));
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("AddBook failed with exception [" + e + "]"));
                throw;
            }
            return bookId;
        }

        public int RentBook(int userId, int bookId, DateTime time, int mediaType)
        {
            int rentalId;
            try
            {
                rentalId = _dao.RentBook(userId, bookId, mediaType);
                if (_handler != null)
                    _handler(this, new RentItEventArgs("RentBook succeeded. UserId [" + userId + "] bookId [" + bookId + "] mediaType [" + mediaType + "]"));
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("RentBook failed with exception [" + e + "]"));
                throw;
            }
            return rentalId;
        }

        public Book GetBookInfo(int bookId)
        {
            Book book;
            try
            {
                book = _dao.GetBookInfo(bookId);
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("GetBookInfo failed with exception [" + e + "]"));
                throw;
            }
            return book;
        }

        public void DeleteBook(int bookId)
        {
            try
            {
                _dao.DeleteBook(bookId);
                if (_handler != null)
                    _handler(this, new RentItEventArgs("DeleteBook succeeded for bookId [" + bookId + "]"));
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("DeleteBook failed with exception [" + e + "]"));
                throw;
            }
        }

        public List<Book> GetAllBooks()
        {
            List<Book> books;
            try
            {
                books = _dao.GetAllBooks();
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("GetAllBooks failed with exception [" + e + "]"));
                throw;
            }
            return books;
        }

        public List<Book> GetPopularBooks()
        {
            List<Book> books;
            try
            {
                books = _dao.GetPopularBooks();
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("GetPopularBooks failed with exception [" + e + "]"));
                throw;
            }
            return books;
        }

        public List<Book> SearchBooks(string searchString)
        {
            List<Book> books;
            try
            {
                books = _dao.SearhBooks(searchString);
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("SearchBooks failed with exception [" + e + "]"));
                throw;
            }
            return books;
        }

        public List<Book> GetBooksByGenre(string genre)
        {
            List<Book> books;
            try
            {
                books = _dao.GetBooksByGenre(genre);
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("GetBooksByGenre failed with exception [" + e + "]"));
                throw;
            }
            return books;
        }

        public Book UpdateBookInfo(int bookId, string title, string author, string description, string genre, DateTime dateAdded, double price)
        {
            Book theBook;
            try
            {
                theBook = _dao.UpdateBook(bookId, title, author, description, genre, dateAdded, price, "", "");
                if (_handler != null)
                    _handler(this,
                             new RentItEventArgs("UpdateBookInfo succeeded for book id [" + bookId +
                                                 "]. New attributes: title [" + title + "] author [" + author +
                                                 "] description [" + description + "] genre [" + genre + "] dateAdded [" +
                                                 dateAdded + "] price [" + price + "]."));
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("UpdateBookInfo failed with exception [" + e + "]"));
                throw;
            }
            return theBook;
        }

        public void UploadAudio(int bookId, MemoryStream MP3)
        {

        }

        public void UploadPDF(int bookId, MemoryStream pdf)
        {
            String relativePath = String.Format("{0}PDF_BookId_{1}.pdf", Path.DirectorySeparatorChar, bookId.ToString());
            _fileSystemHandler.WriteFile(FilePath.SMUPdfPath, relativePath, pdf);
        }

        public void DeleteSMUDatabaseData()
        {
            _dao.DeleteSMUDatabaseData();
        }
    }
}