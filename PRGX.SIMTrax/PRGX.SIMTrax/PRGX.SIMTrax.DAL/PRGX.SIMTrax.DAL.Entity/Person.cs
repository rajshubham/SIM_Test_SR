using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class Person : AuditableEntityWithoutId
    {
        [Key, ForeignKey("Party")]
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string PersonType { get; set; }
        //public System.DateTime CreatedOn { get; set; }
        //public long RefCreatedBy { get; set; }
        //public Nullable<System.DateTime> LastUpdatedOn { get; set; }
        //public Nullable<long> RefLastUpdatedBy { get; set; }
        public byte[] RowVersion { get; set; }
        public virtual ContactPerson ContactPerson { get; set; }
        public virtual Party Party { get; set; }
        public virtual User User { get; set; }
    }
}
