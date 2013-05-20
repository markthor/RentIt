using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentItMvc.Models
{
    public class SelectGenreModel
    {
        public List<GuiGenre> AvailableGenres { get; set; }
        public List<GuiGenre> ChosenGenres { get; set; }
        public int ChannelId { get; set; }
    }
}