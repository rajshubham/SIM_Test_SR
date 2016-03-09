using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity 
{
    public partial class BankAccount :AuditableEntity<long> 
    {
        public BankAccount()
        {
            this.LegalEntityProfiles = new List<LegalEntityProfile>();
        }

     //   public long Id { get; set; }
        public long RefLegalEntity { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string SwiftCode { get; set; }
        public string IBAN { get; set; }
        public string BankName { get; set; }
        public string Address { get; set; }
        public long RefCountryId { get; set; }
        public string PreferredMode { get; set; }
        public string BranchIdentifier { get; set; }

        //public System.DateTime CreatedOn { get; set; }
        //public long RefCreatedBy { get; set; }
        //public Nullable<System.DateTime> LastUpdatedOn { get; set; }
        //public Nullable<long> RefLastUpdatedBy { get; set; }

        public byte[] RowVersion { get; set; }
        public virtual Region Region { get; set; }
        public virtual LegalEntity LegalEntity { get; set; }
        public virtual ICollection<LegalEntityProfile> LegalEntityProfiles { get; set; }
    }
}
