using System.Collections.Generic;

namespace RentItServer
{
    /// <summary>
    /// Partial class of the database entity "Genre". Used to add functionality to the class.
    /// </summary>
    public partial class Genre
    {
        /// <summary>
        /// Gets the wrapper for this genre.
        /// </summary>
        /// <returns></returns>
        public ITU.DatabaseWrapperObjects.Genre GetGenre()
        {
            return new ITU.DatabaseWrapperObjects.Genre(Id, Name);
        }

        /// <summary>
        /// Gets the wrappers for these genres.
        /// </summary>
        /// <param name="genres">The Genres.</param>
        /// <returns></returns>
        public static List<ITU.DatabaseWrapperObjects.Genre> GetTracks(IEnumerable<Genre> genres)
        {
            List<ITU.DatabaseWrapperObjects.Genre> convertedGenres = new List<ITU.DatabaseWrapperObjects.Genre>();
            foreach (Genre g in genres)
            {
                convertedGenres.Add(g.GetGenre());
            }
            return convertedGenres;
        }
    }
}