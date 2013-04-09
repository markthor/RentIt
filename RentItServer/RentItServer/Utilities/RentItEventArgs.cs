using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer.Utilities
{
    /// <summary>
    /// Class containing information about a RentIt-specific event
    /// </summary>
    public class RentItEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RentItEventArgs"/> class.
        /// </summary>
        /// <param name="entry">The entry.</param>
        public RentItEventArgs(string entry)
        {
            Entry = entry;
        }

        /// <summary>
        /// Gets the entry.
        /// </summary>
        /// <value>
        /// The entry.
        /// </value>
        public string Entry { get; private set; }
    }
}