using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebNotesDataBase.Models;

namespace WebNotesDataBase.DAL
{
    public class UnitOfWork<TEntity> : IDisposable where TEntity : class
    {
        public UnitOfWork(WebNotesContext context)
        {
            this.context = context;
        }
        private WebNotesContext context;
        private GenericRepository<TEntity> entityRepository;

        public GenericRepository<TEntity> EntityRepository
        {
            get
            {
                return entityRepository ?? new GenericRepository<TEntity>(context);
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}