using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public string Label { get; set; }
        [Column(TypeName = "ntext")]
        public string Body { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}