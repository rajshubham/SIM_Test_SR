using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class PotentialRole
    {
        public long Id { get; set; }
        public long RefExistingRole { get; set; }
        public long RefPotentialRole { get; set; }
        public virtual Role Role { get; set; }
        public virtual Role Role1 { get; set; }
    }
}
