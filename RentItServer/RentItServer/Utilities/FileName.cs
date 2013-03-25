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
            return String.Format("{0}PDF_BookId_{1}.pdf", Path.DirectorySeparatorChar, bookId.ToString());
        }

        public static string GenerateAudioFileName(int bookId)
        {
            return String.Format("{0}Audio_BookId_{1}.mp3", Path.DirectorySeparatorChar, bookId.ToString());
        }
    }
}