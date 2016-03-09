using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class Credential :AuditableEntity<long>
    {
      //  public long Id { get; set; }
        public string LoginId { get; set; }
        public string Password { get; set; }
        public bool IsLocked { get; set; }
        public Nullable<int> LoginAttemptCount { get; set; }
        public Nullable<System.DateTime> LockedTime { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
        public long RefUser { get; set; }
        //public System.DateTime CreatedOn { get; set; }
        //public long RefCreatedBy { get; set; }
        //public Nullable<System.DateTime> LastUpdatedOn { get; set; }
        //public Nullable<long> RefLastUpdatedBy { get; set; }
       public byte[] RowVersion { get; set; }
        public virtual User User { get; set; }
    }
}
