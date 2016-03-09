using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class Supplier : AuditableEntityWithoutId
   { 
        public Supplier()
        {
            this.CampaignInvitations = new List<CampaignInvitation>();
            this.DiversityStatus = new List<DiversityStatus>();
            this.Invitees = new List<Invitee>();
            this.Invitees1 = new List<Invitee>();
            this.SupplierReferrers = new List<SupplierReferrer>();
        }
        [Key, ForeignKey("Organization")]
        public long Id { get; set; }
        public Nullable<int> TypeOfSeller { get; set; }
        public string WebsiteLink { get; set; }
        public string BusinessDescription { get; set; }
        public string TradingName { get; set; }
        public Nullable<short> RegisteredCountry { get; set; }
        public string EstablishedYear { get; set; }
        public string FacebookAccount { get; set; }
        public string TwitterAccount { get; set; }
        public string LinkedInAccount { get; set; }
        public string MaxContractValue { get; set; }
        public string MinContractValue { get; set; }
        public Nullable<bool> IsSubsidary { get; set; }
        public string UltimateParent { get; set; }
        //public System.DateTime CreatedOn { get; set; }
        //public Nullable<System.DateTime> LastUpdatedOn { get; set; }
        //public long RefCreatedBy { get; set; }
        //public Nullable<long> RefLastUpdatedBy { get; set; }
        public byte[] RowVersion { get; set; }
        public virtual ICollection<CampaignInvitation> CampaignInvitations { get; set; }
        public virtual ICollection<DiversityStatus> DiversityStatus { get; set; }
        public virtual ICollection<Invitee> Invitees { get; set; }
        public virtual ICollection<Invitee> Invitees1 { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual ICollection<SupplierReferrer> SupplierReferrers { get; set; }
    }
}
