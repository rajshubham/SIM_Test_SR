using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class RolePermissionLink :BaseEntity<long>
    {
        //public long Id { get; set; }
        public long RefPermission { get; set; }
        public long RefRole { get; set; }
        public virtual Permission Permission { get; set; }
        public virtual Role Role { get; set; }
    }
}
