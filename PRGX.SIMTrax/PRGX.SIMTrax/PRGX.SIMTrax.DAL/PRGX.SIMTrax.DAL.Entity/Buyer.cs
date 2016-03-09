using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class Buyer : AuditableEntityWithoutId
    {
        public Buyer()
        {
            this.BuyerCampaigns = new List<BuyerCampaign>();
            this.BuyerSupplierReferences = new List<BuyerSupplierReference>();
            this.DiscountVouchers = new List<DiscountVoucher>();
        }
        [Key, ForeignKey("Organization")]
        public long Id { get; set; }
        public Nullable<int> MaxCampaignSupplierCount { get; set; }
        public Nullable<System.DateTime> ActivationDate { get; set; }
        public Nullable<System.DateTime> FirstSignInDate { get; set; }
        public Nullable<bool> HasPaid { get; set; }
        //public System.DateTime CreatedOn { get; set; }
        //public Nullable<System.DateTime> LastUpdatedOn { get; set; }
        //public long RefCreatedBy { get; set; }
        //public Nullable<long> RefLastUpdatedBy { get; set; }
        public byte[] RowVersion { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual ICollection<BuyerCampaign> BuyerCampaigns { get; set; }
        public virtual ICollection<BuyerSupplierReference> BuyerSupplierReferences { get; set; }
        public virtual ICollection<DiscountVoucher> DiscountVouchers { get; set; }
    }
}
