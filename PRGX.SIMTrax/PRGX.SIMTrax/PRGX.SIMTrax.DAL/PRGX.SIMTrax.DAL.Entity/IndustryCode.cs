using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class IndustryCode:BaseEntity<long>
    {
        public IndustryCode()
        {
            this.IndustryCode1 = new List<IndustryCode>();
            this.IndustryCodeOrganizationLinks = new List<IndustryCodeOrganizationLink>();
        }

      //  public long Id { get; set; }
        public string SectorName { get; set; }
        public Nullable<long> RefParentId { get; set; }
        public string CodeNumber { get; set; }
        public string CodeDescription { get; set; }
        public long RefIndustryCodeSet { get; set; }
        public virtual ICollection<IndustryCode> IndustryCode1 { get; set; }
        public virtual IndustryCode IndustryCode2 { get; set; }
        public virtual IndustryCodeSet IndustryCodeSet { get; set; }
        public virtual ICollection<IndustryCodeOrganizationLink> IndustryCodeOrganizationLinks { get; set; }
    }
}
