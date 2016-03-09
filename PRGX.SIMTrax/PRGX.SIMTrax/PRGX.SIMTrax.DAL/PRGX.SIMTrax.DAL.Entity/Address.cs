using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class Address : AuditableEntity<long>
    {

     //   public long Id { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Zone { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public Nullable<long> RefCountryId { get; set; }
        public string ZipCode { get; set; }
        public short AddressType { get; set; }
        public long RefContactMethod { get; set; }
        //public System.DateTime CreatedOn { get; set; }
        //public Nullable<System.DateTime> LastUpdatedOn { get; set; }
        //public long RefCreatedBy { get; set; }
        //public Nullable<long> RefLastUpdatedBy { get; set; }

        public byte[] RowVersion { get; set; }
        public virtual ContactMethod ContactMethod { get; set; }
        public virtual Region Region { get; set; }
    }
}
