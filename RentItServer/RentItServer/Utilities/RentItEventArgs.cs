using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer.Utilities
{
    public class RentItEventArgs : EventArgs
    {
        public RentItEventArgs(string entry)
        {
            Entry = entry;
        }

        public string Entry
        {
            get;
            private set;
        }
    }
}