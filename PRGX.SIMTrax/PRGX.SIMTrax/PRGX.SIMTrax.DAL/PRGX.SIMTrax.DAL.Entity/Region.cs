using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class Region:BaseEntity<long>
    {
        public Region()
        {
            this.Addresses = new List<Address>();
            this.BankAccounts = new List<BankAccount>();
            this.CampaignInvitations = new List<CampaignInvitation>();
            this.DiversityStatusRegions = new List<DiversityStatusRegion>();
            this.DiversityStatusTypeRegions = new List<DiversityStatusTypeRegion>();
            this.IndustryCodeSetRegionLinks = new List<IndustryCodeSetRegionLink>();
            this.PartyIdentifiers = new List<PartyIdentifier>();
            this.PartyIdentifierTypes = new List<PartyIdentifierType>();
            this.PartyRegionLinks = new List<PartyRegionLink>();
            this.Region1 = new List<Region>();
        }

        // public long Id { get; set; }
        public string Name { get; set; }
        public string RegionType { get; set; }
        public Nullable<long> RefParentRegion { get; set; }
        public int DisplayOrder { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        public virtual ICollection<CampaignInvitation> CampaignInvitations { get; set; }
        public virtual ICollection<DiversityStatusRegion> DiversityStatusRegions { get; set; }
        public virtual ICollection<DiversityStatusTypeRegion> DiversityStatusTypeRegions { get; set; }
        public virtual ICollection<IndustryCodeSetRegionLink> IndustryCodeSetRegionLinks { get; set; }
        public virtual ICollection<PartyIdentifier> PartyIdentifiers { get; set; }
        public virtual ICollection<PartyIdentifierType> PartyIdentifierTypes { get; set; }
        public virtual ICollection<PartyRegionLink> PartyRegionLinks { get; set; }
        public virtual ICollection<Region> Region1 { get; set; }
        public virtual Region Region2 { get; set; }
    }
}
