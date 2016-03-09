using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class PartyPartyLink : AuditableEntity<long>
    {
        //   public long Id { get; set; }
        public string PartyPartyLinkType { get; set; }
        public long RefParty { get; set; }
        public long RefLinkedParty { get; set; }
        public string PartyPartyLinkSubType { get; set; }
        //public System.DateTime CreatedOn { get; set; }
        //public Nullable<System.DateTime> LastUpdatedOn { get; set; }
        //public long RefCreatedBy { get; set; }
        //public Nullable<long> RefLastUpdatedBy { get; set; }
        public byte[] RowVersion { get; set; }
        public virtual Party Party { get; set; }
        public virtual Party Party1 { get; set; }
    }
}
