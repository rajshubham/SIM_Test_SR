using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Abstract
{
    public interface IRoleUow
    {
        void SaveChanges();

        IRoleRepository Roles { get; }

        IPotentialRoleRepository PotentialRoles { get; }

        IGenericRepository<UserRoleLink> UserRoleLinks { get; }

        IGenericRepository<RolePermissionLink> RolePermissionLinks { get; }

        IPermissionRepository Permissions { get; }

        void Dispose();

        void Rollback();

        void Commit();

        void BeginTransaction();
    }
}
