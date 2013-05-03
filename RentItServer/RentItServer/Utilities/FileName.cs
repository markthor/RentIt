using System;

namespace RentItServer.Utilities
{
    /// <summary>
    /// This class is used to generate filenames for the different files that can be associated with a book.
    /// </summary>
    public sealed class FileName
    {
        /// <summary>
        /// Generates a filename for the pdf for a book.
        /// </summary>
        /// <param name="bookId">The id of the book</param>
        /// <returns>The filename of the pdf belonging to the book with the given bookId</returns>
        public static string SmuGeneratePdfFileName(int bookId)
        {
            return string.Format("PDF_BookId_{0}.pdf", bookId);
        }

        /// <summary>
        /// Generates a filename for the audio for a book.
        /// </summary>
        /// <param name="bookId">The id of the book</param>
        /// <returns>The filename of the audio belonging to the book with the given bookId</returns>
        public static string SmuGenerateAudioFileName(int bookId)
        {
            return string.Format("Audio_BookId_{0}.mp3", bookId);
        }

        /// <summary>
        /// Generates a filename for the image for a book.
        /// </summary>
        /// <param name="bookId">The id of the book</param>
        /// <returns>The filename of the image belonging to the book with the given bookId</returns>
        public static string SmuGenerateImageFileName(int bookId)
        {
            return string.Format("Image_BookId_{0}.jpg", bookId);
        }

        /// <summary>
        /// Generates a filename for the audio
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="channelId">The channel id.</param>
        /// <param name="artist">The artist.</param>
        /// <param name="duration">The duration.</param>
        /// <returns>
        /// The filename of the audio uploaded by specified user belonging to the specified channel
        /// </returns>
        public static string ItuGenerateAudioFileName(int trackId)
        {
            return (Convert.ToString(trackId) + ".mp3");
        }
    }
}