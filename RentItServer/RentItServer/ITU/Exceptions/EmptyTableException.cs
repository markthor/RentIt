using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer.ITU.Exceptions
{
    internal class EmptyTableException : Exception
    {
        public EmptyTableException(string msg) : base(msg)
        {
        }
    }
}