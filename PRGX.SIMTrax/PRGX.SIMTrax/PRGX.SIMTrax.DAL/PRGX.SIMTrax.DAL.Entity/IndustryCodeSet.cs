using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class IndustryCodeSet :BaseEntity<long>
    {
        public IndustryCodeSet()
        {
            this.IndustryCodes = new List<IndustryCode>();
            this.IndustryCodeSetRegionLinks = new List<IndustryCodeSetRegionLink>();
        }

       // public long Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<IndustryCode> IndustryCodes { get; set; }
        public virtual ICollection<IndustryCodeSetRegionLink> IndustryCodeSetRegionLinks { get; set; }
    }
}
