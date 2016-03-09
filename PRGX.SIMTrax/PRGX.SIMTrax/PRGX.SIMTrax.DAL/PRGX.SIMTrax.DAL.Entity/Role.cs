using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class Role:AuditableEntity<long>
    {
        public Role()
        {
            this.PotentialRoles = new List<PotentialRole>();
            this.PotentialRoles1 = new List<PotentialRole>();
            this.RolePermissionLinks = new List<RolePermissionLink>();
            this.UserRoleLinks = new List<UserRoleLink>();
        }

      //  public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public System.DateTime CreatedOn { get; set; }
        //public long RefCreatedBy { get; set; }
        //public Nullable<System.DateTime> LastUpdatedOn { get; set; }
        //public Nullable<long> RefLastUpdatedBy { get; set; }
        public byte[] RowVersion { get; set; }
        public virtual ICollection<PotentialRole> PotentialRoles { get; set; }
        public virtual ICollection<PotentialRole> PotentialRoles1 { get; set; }
        public virtual ICollection<RolePermissionLink> RolePermissionLinks { get; set; }
        public virtual ICollection<UserRoleLink> UserRoleLinks { get; set; }
    }
}
