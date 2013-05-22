using RentItServer.SMU;

namespace RentItServer
{
    /// <summary>
    /// This partial class extends functionality of the SMUbook entity used by the entity framework
    /// </summary>
    public partial class SMUbook
    {
        /// <summary>
        /// Gets the book representation of this object.
        /// </summary>
        /// <returns></returns>
        public Book GetBook()
        {
            return new Book(id, title, author, description, genre, price, dateAdded, audioNarrator, hit, HasAudio(), HasPdf());
        }

        /// <summary>
        /// Determines whether this instance has audio.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance has audio; otherwise, <c>false</c>.
        /// </returns>
        private bool HasAudio()
        {
            return audioFilePath != null;
        }

        /// <summary>
        /// Determines whether this instance has PDF.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance has PDF; otherwise, <c>false</c>.
        /// </returns>
        private bool HasPdf()
        {
            return PDFFilePath != null;
        }
    }    
}