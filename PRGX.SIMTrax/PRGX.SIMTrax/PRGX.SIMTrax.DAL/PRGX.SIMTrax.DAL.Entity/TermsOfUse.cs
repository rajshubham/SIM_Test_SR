using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class TermsOfUse :BaseEntity<long>
    {
        public TermsOfUse()
        {
            this.AcceptedTermsOfUses = new List<AcceptedTermsOfUse>();
        }

       // public long Id { get; set; }
        public string HTMLText { get; set; }
        public string Version { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? RefCreatedBy { get; set; }
        public virtual ICollection<AcceptedTermsOfUse> AcceptedTermsOfUses { get; set; }
    }
}
