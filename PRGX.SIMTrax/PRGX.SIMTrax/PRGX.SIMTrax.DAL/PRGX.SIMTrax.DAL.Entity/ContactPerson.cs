using PRGX.SIMTrax.DAL.Entity.AuditModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class ContactPerson : AuditableEntityWithoutId
    {
        [Key, ForeignKey("Person")]
        public long Id { get; set; }
        public string JobTitle { get; set; }
        public Nullable<short> ContactType { get; set; }

        //public System.DateTime CreatedOn { get; set; }
        //public Nullable<System.DateTime> LastUpdatedOn { get; set; }
        //public long RefCreatedBy { get; set; }
        //public Nullable<long> RefLastUpdatedBy { get; set; }

        public byte[] RowVersion { get; set; }
        public virtual Person Person { get; set; }
    }
}
