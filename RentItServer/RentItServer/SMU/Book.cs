using System;
using System.Runtime.Serialization;

namespace RentItServer.SMU
{
    /// <summary>
    /// Class representing a book object as it lies in the database. 
    /// Fields not directly related to the book object but contained in the database has been omitted.
    /// This object is immutable.
    /// </summary>
    [DataContract]
    public class Book
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Book"/> class.
        /// </summary>
        /// <param name="id">The id of the book.</param>
        /// <param name="title">The title of the book.</param>
        /// <param name="author">The author of the book.</param>
        /// <param name="description">The description of the book.</param>
        /// <param name="genre">The genre of the book.</param>
        /// <param name="price">The price of the book.</param>
        /// <param name="dateAdded">The date added.</param>
        /// <param name="narrator">The narrator of the book.</param>
        /// <param name="hit">The hits on the book.</param>
        /// <param name="hasAudio">if set to <c>true</c> [has audio].</param>
        /// <param name="hasPdf">if set to <c>true</c> [has PDF].</param>
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
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [DataMember]
        public int Id { get; private set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [DataMember]
        public string Title { get; private set; }
        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        [DataMember]
        public string Author { get; private set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public string Description { get; private set; }
        /// <summary>
        /// Gets or sets the genre.
        /// </summary>
        /// <value>
        /// The genre.
        /// </value>
        [DataMember]
        public string Genre { get; private set; }
        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        [DataMember]
        public double Price { get; private set; }
        /// <summary>
        /// Gets or sets the narrator.
        /// </summary>
        /// <value>
        /// The narrator.
        /// </value>
        [DataMember]
        public string Narrator { get; private set; }
        /// <summary>
        /// Gets or sets the date added.
        /// </summary>
        /// <value>
        /// The date added.
        /// </value>
        [DataMember]
        public DateTime DateAdded { get; private set; }
        /// <summary>
        /// Gets or sets the hit.
        /// </summary>
        /// <value>
        /// The hit.
        /// </value>
        [DataMember]
        public int Hit { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance has audio.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has audio; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool HasAudio { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance has PDF.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has PDF; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool HasPdf { get; private set; }
    }
}