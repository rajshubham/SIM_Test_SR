using PRGX.SIMTrax.DAL;
using PRGX.SIMTrax.DAL.Abstract;
using PRGX.SIMTrax.Domain.Util;
using PRGX.SIMTrax.ServiceFacade.Abstract;
using PRGX.SIMTrax.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRGX.SIMTrax.ServiceFacade.Mapper;
using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.DAL.Entity;

namespace PRGX.SIMTrax.ServiceFacade
{
    public class RoleServiceFacade : IRoleServiceFacade
    {
        private readonly IRoleUow _roleUow;

        public RoleServiceFacade()
        {
            _roleUow = new RoleUow();
        }

        public List<ViewModel.Role> GetRoles(string existingRole)
        {
            try
            {
                Logger.Info("RoleServiceFacade : GetRoles() : Enter the method");

                var roles = new List<ViewModel.Role>();
                var existingRolePM = _roleUow.Roles.GetAll().Where(r => r.Name.Trim().Equals(existingRole)).FirstOrDefault();

                if (null != existingRolePM)
                {
                    var potentialRoles = _roleUow.PotentialRoles.GetAll().Where(p => p.RefExistingRole == existingRolePM.Id).ToList();
                    foreach(var potentialRolePM in potentialRoles)
                    {
                        var rolePM = _roleUow.Roles.GetById(potentialRolePM.RefPotentialRole);
                        roles.Add(rolePM.ToRoleViewModel());
                    }
                }

                Logger.Info("RoleServiceFacade : GetRoles() : Exit the method");
                return roles;
            }
            catch(Exception ex)
            {
                Logger.Error("RoleServiceFacade : GetRoles() : Caught an error" + ex);
                throw;
            }
        }

        public bool CheckAuditorPermission(int permissionId, long auditorUserId)
        {
            Logger.Info("RoleServiceFacade : CheckAuditorPermission() : Entering the method.");
            try
            {
                var isPermissionAssigned = false;

                var userRoleLink = _roleUow.UserRoleLinks.GetAll().Where(x => x.RefUser == auditorUserId).FirstOrDefault();

                if (null != userRoleLink)
                {
                    var rolePermissionLink = _roleUow.RolePermissionLinks.GetAll().Where(x => x.RefPermission == permissionId && x.RefRole == userRoleLink.RefRole);
                    if (null != rolePermissionLink)
                    {
                        isPermissionAssigned = true;
                    }
                }

                Logger.Info("RoleServiceFacade : CheckAuditorPermission() : Exiting the method.");
                return isPermissionAssigned;
            }
            catch (Exception ex)
            {
                Logger.Error("RoleServiceFacade : CheckAuditorPermission() : Caught an exception." + ex);
                throw;
            }
        }

        public List<AccessType> GetAllAccessTypes(int accessType, int pageSize, int index, out int total)
        {
            Logger.Info("RoleServiceFacade : GetAllAccessTypes() : Entering the method.");
            try
            {
                Logger.Info("RoleServiceFacade : GetAllAccessTypes() : Exiting the method.");

                return _roleUow.PotentialRoles.GetAllAccessTypes(accessType, pageSize, index, out total).ToRoleViewModel();
            }
            catch (Exception ex)
            {
                Logger.Error("RoleServiceFacade : GetAllAccessTypes() : Caught an exception." + ex);
                throw;
            }
        }

        public ViewModel.Role GetRoleDetails(long roleId)
        {
            try
            {
                Logger.Info("RoleServiceFacade : GetRoleDetails() : Entering the method.");
                var role = new ViewModel.Role();
                var rolePM = _roleUow.Roles.GetRoleDetails(roleId);
                if (null != rolePM)
                    role = rolePM.ToRolePermissionMappingViewModel();

                Logger.Info("RoleServiceFacade : GetRoleDetails() : Exiting the method.");
                return role;
            }
            catch (Exception ex)
            {
                Logger.Error("RoleServiceFacade : GetRoleDetails() : Caught an exception." + ex);
                throw;
            }
        }

        public bool AddorUpdateRole(long roleId, string roleName, string roleDescription, List<long> permissions, long userId, long existingRoleId)
        {
            IRoleUow _roleUow = null;
            try
            {
                Logger.Info("RoleServiceFacade : AddorUpdateBuyerRole() : Enter into method");
                _roleUow = new RoleUow();
                var result = false;

                _roleUow.BeginTransaction();
                if (roleId == 0)
                {
                    var potentialRole = new List<DAL.Entity.PotentialRole>();
                    potentialRole.Add(new DAL.Entity.PotentialRole()
                    {
                        RefExistingRole = existingRoleId
                    });
                    var role = new DAL.Entity.Role()
                    {
                        Name = roleName,
                        Description = roleDescription,
                        RefCreatedBy = userId,
                        RefLastUpdatedBy = userId,
                        PotentialRoles1 = potentialRole
                    };
                    _roleUow.Roles.Add(role);
                    _roleUow.SaveChanges();

                    foreach (var item in permissions)
                    {
                        var permissionItem = new DAL.Entity.RolePermissionLink()
                        {
                            RefPermission = item,
                            RefRole = role.Id
                        };
                        _roleUow.RolePermissionLinks.Add(permissionItem);
                        _roleUow.SaveChanges();
                    }
                }
                else
                {
                    var role = _roleUow.Roles.GetAll().FirstOrDefault(u => u.Id == roleId);
                    if (role != null)
                    {
                        role.RefLastUpdatedBy = userId;
                        role.Description = roleDescription;
                        role.Name = roleName;
                        _roleUow.Roles.Update(role);
                        _roleUow.SaveChanges();

                        var existingPermissions = _roleUow.RolePermissionLinks.GetAll().Where(u => u.RefRole == roleId).ToList();
                        _roleUow.RolePermissionLinks.DeleteRange(existingPermissions);
                        _roleUow.SaveChanges();

                        foreach (var item in permissions)
                        {
                            var permissionItem = new DAL.Entity.RolePermissionLink()
                            {
                                RefPermission = item,
                                RefRole = roleId
                            };
                            _roleUow.RolePermissionLinks.Add(permissionItem);
                            _roleUow.SaveChanges();
                        }
                    }
                }
                _roleUow.Commit();
                result = true;

                Logger.Info("RoleServiceFacade : AddorUpdateBuyerRole() : Exit from method");
                return result;
            }
            catch (Exception ex)
            {
                if (_roleUow != null)
                    _roleUow.Rollback();
                Logger.Info("RoleServiceFacade : AddorUpdateBuyerRole() : Caught an exception " + ex);
                throw;
            }
        }

        public bool DeleteRole(long roleId)
        {
            IRoleUow _roleUow = null;
            try
            {
                Logger.Info("RoleServiceFacade : DeleteRole() : Enter into method");
                _roleUow = new RoleUow();
                var result = false;

                _roleUow.BeginTransaction();

                var rolePM = _roleUow.Roles.GetById(roleId);
                if (null != rolePM)
                {
                    var existingPermissions = _roleUow.RolePermissionLinks.GetAll().Where(u => u.RefRole == roleId).ToList();
                    _roleUow.RolePermissionLinks.DeleteRange(existingPermissions);
                    _roleUow.SaveChanges();

                    var potentialRole = _roleUow.PotentialRoles.GetAll().FirstOrDefault(c => c.RefPotentialRole == roleId);
                    if (null != potentialRole)
                    {
                        _roleUow.PotentialRoles.Delete(potentialRole);
                        _roleUow.SaveChanges();
                    }

                    _roleUow.Roles.Delete(rolePM);
                    _roleUow.SaveChanges();
                }
                _roleUow.Commit();
                result = true;

                Logger.Info("RoleServiceFacade : DeleteRole() : Exit from method");
                return result;
            }
            catch (Exception ex)
            {
                if (_roleUow != null)
                    _roleUow.Rollback();
                Logger.Info("RoleServiceFacade : DeleteRole() : Caught an exception " + ex);
                throw;
            }
        }

        public List<ItemList> GetUserPermissionBasedOnUserId(long userId)
        {
            try
            {
                Logger.Info("RoleServiceFacade : GetUserPermissionBasedOnRoleId() : Entering the method.");
                var permissionList = _roleUow.Roles.GetUserPermissionBasedOnUserId(userId);

                Logger.Info("RoleServiceFacade : GetUserPermissionBasedOnRoleId() : Exiting the method.");
                return permissionList;
            }
            catch (Exception ex)
            {
                Logger.Error("RoleServiceFacade : GetUserPermissionBasedOnRoleId() : Caught an exception." + ex);
                throw;
            }
        }

        public List<ItemList> GetUserListByPermission(long permission)
        {
            try
            {
                Logger.Info("RoleServiceFacade : GetUserListByPermission() : Entering the method.");
                var usersList = _roleUow.Roles.GetUserListByPermission(permission);

                Logger.Info("RoleServiceFacade : GetUserListByPermission() : Exiting the method.");
                return usersList;
            }
            catch (Exception ex)
            {
                Logger.Error("RoleServiceFacade : GetUserListByPermission() : Caught an exception." + ex);
                throw;
            }
        }

        public List<long> GetRolesForUser(long userId)
        {
            try
            {
                Logger.Info("RoleServiceFacade : GetRolesForUser() : Enter the method");

                var roles = new List<long>();
                roles = _roleUow.UserRoleLinks.GetAll().Where(r => r.RefUser == userId).Select(x => x.RefRole).ToList();

                Logger.Info("RoleServiceFacade : GetRolesForUser() : Exit the method");
                return roles;
            }
            catch (Exception ex)
            {
                Logger.Error("RoleServiceFacade : GetRolesForUser() : Caught an error" + ex);
                throw;
            }
        }

        public bool AddOrUpdateUserRoleLinks(long userId, List<long> roleIds)
        {
            IRoleUow _roleUow = null;
            try
            {
                Logger.Info("RoleServiceFacade : AddOrUpdateUserRoleLinks() : Enter into method");
                _roleUow = new RoleUow();
                var result = false;

                _roleUow.BeginTransaction();

                var existingUserRoleLinks = _roleUow.UserRoleLinks.GetAll().Where(u => u.RefUser == userId).ToList();
                if (existingUserRoleLinks.Count > 0)
                {
                    _roleUow.UserRoleLinks.DeleteRange(existingUserRoleLinks);
                    _roleUow.SaveChanges();
                }

               foreach(var roleId in roleIds)
                {
                    var userRoleLink = new UserRoleLink() {
                        RefRole = roleId,
                        RefUser = userId,
                    };
                    _roleUow.UserRoleLinks.Add(userRoleLink);
                    _roleUow.SaveChanges();
                }

                _roleUow.Commit();
                result = true;

                Logger.Info("RoleServiceFacade : AddOrUpdateUserRoleLinks() : Exit from method");
                return result;
            }
            catch (Exception ex)
            {
                if (_roleUow != null)
                    _roleUow.Rollback();
                Logger.Info("RoleServiceFacade : AddOrUpdateUserRoleLinks() : Caught an exception " + ex);
                throw;
            }
        }

        public bool DeleteUserRoleLinks(long userId)
        {
            IRoleUow _roleUow = null;
            try
            {
                Logger.Info("RoleServiceFacade : DeleteUserRoleLinks() : Enter into method");
                _roleUow = new RoleUow();
                var result = false;

                _roleUow.BeginTransaction();

                var existingUserRoleLinks = _roleUow.UserRoleLinks.GetAll().Where(u => u.RefUser == userId).ToList();
                if (existingUserRoleLinks.Count > 0)
                {
                    _roleUow.UserRoleLinks.DeleteRange(existingUserRoleLinks);
                    _roleUow.SaveChanges();
                }
    
                _roleUow.Commit();
                result = true;

                Logger.Info("RoleServiceFacade : DeleteUserRoleLinks() : Exit from method");
                return result;
            }
            catch (Exception ex)
            {
                if (_roleUow != null)
                    _roleUow.Rollback();
                Logger.Info("RoleServiceFacade : DeleteUserRoleLinks() : Caught an exception " + ex);
                throw;
            }
        }
    }
}
