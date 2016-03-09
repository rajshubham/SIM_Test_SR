using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class DiversityStatusRegion : BaseEntity<long>
    {
        //public long Id { get; set; }
        public long RefDiversityStatus { get; set; }
        public long RefRegion { get; set; }
        public virtual DiversityStatus DiversityStatu { get; set; }
        public virtual Region Region { get; set; }
    }
}
