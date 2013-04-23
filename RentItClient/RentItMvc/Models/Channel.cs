using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace RentItMvc.Models
{
    public class Channel
    {
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
        [Display(Name = "Rating")]
        public double? Rating{ get; set; }

        [Required]
        [Display(Name = "Hits")]
        public int? Hits{ get; set; } 

        [Required]
        [Display(Name = "OwnerId")]
        public int OwnerId{ get; set; }

        /**[Required]
        [Display(Name = "Comments")]
        public List<Comment> Comments { get; set; }
        

        [Required]
        [Display(Name = "Name")]
        public List<Subscriber> Subscribers{ get; set; }
        
        public List<Genre> Genres{ get; set; }
        public List<Track> Tracks { get; set; }**/
    }
}