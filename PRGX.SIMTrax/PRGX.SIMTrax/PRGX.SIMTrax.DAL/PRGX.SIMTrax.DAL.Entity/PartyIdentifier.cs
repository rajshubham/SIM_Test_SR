using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class PartyIdentifier :AuditableEntity<long>
    {
        public PartyIdentifier()
        {
            this.PartyIdentifierDocuments = new List<PartyIdentifierDocument>();
        }

    //    public long Id { get; set; }
        public long RefPartyIdentifierType { get; set; }
        public string IdentifierNumber { get; set; }
        public long RefParty { get; set; }
        public long RefRegion { get; set; }
        //public System.DateTime CreatedOn { get; set; }
        //public Nullable<System.DateTime> LastUpdatedOn { get; set; }
        //public long RefCreatedBy { get; set; }
        //public Nullable<long> RefLastUpdatedBy { get; set; }
        public byte[] RowVersion { get; set; }
        public virtual Party Party { get; set; }
        public virtual PartyIdentifierType PartyIdentifierType { get; set; }
        public virtual Region Region { get; set; }
        public virtual ICollection<PartyIdentifierDocument> PartyIdentifierDocuments { get; set; }
    }
}
