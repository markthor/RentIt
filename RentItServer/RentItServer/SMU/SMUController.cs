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
        private static string _mediaFileDirectoryPath = "C:" + Path.DirectorySeparatorChar +
            "Users" + Path.DirectorySeparatorChar +
            "Rentit21" + Path.DirectorySeparatorChar +
            "Documents" + Path.DirectorySeparatorChar +
            "SMU" + Path.DirectorySeparatorChar +
            "MediaFiles";
        //Singleton instance of the class
        private static SMUController _instance;
        //Data access object for database IO
        private readonly SMUDao _dao = SMUDao.GetInstance();
        //The logger
        private readonly Logger _logger = new Logger("", entry);
        //Log eventHandler
        public delegate void LogEvent(object sender, string LogMessage);
        //Event cast when log must make an entry
        public static event LogEvent entry;
        //Data access object for file system IO
        private readonly RentItServer.ITU.FileSystemHandler _fileSystemHandler = new RentItServer.ITU.FileSystemHandler(_mediaFileDirectoryPath);

        /// <summary>
        /// Accessor method to access the only instance of the class
        /// </summary>
        /// <returns>The singleton instance of the class</returns>
        public static SMUController GetInstance()
        {
            return _instance ?? (_instance = new SMUController());
        }

        public int LogIn(string username, string password)
        {
            int id = _dao.LogIn(username, password);
            entry(this, "LogIn: " + username + "-" + password);
            return id;
        }

        public int SignUp(string username, string password, string email, bool isAdmin)
        {
            int id = _dao.SignUp(email, username, password, isAdmin);
            entry(this, "SignUp: " + email + "-" + username + "-" + password);
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

        public int AddBook(int userId, string title, string author, string description, string genre, DateTime dateAdded, double price,
                            string pdfFilePath, string imageFilePath)
        {
            return _dao.AddBook(title, author, description, genre, dateAdded, price, pdfFilePath, imageFilePath);
        }

        public int RentBook(int userId, int bookId, int mediaType)
        {
            return _dao.RentBook(userId, bookId, mediaType);
        }


        public void DeleteBook(int bookId)
        {
            _dao.DeleteBook(bookId);
        }

        public void UploadAudio(int bookId, MemoryStream MP3)
        {

        }


        public void UploadPDF(int bookId, MemoryStream PDF)
        {
            String relativePath = String.Format("{0}{1}{0}{2}", Path.DirectorySeparatorChar, "PDF", bookId.ToString());
            _fileSystemHandler.WriteFile(relativePath, PDF);
        }
    }
}