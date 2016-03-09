using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository.Abstract
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Role GetRoleDetails(long roleId);

        List<ItemList> GetUserPermissionBasedOnUserId(long userId);

        List<ItemList> GetUserListByPermission(long permission);
    }
}
