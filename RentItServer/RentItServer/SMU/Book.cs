using System;
using System.Runtime.Serialization;

namespace RentItServer.SMU
{
    [DataContract]
    public class Book
    {
        public Book(int id, string title, string author, string description, string genre, double price, DateTime dateAdded, string narrator, int hit)
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
    }
}