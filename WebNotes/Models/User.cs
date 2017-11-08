using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebNotes.Models
{
    public class User
    {
        public User() { }
        public User(int UserId, string NameAuthor, DateTime Birthday, string Email, string Password)
        {
            this.UserId = UserId;
            this.NameAuthor = NameAuthor;
            this.Birthday = Birthday;
            this.Email = Email;
            this.Password = Password;
        }
        public int UserId { get; set; }
        public string NameAuthor { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}