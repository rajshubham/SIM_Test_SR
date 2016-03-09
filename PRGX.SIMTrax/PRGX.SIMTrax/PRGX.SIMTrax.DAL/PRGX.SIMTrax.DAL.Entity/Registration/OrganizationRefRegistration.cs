using PRGX.SIMTrax.DAL.Entity.AuditModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Entity.Registration
{
    [Table("Organization")]
    public class OrganizationRefRegistration : AuditableEntity<long>
    {
        public OrganizationRefRegistration()
        {
            this.Buyers = new List<Buyer>();
            this.Suppliers = new List<Supplier>();
        }
        
        public string OrganizationType { get; set; }
        public long RefParty { get; set; }
        public Nullable<long> RefLegalEntity { get; set; }
        public virtual ICollection<Buyer> Buyers { get; set; }
        public virtual LegalEntity LegalEntity { get; set; }
        public virtual PartyRefRegistration Party { get; set; }
        public virtual ICollection<Supplier> Suppliers { get; set; }
    }
}
