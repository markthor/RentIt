using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using RentItServer.Utilities;

namespace RentItServer.SMU
{
    public class SMUController
    {
        private static readonly string DirectoryPath =  "C:" + Path.DirectorySeparatorChar +
                                                        "Users" + Path.DirectorySeparatorChar +
                                                        "Mr.Green" + Path.DirectorySeparatorChar +
                                                        "Documents" + Path.DirectorySeparatorChar +
                                                        "SMU" + Path.DirectorySeparatorChar;

        private static readonly string LogFileName =  "SmuLogs.txt";
        //Singleton instance of the class
        private static SMUController _instance;
        //Data access object for database IO
        private readonly SMUDao _dao = SMUDao.GetInstance();
        //The logger
        private readonly Logger _logger;
        //Event cast when log must make an _handler
        private static EventHandler _handler;
        //Data access object for file system IO
        //private readonly RentItServer.ITU.FileSystemHandler _fileSystemHandler = new RentItServer.ITU.FileSystemHandler(DirectoryPath);
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
            _logger = new Logger(DirectoryPath + LogFileName, ref _handler);
        }

        public int LogIn(string email, string password)
        {
            int id = _dao.LogIn(email, password);
            if(_handler != null)
                _handler(this, new RentItEventArgs("LogIn: " + email + "-" + password));
            return id;
        }

        public int SignUp(string email, string username, string password, bool isAdmin)
        {
            int id = _dao.SignUp(email, username, password, isAdmin);
            if(_handler != null)
                _handler(this, new RentItEventArgs("SignUp: " + email + "-" + username + "-" + password));
            return id;
        }

        public SMUuser GetUser(int id)
        {
            return _dao.GetUser(id);
        }

        public void UpdateUserInfo(int userId, string email, string username, string password, bool isAdmin)
        {
            _dao.UpdateUserInfo(userId, email, username, password, isAdmin);
        }

        public void DeleteAccount(int userId)
        {
            _dao.DeleteAccount(userId);
        }

        public int HasRental(int userId, int bookId)
        {
            return _dao.HasRental(userId, bookId);
        }

        public int AddBook(string title, string author, string description, string genre, DateTime dateAdded, double price)
        {
            return _dao.AddBook(title, author, description, genre, dateAdded, price);
        }

        public int RentBook(int userId, int bookId, DateTime time, int mediaType)
        {
            return _dao.RentBook(userId, bookId, mediaType);
        }

        public Book GetBookInfo(int bookId)
        {
            return _dao.GetBookInfo(bookId);
        }

        public void DeleteBook(int bookId)
        {
            _dao.DeleteBook(bookId);
        }

        public List<Book> GetAllBooks()
        {
            return _dao.GetAllBooks();
        }

        public void UploadAudio(int bookId, MemoryStream MP3)
        {

        }


        public void UploadPDF(int bookId, MemoryStream PDF)
        {
            String relativePath = String.Format("{0}{1}{0}{2}.pdf", Path.DirectorySeparatorChar, "PDF", bookId.ToString());
        //    _fileSystemHandler.WriteFile(relativePath, PDF);
        }
    }
}