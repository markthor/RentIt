using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer.Utilities
{
    public class RentItEvtArgs : EventArgs
    {
        public RentItEvtArgs(string entry)
        {
            this.entry = entry;
        }
        public string entry
        {
            get;
            private set;
        }
    }
}