using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer
{
    public partial class Genre
    {
        public ITU.DatabaseWrapperObjects.Genre GetGenre()
        {
            return new ITU.DatabaseWrapperObjects.Genre(Id, Name);
        }
    }
}