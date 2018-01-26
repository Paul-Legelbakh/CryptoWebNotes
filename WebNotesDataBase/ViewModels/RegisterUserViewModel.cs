using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebNotesDataBase.Models;

namespace WebNotesDataBase.ViewModels
{
    public class RegisterUserViewModel
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string NameAuthor { get; set; }

        public DateTime Birthday { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 6)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [StringLength(20, MinimumLength = 8)]
        public string Pass { get; set; }

        [Required]
        [Display(Name = "Password confirm")]
        [Compare("Pass", ErrorMessage = "Passwords don't match")]
        [StringLength(20, MinimumLength = 8)]
        public string PassConfirm { get; set; }
    }
}