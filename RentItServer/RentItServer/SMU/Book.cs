using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer.SMU
{
    [Serializable]
    public class Book
    {
        public Book(int id, string title, string author, string description, string genre, double price, DateTime dateAdded, Nullable<int> audioId, int hit)
        {
            this.id = id;
            this.title = title;
            this.author = author;
            this.description = description;
            this.genre = genre;
            this.price = price;
            this.dateAdded = dateAdded;
            this.audioId = audioId;
            this.audioNarrator = audioNarrator;
            this.hit = hit;
        }

        public int id { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string description { get; set; }
        public string genre { get; set; }
        public double price { get; set; }
        public DateTime dateAdded { get; set; }
        public Nullable<int> audioId { get; set; }
        public string audioNarrator { get; set; }
        public int hit { get; set; }
    }
}