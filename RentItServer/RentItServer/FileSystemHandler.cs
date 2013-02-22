using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer
{
    public class FileSystemHandler
    {
        //Singleton instance of the class
        public static FileSystemHandler _instance;

        /// <summary>
        /// Private to ensure local instantiation.
        /// </summary>
        private FileSystemHandler()
        {
        }

        /// <summary>
        /// Accessor method to access the only instance of the class
        /// </summary>
        /// <returns>The singleton instance of the class</returns>
        public static FileSystemHandler GetInstance()
        {
            if (_instance == null)
            {
                _instance = new FileSystemHandler();
            }
            return _instance;
        }
    }
}