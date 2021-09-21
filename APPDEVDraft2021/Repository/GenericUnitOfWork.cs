using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using APPDEVDraft2021.Models;
using APPDEVDraft2021.Repository;

namespace APPDEVDraft2021.Repository
{
    public class GenericUnitOfWork : IDisposable
    {
        private ApplicationDbContext DBEntity = new ApplicationDbContext();
        public IRepository<Tbl_EntityType> GetRepositoryInstance<Tbl_EntityType>() where Tbl_EntityType : class
        {
          
            return new GenericRespository<Tbl_EntityType>(DBEntity);
        }

        public void SaveChanges()
        {
            DBEntity.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    DBEntity.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;

    }
}