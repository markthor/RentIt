using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer
{
    public class TrackPrioritizer
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

    public class TrackPlay
    {
        public int TrackId
        {
            get;
            set;
        }

        public DateTime PlayTime
        {
            get;
            set;
        }

        public TrackPlay(DateTime date, int id)
        {

        }
    }

    public class Track
    {
        public int Upvotes
        {
            get;
            set;
        }

        public int Downvotes
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int Id
        {
            get;
            set;
        }

        public Track(string name, int id)
        {
            Name = name;
            Id = id;
        }
    }
}