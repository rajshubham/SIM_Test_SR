using PRGX.SIMTrax.DAL.Entity.AuditModel;
using System;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class DiversityStatusTypeRegion : BaseEntity<long>
    {
        //public long Id { get; set; }
        public long RefDiversityStatusType { get; set; }
        public long RefApplicableRegion { get; set; }
        public Nullable<bool> IsApplicableInSubRegion { get; set; }
        public virtual DiversityStatusType DiversityStatusType { get; set; }
        public virtual Region Region { get; set; }
    }
}
