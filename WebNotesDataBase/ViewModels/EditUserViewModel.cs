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
        [StringLength(30)]
        public string NameAuthor { get; set; }
        public DateTime Birthday { get; set; }
        [Required]
        public string Pass { get; set; }
    }
}