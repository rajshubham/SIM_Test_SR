using PRGX.SIMTrax.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository.Abstract
{
    public interface IPotentialRoleRepository : IGenericRepository<PotentialRole>
    {
        List<PotentialRole> GetAllAccessTypes(int accessType, int pageSize, int index, out int total);
    }
}
