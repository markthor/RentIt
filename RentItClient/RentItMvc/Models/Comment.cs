using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentItMvc.Models
{
    public class Comment
    {

        public Comment()
        {
            userId = -1;
            Content = "default content";
            Date = DateTime.MinValue;
        }

        [Required]
        [Display(Name = "UserId")]
        public int userId { get; set; }
       
        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
    }
}