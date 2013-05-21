using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItMvc.Models
{
    public class GuiGenre
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            var item = obj as GuiGenre;

            if (item == null)
            {
                return false;
            }
            return this.Id == item.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}