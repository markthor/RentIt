using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RentItServer.ITU.DatabaseWrapperObjects
{
    /// <summary>
    /// Wrapper object for the database entity "Track". Hides some database specific details about the object.
    /// </summary>[DataContract]
    public class Track
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Track"/> class.
        /// </summary>
        public Track()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Track"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="path">The path.</param>
        /// <param name="name">The name.</param>
        /// <param name="artist">The artist.</param>
        /// <param name="length">The length.</param>
        /// <param name="upVotes">Up votes.</param>
        /// <param name="downVotes">Down votes.</param>
        /// <param name="channelId">The channel id.</param>
        public Track(int id, string path, string name, string artist, int length,
                     int upVotes, int downVotes, int channelId)
        {
            Id = id;
            Path = path;
            Name = name;
            Artist = artist;
            Length = length;
            UpVotes = upVotes;
            DownVotes = downVotes;
            ChannelId = channelId;
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
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        [DataMember]
        public string Path { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the artist.
        /// </summary>
        /// <value>
        /// The artist.
        /// </value>
        [DataMember]
        public string Artist { get; set; }
        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        [DataMember]
        public int Length { get; set; }
        /// <summary>
        /// Gets or sets up votes.
        /// </summary>
        /// <value>
        /// Up votes.
        /// </value>
        [DataMember]
        public int UpVotes { get; set; }
        /// <summary>
        /// Gets or sets down votes.
        /// </summary>
        /// <value>
        /// Down votes.
        /// </value>
        [DataMember]
        public int DownVotes { get; set; }

        /// <summary>
        /// Gets or sets the channel id.
        /// </summary>
        /// <value>
        /// The channel id.
        /// </value>
        [DataMember]
        public int ChannelId { get; set; }

        /// <summary>
        /// Gets the tracks.
        /// </summary>
        /// <param name="tracks">The tracks.</param>
        /// <returns></returns>
        public static Track[] GetTracks(IEnumerable<RentItServer.Track> tracks)
        {
            if (tracks == null) return null;
            List<Track> convertedTracks = new List<Track>();
            foreach (RentItServer.Track track in tracks)
            {
                convertedTracks.Add(track.GetTrack());
            }
            return convertedTracks.ToArray();
        }
    }
}