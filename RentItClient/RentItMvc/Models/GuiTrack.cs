using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentItMvc.Models
{
    public class GuiTrack
    {
        [Required]
        [Display(Name = "TrackName")]
        public string TrackName { get; set; }

        [Required]
        [Display(Name = "ArtistName")]
        public string ArtistName { get; set; }

        [Required]
        [Display(Name = "Id")]
        public int Id { get; set; }
    }
}