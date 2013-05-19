using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer
{
    /// <summary>
    /// Partial class of the database entity "Genre". Used to add functionality to the class.
    /// </summary>
    public partial class Genre
    {
        /// <summary>
        /// Gets the wrapper for this genre.
        /// </summary>
        /// <returns></returns>
        public ITU.DatabaseWrapperObjects.Genre GetGenre()
        {
            return new ITU.DatabaseWrapperObjects.Genre(Id, Name);
        }
    }
}