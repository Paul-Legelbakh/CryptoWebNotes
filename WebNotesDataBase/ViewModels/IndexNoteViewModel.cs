using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebNotesDataBase.DAL;
using WebNotesDataBase.Models;

namespace WebNotesDataBase.ViewModels
{
    public class IndexNoteViewModel
    {
        public IndexNoteViewModel() { }
        public int NoteId { get; set; }
        [Display(Name = "Created and edited date")]
        public string Created_Edited { get; set; }
        [Display(Name = "Title")]
        public string Label { get; set; }
        [Display(Name = "Text data")]
        public string Body { get; set; }
        [Display(Name = "Author")]
        public string NameAuthor { get; set; }
        
    }
}