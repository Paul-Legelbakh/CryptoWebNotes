using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebNotes.Models
{
    public class Note
    {
        public Note() { } //constructor for empty elements
        public Note(int NoteId, DateTime CreatedDate, DateTime EditedDate, int UserId, string Label, string Body)
        //constructor with initialization
        {
            this.NoteId = NoteId;
            this.CreatedDate = CreatedDate;
            this.EditedDate = EditedDate;
            this.UserId = UserId;
            this.Label = Label;
            this.Body = Body;
        }
        public int NoteId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime EditedDate { get; set; }
        public int UserId { get; set; }
        public string Label { get; set; }
        public string Body { get; set; }
    }
}