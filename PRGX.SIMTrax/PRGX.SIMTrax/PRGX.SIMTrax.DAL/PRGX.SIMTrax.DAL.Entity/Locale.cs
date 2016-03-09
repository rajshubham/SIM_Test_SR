using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class Locale :BaseEntity<long>
    {
        public Locale()
        {
            this.CampaignMessages = new List<CampaignMessage>();
            this.EmailTemplates = new List<EmailTemplate>();
            this.MasterDatas = new List<MasterData>();
        }

     //   public long Id { get; set; }
        public string ISOCode { get; set; }
        public virtual ICollection<CampaignMessage> CampaignMessages { get; set; }
        public virtual ICollection<EmailTemplate> EmailTemplates { get; set; }
        public virtual ICollection<MasterData> MasterDatas { get; set; }
    }
}
