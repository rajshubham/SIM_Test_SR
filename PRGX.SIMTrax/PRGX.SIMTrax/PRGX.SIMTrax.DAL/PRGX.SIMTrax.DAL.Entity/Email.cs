using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class Email : BaseEntity<long>
    {
        //public long Id { get; set; }
        public string EmailAddress { get; set; }
        public long RefContactMethod { get; set; }
        public virtual ContactMethod ContactMethod { get; set; }
    }
}
