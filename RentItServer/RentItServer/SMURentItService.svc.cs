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
    public class SMURentItService : ISMURentItService
    {
        public int SignUp(string email, string name, string password, bool isAdmin)
        {
            throw new NotImplementedException();
        }

        public int LogIn(string email, string password)
        {
            throw new NotImplementedException();
        }

        public User GetUserInfo(int userId)
        {
            throw new NotImplementedException();
        }

        public User UpdateUserInfo(int userId, string email, string username, string password, bool isAdmin)
        {
            throw new NotImplementedException();
        }

        public void DeleteAccount(int userId)
        {
            throw new NotImplementedException();
        }

        public int HasRental(int userId, int bookId)
        {
            throw new NotImplementedException();
        }

        public List<Book> GetAllBooks()
        {
            throw new NotImplementedException();
        }

        public List<Book> GetPopularBooks()
        {
            throw new NotImplementedException();
        }

        public List<Book> SearchBooks(string searchString)
        {
            throw new NotImplementedException();
        }

        public List<Book> GetBooksByGenre(string genre)
        {
            throw new NotImplementedException();
        }

        public Book GetBookInfo(int bookId)
        {
            throw new NotImplementedException();
        }

        public int RentBook(int userId, int bookId, DateTime startDate, int mediaType)
        {
            throw new NotImplementedException();
        }

        public MemoryStream DownloadPDF(int bookId)
        {
            throw new NotImplementedException();
        }

        public MemoryStream DownloadAudio(int bookId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteBook(int bookId)
        {
            throw new NotImplementedException();
        }

        public int UploadBook(string title, string author, string description, string genre, DateTime dateAdded, double price)
        {
            throw new NotImplementedException();
        }

        public Book UpdateBook(int bookId, string title, string author, string description, string genre, DateTime dateAdded,
                               double price)
        {
            throw new NotImplementedException();
        }

        public void UploadAudio(int bookId, MemoryStream MP3)
        {
            throw new NotImplementedException();
        }

        public void UploadPDF(int bookId, MemoryStream PDF)
        {
            SMUController.GetInstance().UploadPDF(bookId, PDF);
        }
    }
}
