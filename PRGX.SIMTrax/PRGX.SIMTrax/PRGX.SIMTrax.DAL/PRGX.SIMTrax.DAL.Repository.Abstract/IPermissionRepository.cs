using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository.Abstract
{
    public interface IPermissionRepository : IGenericRepository<Permission>
    {
        List<RolePermission> GetAllPermissions(int roleId);
    }
}
