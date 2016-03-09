using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class ContactMethod : AuditableEntity<long>
    {
        public ContactMethod()
        {
            this.Addresses = new List<Address>();
            this.Emails = new List<Email>();
            this.Phones = new List<Phone>();
            this.LegalEntityProfiles = new List<LegalEntityProfile>();
            this.PartyContactMethodLinks = new List<PartyContactMethodLink>();
        }

      //  public long Id { get; set; }
        public string ContactMethodType { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Email> Emails { get; set; }
        public virtual ICollection<Phone> Phones { get; set; }
        public virtual ICollection<PartyContactMethodLink> PartyContactMethodLinks { get; set; }
        public virtual ICollection<LegalEntityProfile> LegalEntityProfiles { get; set; }
    }
}
