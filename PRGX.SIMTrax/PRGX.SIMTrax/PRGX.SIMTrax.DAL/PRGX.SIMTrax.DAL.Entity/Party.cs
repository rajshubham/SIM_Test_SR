using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class Party :AuditableEntity<long>
    {
        public Party()
        {
            this.PartyContactMethodLinks = new List<PartyContactMethodLink>();
            this.EmailAudits = new List<EmailAudit>();
            this.Organizations1 = new List<Organization>();
            this.PartyIdentifiers = new List<PartyIdentifier>();
            this.PartyPartyLinks = new List<PartyPartyLink>();
            this.PartyPartyLinks1 = new List<PartyPartyLink>();
            this.PartyProfiles = new List<PartyProfile>();
            this.PartyProfiles1 = new List<PartyProfile>();
            this.PartyRegionLinks = new List<PartyRegionLink>();
        
            this.LegalEntityProfiles = new List<LegalEntityProfile>();
        }

        //     public long Id { get; set; }
        public string PartyName { get; set; }
        public string PartyType { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public long? ProjectSource { get; set; }
        //public System.DateTime CreatedOn { get; set; }
        //public long RefCreatedBy { get; set; }
        //public Nullable<System.DateTime> LastUpdatedOn { get; set; }
        //public Nullable<long> RefLastUpdatedBy { get; set; }
        public byte[] RowVersion { get; set; }
        public virtual ICollection<PartyContactMethodLink> PartyContactMethodLinks { get; set; }
        public virtual ICollection<EmailAudit> EmailAudits { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual ICollection<Organization> Organizations1 { get; set; }
        public virtual ICollection<PartyIdentifier> PartyIdentifiers { get; set; }
        public virtual ICollection<PartyPartyLink> PartyPartyLinks { get; set; }
        public virtual ICollection<PartyPartyLink> PartyPartyLinks1 { get; set; }
        public virtual ICollection<PartyProfile> PartyProfiles { get; set; }
        public virtual ICollection<PartyProfile> PartyProfiles1 { get; set; }
        public virtual ICollection<PartyRegionLink> PartyRegionLinks { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<LegalEntityProfile> LegalEntityProfiles { get; set; }
    }
}
