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
    
    public partial class users
    {
        public users()
        {
            this.channels = new HashSet<channels>();
            this.comments = new HashSet<comments>();
            this.votes = new HashSet<votes>();
            this.channels1 = new HashSet<channels>();
        }
    
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    
        public virtual ICollection<channels> channels { get; set; }
        public virtual ICollection<comments> comments { get; set; }
        public virtual ICollection<votes> votes { get; set; }
        public virtual ICollection<channels> channels1 { get; set; }
    }
}