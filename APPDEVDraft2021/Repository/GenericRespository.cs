using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using APPDEVDraft2021.Models;

namespace APPDEVDraft2021.Repository
{
    public class GenericRespository<Tbl_Entity> : IRepository<Tbl_Entity> where Tbl_Entity : class
    {

        DbSet<Tbl_Entity> _dbSet;

        public ApplicationDbContext _DBEntity;

        public GenericRespository(ApplicationDbContext DBEntity)
        {
            _DBEntity = DBEntity;
            _dbSet = _DBEntity.Set<Tbl_Entity>();
        }
        public IEnumerable<Tbl_Entity> GetProduct()
        {
            return _dbSet.ToList();
        }

        public IEnumerable<Tbl_Entity> GetQuotation()
        {
            return _dbSet.ToList();
        }
        public void Add(Tbl_Entity entity)
        {
            _dbSet.Add(entity);
            _DBEntity.SaveChanges();
        }

        public int GetAllrecordCount()
        {
            return _dbSet.Count();
        }

        public IEnumerable<Tbl_Entity> GetAllRecords()
        {
            return _dbSet.ToList();
        }

        public IQueryable<Tbl_Entity> GetAllRecordsIQueryable()
        {
            return _dbSet;
        }

        public Tbl_Entity GetFirstorDefault(int recordId)
        {
            return _dbSet.Find(recordId);
        }

        public Tbl_Entity GetFirstorDefaultByParameter(Expression<Func<Tbl_Entity, bool>> wherePredict)
        {
            return _dbSet.Where(wherePredict).FirstOrDefault();
        }

        public IEnumerable<Tbl_Entity> GetListParameter(Expression<Func<Tbl_Entity, bool>> wherePredict)
        {
            return _dbSet.Where(wherePredict).ToList();
        }

        public IEnumerable<Tbl_Entity> GetResultBySqlprocedure(string query, params object[] parameters)
        {
            if (parameters != null)
            {
                return _DBEntity.Database.SqlQuery<Tbl_Entity>(query, parameters).ToList();
            }
            else
                return _DBEntity.Database.SqlQuery<Tbl_Entity>(query).ToList();
        }

        public void InactiveAndDeleteMarkByWhereClause(Expression<Func<Tbl_Entity, bool>> wherePredict, Action<Tbl_Entity> ForEachPredict)
        {
            _dbSet.Where(wherePredict).ToList().ForEach(ForEachPredict);
        }

        public void Remove(Tbl_Entity entity)
        {
            if (_DBEntity.Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);
            _dbSet.Remove(entity);
        }

        public void RemovebyWhereClause(Expression<Func<Tbl_Entity, bool>> wherePredict)
        {
            Tbl_Entity entity = _dbSet.Where(wherePredict).FirstOrDefault();
            Remove(entity);
        }

        public void RemoveRangeBywhereClause(Expression<Func<Tbl_Entity, bool>> wherePredict)
        {
            List<Tbl_Entity> entity = _dbSet.Where(wherePredict).ToList();
            foreach (var ent in entity)
            {
                Remove(ent);
            }
        }

        public void Update(Tbl_Entity entity)
        {
            
           
            _dbSet.Attach(entity);
            _DBEntity.Entry(entity).State = EntityState.Modified;
           
            _DBEntity.SaveChanges();
        }

        //public void UpdateUser(CustomerTbl customer, Tbl_Entity entity)
       // {
        //    var existinguser = _DBEntity.Users.FirstOrDefault(p => p. == user.UserID);
        //
        //    if (existinguser != null)
        //    {
        //        _dbSet.Attach(entity);
         //       _DBEntity.Entry(existinguser).State = EntityState.Modified;
                
        //        _DBEntity.SaveChanges();
         //   }



       // }
        public void UpdateProduct(StockServiceTbl product , Tbl_Entity entity)
        {
            var existingproduct = _DBEntity.StockServiceTbls.FirstOrDefault(p => p.StockID == product.StockID);

            if(existingproduct != null)
            {
                _dbSet.Attach(entity);
                _DBEntity.Entry(existingproduct).State = EntityState.Modified;
                _DBEntity.Entry(existingproduct).Property(x => x.ProductImage).IsModified = false;
                _DBEntity.SaveChanges();
            }
                
               
            
        }



        public void UpdateByWhereClause(Expression<Func<Tbl_Entity, bool>> wherePredict, Action<Tbl_Entity> ForEachPredict)
        {
            _dbSet.Where(wherePredict).ToList().ForEach(ForEachPredict);
        }

        public IEnumerable<Tbl_Entity> GetRecordsToShow(int PageNo, int PageSize, int CurrentPage, Expression<Func<Tbl_Entity, bool>> wherePredict, Expression<Func<Tbl_Entity, int>> orderByPredict)
        {
            if (wherePredict != null)
            {
                return _dbSet.OrderBy(orderByPredict).Where(wherePredict).ToList();

            }

            else
            {
                return _dbSet.OrderBy(orderByPredict).ToList();
            }
        }
    }

}
