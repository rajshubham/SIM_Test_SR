using PRGX.SIMTrax.DAL.Entity.AuditModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Entity.Registration
{
    [Table("Person")]
    public class PersonRefRegistration : AuditableEntity<long>
    {
        public PersonRefRegistration()
        {
            this.UserRefRegistrations = new List<UserRefRegistration>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string PersonType { get; set; }
        public long RefParty { get; set; }
        public byte[] RowVersion { get; set; }
        public virtual PartyRefRegistration Party { get; set; }
        public virtual ICollection<UserRefRegistration> UserRefRegistrations { get; set; }
    }
}
