using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebNotesDataBase.Models;

namespace WebNotesDataBase.ViewModels
{
    public class EditUserViewModel
    {
        public int UserId { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 4)]
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        [Required]
        [Display(Name = "Password")]
        public string Pass { get; set; }
    }
}