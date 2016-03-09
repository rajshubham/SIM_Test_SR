using PRGX.SIMTrax.DAL.Entity.AuditModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class CampaignMessage : BaseEntity<long>
    {
        //public long Id { get; set; }
        public string WelcomeMessage { get; set; }
        public long RefCampaign { get; set; }
        public Nullable<long> RefLocale { get; set; }
        public virtual BuyerCampaign BuyerCampaign { get; set; }
        public virtual Locale Locale { get; set; }
    }
}
