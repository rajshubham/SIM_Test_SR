using PRGX.SIMTrax.DAL.Entity.AuditModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Entity.Registration
{
    [Table("Party")]
    public class PartyRefRegistration : AuditableEntity<long>
    {
        public PartyRefRegistration()
        {
            this.OrganizationRefRegistrations = new List<OrganizationRefRegistration>();
            this.PartyPartyLinks = new List<PartyPartyLink>();
            this.PartyPartyLinks1 = new List<PartyPartyLink>();
            this.PeopleRefRegistration = new List<PersonRefRegistration>();
        }
        
        public string PartyName { get; set; }
        public string PartyType { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public byte[] RowVersion { get; set; }
        public virtual ICollection<OrganizationRefRegistration> OrganizationRefRegistrations { get; set; }
        public virtual ICollection<PartyPartyLink> PartyPartyLinks { get; set; }
        public virtual ICollection<PartyPartyLink> PartyPartyLinks1 { get; set; }
        public virtual ICollection<PersonRefRegistration> PeopleRefRegistration { get; set; }
    }
}
