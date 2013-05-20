using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentItMvc.Models
{
    public class SelectedGenrePostModel
    {
        public int ChannelId { get; set; }
        public List<int> ChosenGenres { get; set; }
    }
}