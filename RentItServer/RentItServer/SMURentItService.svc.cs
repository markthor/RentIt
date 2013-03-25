using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using RentItServer.SMU;

namespace RentItServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SMURentItService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SMURentItService.svc or SMURentItService.svc.cs at the Solution Explorer and start debugging.
    public class SMURentItService : ISMURentItService{
        private SMUController _smuController = SMUController.GetInstance();

        public int SignUp(string email, string name, string password, bool isAdmin)
        {
            return _smuController.SignUp(email, name, password, isAdmin);
        }

        public int LogIn(string email, string password)
        {
            return _smuController.LogIn(email, password);
        }

        public SMU.User GetUserInfo(int userId)
        {
            return _smuController.GetUser(userId);
        }

        public SMU.User UpdateUserInfo(int userId, string email, string username, string password, bool isAdmin)
        {
            return _smuController.UpdateUserInfo(userId, email, username, password, isAdmin);
        }

        public void DeleteAccount(int userId)
        {
            _smuController.DeleteAccount(userId);
        }

        public int HasRental(int userId, int bookId)
        {
            return _smuController.HasRental(userId, bookId);
        }

        public List<Book> GetAllBooks()
        {
            return _smuController.GetAllBooks();
        }

        public List<Book> GetPopularBooks()
        {
            return _smuController.GetPopularBooks();
        }

        public List<Book> SearchBooks(string searchString)
        {
            return _smuController.SearchBooks(searchString);
        }

        public List<Book> GetBooksByGenre(string genre)
        {
            return _smuController.GetBooksByGenre(genre);
        }

        public Book GetBookInfo(int bookId)
        {
            return _smuController.GetBookInfo(bookId);
        }

        public int RentBook(int userId, int bookId, DateTime startDate, int mediaType)
        {
            return _smuController.RentBook(userId, bookId, startDate, mediaType);
        }

        public MemoryStream DownloadAudio(int bookId)
        {
            throw new NotImplementedException();
        }

        public void DeleteBook(int bookId)
        {
            _smuController.DeleteBook(bookId);
        }

        public int UploadBook(string title, string author, string description, string genre, DateTime dateAdded, double price)
        {
            return _smuController.AddBook(title, author, description,genre,dateAdded, price);
        }

        public Book UpdateBook(int bookId, string title, string author, string description, string genre, DateTime dateAdded,
                               double price)
        {
            return _smuController.UpdateBookInfo(bookId, title, author, description, genre, dateAdded, price);
        }

        public void UploadAudio(int bookId, MemoryStream MP3)
        {
            throw new NotImplementedException();
        }

        public void UploadPDF(int bookId, MemoryStream pdf)
        {
            SMUController.GetInstance().UploadPDF(bookId, pdf);
        }

        public MemoryStream DownloadPDF(int bookId)
        {
            return SMUController.GetInstance().DownloadPDF(bookId);
        }
    }
}
