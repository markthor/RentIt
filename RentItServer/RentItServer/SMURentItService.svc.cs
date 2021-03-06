﻿using System;
using System.IO;

using RentItServer.SMU;

namespace RentItServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SMURentItService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SMURentItService.svc or SMURentItService.svc.cs at the Solution Explorer and start debugging.
    public class SMURentItService : ISMURentItService{
        
        private readonly SMUController _smuController = SMUController.GetInstance();

        /// <summary>
        /// Signs up a new user account.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="name">The name</param>
        /// <param name="password">The password</param>
        /// <param name="isAdmin">if user is admin</param>
        /// <returns>
        /// The id of the user, -1 if email is already in use
        /// </returns>
        public int SignUp(string email, string name, string password, bool isAdmin)
        {
            return _smuController.SignUp(email, name, password, isAdmin);
        }

        /// <summary>
        /// Log in the existing user
        /// </summary>
        /// <param name="email">The email</param>
        /// <param name="password">The password</param>
        /// <returns>
        /// The id of the user, -1 if email/password pair is invalid
        /// </returns>
        public int LogIn(string email, string password)
        {
            return _smuController.LogIn(email, password);
        }

        /// <summary>
        /// Gets the user account info
        /// </summary>
        /// <param name="userId">The user id</param>
        /// <returns>
        /// The user with the associated id, null if userId does not exist
        /// </returns>
        public SMU.User GetUserInfo(int userId)
        {
            return _smuController.GetUserInfo(userId);
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
        public SMU.User UpdateUserInfo(int userId, string email, string username, string password, bool? isAdmin)
        {
            return _smuController.UpdateUserInfo(userId, email, username, password, isAdmin);
        }

        /// <summary>
        /// Deletes the user account.
        /// </summary>
        /// <param name="userId">The user id.</param>
        public void DeleteAccount(int userId)
        {
            _smuController.DeleteAccount(userId);
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
            return _smuController.HasRental(userId, bookId);
        }

        /// <summary>
        /// Gets the rental for the specified user id and book id. The rental with highest mediatype is always chosen
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="bookId">The book id.</param>
        /// <returns>
        /// The rental
        /// </returns>
        public Rental[] GetRental(int userId, int bookId)
        {
            return _smuController.GetRental(userId, bookId);
        }

        /// <summary>
        /// Gets all books.
        /// </summary>
        /// <returns>
        /// An array containing all books on the server
        /// </returns>
        public Book[] GetAllBooks()
        {
            return _smuController.GetAllBooks();
        }

        /// <summary>
        /// Gets the 30 most popular books.
        /// </summary>
        /// <returns>
        /// An array containing 30 books with the most hits
        /// </returns>
        public Book[] GetPopularBooks()
        {
            return _smuController.GetPopularBooks();
        }

        /// <summary>
        /// Gets the books that have been added within the last 30 days.
        /// </summary>
        /// <returns>
        /// An array of all books added within the last 30 days
        /// </returns>
        public Book[] GetNewBooks()
        {
            return _smuController.GetNewBooks();
        }

        /// <summary>
        /// Searches for books containing the search string.
        /// </summary>
        /// <param name="Name">The search string.</param>
        /// <returns>
        /// An array containing all books containing the search string
        /// </returns>
        public Book[] SearchBooks(string Name)
        {
            return _smuController.SearchBooks(Name);
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
            return _smuController.GetBooksByGenre(genre);
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
            return _smuController.GetBookInfo(bookId);
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
            return _smuController.RentBook(userId, bookId, mediaType);
        }

        /// <summary>
        /// Deletes the book.
        /// </summary>
        /// <param name="bookId">The book id.</param>
        public void DeleteBook(int bookId)
        {
            _smuController.DeleteBook(bookId);
        }

        /// <summary>
        /// Adds a book object to the databse withoud associated PDF or audio files.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="author">The author.</param>
        /// <param name="description">The description.</param>
        /// <param name="genre">The genre.</param>
        /// <param name="price">The price.</param>
        /// <param name="image">The stream containing the image</param>
        /// <returns>
        /// The id of the book.
        /// </returns>
        public int UploadBook(string title, string author, string description, string genre, double price, MemoryStream image)
        {
            return _smuController.AddBook(title, author, description,genre, price, image);
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
        public Book UpdateBook(int bookId, string title, string author, string description, string genre, double? price, MemoryStream image)
        {
            return _smuController.UpdateBookInfo(bookId, title, author, description, genre, price, image);
        }

        /// <summary>
        /// Uploads an audio file to a book. Overrides if audio file already exists.
        /// </summary>
        /// <param name="bookId">The book id.</param>
        /// <param name="mp3">The Mp3.</param>
        public void UploadAudio(int bookId, MemoryStream mp3, string narrator)
        {
            _smuController.UploadAudio(bookId, mp3, narrator);
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
            return _smuController.DownloadAudio(bookId);
        }

        /// <summary>
        /// Uploads the PDF.
        /// </summary>
        /// <param name="bookId">The book id.</param>
        /// <param name="pdf">The PDF.</param>
        public void UploadPdf(int bookId, MemoryStream pdf)
        {
            _smuController.UploadPDF(bookId, pdf);
        }

        /// <summary>
        /// Downloads the PDF for the book.
        /// </summary>
        /// <param name="bookId">The book id.</param>
        /// <returns>
        /// Stream containing the contents of the pdf.
        /// </returns>
        public MemoryStream DownloadPdf(int bookId)
        {
            return _smuController.DownloadPDF(bookId);
        }

        /// <summary>
        /// Metod to download an the image for a specific book
        /// </summary>
        /// <param name="bookId">The book id</param>
        /// <returns>Memorystream containing a .jpg image file</returns>
        public MemoryStream DownloadImage(int bookId)
        {
            return _smuController.DownloadImage(bookId);
        }

        /// <summary>
        /// Method to retreieve all active rentals for a specific user
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>Array of all active rentals for the given user</returns>
        public Rental[] GetActiveUserRentals(int userId)
        {
            return _smuController.GetActiveUserRentals(userId);
        }

        /// <summary>
        /// Returns all rentals for the specified user
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>Array of all rentals ever created by the user</returns>
        public Rental[] GetAllUserRentals(int userId)
        {
            return _smuController.GetAllUserRentals(userId);
        }
    }
}
