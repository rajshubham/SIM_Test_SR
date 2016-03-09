using PRGX.SIMTrax.DAL.Entity.AuditModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class SupplierReferrer : AuditableEntity<long>
    {
        //public int Id { get; set; }
        public long RefCampaign { get; set; }
        public long RefSupplier { get; set; }
        //public System.DateTime CreatedOn { get; set; }
        //public long RefCreatedBy { get; set; }
        //public System.DateTime LastUpdatedOn { get; set; }
        //public long RefLastUpdatedBy { get; set; }
        public bool LandingReferrer { get; set; }
        public virtual BuyerCampaign BuyerCampaign { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}
