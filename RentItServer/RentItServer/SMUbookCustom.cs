using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RentItServer.SMU;

namespace RentItServer
{
    public partial class SMUbook
    {
        public Book GetBook()
        {
            return new Book(id, title, author, description, genre, price, dateAdded, audioId, hit);
        }
    }
}