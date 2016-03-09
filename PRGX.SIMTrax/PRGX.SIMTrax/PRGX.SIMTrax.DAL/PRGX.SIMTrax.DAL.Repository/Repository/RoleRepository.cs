using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Abstract;
using PRGX.SIMTrax.DAL.Repository.Context;
using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository.Repository
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(DbContext context) : base(context) { }

        public Role GetRoleDetails(long roleId)
        {
            try
            {
                Logger.Info("RoleRepository : GetRoleDetails() : Entering the method.");
                var rolePM = this.All().Include("RolePermissionLinks").Include("PotentialRoles1").Include("RolePermissionLinks.Permission").FirstOrDefault(r => r.Id == roleId);

                Logger.Info("RoleRepository : GetRoleDetails() : Exiting the method.");
                return rolePM;
            }
            catch (Exception ex)
            {
                Logger.Error("RoleRepository : GetRoleDetails() : Caught an exception." + ex);
                throw;
            }
        }

        public List<ItemList> GetUserPermissionBasedOnUserId(long userId)
        {
            try
            {
                Logger.Info("RoleRepository : GetUserPermissionBasedOnRoleId() : Entering the method.");
                var permissionList = new List<ItemList>();

                using (var ctx = new RoleContext())
                {
                    var roleIds = ctx.UserRoleLinks.Where(x => x.RefUser == userId).Select(x => x.RefRole).ToList();
                    permissionList = ctx.RolePermissionLinks.Where(x => roleIds.Contains(x.RefRole)).Select(x => new ItemList
                    {
                        Text = "",
                        Value = x.RefPermission
                    }).ToList();
                }

                Logger.Info("RoleRepository : GetUserPermissionBasedOnRoleId() : Exiting the method.");
                return permissionList;
            }
            catch (Exception ex)
            {
                Logger.Error("RoleRepository : GetUserPermissionBasedOnRoleId() : Caught an exception." + ex);
                throw;
            }
        }

        public List<ItemList> GetUserListByPermission(long permission)
        {
            Logger.Info("RolesRepository : GetUserListByPermission() : Entering the method.");
            try
            {
                var listUsers = new List<ItemList>();
                using (var context = new RoleContext())
                {
                    context.Configuration.LazyLoadingEnabled = true;
                    var roles = context.RolePermissionLinks.Where(x => x.RefPermission == permission).Select(x => x.RefRole).ToList();
                    listUsers = context.UserRoleLinks.Where(x => roles.Contains(x.RefRole)).Select(x => new ItemList()
                    {
                        Text = x.User.Person.Party.PartyName,
                        Value = x.RefUser
                    }).ToList();
                }
                Logger.Info("RolesRepository : GetUserListByPermission() : Exiting the method.");
                return listUsers;
            }
            catch (Exception ex)
            {
                Logger.Error("RolesRepository : GetUserListByPermission() : Caught an exception." + ex);
                throw;
            }
        }
    }
}
