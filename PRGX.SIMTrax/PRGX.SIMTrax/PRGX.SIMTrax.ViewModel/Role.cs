using PRGX.SIMTrax.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.ViewModel
{
    public class Role
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public long ExistingRoleId { get; set; }
        public List<RolePermission> RolePermissions { get; set; }
        public List<RolePermission> UserPermission { get; set; }
        public List<RolePermission> QuestionnairePermission { get; set; }
        public List<RolePermission> BuyerPermission { get; set; }
        public List<RolePermission> SupplierPermission { get; set; }
        public List<RolePermission> FinancePermission { get; set; }
        public List<RolePermission> NavigatePermission { get; set; }
    }
}
