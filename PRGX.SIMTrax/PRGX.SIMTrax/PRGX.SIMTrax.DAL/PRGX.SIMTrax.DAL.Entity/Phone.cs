using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class Phone:BaseEntity<long>
    { 
    //    public long Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Type { get; set; }
        public long RefContactMethod { get; set; }
        public virtual ContactMethod ContactMethod { get; set; }
    }
}
