using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.Domain.Model
{
    public class AccessType
    {
        public long PotentialRoleId { get; set; }
        public string PotentialRoleName { get; set; }
        public string PotentialRoleDescription { get; set; }
        public long ExistingRoleId { get; set; }
        public string ExistingRoleName { get; set; }
    }
}
