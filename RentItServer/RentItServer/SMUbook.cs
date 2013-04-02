//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RentItServer
{
    using System;
    using System.Collections.Generic;
    
    public partial class SMUbook
    {
        public SMUbook()
        {
            this.SMUrentals = new HashSet<SMUrental>();
        }
    
        public int id { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string description { get; set; }
        public string genre { get; set; }
        public double price { get; set; }
        public System.DateTime dateAdded { get; set; }
        public string PDFFilePath { get; set; }
        public string imageFilePath { get; set; }
        public int hit { get; set; }
        public string audioFilePath { get; set; }
        public string audioNarrator { get; set; }
    
        public virtual ICollection<SMUrental> SMUrentals { get; set; }
    }
}
