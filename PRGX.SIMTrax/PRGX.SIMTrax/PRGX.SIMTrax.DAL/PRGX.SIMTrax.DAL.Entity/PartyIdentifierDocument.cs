using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class PartyIdentifierDocument:AuditableEntity<long>
    {
     //   public long Id { get; set; }
        public long RefPartyIdentifier { get; set; }
        public long RefDocument { get; set; }
        //public System.DateTime CreatedOn { get; set; }
        //public Nullable<System.DateTime> LastUpdatedOn { get; set; }
        //public long RefCreatedBy { get; set; }
        //public Nullable<long> RefLastUpdatedBy { get; set; }
        public byte[] RowVersion { get; set; }
        public virtual Document Document { get; set; }
        public virtual PartyIdentifier PartyIdentifier { get; set; }
    }
}
