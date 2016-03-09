using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class PartyRegionLink:BaseEntity<long>
    {
       // public long Id { get; set; }
        public string LinkType { get; set; }
        public long RefParty { get; set; }
        public long RefRegion { get; set; }
        public virtual Party Party { get; set; }
        public virtual Region Region { get; set; }
    }
}
