using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace RentItMvc.Models
{
    public class GuiChannel
    {
        public GuiChannel()
        {
            Name = "";
            Id = -1;
            Description = "";
            Plays = 0;
            OwnerId = -1;
            Tracks = new List<GuiTrack>();
            StreamUri = "";
            Genres = new string[0];
        }
        [Required]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        [Remote("IsChannelNameAvailable", "Validation", AdditionalFields = "Id")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description{ get; set; }

        [Required]
        [Display(Name = "Plays")]
        public int Plays{ get; set; } 

        [Required]
        [Display(Name = "OwnerId")]
        public int OwnerId{ get; set; }

        [Required]
        [Display(Name = "TrackList")]
        public List<GuiTrack> Tracks { get; set; }

        public string StreamUri { get; set; }

        public int Subscribers { get; set; }

        public string[] Genres { get; set; }
    }
}