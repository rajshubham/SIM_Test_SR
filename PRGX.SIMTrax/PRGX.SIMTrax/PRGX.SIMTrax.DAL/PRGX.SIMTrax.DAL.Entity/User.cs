using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class User : AuditableEntityWithoutId
    {
        public User()
        {
            this.AcceptedTermsOfUses = new List<AcceptedTermsOfUse>();
            this.BuyerCampaigns = new List<BuyerCampaign>();
            this.Credentials = new List<Credential>();
            this.TemporaryPasswordUrls = new List<TemporaryPasswordUrl>();
            this.UserRoleLinks = new List<UserRoleLink>();
        }
        [Key, ForeignKey("Person")]
        public long Id { get; set; }
        public bool NeedsPasswordChange { get; set; }
        public long UserType { get; set; }
        //public System.DateTime CreatedOn { get; set; }
        //public long RefCreatedBy { get; set; }
        //public Nullable<System.DateTime> LastUpdatedOn { get; set; }
        //public Nullable<long> RefLastUpdatedBy { get; set; }
        public byte[] RowVersion { get; set; }
        public virtual ICollection<AcceptedTermsOfUse> AcceptedTermsOfUses { get; set; }
        public virtual ICollection<BuyerCampaign> BuyerCampaigns { get; set; }
        public virtual ICollection<Credential> Credentials { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<TemporaryPasswordUrl> TemporaryPasswordUrls { get; set; }
        public virtual ICollection<UserRoleLink> UserRoleLinks { get; set; }
    }
}
