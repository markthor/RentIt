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
    
    public partial class votes
    {
        public int trackId { get; set; }
        public int userId { get; set; }
        public int value { get; set; }
        public Nullable<System.DateTime> date { get; set; }
    
        public virtual tracks tracks { get; set; }
        public virtual users users { get; set; }
    }
}
