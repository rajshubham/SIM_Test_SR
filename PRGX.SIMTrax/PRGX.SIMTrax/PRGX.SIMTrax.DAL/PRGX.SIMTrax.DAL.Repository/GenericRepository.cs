using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Repository.Abstract;
using System.Linq;

namespace PRGX.SIMTrax.DAL.Repository
{
    /// <summary>
    /// The EF-dependent, generic repository for data access
    /// </summary>
    /// <typeparam name="T">Type of entity for this Repository.</typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public GenericRepository(DbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");
            DbContext = dbContext;
            DbSet = DbContext.Set<T>();
        }

        protected DbContext DbContext { get; set; }

        protected DbSet<T> DbSet { get; set; }

        public virtual IEnumerable<T> GetAll()
        {
            return DbSet;
        }

        public virtual IQueryable<T> All()
        {
            return DbSet.AsQueryable();
        }

        public T GetById(System.Guid id)
        {
             return DbSet.Find(id);      
        }

        public T GetById(long id)
        {
            return DbSet.Find(id);
        }

        public virtual T Add(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
                return entity;
            }
            else
            {
                return DbSet.Add(entity);
            }
        }

        public virtual List<T> AddRange(List<T> entities)
        {
            List<T> returnList = new List<T>();

            foreach(var entity in entities)
            {
               
                var addedEntity = Add(entity);
                if (null != addedEntity)
                    returnList.Add(addedEntity);
            }

            return returnList;           
        }

     

        public virtual void Update(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            
            if (dbEntityEntry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }
        }
        public virtual void DeleteRange(List<T> entities)
        {
            foreach (var entity in entities)
            { 
                Delete(entity);
            }
        }
        public virtual void Delete(System.Guid id)
        {
            var entity = GetById(id);
            if (entity == null) return; // not found; assume already deleted.
            Delete(entity);
        }
    }
}
