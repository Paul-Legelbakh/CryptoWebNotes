using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebNotesDataBase.Models;

namespace WebNotesDataBase.ViewModels
{
    public class LoginUserViewModel
    {
        public int UserId { get; set; }
        [Required]
        [StringLength(40, MinimumLength = 6)]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Password")]
        [StringLength(20, MinimumLength = 8)]
        public string Pass { get; set; }
    }
}