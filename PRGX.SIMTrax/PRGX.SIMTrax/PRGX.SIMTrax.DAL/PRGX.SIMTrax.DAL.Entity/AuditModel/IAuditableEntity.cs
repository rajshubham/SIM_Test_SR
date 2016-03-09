using System;

namespace PRGX.SIMTrax.DAL.Entity.AuditModel
{
    public interface IAuditableEntity
    {
        System.DateTime CreatedOn { get; set; }
        long RefCreatedBy { get; set; }
        Nullable<System.DateTime> LastUpdatedOn { get; set; }
        Nullable<long> RefLastUpdatedBy { get; set; }
    }
}
