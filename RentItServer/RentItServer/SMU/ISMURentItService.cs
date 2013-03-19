using System;
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
        int SignUp(string email, string name, string password);

        //existing user login (with email address)
        //returns 0 if email/pass is invalid
        //returns int userId if login successful
        //return -1 if email/pass is invalid
        //return userId if logIn successful
        [OperationContract]
        int LogIn(string email, string password);

        //get user account infoy
        //not sure what this will return. We want to get all info for a user (name, password, email etc.)
        [OperationContract]
        User GetUserInfo(int userId);

        //update user information
        //returns true if changes updated
        [OperationContract]
        bool UpdateUserInfo(int userId, string email, string username, string password);

        //delete user account
        //returns true if account deleted
        [OperationContract]
        bool DeleteAccount(int userId);

        //returns a list of books arranged by specified parameter (date, hit, etc)
        //list will contain listSize number of books 
        //List<Book> GetBooks(string sortString, int listSize);

        //get all information for a specific book
        //not sure what this will return. We want to get all info like Title, Author, Description, etc for a book.
        //Book GetBookInfo(int bookId);

        //check if user has rented a specific book
        //return -1 if not rented
        //return 0 if rented PDF
        //return 1 if rented audio
        //return 2 if rented both
        int HasRental(int userId, int bookId);

        //return rentId if successful
        // For mediaType:
        // 0 = book
        // 1 = audio
        // 2 = both
        int RentBook(int userId, int bookId, DateTime startDate, int mediaType);

        /***********************************************
         * Admin stuff
         * *********************************************/

        //delete a book
        bool DeleteBook(int userId, int bookId);

        int AddBook(int userId, string title, string author, string description, string genre, double price,
                            string pdfFilePath, string imageFilePath);
    }
}
