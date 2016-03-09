using PRGX.SIMTrax.DAL.Repository.Abstract;
using PRGX.SIMTrax.DAL.Repository.Helper;

namespace PRGX.SIMTrax.DAL
{
    public abstract class GenericUow
    {
        protected IRepositoryProvider RepositoryProvider { get; set; }

        protected IGenericRepository<T> GetStandardRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepositoryForEntityType<T>();
        }

        protected T GetRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepository<T>();
        }
    }
}
