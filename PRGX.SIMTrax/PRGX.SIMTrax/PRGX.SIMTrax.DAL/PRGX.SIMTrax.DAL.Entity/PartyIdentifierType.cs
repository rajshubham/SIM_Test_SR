using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class PartyIdentifierType :BaseEntity<long>
    {
        public PartyIdentifierType()
        {
            this.PartyIdentifiers = new List<PartyIdentifier>();
        }

      //  public long Id { get; set; }
        public string IdentifierType { get; set; }
        public long RefRegion { get; set; }
        public virtual ICollection<PartyIdentifier> PartyIdentifiers { get; set; }
        public virtual Region Region { get; set; }
    }
}
