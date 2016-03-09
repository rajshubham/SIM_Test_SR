using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class PartyProfile :AuditableEntity<long>
    {
       // public long Id { get; set; }
        public long RefSubjectId { get; set; }
        public long RefAccessorId { get; set; }
        public string Type { get; set; }
        public string Explanation { get; set; }
        //public System.DateTime CreatedOn { get; set; }
        //public long RefCreatedBy { get; set; }
        //public Nullable<System.DateTime> LastUpdatedOn { get; set; }
        //public Nullable<long> RefLastUpdatedBy { get; set; }
        public byte[] RowVersion { get; set; }
        public virtual Party Party { get; set; }
        public virtual Party Party1 { get; set; }
    }
}
