using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer
{
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
}