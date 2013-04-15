using System;
using System.Collections.Generic;
using System.IO;
using RentItServer.Utilities;

namespace RentItServer.SMU
{
    public class SMUController
    {
        //Singleton instance of the class
        private static SMUController _instance;
        //Data access object for database IO
        private readonly SMUDao _dao = SMUDao.GetInstance();
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
        private SMUController() { }

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
            }
            catch (ArgumentException)
            {
                id = -1;
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
            return _dao.SignUp(email, username, password, isAdmin);
        }

        /// <summary>
        /// Gets the user account info.
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>
        /// The user with the associated userId, null if userId does not exist
        /// </returns>
        public User GetUserInfo(int userId)
        {
            return _dao.GetUserInfo(userId);
        }

        /// <summary>
        /// Updates the user info.
        /// </summary>
        /// <param name="userId">The user userId.</param>
        /// <param name="email">The email. Can be null.</param>
        /// <param name="username">The username. Can be null.</param>
        /// <param name="password">The password. Can be null.</param>
        /// <param name="isAdmin">The users admin status. Can be null.</param>
        /// <returns>
        /// The updated user
        /// </returns>
        public User UpdateUserInfo(int userId, string email, string username, string password, bool? isAdmin)
        {
            if (userId < 0) throw new ArgumentException("userId was below 0");
            if (email != null && email.Equals("")) throw new ArgumentException("email was empty");
            if (username != null && username.Equals("")) throw new ArgumentException("username was empty");
            if (password != null && password.Equals("")) throw new ArgumentException("password was empty");

            if (email != null)
            {
                if (_dao.CheckIfEmailExistsInDb(email))
                {
                    throw new ArgumentException("Email is already in use");
                }
            }
            return _dao.UpdateUserInfo(userId, email, username, password, isAdmin);
        }

        /// <summary>
        /// Deletes the user account.
        /// </summary>
        /// <param name="userId">The user userId.</param>
        public void DeleteAccount(int userId)
        {
            if (userId < 0) throw new ArgumentException("userId < 0");

            _dao.DeleteAccount(userId);
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
            if (userId < 0) throw new ArgumentException("userId < 0");

            return _dao.HasRental(userId, bookId);
        }

        /// <summary>
        /// Gets the rental for the specified user id and book id.The rental with highest mediatype is always chosen
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="bookId">The book id.</param>
        /// <returns>
        /// The rental
        /// </returns>
        public Rental[] GetRental(int userId, int bookId)
        {
            return _dao.GetRental(userId, bookId);
        }

        /// <summary>
        /// Adds a book object to the database withoud associated PDF or audio files.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="author">The author.</param>
        /// <param name="description">The description.</param>
        /// <param name="genre">The genre.</param>
        /// <param name="price">The price.</param>
        /// <param name="image">The MemoryStream containing the image</param>
        /// <returns>
        /// The id of the book.
        /// </returns>
        public int AddBook(string title, string author, string description, string genre, double price, MemoryStream image)
        {
            if (title == null) throw new ArgumentNullException("title");
            if (author == null) throw new ArgumentNullException("author");
            if (description == null) throw new ArgumentNullException("description");
            if (genre == null) throw new ArgumentNullException("genre");
            if (price < 0.0) throw new ArgumentException("price < 0.0");
            if (title.Equals("")) throw new ArgumentException("title was empty");
            if (author.Equals("")) throw new ArgumentException("author was empty");
            if (genre.Equals("")) throw new ArgumentException("genre was empty");

            int bookId = _dao.AddBook(title, author, description, genre, DateTime.UtcNow, price);
            SaveImage(bookId, image);
            return bookId;
        }

        /// <summary>
        /// Rents a book or and audio file to a user.
        /// </summary>
        /// <param name="userId">The user id</param>
        /// <param name="bookId">The book id</param>
        /// <param name="mediaType">0 if PDF, 1 if audio, 2 if both PDF and audio</param>
        /// <returns>
        /// The id of the rental object
        /// </returns>
        public int RentBook(int userId, int bookId, int mediaType)
        {
            if (userId < 0) throw new ArgumentException("userId < 0");

            return _dao.RentBook(userId, bookId, DateTime.UtcNow, mediaType);
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
            return _dao.GetBookRepresentation(_dao.GetBookInfo(bookId));
        }

        /// <summary>
        /// Deletes the book.
        /// </summary>
        /// <param name="bookId">The book id.</param>
        public void DeleteBook(int bookId)
        {
            SMUbook book = _dao.GetBookInfo(bookId);
            _dao.DeleteBook(bookId);

            if (book.PDFFilePath != null && !book.PDFFilePath.Equals(string.Empty))
            {
                _fileSystemHandler.DeleteFile(book.PDFFilePath);
            }
            if (book.imageFilePath != null && !book.imageFilePath.Equals(string.Empty))
            {
                _fileSystemHandler.DeleteFile(book.imageFilePath);
            }
            if (book.audioFilePath != null)
            {
                _fileSystemHandler.DeleteFile(book.audioFilePath);
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
            return _dao.GetAllBooks().ToArray();
        }

        /// <summary>
        /// Gets the 30 most popular books.
        /// </summary>
        /// <returns>
        /// An array containing 30 books with the most hits
        /// </returns>
        public Book[] GetPopularBooks()
        {
            return _dao.GetPopularBooks().ToArray();
        }

        /// <summary>
        /// Gets the books that have been added within the last 30 days.
        /// </summary>
        /// <returns>
        /// An array of all books added within the last 30 days
        /// </returns>
        public Book[] GetNewBooks()
        {
            return _dao.GetNewBooks().ToArray();
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
            if (searchString == null) throw new ArgumentNullException("searchString");

            return _dao.SearchBooks(searchString).ToArray();
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
            if (genre == null) throw new ArgumentNullException("genre");

            return _dao.GetBooksByGenre(genre).ToArray();
        }

        /// <summary>
        /// Updates the book.
        /// </summary>
        /// <param name="bookId">The book id. Can be null.</param>
        /// <param name="title">The title. Can be null.</param>
        /// <param name="author">The author. Can be null.</param>
        /// <param name="description">The description. Can be null.</param>
        /// <param name="genre">The genre. Can be null.</param>
        /// <param name="price">The price. Negative values will not be saved (use if price is unchanged)</param>
        /// <param name="image">The image.</param>
        /// <returns>
        /// The updated book
        /// </returns>
        public Book UpdateBookInfo(int bookId, string title, string author, string description, string genre, double? price, MemoryStream image)
        {
            Book book = _dao.UpdateBook(bookId, title, author, description, genre, price);
            if (image != null)
                SaveImage(bookId, image);
            return book;
        }

        /// <summary>
        /// Uploads an audio file to a book. Overrides if audio file already exists.
        /// </summary>
        /// <param name="bookId">The book id.</param>
        /// <param name="mp3">The mp3.</param>
        /// <param name="narrator">The narrator.</param>
        public void UploadAudio(int bookId, MemoryStream mp3, string narrator)
        {
            _fileSystemHandler.WriteFile(FilePath.SMUAudioPath, FileName.GenerateAudioFileName(bookId), mp3);
            _dao.AddAudio(bookId, FilePath.SMUAudioPath.GetPath() + FileName.GenerateAudioFileName(bookId), narrator);
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
            return _fileSystemHandler.ReadFile(_dao.GetAudioPath(bookId));
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
            return _fileSystemHandler.ReadFile(_dao.GetPdfPath(bookId));
        }

        /// <summary>
        /// Calls delete database on the Dao
        /// </summary>
        public void DeleteSMUDatabaseData()
        {
            _dao.DeleteSMUDatabaseData();
        }

        /// <summary>
        /// Saves an image in the filesystem and adds it to a Book in the Database
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="image">Memorystream containing image</param>
        private void SaveImage(int bookId, MemoryStream image)
        {
            string filename = FileName.GenerateImageFileName(bookId);
            _fileSystemHandler.WriteFile(FilePath.SMUImagePath, filename, image);
            string fullPath = string.Concat(FilePath.SMUImagePath.GetPath(), filename);
            _dao.AddImage(bookId, fullPath);
        }

        /// <summary>
        /// Downloads an image from the filesystem
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns>Memorystream containing image</returns>
        public MemoryStream DownloadImage(int bookId)
        {
            return _fileSystemHandler.ReadFile(_dao.GetImagePath(bookId));
        }

        /// <summary>
        /// Returns the active rentals made by a User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Array of rentals</returns>
        public Rental[] GetActiveUserRentals(int userId)
        {
            List<Rental> activeRentals = new List<Rental>();

            foreach (Rental rental in _dao.GetUserRentals(userId))
            {
                if (rental.StartDate.AddDays(7) > DateTime.UtcNow)
                {
                    activeRentals.Add(rental);
                }
            }
            return activeRentals.ToArray();
        }

        /// <summary>
        /// Returns all Rentals made by a User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Array of Rentals</returns>
        public Rental[] GetAllUserRentals(int userId)
        {
            return _dao.GetUserRentals(userId).ToArray();
        }
    }
}