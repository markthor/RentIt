using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RentItServer
{
    class DAO
    {
        //Singleton instance of the class
        public static DAO _instance;

        /// <summary>
        /// Private to ensure local instantiation.
        /// </summary>
        private DAO()
        {
        }

        /// <summary>
        /// Accessor method to access the only instance of the class
        /// </summary>
        /// <returns>The singleton instance of the class</returns>
        public static DAO GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DAO();
            }
            return _instance;
        }
    }
}
