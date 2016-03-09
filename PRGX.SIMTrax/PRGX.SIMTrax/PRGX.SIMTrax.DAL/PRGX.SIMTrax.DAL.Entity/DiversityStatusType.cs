using PRGX.SIMTrax.DAL.Entity.AuditModel;
using System.Collections.Generic;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class DiversityStatusType : BaseEntity<long>
    {
        public DiversityStatusType()
        {
            this.DiversityStatus = new List<DiversityStatus>();
            this.DiversityStatusTypeRegions = new List<DiversityStatusTypeRegion>();
        }

        //public long Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<DiversityStatus> DiversityStatus { get; set; }
        public virtual ICollection<DiversityStatusTypeRegion> DiversityStatusTypeRegions { get; set; }
    }
}
