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
    
    public partial class SMUaudio
    {
        public SMUaudio()
        {
            this.SMUbooks = new HashSet<SMUbook>();
            this.SMUrentals = new HashSet<SMUrental>();
        }
    
        public int id { get; set; }
        public string narrator { get; set; }
        public string filePath { get; set; }
    
        public virtual ICollection<SMUbook> SMUbooks { get; set; }
        public virtual ICollection<SMUrental> SMUrentals { get; set; }
    }
}