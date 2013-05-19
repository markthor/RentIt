using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItServer
{
    /// <summary>
    /// Partial class of the database entity "Track". Used to add functionality to the class.
    /// </summary>
    public partial class Track
    {
        /// <summary>
        /// Gets the wrapper for this track.
        /// </summary>
        /// <returns></returns>
        public ITU.DatabaseWrapperObjects.Track GetTrack()
        {
            return new ITU.DatabaseWrapperObjects.Track(Id, Path, Name, Artist, Length, UpVotes, DownVotes, ChannelId);
        }

        /// <summary>
        /// Gets the wrappers for these tracks.
        /// </summary>
        /// <param name="tracks">The tracks.</param>
        /// <returns></returns>
        public static List<ITU.DatabaseWrapperObjects.Track> GetTracks(IEnumerable<Track> tracks)
        {
            List<ITU.DatabaseWrapperObjects.Track> convertedTracks = new List<ITU.DatabaseWrapperObjects.Track>();
            foreach (Track t in tracks)
            {
                convertedTracks.Add(t.GetTrack());
            }
            return convertedTracks;
        }
    }
}