namespace WebNotes.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebNotes.Models;

    public class WebNotesContext : DbContext
    {
        public sealed class WebNotesInitializer
        : DbMigrationsConfiguration<WebNotesContext>
        {
            //initializator of database
            public WebNotesInitializer()
            {
                AutomaticMigrationsEnabled = true;
            }

            protected override void Seed(WebNotesContext context)
            {
                //if (context.Users == null)
                //{
                //    List<User> users = new List<User>
                //    {
                //    new User { UserId = 1, NameAuthor = "Unnamed", Birthday = new DateTime(2000, 10, 10), Email = "immortalis82@gmail.com", Password = "1234567890" }
                //    };
                //    foreach (User user in users) context.Users.Add(user);
                //    context.SaveChanges();
                //}

                //initialization if database is empty
                if (context.Notes == null)
                    {
                    List<Note> notes = new List<Note>
                    {
                    new Note { NoteId = 1, UserName = "Default", CreatedDate = DateTime.Now,  EditedDate = DateTime.Now, Label = "Tech", Body = "null"}
                    };
                    foreach (Note note in notes) context.Notes.Add(note);
                    context.SaveChanges();
                }
                base.Seed(context);
            }
            //initialization of database with method uses protected access level
            public void Init(WebNotesContext context) 
            {
                Seed(context);
            }
        }

        //method of create new connection
        public WebNotesContext() : base("WebNotesDataBase")
        {
            WebNotesInitializer initializer = new WebNotesInitializer();
            initializer.Init(this);
        }
        //add table to database
        //public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
    }

}