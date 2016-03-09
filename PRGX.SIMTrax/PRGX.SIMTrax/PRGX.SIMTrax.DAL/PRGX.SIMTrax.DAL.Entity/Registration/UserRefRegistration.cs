using PRGX.SIMTrax.DAL.Entity.AuditModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRGX.SIMTrax.DAL.Entity.Registration
{
    [Table("User")]
    public class UserRefRegistration :AuditableEntity<long>
    {
        public UserRefRegistration()
        {
            this.AcceptedTermsOfUses = new List<AcceptedTermsOfUse>();
            this.Credentials = new List<Credential>();
        }
        public bool NeedsPasswordChange { get; set; }
        public long UserType { get; set; }
        public long RefPerson { get; set; }
        public byte[] RowVersion { get; set; }
        public virtual ICollection<AcceptedTermsOfUse> AcceptedTermsOfUses { get; set; }
        public virtual ICollection<Credential> Credentials { get; set; }
        public virtual PersonRefRegistration Person { get; set; }
    }
}
