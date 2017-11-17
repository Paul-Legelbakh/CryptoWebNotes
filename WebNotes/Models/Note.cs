using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebNotes.Models
{
    public class Note
    {
        //constructor for empty elements
        public Note() { } 
        public Note(int NoteId, DateTime? CreatedDate, DateTime? EditedDate, string UserName, string Label, string Body)
        //constructor with initialization
        {
            this.NoteId = NoteId;
            this.CreatedDate = CreatedDate;
            this.EditedDate = EditedDate;
            this.UserName = UserName;
            this.Label = Label;
            this.Body = Body;
        }
        //attributes for create table of database
        public int NoteId { get; set; }
        //used empty attribute type of date, because exists the conflicts with data type of the SQL database in datetime
        public DateTime? CreatedDate { get; set; } 
        public DateTime? EditedDate { get; set; }
        public string UserName { get; set; }
        public string Label { get; set; }
        public string Body { get; set; }
    }
}