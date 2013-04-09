using System;
namespace RentItServer
{
    interface ISMURentItServiceAASDAD
    {
        void DeleteAccount(int userId);
        void DeleteBook(int bookId);
        System.IO.MemoryStream DownloadAudio(int bookId);
        System.IO.MemoryStream DownloadImage(int bookId);
        System.IO.MemoryStream DownloadPdf(int bookId);
        RentItServer.SMU.Rental[] GetActiveUserRentals(int userId);
        RentItServer.SMU.Book[] GetAllBooks();
        RentItServer.SMU.Rental[] GetAllUserRentals(int userId);
        RentItServer.SMU.Book GetBookInfo(int bookId);
        RentItServer.SMU.Book[] GetBooksByGenre(string genre);
        RentItServer.SMU.Book[] GetNewBooks();
        RentItServer.SMU.Book[] GetPopularBooks();
        RentItServer.SMU.Rental[] GetRental(int userId, int bookId);
        RentItServer.SMU.User GetUserInfo(int userId);
        int HasRental(int userId, int bookId);
        int LogIn(string email, string password);
        int RentBook(int userId, int bookId, int mediaType);
        RentItServer.SMU.Book[] SearchBooks(string searchString);
        int SignUp(string email, string name, string password, bool isAdmin);
        RentItServer.SMU.Book UpdateBook(int bookId, string title, string author, string description, string genre, DateTime dateAdded, double price, System.IO.MemoryStream image);
        RentItServer.SMU.User UpdateUserInfo(int userId, string email, string username, string password, bool isAdmin);
        void UploadAudio(int bookId, System.IO.MemoryStream mp3, string narrator);
        int UploadBook(string title, string author, string description, string genre, double price, System.IO.MemoryStream image);
        void UploadPdf(int bookId, System.IO.MemoryStream pdf);
    }
}
