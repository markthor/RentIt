using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RentItServer
{
    class TrackPrioritizer
    {
        //Singleton instance of the class
        public static TrackPrioritizer _instance;

        /// <summary>
        /// Private to ensure local instantiation.
        /// </summary>
        private TrackPrioritizer()
        {
        }

        /// <summary>
        /// Accessor method to access the only instance of the class
        /// </summary>
        /// <returns>The singleton instance of the class</returns>
        public static TrackPrioritizer GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TrackPrioritizer();
            }
            return _instance;
        }
    }
}
