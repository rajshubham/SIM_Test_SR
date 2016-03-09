using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using prgxResource = PRGX.SIMTrax.Domain.Resource.ApplicationLanguage.PRGXResource;

namespace PRGX.SIMTrax.ViewModel
{
    public class AuditorUser
    {
        public long Id { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = Constants.PASSWORD_MESSAGE, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.PASSWORD_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = Constants.PASSWORD_MESSAGE, ResourceType = typeof(prgxResource))]
        [Compare("Password", ErrorMessage = null, ErrorMessageResourceName = Constants.COMPARE_PASSWORD_AND_CONFIRM_PASSWORD_FAIL, ErrorMessageResourceType = typeof(prgxResource))]
        public string ConfirmPassword { get; set; }


        [EmailAddress(ErrorMessage = null, ErrorMessageResourceName = Constants.INVALID_EMAIL_ADDRESS, ErrorMessageResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.EMAIL_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string SelectedRoles { get; set; }
        public UserType UserType { get; set; }
        public bool IsActive { get; set; }
        public long OrganizationPartyId { get; set; }
        public List<Role> Roles { get; set; }
        //public List<AuditorRoleVM> AuditorRoles { get; set; }
    }
}
