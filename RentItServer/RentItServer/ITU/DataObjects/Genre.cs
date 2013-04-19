using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RentItServer.ITU.DataObjects
{
    [DataContract]
    public class Genre
    {
        public Genre(int id, string name)
        {
            Id = id;
            Name = name;
        }

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}