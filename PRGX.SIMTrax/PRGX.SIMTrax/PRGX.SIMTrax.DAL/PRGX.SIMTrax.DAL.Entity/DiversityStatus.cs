using PRGX.SIMTrax.DAL.Entity.AuditModel;
using System.Collections.Generic;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class DiversityStatus : BaseEntity<long>
    {
        public DiversityStatus()
        {
            this.DiversityStatusRegions = new List<DiversityStatusRegion>();
        }

        //public long Id { get; set; }
        public long RefSupplier { get; set; }
        public long RefDiversityStatusType { get; set; }
        public virtual DiversityStatusType DiversityStatusType { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<DiversityStatusRegion> DiversityStatusRegions { get; set; }
    }
}
