using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class IndustryCodeSetRegionLink :BaseEntity<long>
    {
       // public long Id { get; set; }
        public long RefIndustryCodeSet { get; set; }
        public long RefRegion { get; set; }
        public virtual IndustryCodeSet IndustryCodeSet { get; set; }
        public virtual Region Region { get; set; }
    }
}
