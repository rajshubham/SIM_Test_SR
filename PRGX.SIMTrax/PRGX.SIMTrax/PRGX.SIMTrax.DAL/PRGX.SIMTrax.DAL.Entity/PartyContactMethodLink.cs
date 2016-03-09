using PRGX.SIMTrax.DAL.Entity.AuditModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Entity
{
    public class PartyContactMethodLink : BaseEntity<long>
    {
        public long RefParty { get; set; }
        public long RefContactMethod { get; set; }
        public virtual ContactMethod ContactMethod { get; set; }
        public virtual Party Party { get; set; }

    }
}
