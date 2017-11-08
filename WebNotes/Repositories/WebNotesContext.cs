namespace WebNotes.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using WebNotes.Models;

    public class WebNotesContext : DbContext
    {
        public class WebNotesInitializer
        : DropCreateDatabaseIfModelChanges<WebNotesContext>
        {
            // В этом методе можно заполнить таблицу по умолчанию
            protected override void Seed(WebNotesContext context)
            {
                List<User> users = new List<User>
                {
                    new User { UserId = 1, NameAuthor = "Unnamed", Birthday = new DateTime(2000, 10, 10), Email = "immortalis82@gmail.com", Password = "1234567890" }
                };
                List<Note> notes = new List<Note>
                {
                    new Note { NoteId = 1, UserId = 1, CreatedDate = DateTime.Now,  EditedDate = DateTime.Now, Label = "Tech", Body = "null"}
                    // ...
                };
                foreach (User user in users) context.Users.Add(user);
                foreach (Note note in notes) context.Notes.Add(note);

                context.SaveChanges();
                base.Seed(context);
            }
        }
        // Контекст настроен для использования строки подключения "WebNotesContext" из файла конфигурации  
        // приложения (App.config или Web.config). По умолчанию эта строка подключения указывает на базу данных 
        // "WebNotes.Repositories.WebNotesContext" в экземпляре LocalDb. 
        // 
        // Если требуется выбрать другую базу данных или поставщик базы данных, измените строку подключения "WebNotesContext" 
        // в файле конфигурации приложения.
        public WebNotesContext() : base("WebNotes")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }

        // Добавьте DbSet для каждого типа сущности, который требуется включить в модель. Дополнительные сведения 
        // о настройке и использовании модели Code First см. в статье http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}