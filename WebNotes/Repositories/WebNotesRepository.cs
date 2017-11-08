using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebNotes.Models;

namespace WebNotes.Repositories
{
    public class WebNotesRepository
    {
        public void CreateNote(Note note)
        {
            WebNotesContext database = new WebNotesContext();
            database.Notes.Load();
            note.CreatedDate = DateTime.Now;
            note.EditedDate = DateTime.Now;
            if (note != null)
            {
                database.Notes.Add(note);
                database.SaveChanges();
            }
        }

        public void DeleteNote(int id)
        {
            WebNotesContext database = new WebNotesContext();
            database.Notes.Load();
            Note noteForRemove = database.Notes.Find(id);
            database.Notes.Remove(noteForRemove);
            database.SaveChanges();
        }

        public List<Note> GetNotes()
        {
            WebNotesContext database = new WebNotesContext();
            database.Notes.Load();
            List<Note> notes = new List<Note>();
            notes = database.Notes.ToList();
            database.SaveChanges();
            return notes;
        }

        public void UpdateNote(Note note)
        {
            WebNotesContext database = new WebNotesContext();
            database.Notes.Load();
            var obj = database.Notes.SingleOrDefault(nt => nt.NoteId == note.NoteId);
            if (obj != null)
            {
                obj.NoteId = note.NoteId;
                obj.CreatedDate = note.CreatedDate;
                obj.EditedDate = DateTime.Now;
                obj.Label = note.Label;
                obj.UserId = note.UserId;
                obj.Body = note.Body;
                //database.Notes.Attach(note);
                //database.Entry(note).State = EntityState.Modified;
                database.SaveChanges();
            }
        }

        public Note GetNote(int id)
        {
            WebNotesContext database = new WebNotesContext();
            database.Notes.Load();
            Note findedNote = new Note();
            findedNote = database.Notes.Find(id);
            database.SaveChanges();
            return findedNote;
        }

        public void CreateUser(User user)
        {
            WebNotesContext database = new WebNotesContext();
            database.Users.Load();
            if (user != null)
            {
                database.Users.Add(user);
                database.SaveChanges();
            }
        }

        public void DeleteUser(int id)
        {
            WebNotesContext database = new WebNotesContext();
            database.Users.Load();
            User userForRemove = database.Users.Find(id);
            database.Users.Remove(userForRemove);
            database.SaveChanges();
        }

        public List<User> GetUsers()
        {
            WebNotesContext database = new WebNotesContext();
            database.Users.Load();
            List<User> users = new List<User>();
            users = database.Users.ToList();
            database.SaveChanges();
            return users;
        }

        public void UpdateUser(User user)
        {
            WebNotesContext database = new WebNotesContext();
            database.Users.Load();
            var obj = database.Users.SingleOrDefault(ur => ur.UserId == user.UserId);
            if (obj != null)
            {
                obj.UserId = user.UserId;
                obj.NameAuthor = user.NameAuthor;
                obj.Birthday = user.Birthday;
                obj.Email = user.Email;
                obj.Password = user.Password;
                //database.Users.Attach(user);
                //database.Entry(user).State = EntityState.Modified;
                database.SaveChanges();
            }
        }

        public User GetUser(int id)
        {
            WebNotesContext database = new WebNotesContext();
            database.Users.Load();
            User findedUser = new User();
            findedUser = database.Users.Find(id);
            database.SaveChanges();
            return findedUser;
        }
    }
}