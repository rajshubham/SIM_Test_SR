using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class UserRoleLink:BaseEntity<long>
    {
      //  public long Id { get; set; }
        public long RefUser { get; set; }
        public long RefRole { get; set; }
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
