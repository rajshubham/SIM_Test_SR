using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.Domain.Model
{
    public class UserDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long UserId { get; set; }
        public long  RefPersonId { get; set; }
        public long RefPersonPartyId { get; set; }
        public string JobTitle { get; set; }
        
        public string LoginId { get; set; }
        public string OrganisationName { get; set; }
        public long RefOrganisationPartyId { get; set; }
        public string PrimaryFirstName { get; set; }
        public string PrimaryLastName { get; set; }
        public string PrimaryContactTelephone { get; set; }
        public string PrimaryContactJobTitle { get; set; }
        public string PrimaryContactEmail { get; set; }

        public string Password { get; set; }
        public bool NeedPasswordChange { get; set; }
        public long UserType { get; set; }
        public long PrimaryContactPartyId { get; set; }
        public bool IsActive { get; set; }
    }
}
