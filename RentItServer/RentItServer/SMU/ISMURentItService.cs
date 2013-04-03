using System;
using System.IO;
using System.ServiceModel;

namespace RentItServer.SMU
{
    [ServiceContract]
    public interface ISMURentItService
    {
        /// <summary>
        /// Signs up a new user account.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="name">The name.</param>
        /// <param name="password">The password.</param>
        /// <param name="isAdmin">if user is admin.</param>
        /// <returns>The id of the user, -1 if email is already in use</returns>
        [OperationContract]
        int SignUp(string email, string name, string password, bool isAdmin);

        /// <summary>
        /// Log in the existing user.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns>The id of the user, -1 if email/password pair is invalid</returns>
        [OperationContract]
        int LogIn(string email, string password);

        /// <summary>
        /// Gets the user account info.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>The user with the associated id, null if userId does not exist</returns>
        [OperationContract]
        User GetUserInfo(int userId);

        /// <summary>
        /// Updates the user info.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="email">The email.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="isAdmin">The users admin status.</param>
        /// <returns>The updated user</returns>
        [OperationContract]
        User UpdateUserInfo(int userId, string email, string username, string password, bool isAdmin);

        /// <summary>
        /// Deletes the user account.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <exception cref="ArgumentException">userId doesn't exist.</exception>
        [OperationContract]
        void DeleteAccount(int userId);

        /// <summary>
        /// Gets all books.
        /// </summary>
        /// <returns>An array containing all books on the server</returns>
        [OperationContract]
        Book[] GetAllBooks();

        /// <summary>
        /// Gets the 30 most popular books.
        /// </summary>
        /// <returns>An array containing 30 books with the most hits</returns>
        [OperationContract]
        Book[] GetPopularBooks();

        /// <summary>
        /// Gets the books that have been added within the last 30 days.
        /// </summary>
        /// <returns>An array of all books added within the last 30 days</returns>
        [OperationContract]
        Book[] GetNewBooks();

        /// <summary>
        /// Searches for books containing the search string.
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <returns>An array containing all books containing the search string</returns>
        [OperationContract]
        Book[] SearchBooks(String searchString);

        //returns books with the specified genre.
        /// <summary>
        /// Gets the books with the specified genre.
        /// </summary>
        /// <param name="genre">The genre.</param>
        /// <returns>Am array of all books matching the genre</returns>
        [OperationContract]
        Book[] GetBooksByGenre(String genre);

        /// <summary>
        /// Gets the book object associated with the book id.
        /// </summary>
        /// <param name="bookId">The book id.</param>
        /// <returns>The book</returns>
        /// <exception cref="ArgumentException">A book with the given id does not exist</exception>
        [OperationContract]
        Book GetBookInfo(int bookId);

        /// <summary>
        /// Determines whether the specified user id has a book rental.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="bookId">The book id.</param>
        /// <returns>0 if user rented a pdf, 1 if user rented audio, 2 if user rented both, -1 if user haven't rented</returns>
        [OperationContract]
        int HasRental(int userId, int bookId);

        /// <summary>
        /// Gets the rental for the specified user id and book id. The rental with highest mediatype is always chosen
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="bookId">The book id.</param>
        /// <returns>The rental</returns>
        [OperationContract]
        Rental[] GetRental(int userId, int bookId);

        /// <summary>
        /// Rents a book or and audio file to a user.
        /// </summary>
        /// <param name="userId">The user id</param>
        /// <param name="bookId">The book id</param>
        /// <param name="mediaType">0 if PDF, 1 if audio, 2 if both PDF and audio</param>
        /// <returns>The id of the rental object</returns>
        [OperationContract]
        int RentBook(int userId, int bookId, int mediaType);

        /// <summary>
        /// Downloads the PDF for the book.
        /// </summary>
        /// <param name="bookId">The book id.</param>
        /// <returns>Stream containing the contents of the pdf.</returns>
        [OperationContract]
        MemoryStream DownloadPdf(int bookId);

        /// <summary>
        /// Downloads the audio for the book.
        /// </summary>
        /// <param name="bookId">The book id.</param>
        /// <returns>Stream containing the audio.</returns>
        [OperationContract]
        MemoryStream DownloadAudio(int bookId);

        /***********************************************
         * Admin stuff
         * *********************************************/

        /// <summary>
        /// Deletes the book.
        /// </summary>
        /// <param name="bookId">The book id.</param>
        /// <exception cref="ArgumentException">book id does not exist.</exception>
        [OperationContract]
        void DeleteBook(int bookId);

        /// <summary>
        /// Adds a book object to the databse withoud associated PDF or audio files.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="author">The author.</param>
        /// <param name="description">The description.</param>
        /// <param name="genre">The genre.</param>
        /// <param name="price">The price.</param>
        /// <param name="image">The MemoryStream containing the image</param>
        /// <returns>The id of the book.</returns>
        [OperationContract]
        int UploadBook(string title, string author, string description, string genre, double price, MemoryStream image);

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
        /// <param name="image">The MemoryStream containing the image</param>
        /// <returns>The updated book</returns>
        [OperationContract]
        Book UpdateBook(int bookId, String title, String author, String description, String genre, DateTime dateAdded, double price, MemoryStream image);

        /// <summary>
        /// Uploads an audio file to a book. Overrides if audio file already exists.
        /// </summary>
        /// <param name="bookId">The book id.</param>
        /// <param name="mp3">The Mp3.</param>
        /// <param name="narrator">The narrator.</param>
        [OperationContract]
        void UploadAudio(int bookId, MemoryStream mp3, string narrator);

        /// <summary>
        /// Uploads a PDF file to a book. Overrides if PDF file already exists.
        /// </summary>
        /// <param name="bookId">The id of the book</param>
        /// <param name="pdf">The MemoryStream containing the pdf</param>
        [OperationContract]
        void UploadPdf(int bookId, MemoryStream pdf);

        [OperationContract]
        MemoryStream DownloadImage(int bookId);

        [OperationContract]
        Rental[] GetActiveUserRentals(int userId);

        [OperationContract]
        Rental[] GetAllUserRentals(int userId);
    }
}
