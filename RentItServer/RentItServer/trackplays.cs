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
    
    public partial class trackplays
    {
        public int trackId { get; set; }
        public System.DateTime date { get; set; }
    
        public virtual tracks tracks { get; set; }
    }
}
