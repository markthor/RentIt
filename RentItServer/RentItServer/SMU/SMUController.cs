using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using RentItServer.Utilities;

namespace RentItServer.SMU
{
    public class SMUController
    {
        private const string LogFileName = "SmuLogs.txt";
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

        /// <summary>
        /// Prevents a default instance of the <see cref="SMUController"/> class from being created.
        /// </summary>
        private SMUController()
        {
            _logger = new Logger(FilePath.SMULogPath.GetPath() + LogFileName, ref _handler);
        }

        /// <summary>
        /// Log in the existing user.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// The id of the user, -1 if email/password pair is invalid
        /// </returns>
        public int LogIn(string email, string password)
        {
            int id;
            try
            {
                id = _dao.LogIn(email, password);
                if (_handler != null)
                    _handler(this, new RentItEventArgs("LogIn: " + email + "-" + password));
            }
            catch (ArgumentException)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("LogIn failed with due to email/password mismatch"));
                id = -1;
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("LogIn failed with due to exception [" + e + "]"));
                throw;
            }
            return id;
        }

        /// <summary>
        /// Signs up a new user account.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="isAdmin">if user is admin.</param>
        /// <returns>
        /// The id of the user, -1 if email is already in use
        /// </returns>
        public int SignUp(string email, string username, string password, bool isAdmin)
        {
            if (_dao.CheckIfEmailExistsInDb(email))
            {
                throw new ArgumentException(string.Format("A user with email {0} already exists", email));
            }
            try
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("SignUp: " + email + "-" + username + "-" + password));
                return _dao.SignUp(email, username, password, isAdmin);
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("SignUp failed with exception [" + e + "]"));
                throw;
            }
        }

        /// <summary>
        /// Gets the user account info.
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>
        /// The user with the associated userId, null if userId does not exist
        /// </returns>
        public User GetUser(int userId)
        {
            User user;
            try
            {
                user = _dao.GetUser(userId);
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("GetUser failed with exception [" + e + "]"));
                throw;
            }
            return user;
        }

        /// <summary>
        /// Updates the user info.
        /// </summary>
        /// <param name="userId">The user userId.</param>
        /// <param name="email">The email.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="isAdmin">The users admin status.</param>
        /// <returns>
        /// The updated user
        /// </returns>
        public User UpdateUserInfo(int userId, string email, string username, string password, bool isAdmin)
        {
            User user;
            try
            {
                user = _dao.UpdateUserInfo(userId, email, username, password, isAdmin);
                if (_handler != null)
                    _handler(this, new RentItEventArgs("UpdateUserInfo. User userId [" + userId + "]'s new attributes: email [" + email + "] username [" + username + "] password [" + password + "] isAdmin [" + isAdmin + "]"));
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("UpdateUserInfo failed with exception [" + e + "]"));
                throw;
            }
            return user;
        }

        /// <summary>
        /// Deletes the user account.
        /// </summary>
        /// <param name="userId">The user userId.</param>
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

        /// <summary>
        /// Determines whether the specified user id has a book rental.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="bookId">The book id.</param>
        /// <returns>
        /// 0 if user rented a pdf, 1 if user rented audio, 2 if user rented both, -1 if user haven't rented
        /// </returns>
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

        /// <summary>
        /// Adds a book object to the database withoud associated PDF or audio files.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="author">The author.</param>
        /// <param name="description">The description.</param>
        /// <param name="genre">The genre.</param>
        /// <param name="dateAdded">The date added.</param>
        /// <param name="price">The price.</param>
        /// <returns>
        /// The id of the book.
        /// </returns>
        public int AddBook(string title, string author, string description, string genre, DateTime dateAdded, double price, MemoryStream image)
        {
            int bookId;
            try
            {
                bookId = _dao.AddBook(title, author, description, genre, dateAdded, price);
                if (_handler != null)
                    _handler(this, new RentItEventArgs("AddBook succeeded. Title [" + title + "] Author [" + author + "] Description [" + description + "] Genre [" + genre + "] Price [" + price + "]"));
                SaveImage(bookId, image); //some error handling maybe? logging?
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("AddBook failed with exception [" + e + "]"));
                throw;
            }
            return bookId;
        }

        /// <summary>
        /// Rents a book or and audio file to a user.
        /// </summary>
        /// <param name="userId">The user id</param>
        /// <param name="bookId">The book id</param>
        /// <param name="startDate">The date that the rent starts</param>
        /// <param name="mediaType">0 if PDF, 1 if audio, 2 if both PDF and audio</param>
        /// <returns>
        /// The id of the rental object
        /// </returns>
        public int RentBook(int userId, int bookId, int mediaType)
        {
            int rentalId;
            try
            {
                rentalId = _dao.RentBook(userId, bookId, DateTime.Now, mediaType);
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

        /// <summary>
        /// Gets the book object associated with the book id.
        /// </summary>
        /// <param name="bookId">The book id.</param>
        /// <returns>
        /// The book
        /// </returns>
        public Book GetBookInfo(int bookId)
        {
            Book book;
            try
            {
                book = _dao.GetBookRepresentation(_dao.GetBookInfo(bookId));
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("GetBookInfo failed with exception [" + e + "]"));
                throw;
            }
            return book;
        }

        /// <summary>
        /// Deletes the book.
        /// </summary>
        /// <param name="bookId">The book id.</param>
        public void DeleteBook(int bookId)
        {
            try
            {
                SMUbook book = _dao.GetBookInfo(bookId);
                _dao.DeleteBook(bookId);
                string audio = null;
                try
                {
                    if (book.PDFFilePath != null && book.PDFFilePath.Equals(string.Empty))
                    {
                        _fileSystemHandler.DeleteFile(book.PDFFilePath);
                    }
                    if (book.imageFilePath != null && book.imageFilePath.Equals(string.Empty))
                    {
                        _fileSystemHandler.DeleteFile(book.imageFilePath);
                    }
                    if (book.audioFilePath != null)
                    {
                        audio = book.audioFilePath;
                        _fileSystemHandler.DeleteFile(book.audioFilePath);
                    }
                }
                catch (Exception e)
                {
                    string msg = "DeleteBook failed with exception [" + e + "], database entry was deleted successfully. ";
                    if (_fileSystemHandler.Exists(book.PDFFilePath))
                        msg += "Pdf file was not deleted at path ["+book.PDFFilePath+"]. ";
                    if (_fileSystemHandler.Exists(book.imageFilePath))
                        msg += "Image file was not deleted at path [" + book.imageFilePath + "]. ";
                    if (audio != null && _fileSystemHandler.Exists(audio))
                        msg += "Audio file was not deleted at path ["+audio+"]. ";

                    if (_handler != null)
                        _handler(this, new RentItEventArgs(msg));
                }
            }
            catch (Exception e)
            {
                //Delete book failed
                if (_handler != null)
                    _handler(this, new RentItEventArgs("DeleteBook failed with exception [" + e + "], no changes occurred."));
                throw;
            }
        }

        /// <summary>
        /// Gets all books.
        /// </summary>
        /// <returns>
        /// An array containing all books on the server
        /// </returns>
        public Book[] GetAllBooks()
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
            return books.ToArray();
        }

        /// <summary>
        /// Gets the 30 most popular books.
        /// </summary>
        /// <returns>
        /// An array containing 30 books with the most hits
        /// </returns>
        public Book[] GetPopularBooks()
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
            return books.ToArray();
        }

        /// <summary>
        /// Gets the books that have been added within the last 30 days.
        /// </summary>
        /// <returns>
        /// An array of all books added within the last 30 days
        /// </returns>
        public Book[] GetNewBooks()
        {
            List<Book> books;
            try
            {
                books = _dao.GetNewBooks();
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("GetNewBooks failed with exception [" + e + "]"));
                throw;
            }
            return books.ToArray();
        }

        /// <summary>
        /// Searches for books containing the search string.
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <returns>
        /// An array containing all books containing the search string
        /// </returns>
        public Book[] SearchBooks(string searchString)
        {
            List<Book> books;
            try
            {
                books = _dao.SearchBooks(searchString);
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("SearchBooks failed with exception [" + e + "]"));
                throw;
            }
            return books.ToArray();
        }

        /// <summary>
        /// Gets the books with the specified genre.
        /// </summary>
        /// <param name="genre">The genre.</param>
        /// <returns>
        /// Am array of all books matching the genre
        /// </returns>
        public Book[] GetBooksByGenre(string genre)
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
            return books.ToArray();
        }

        /// <summary>
        /// Updates the book.
        /// </summary>
        /// <param name="bookId">The book id. Can be null.</param>
        /// <param name="title">The title. Can be null.</param>
        /// <param name="author">The author. Can be null.</param>
        /// <param name="description">The description. Can be null.</param>
        /// <param name="genre">The genre. Can be null.</param>
        /// <param name="dateAdded">The date added.</param>
        /// <param name="price">The price. Negative values will not be saved (use if price is unchanged)</param>
        /// <returns>
        /// The updated book
        /// </returns>
        public Book UpdateBookInfo(int bookId, string title, string author, string description, string genre, DateTime dateAdded, double price, MemoryStream image)
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
                if (image != null)
                    SaveImage(bookId, image); //Error handling? logging?
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("UpdateBookInfo failed with exception [" + e + "]"));
                throw;
            }
            return theBook;
        }

        /// <summary>
        /// Uploads an audio file to a book. Overrides if audio file already exists.
        /// </summary>
        /// <param name="bookId">The book id.</param>
        /// <param name="mp3">The mp3.</param>
        /// <param name="narrator">The narrator.</param>
        public void UploadAudio(int bookId, MemoryStream mp3, string narrator)
        {
            try
            {
                _fileSystemHandler.WriteFile(FilePath.SMUAudioPath, FileName.GenerateAudioFileName(bookId), mp3);
                _dao.AddAudio(bookId, FilePath.SMUAudioPath.GetPath() + FileName.GenerateAudioFileName(bookId), narrator);
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("UploadAudio failed with exception [" + e + "]"));
                throw;
            }
        }

        /// <summary>
        /// Downloads the audio for the book.
        /// </summary>
        /// <param name="bookId">The book id.</param>
        /// <returns>
        /// Stream containing the audio.
        /// </returns>
        public MemoryStream DownloadAudio(int bookId)
        {
            MemoryStream theAudio;
            try
            {
                //TODO: DAO should probably have a method that gives you filepath given a certain book id
                theAudio = _fileSystemHandler.ReadFile(FilePath.SMUAudioPath, FileName.GenerateAudioFileName(bookId));
                theAudio.Position = 0L;
            }
            catch (Exception e)
            {
                if (_handler != null)
                    _handler(this, new RentItEventArgs("DownloadAudio failed with exception [" + e + "]"));
                throw;
            }
            return theAudio;
        }

        /// <summary>
        /// Uploads the PDF.
        /// </summary>
        /// <param name="bookId">The book id.</param>
        /// <param name="pdf">The PDF.</param>
        public void UploadPDF(int bookId, MemoryStream pdf)
        {
            string filename = FileName.GeneratePdfFileName(bookId);
            _fileSystemHandler.WriteFile(FilePath.SMUPdfPath, filename, pdf);
            string fullPath = string.Concat(FilePath.SMUPdfPath.GetPath(), filename);
            _dao.AddPdf(bookId, fullPath);
        }

        /// <summary>
        /// Downloads the PDF for the book.
        /// </summary>
        /// <param name="bookId">The book id.</param>
        /// <returns>
        /// Stream containing the contents of the pdf.
        /// </returns>
        public MemoryStream DownloadPDF(int bookId)
        {
            string filename = FileName.GeneratePdfFileName(bookId);
            return _fileSystemHandler.ReadFile(FilePath.SMUPdfPath, filename);
        }

        public void DeleteSMUDatabaseData()
        {
            _dao.DeleteSMUDatabaseData();
        }

        private void SaveImage(int bookId, MemoryStream image)
        {
            string filename = FileName.GenerateImageFileName(bookId);
            _fileSystemHandler.WriteFile(FilePath.SMUImagePath, filename, image);
            string fullPath = string.Concat(FilePath.SMUImagePath.GetPath(), filename);
            _dao.AddImage(bookId, fullPath);
        }
    }
}