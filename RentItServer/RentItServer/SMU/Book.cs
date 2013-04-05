using System;

namespace RentItServer.SMU
{
    [Serializable]
    public class Book
    {
        public Book(int id, string title, string author, string description, string genre, double price, DateTime dateAdded, string narrator, int hit)
        {
            this.id = id;
            this.title = title;
            this.author = author;
            this.description = description;
            this.genre = genre;
            this.price = price;
            this.dateAdded = dateAdded;
            this.hit = hit;
            this.narrator = narrator;
        }

        public int id { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string description { get; set; }
        public string genre { get; set; }
        public double price { get; set; }
        public string narrator { get; set; }
        public DateTime dateAdded { get; set; }
        public int hit { get; set; }
    }
}