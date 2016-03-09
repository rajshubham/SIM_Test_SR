using System;

namespace PRGX.SIMTrax.DAL.Entity.AuditModel
{
    public abstract class AuditableEntity<T> :  BaseEntity<T>, IAuditableEntity
    {
        public System.DateTime CreatedOn { get; set; }
        public long RefCreatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedOn { get; set; }
        public Nullable<long> RefLastUpdatedBy { get; set; }
    }

    public abstract class AuditableEntityWithoutId : IAuditableEntity
    {
        public System.DateTime CreatedOn { get; set; }
        public long RefCreatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedOn { get; set; }
        public Nullable<long> RefLastUpdatedBy { get; set; }
    }
}
