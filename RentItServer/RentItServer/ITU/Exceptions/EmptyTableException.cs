using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer.ITU.Exceptions
{
    /// <summary>
    /// This exception is thrown when a query is being attempted on an empty table
    /// </summary>
    internal class EmptyTableException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyTableException"/> class.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public EmptyTableException(string msg) : base(msg)
        {
        }
    }
}