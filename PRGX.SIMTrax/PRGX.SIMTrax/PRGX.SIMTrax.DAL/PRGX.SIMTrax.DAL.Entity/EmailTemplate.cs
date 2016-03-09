using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class EmailTemplate:BaseEntity<long>
    {
        public EmailTemplate()
        {
            this.BuyerCampaigns = new List<BuyerCampaign>();
        }

        // public int Id { get; set; }
        public string Mnemonic { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }
        public Nullable<long> RefLocale { get; set; }
        public virtual Locale Locale { get; set; }
        public virtual ICollection<BuyerCampaign> BuyerCampaigns { get; set; }
    }
}
