using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace RentItServer.Utilities
{
    public sealed class FileName
    {
        public static string GeneratePdfFileName(int bookId)
        {
            return string.Format("PDF_BookId_{0}.pdf", bookId);
        }

        public static string GenerateAudioFileName(int bookId)
        {
            return string.Format("Audio_BookId_{0}.mp3", bookId);
        }

        public static string GenerateImageFileName(int bookId)
        {
            return string.Format("Image_BookId_{0}.jpg", bookId);
        }
    }
}