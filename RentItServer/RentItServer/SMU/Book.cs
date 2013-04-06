using System;
using System.Runtime.Serialization;

namespace RentItServer.SMU
{
    [DataContract]
    public class Book
    {
        public Book(int id, string title, string author, string description, string genre, double price, DateTime dateAdded, string narrator, int hit, bool hasAudio, bool hasPdf)
        {
            Id = id;
            Title = title;
            Author = author;
            Description = description;
            Genre = genre;
            Price = price;
            DateAdded = dateAdded;
            Hit = hit;
            Narrator = narrator;
            HasAudio = hasAudio;
            HasPdf = hasPdf;
        }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Author { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Genre { get; set; }
        [DataMember]
        public double Price { get; set; }
        [DataMember]
        public string Narrator { get; set; }
        [DataMember]
        public DateTime DateAdded { get; set; }
        [DataMember]
        public int Hit { get; set; }
        [DataMember]
        public bool HasAudio { get; set; }
        [DataMember]
        public bool HasPdf { get; set; }
    }
}