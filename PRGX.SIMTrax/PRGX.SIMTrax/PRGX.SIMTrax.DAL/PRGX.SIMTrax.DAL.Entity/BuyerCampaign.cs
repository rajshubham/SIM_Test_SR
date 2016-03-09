using PRGX.SIMTrax.DAL.Entity.AuditModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class BuyerCampaign : AuditableEntity<long>
    {
        public BuyerCampaign()
        {
            this.CampaignMessages = new List<CampaignMessage>();
            this.CampaignInvitations = new List<CampaignInvitation>();
            this.SupplierReferrers = new List<SupplierReferrer>();
        }

        //public long Id { get; set; }
        public string CampaignName { get; set; }
        public Nullable<System.DateTime> CampaignStartDate { get; set; }
        public Nullable<System.DateTime> CampaignEndDate { get; set; }
        public Nullable<int> SupplierCount { get; set; }
        public string CampaignUrl { get; set; }
        public long CampaignType { get; set; }
        public long CampaignStatus { get; set; }
        public Nullable<long> TemplateType { get; set; }
        public string DataSource { get; set; }
        public string Notes { get; set; }
        public Nullable<long> RefEmailTemplate { get; set; }
        public Nullable<bool> IsDownloaded { get; set; }
        public string PreRegisteredFilePath { get; set; }
        public Nullable<long> RefAuditorId { get; set; }
        public Nullable<long> RefCampaignLogo { get; set; }
        public Nullable<long> RefBuyer { get; set; }
        public Nullable<long> RefDiscountVoucher { get; set; }
        //public System.DateTime CreatedOn { get; set; }
        //public long RefCreatedBy { get; set; }
        //public Nullable<System.DateTime> LastUpdatedOn { get; set; }
        //public Nullable<long> RefLastUpdatedBy { get; set; }
        public virtual Buyer Buyer { get; set; }
        public virtual DiscountVoucher DiscountVoucher { get; set; }
        public virtual Document Document { get; set; }
        public virtual EmailTemplate EmailTemplate { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<CampaignMessage> CampaignMessages { get; set; }
        public virtual ICollection<CampaignInvitation> CampaignInvitations { get; set; }
        public virtual ICollection<SupplierReferrer> SupplierReferrers { get; set; }
    }
}
