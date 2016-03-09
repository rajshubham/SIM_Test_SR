using System.Collections.Generic;
using System.Linq;

namespace PRGX.SIMTrax.DAL.Repository.Abstract
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(System.Guid id);
        T GetById(long id);
        T Add(T entity);
        List<T> AddRange(List<T> entities);
        void Update(T entity);
        void Delete(T entity);
        void DeleteRange(List<T> entity);
        void Delete(System.Guid id);
        IQueryable<T> All();
    }
}
