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
        public string Created_Edited { get; set; }
        public string Label { get; set; }
        public string Body { get; set; }
        public string NameAuthor { get; set; }
        
    }
}