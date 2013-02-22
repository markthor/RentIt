using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer
{
    public class Controller
    {
        //Singleton instance of the class
        public static Controller _instance;
        //Data access object for database IO
        public static DAO _dao = DAO.GetInstance();
        //Responsible for choosing the next track
        public static TrackPrioritizer _trackPrioritizer = TrackPrioritizer.GetInstance();
        //Data access object for file system IO
        public static FileSystemHandler _fileSystemHandler = FileSystemHandler.GetInstance();

        /// <summary>
        /// Private to ensure local instantiation.
        /// </summary>
        private Controller()
        {
        }

        /// <summary>
        /// Accessor method to access the only instance of the class
        /// </summary>
        /// <returns>The singleton instance of the class</returns>
        public static Controller GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Controller();
            }
            return _instance;
        }
    }
}