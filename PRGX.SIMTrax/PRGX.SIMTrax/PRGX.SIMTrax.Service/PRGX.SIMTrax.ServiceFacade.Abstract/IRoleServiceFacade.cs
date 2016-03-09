using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.Domain.Util;
using PRGX.SIMTrax.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.ServiceFacade.Abstract
{
    public interface IRoleServiceFacade
    {
        List<Role> GetRoles(string existingRole);

        bool CheckAuditorPermission(int permissionId, long auditorUserId);

        List<AccessType> GetAllAccessTypes(int accessType, int pageSize, int index, out int total);

        Role GetRoleDetails(long roleId);

        bool AddorUpdateRole(long roleId, string roleName, string roleDescription, List<long> permissions, long userId, long existingRoleId);

        bool DeleteRole(long roleId);

        List<ItemList> GetUserPermissionBasedOnUserId(long userId);

        List<ItemList> GetUserListByPermission(long permission);

        List<long> GetRolesForUser(long userId);

        bool AddOrUpdateUserRoleLinks(long userId, List<long> roleIds);

        bool DeleteUserRoleLinks(long userId);
    }
}
