using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using RentItServer.ITU;

namespace RentItServer.SMU
{
    [ServiceContract]
    public interface ISMURentItService
    {
        //sign up a new user account
        //creates and int userId in database
        //returns 0 if sign up successful
        //returns 1 if email already in use
        //return -1 if email is already in use
        //return userId if sign up is successful
        [OperationContract]
        int SignUp(string email, string name, string password, bool isAdmin);

        //existing user login (with email address)
        //returns 0 if email/pass is invalid
        //returns int userId if login successful
        //return -1 if email/pass is invalid
        //return userId if logIn successful
        [OperationContract]
        int LogIn(string email, string password);

        //get user account info
        //not sure what this will return. We want to get all info for a user (name, password, email etc.)
        [OperationContract]
        User GetUserInfo(int userId);

        //update user information
        //returns true if changes updated
        [OperationContract]
        bool UpdateUserInfo(int userId, string email, string username, string password, bool isAdmin);

        //delete user account
        //returns true if account deleted
        [OperationContract]
        bool DeleteAccount(int userId);

        //returns a list of books arranged by specified parameter (date, hit, etc)
        //list will contain listSize number of books 
        //List<Book> GetBooks(string sortString, int listSize);

        //check if user has rented a specific book
        //return -1 if not rented
        //return 0 if rented PDF
        //return 1 if rented audio
        //return 2 if rented both
        [OperationContract]
        int HasRental(int userId, int bookId);

        //returns all books
        [OperationContract]
        List<Book> getAllBooks();

        //returns up to 30 books with the most hits.
        [OperationContract]
        List<Book> getPopularBooks();

        //returns books that contains the search string in its title or author.
        [OperationContract]
        List<Book> SearchBooks(String searchString);

        //returns books with the specified genre.
        [OperationContract]
        List<Book> GetBooksByGenre(String genre);

        //returns the book object with the specified id.
        [OperationContract]
        Book GetBookInfo(int bookId);

        /// <summary>
        /// Rents a book or and audio file to a user.
        /// </summary>
        /// <param name="userId">The user id</param>
        /// <param name="bookId">The book id</param>
        /// <param name="startDate">The date that the rent starts</param>
        /// <param name="mediaType">0 if PDF, 1 if audio, 2 if both PDF and audio</param>
        /// <returns>The id of the rental object</returns>
        [OperationContract]
        int RentBook(int userId, int bookId, DateTime startDate, int mediaType);

        //Returns a MemoryStream that contains the PDF file.
        [OperationContract]
        MemoryStream DownloadPDF(int bookId);

        //Returns a MemoryStream that contains the audio file.
        [OperationContract]
        MemoryStream DownloadAudio(int bookId);

        /***********************************************
         * Admin stuff
         * *********************************************/

        //delete a book
        [OperationContract]
        bool DeleteBook(int userId, int bookId);

        //Adds a book object withoud PDF or audio files.
        [OperationContract]
        int UploadBook(int userId, string title, string author, string description, string genre, DateTime dateAdded, double price);

        //Updates information of an existing book object.
        [OperationContract]
        Book UpdateBook(int bookId, String title, String author, String description, String genre, DateTime dateAdded, double price);

        //Uploads an audio file to a book. Overrides if audio is already existing.
        [OperationContract]
        void UploadAudio(int bookId, MemoryStream MP3);

        //Uploads a PDF file to a book. Overrides if PDF is already existing.
        [OperationContract]
        void UploadPDF(int bookID, MemoryStream PDF);
    }
}
