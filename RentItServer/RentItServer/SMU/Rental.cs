using System;
using System.Runtime.Serialization;

namespace RentItServer.SMU
{
    [DataContract]
    public class Rental
    {
        public Rental(int id, int userId, int? bookId, DateTime startDate, int mediaType)
        {
            Id = id;
            UserId = userId;
            BookId = bookId;
            StartDate = startDate;
            MediaType = mediaType;
        }

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public Nullable<int> BookId { get; set; }
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public int MediaType { get; set; }
    }
}