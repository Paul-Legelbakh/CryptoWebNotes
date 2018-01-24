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
        [StringLength(30)]
        public string NameAuthor { get; set; }
        public DateTime Birthday { get; set; }
        [Required]
        [StringLength(40)]
        public string Email { get; set; }
        [Required]
        [StringLength(20)]
        public string Pass { get; set; }
    }
}