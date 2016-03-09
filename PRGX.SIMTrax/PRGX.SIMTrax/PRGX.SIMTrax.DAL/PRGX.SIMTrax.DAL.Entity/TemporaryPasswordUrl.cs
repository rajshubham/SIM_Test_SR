using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class TemporaryPasswordUrl :BaseEntity<long>
    {
      //  public long Id { get; set; }
        public string PasswordURL { get; set; }
        public long RefUser { get; set; }
        public string Token { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public virtual User User { get; set; }
    }
}
