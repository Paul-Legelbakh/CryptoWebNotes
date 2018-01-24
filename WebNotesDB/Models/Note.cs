using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebNotesDB.Models
{
    public class Note
    {
        //constructor for empty elements
        public Note() { }
        [Key]
        public int NoteId { get; set; }
        //used empty attribute type of date, because exists the conflicts with data type of the SQL database in datetime
        
        public DateTime? CreatedDate { get; set; }
        public DateTime? EditedDate { get; set; }
        public string UserName { get; set; }
        public string Label { get; set; }
        public string Body { get; set; }
    }
}
