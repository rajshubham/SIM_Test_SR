using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.Domain.Model
{
    public class RolePermission
    {
        public long PermissionId { get; set; }
        public string Description { get; set; }
        public bool IsChecked { get; set; }
        public long RoleId { get; set; }
        public long Id { get; set; }
    }
}
