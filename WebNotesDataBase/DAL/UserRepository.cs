using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebNotesDataBase.Models;

namespace WebNotesDataBase.DAL
{
    public class UserRepository
    {
        public WebNotesContext context = new WebNotesContext();
        DbSet<User> dbSet { get; set; }
        public UnitOfWork<User> unitOfWork;

        public UserRepository()
        {
            unitOfWork = new UnitOfWork<User>(context);
            dbSet = context.Set<User>();
        }

        public User GetByEmail(string value)
        {
            User user = dbSet.SingleOrDefault(e => e.Email == value);
            return user;
        }
    }
}