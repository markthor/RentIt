using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentItMvc.Models
{
    public class Account
    {
        [Required]
        [Display(Name = "User name")]
        [Remote("IsUsernameAvailable", "Validation")]
        public string Username { get; set; }
        
        [Required]
        [Display(Name = "Email address")]
        [Remote("IsEmailAvailable", "Validation")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(20, ErrorMessage = "Must be between 4 and 20 chars.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Remote("IsCurrentPasswordCorrect", "Validation")]
        public string CurrentPassword { get; set; }

        [StringLength(20, ErrorMessage = "Must be between 4 and 20 chars.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Passwords doesn't match.")]
        public string ConfirmPassword { get; set; }

        public string UsernameOrEmail { get; set; }
    }
}