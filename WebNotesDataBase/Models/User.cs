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

        public string NameAuthor { get; set; }

        public DateTime Birthday { get; set; }

        [StringLength(1000)]
        public string Email { get; set; }

        public string Pass { get; set; }

        public ICollection<Note> Notes { get; set; }
    }
}