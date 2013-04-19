using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer
{
    public partial class Genre
    {
        public ITU.DataObjects.Genre GetGenre()
        {
            return new ITU.DataObjects.Genre(Id, Name);
        }
    }
}