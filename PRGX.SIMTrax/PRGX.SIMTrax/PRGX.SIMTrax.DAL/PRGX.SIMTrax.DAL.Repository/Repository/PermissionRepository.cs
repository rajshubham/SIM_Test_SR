using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Abstract;
using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository.Repository
{
    public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(DbContext context) : base(context) { }

        public List<RolePermission> GetAllPermissions(int roleId)
        {
            try
            {
                var rolePermissionList = new List<RolePermission>();
                Logger.Info("PermissionRepository : GetAllPermissions() : Enter the method");

                var permissionList = this.All().Include("RolePermissionLinks").ToList();

                foreach (var permission in permissionList)
                {
                    rolePermissionList.Add(new RolePermission() {
                        PermissionId = permission.Id,
                        Description = permission.Description,
                        IsChecked = permission.RolePermissionLinks.Any() ? true : false
                    });
                }

                Logger.Info("PermissionRepository : GetAllPermissions() : Exit the method");
                return rolePermissionList;
            }
            catch (Exception ex)
            {
                Logger.Error("PermissionRepository : GetAllPermissions() : Caught an exception" + ex);
                throw ex;
            }
        }
    }
}
