using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class IndustryCodeOrganizationLink:BaseEntity<long>
    {
      //  public long Id { get; set; }
        public long RefOrganization { get; set; }
        public long RefIndustryCode { get; set; }
        public virtual IndustryCode IndustryCode { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
