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
    
    public partial class SMUrental
    {
        public SMUrental()
        {
            this.mediaType = -1;
        }
    
        public int id { get; set; }
        public int userId { get; set; }
        public Nullable<int> bookId { get; set; }
        public System.DateTime startDate { get; set; }
        public int mediaType { get; set; }
    
        public virtual SMUbook SMUbook { get; set; }
        public virtual SMUuser SMUuser { get; set; }
    }
}
