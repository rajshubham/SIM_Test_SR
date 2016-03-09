using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class Permission:AuditableEntity<long>
    {
        public Permission()
        {
            this.RolePermissionLinks = new List<RolePermissionLink>();
        }

       // public long Id { get; set; }
        public string Description { get; set; }
        //public System.DateTime CreatedOn { get; set; }
        //public long RefCreatedBy { get; set; }
        //public Nullable<System.DateTime> LastUpdatedOn { get; set; }
        //public Nullable<long> RefLastUpdatedBy { get; set; }
        public byte[] RowVersion { get; set; }
        public virtual ICollection<RolePermissionLink> RolePermissionLinks { get; set; }
    }
}
