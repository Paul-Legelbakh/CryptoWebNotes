using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebNotesDataBase.Models
{
    public class Note
    {
        public Note(){}
        [Key]
        public int NoteId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? EditedDate { get; set; }
        [Required]
        [StringLength(20)]
        public string Label { get; set; }
        [Required]
        [StringLength(5000)]
        public string Body { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}