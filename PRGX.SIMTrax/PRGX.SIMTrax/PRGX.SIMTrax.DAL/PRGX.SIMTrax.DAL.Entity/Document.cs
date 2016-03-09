using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class Document :BaseEntity<long>
    {
        public Document()
        {
            this.BuyerCampaigns = new List<BuyerCampaign>();
            this.Organizations = new List<Organization>();
            this.PartyIdentifierDocuments = new List<PartyIdentifierDocument>();
        }

   //     public long Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public Nullable<long> ContentLength { get; set; }
        public string ContentType { get; set; }
        public virtual ICollection<BuyerCampaign> BuyerCampaigns { get; set; }
        public virtual ICollection<Organization> Organizations { get; set; }
        public virtual ICollection<PartyIdentifierDocument> PartyIdentifierDocuments { get; set; }
    }
}
