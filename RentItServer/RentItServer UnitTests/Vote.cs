//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RentItServer_UnitTests
{
    using System;
    using System.Collections.Generic;
    
    public partial class Vote
    {
        public int UserId { get; set; }
        public int TrackId { get; set; }
        public int Value { get; set; }
        public System.DateTime Date { get; set; }
    
        public virtual Track Track { get; set; }
        public virtual User User { get; set; }
    }
}
