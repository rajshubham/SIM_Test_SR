using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.Domain.Util;
using PRGX.SIMTrax.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.ServiceFacade.Mapper
{
    public static class RoleMapper
    {
        public static List<ViewModel.Role> ToRoleViewModel(this List<DAL.Entity.Role> rolePMList)
        {
            return rolePMList.Select(r => new ViewModel.Role() {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description
            }).ToList();
        }

        public static ViewModel.Role ToRoleViewModel(this DAL.Entity.Role rolePM)
        {
            return new ViewModel.Role()
            {
                Id = rolePM.Id,
                Name = rolePM.Name,
                Description = rolePM.Description
            };
        }

        public static List<AccessType> ToRoleViewModel(this List<PotentialRole> potentialRoleList)
        {
            return potentialRoleList.Select(r => new AccessType()
            {
                PotentialRoleId = r.Role1.Id,
                PotentialRoleName = r.Role1.Name,
                PotentialRoleDescription = r.Role1.Description,
                ExistingRoleId = r.RefExistingRole,
                ExistingRoleName = CommonMethods.Description((RoleType)r.RefExistingRole)
            }).ToList();
        }

        public static List<RolePermission> ToRolePermissionDomainModel(this List<RolePermissionLink> rolePermissionPMList)
        {
            return rolePermissionPMList.Select(v => new RolePermission()
            {
                PermissionId = v.RefPermission,
                IsChecked = true,
                RoleId = v.RefRole,
                Id = v.Id,
                Description = v.Permission != null ? v.Permission.Description : string.Empty
            }).ToList();
        }

        public static ViewModel.Role ToRolePermissionMappingViewModel(this DAL.Entity.Role rolePM)
        {
            return new ViewModel.Role()
            {
                Id = rolePM.Id,
                Name = rolePM.Name,
                Description = rolePM.Description,
                ExistingRoleId = rolePM.PotentialRoles1.FirstOrDefault(c => c.RefPotentialRole == rolePM.Id) != null ? rolePM.PotentialRoles1.FirstOrDefault(c => c.RefPotentialRole == rolePM.Id).RefExistingRole : 0,
                RolePermissions = rolePM.RolePermissionLinks.ToList().ToRolePermissionDomainModel()
            };
        }
    }
}
