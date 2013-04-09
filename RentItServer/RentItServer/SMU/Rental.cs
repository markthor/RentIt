using System;
using System.Runtime.Serialization;

namespace RentItServer.SMU
{
    /// <summary>
    /// Class representing a rental object as it lies in the database. 
    /// Fields not directly related to the rental object but contained in the database are not included in this object.
    /// This object is immutable.
    /// </summary>
    [DataContract]
    public class Rental
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rental"/> class.
        /// </summary>
        /// <param name="id">The id of the rental.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="bookId">The book id.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="mediaType">Type of the media. 0 for pdf only, 1 for audio only, 2 for both</param>
        public Rental(int id, int userId, int? bookId, DateTime startDate, int mediaType)
        {
            Id = id;
            UserId = userId;
            BookId = bookId;
            StartDate = startDate;
            MediaType = mediaType;
        }

        [DataMember]
        public int Id { get; private set; }
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>
        /// The user id.
        /// </value>
        [DataMember]
        public int UserId { get; private set; }
        /// <summary>
        /// Gets or sets the book id.
        /// </summary>
        /// <value>
        /// The book id.
        /// </value>
        [DataMember]
        public int? BookId { get; private set; }
        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        [DataMember]
        public DateTime StartDate { get; private set; }
        /// <summary>
        /// Gets or sets the type of the media.
        /// </summary>
        /// <value>
        /// The type of the media.
        /// </value>
        [DataMember]
        public int MediaType { get; private set; }
    }
}