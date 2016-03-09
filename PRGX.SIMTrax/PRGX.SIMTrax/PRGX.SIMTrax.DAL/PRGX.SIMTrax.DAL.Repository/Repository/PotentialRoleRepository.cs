 using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Abstract;
using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository.Repository
{
    public class PotentialRoleRepository : GenericRepository<PotentialRole>, IPotentialRoleRepository
    {
        public PotentialRoleRepository(DbContext context) : base(context) { }

        public List<PotentialRole> GetAllAccessTypes(int accessType, int pageSize, int index, out int total)
        {
            try
            {
                Logger.Info("PotentialRoleRepository : GetAllAccessTypes() : Enter the method");

                var roles = new List<PotentialRole>();

                var query = this.All().Include(r => r.Role1).AsQueryable();

                if ((RoleType)accessType != RoleType.None)
                    query = query.Where(x => x.RefExistingRole == accessType).AsQueryable();

                int skipCount = Convert.ToInt32(index - 1) * pageSize;
                roles = query.OrderBy(elem => elem.Id).Skip(skipCount).Take(pageSize).ToList();
                total = query.Count();

                Logger.Info("PotentialRoleRepository : GetAllAccessTypes() : Exit the method");
                return roles;
            }
            catch (Exception ex)
            {
                Logger.Error("PotentialRoleRepository : GetAllAccessTypes() : Caught an error" + ex);
                throw;
            }
        }
    }
}
