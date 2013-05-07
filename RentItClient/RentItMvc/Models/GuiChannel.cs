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
        public static GuiChannel GuiChannelFactory(RentItService.Channel channel)
        {
            GuiChannel guiChannel = null;
            if (channel != null)
            {
                guiChannel = new GuiChannel();
                guiChannel.Name = channel.Name;
                guiChannel.Id = channel.Id;
                guiChannel.Description = channel.Description;
                guiChannel.OwnerId = channel.Owner.Id;
                guiChannel.Hits = channel.Hits != null ? channel.Hits.Value : 0;    
            }
            return guiChannel;
        }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description{ get; set; }

        [Required]
        [Display(Name = "UpVotes")]
        public double Upvotes{ get; set; }

        [Required]
        [Display(Name = "DownVotes")]
        public double DownVotes { get; set; }

        [Required]
        [Display(Name = "Hits")]
        public int Hits{ get; set; } 

        [Required]
        [Display(Name = "OwnerId")]
        public int OwnerId{ get; set; }

        [Required]
        [Display(Name = "TrackList")]
        public List<GuiTrack> Tracks { get; set; }
    }
}