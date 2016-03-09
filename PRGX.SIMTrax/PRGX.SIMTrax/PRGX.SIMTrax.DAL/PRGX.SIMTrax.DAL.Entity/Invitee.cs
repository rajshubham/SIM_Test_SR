using PRGX.SIMTrax.DAL.Entity.AuditModel;
using System;
using System.Collections.Generic;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class Invitee : AuditableEntity<long>
    {
        public Invitee()
        {
            this.BuyerSupplierReferences = new List<BuyerSupplierReference>();
        }

        //public long Id { get; set; }
        public string ClientName { get; set; }
        public string ContactName { get; set; }
        public Nullable<long> RefSupplier { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string MailingAddress { get; set; }
        public string Fax { get; set; }
        public Nullable<bool> CanWeContact { get; set; }
        public string ClientRole { get; set; }
        public long RefReferee { get; set; }
        //public System.DateTime CreatedOn { get; set; }
        //public Nullable<System.DateTime> LastModifiedOn { get; set; }
        //public long RefCreatedBy { get; set; }
        //public Nullable<long> RefLastUpdatedBy { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual Supplier Supplier1 { get; set; }
        public virtual ICollection<BuyerSupplierReference> BuyerSupplierReferences { get; set; }
    }
}
