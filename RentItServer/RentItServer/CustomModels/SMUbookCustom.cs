using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RentItServer.SMU;

namespace RentItServer
{
    public partial class SMUbook
    {
        public Book GetBook()
        {
            return new Book(id, title, author, description, genre, price, dateAdded, audioNarrator, hit, HasAudio(), HasPdf());
        }

        private bool HasAudio()
        {
            return this.audioFilePath != null;
        }

        private bool HasPdf()
        {
            return this.PDFFilePath != null;
        }
    }    
}