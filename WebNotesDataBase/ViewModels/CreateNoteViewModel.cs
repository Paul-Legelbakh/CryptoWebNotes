using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebNotesDataBase.Models;

namespace WebNotesDataBase.ViewModels
{
    public class CreateNoteViewModel
    {
        public int NoteId { get; set; }
        public DateTime? EditedDate { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string Label { get; set; }
        [Required]
        [StringLength(4000, MinimumLength = 10)]
        public string Body { get; set; }
        public int UserId { get; set; }
    }
}