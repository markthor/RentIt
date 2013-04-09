namespace RentItServer.Utilities
{
    public sealed class FileName
    {
        /// <summary>
        /// Generates a filename for the pdf for a book.
        /// </summary>
        /// <param name="bookId">The id of the book</param>
        /// <returns>The filename of the pdf belonging to the book with the given bookId</returns>
        public static string GeneratePdfFileName(int bookId)
        {
            return string.Format("PDF_BookId_{0}.pdf", bookId);
        }

        /// <summary>
        /// Generates a filename for the audio for a book.
        /// </summary>
        /// <param name="bookId">The id of the book</param>
        /// <returns>The filename of the audio belonging to the book with the given bookId</returns>
        public static string GenerateAudioFileName(int bookId)
        {
            return string.Format("Audio_BookId_{0}.mp3", bookId);
        }

        /// <summary>
        /// Generates a filename for the image for a book.
        /// </summary>
        /// <param name="bookId">The id of the book</param>
        /// <returns>The filename of the image belonging to the book with the given bookId</returns>
        public static string GenerateImageFileName(int bookId)
        {
            return string.Format("Image_BookId_{0}.jpg", bookId);
        }
    }
}