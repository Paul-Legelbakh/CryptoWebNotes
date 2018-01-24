using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebNotesDataBase.Models
{
    public class User
    {
        public User()
        {
            Notes = new List<Note>();
        }
        [Key]
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
        public ICollection<Note> Notes { get; set; }
    }
}