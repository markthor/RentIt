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
    
    public partial class Channel
    {
        public Channel()
        {
            this.Comments = new HashSet<Comment>();
            this.Subscribers = new HashSet<User>();
            this.Genres = new HashSet<Genre>();
            this.Tracks = new HashSet<Track>();
        }
    
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<double> Rating { get; set; }
        public Nullable<int> Hits { get; set; }
        public string StreamUri { get; set; }
    
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual User ChannelOwner { get; set; }
        public virtual ICollection<User> Subscribers { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
        public virtual ICollection<Track> Tracks { get; set; }
    }
}
