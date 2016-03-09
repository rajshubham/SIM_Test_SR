using PRGX.SIMTrax.DAL.Entity.AuditModel;
using System;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class LegalEntityProfile : BaseEntity<long>
    {
        //public long Id { get; set; }
        public string ProfileType { get; set; }
        public long RefPartyId { get; set; }
        public Nullable<long> RefContactMethod { get; set; }
        public Nullable<long> RefBank { get; set; }
        public virtual BankAccount BankAccount { get; set; }
        public virtual ContactMethod ContactMethod { get; set; }
        public virtual Party Party { get; set; }
    }
}
