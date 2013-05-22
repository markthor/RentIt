using System.Linq;
using System.Runtime.Serialization;

namespace RentItServer.ITU.DatabaseWrapperObjects
{
    /// <summary>
    /// Wrapper object for the database entity "Channel". Hides some database specific details about the object.
    /// </summary>
    [DataContract]
    public class Channel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Channel"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="rating">The rating.</param>
        /// <param name="hits">The hits.</param>
        /// <param name="ownerId">The owner id.</param>
        /// <param name="streamUri">The stream URI.</param>
        public Channel(int id, string name, string description, double? rating, int? hits, int ownerId, string streamUri)
        {
            Id = id;
            Name = name;
            Description = description;
            Rating = rating;
            Hits = hits;
            OwnerId = ownerId;
            StreamUri = streamUri;
            Genres = DatabaseDao.GetInstance().GetChannelGenres(id).ToArray(); // This is not pretty
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        /// <value>
        /// The rating.
        /// </value>
        [DataMember]
        public double? Rating { get; set; }
        /// <summary>
        /// Gets or sets the hits.
        /// </summary>
        /// <value>
        /// The hits.
        /// </value>
        [DataMember]
        public int? Hits { get; set; }
        /// <summary>
        /// Gets or sets the owner id.
        /// </summary>
        /// <value>
        /// The owner id.
        /// </value>
        [DataMember]
        public int OwnerId { get; set; }
        /// <summary>
        /// Gets or sets the stream URI.
        /// </summary>
        /// <value>
        /// The stream URI.
        /// </value>
        [DataMember]
        public string StreamUri { get; set; }
        /// <summary>
        /// Gets or sets the genres.
        /// </summary>
        /// <value>
        /// The genres.
        /// </value>
        [DataMember]
        public string[] Genres { get; set; } 
    }
}