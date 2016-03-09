using PRGX.SIMTrax.DAL.Entity.AuditModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class DiscountVoucher : AuditableEntity<long>
    {
        public DiscountVoucher()
        {
            this.BuyerCampaigns = new List<BuyerCampaign>();
        }

        //public long Id { get; set; }
        public string PromotionalCode { get; set; }
        public decimal DiscountPercent { get; set; }
        public System.DateTime PromotionStartDate { get; set; }
        public System.DateTime PromotionEndDate { get; set; }
        public Nullable<long> RefBuyer { get; set; }
        //public System.DateTime CreatedOn { get; set; }
        //public long RefCreatedBy { get; set; }
        //public Nullable<System.DateTime> LastUpdatedOn { get; set; }
        //public Nullable<long> RefLastUpdatedBy { get; set; }
        public virtual Buyer Buyer { get; set; }
        public ICollection<BuyerCampaign> BuyerCampaigns { get; set; }
    }
}
