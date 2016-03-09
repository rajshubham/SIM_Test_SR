using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class LegalEntity:AuditableEntity<long>
    {
        public LegalEntity()
        {
            this.BankAccounts = new List<BankAccount>();
            this.Organizations = new List<Organization>();
        }

      //  public long Id { get; set; }
        public string PartyName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        //public System.DateTime CreatedOn { get; set; }
        //public long RefCreatedBy { get; set; }
        //public Nullable<System.DateTime> LastUpdatedOn { get; set; }
        //public Nullable<long> RefLastUpdatedBy { get; set; }
        public byte[] RowVersion { get; set; }
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        public virtual ICollection<Organization> Organizations { get; set; }
    }
}
