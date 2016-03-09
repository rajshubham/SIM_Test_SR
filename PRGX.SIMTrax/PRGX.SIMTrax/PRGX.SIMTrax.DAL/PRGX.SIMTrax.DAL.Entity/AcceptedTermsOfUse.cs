using PRGX.SIMTrax.DAL.Entity.AuditModel;
using System;
using System.Collections.Generic;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class AcceptedTermsOfUse : BaseEntity<long>
    {
        //public long Id { get; set; }

        public long RefTermsOfUse { get; set; }
        public long RefAcceptingUser { get; set; }
        public System.DateTime AcceptedDate { get; set; }
        public virtual TermsOfUse TermsOfUse { get; set; }
        public virtual User User { get; set; }
    }
}
