using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RentItServer;

namespace RentItServer.SMU
{
    [Serializable]
    public class Rental
    {
        public Rental(int userId, int? bookId, DateTime startDate, int mediaType, SMUbook book, SMUuser user)
        {
            this.userId = userId;
            this.bookId = bookId;
            this.startDate = startDate;
            this.mediaType = mediaType;
            this.book = book.GetBook();
            this.user = user.GetUser();
        }

        public int userId { get; set; }
        public Nullable<int> bookId { get; set; }
        public DateTime startDate { get; set; }
        public int mediaType { get; set; }

        public Book book { get; set; }
        public User user { get; set; }
    }
}