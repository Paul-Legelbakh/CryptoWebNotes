using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebNotesDataBase.Models;

namespace WebNotesDataBase.DAL
{
    public class NoteRepository
    {
        public WebNotesContext context = new WebNotesContext();
        public UnitOfWork<Note> unitOfWork;
        DbSet<Note> dbSet { get; set; }
        public NoteRepository()
        {
            unitOfWork = new UnitOfWork<Note>(context);
            dbSet = context.Set<Note>();
        }

        public List<Note> GetListByUserID(int userId)
        {
            List<Note> notes = unitOfWork.EntityRepository.Get().ToList();
            List<Note> finded = new List<Note>();
            foreach (Note note in notes)
            {
                if (note.User.UserId == userId) finded.Add(note);
            }
            return finded;
        }
    }
}