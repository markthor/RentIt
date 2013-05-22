using System;

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
        /// <param name="message">Message that describes the error.</param>
        public EmptyTableException(string message)
            : base(message)
        {
        }
    }
}