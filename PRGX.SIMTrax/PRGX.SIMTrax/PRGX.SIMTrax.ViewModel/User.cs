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
    public class User
    {
        [Display(Name = Constants.FIRST_NAME, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.FIRST_NAME_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string FirstName { get; set; }

        [Display(Name = Constants.LAST_NAME, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.LAST_NAME_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = null, ErrorMessageResourceName = Constants.INVALID_EMAIL_ADDRESS, ErrorMessageResourceType = typeof(prgxResource))]
        [StringLength(50, MinimumLength = 5, ErrorMessage = null, ErrorMessageResourceName = Constants.EMAIL_ADDRESS_LENGTH, ErrorMessageResourceType = typeof(prgxResource))]
        [Display(Name = Constants.EMAIL, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.EMAIL_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string Email { get; set; }

        [EmailAddress(ErrorMessage = null, ErrorMessageResourceName = Constants.INVALID_EMAIL_ADDRESS, ErrorMessageResourceType = typeof(prgxResource))]
        [StringLength(50, MinimumLength = 5, ErrorMessage = null, ErrorMessageResourceName = Constants.EMAIL_ADDRESS_LENGTH, ErrorMessageResourceType = typeof(prgxResource))]
        [Display(Name = Constants.EMAIL, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.EMAIL_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string LoginId { get; set; }

        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.PASSWORD_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        [DataType(DataType.Password)]
        [StringLength(15, MinimumLength = 6, ErrorMessage = null, ErrorMessageResourceName = Constants.PASSWORD_LENGTH, ErrorMessageResourceType = typeof(prgxResource))]
        [RegularExpression("((?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#*$%_!&]).{6,15})", ErrorMessage = null, ErrorMessageResourceName = Constants.PASSWORD_GUIDELINES, ErrorMessageResourceType = typeof(prgxResource))]
        [Display(Name = Constants.PASSWORD_MESSAGE, ResourceType = typeof(prgxResource))]
        public string Password { get; set; }

        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.PASSWORD_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        [DataType(DataType.Password)]
        [Display(Name = Constants.CONFIRM_PASSWORD_MESSAGE, ResourceType = typeof(prgxResource))]
        [RegularExpression("((?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#*$%_!&]).{6,15})", ErrorMessage = null, ErrorMessageResourceName = Constants.PASSWORD_GUIDELINES, ErrorMessageResourceType = typeof(prgxResource))]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = null, ErrorMessageResourceName = Constants.COMPARE_PASSWORD_AND_CONFIRM_PASSWORD_FAIL, ErrorMessageResourceType = typeof(prgxResource))]
        public string ConfirmPassword { get; set; }

        public bool IsAdminUser { get; set; }
        public UserType UserType { get; set; }
        public long UserId { get; set; }
        public string UserTypeDescription { get; set; }
        public System.DateTime CreatedDate { get; set; }

        [Display(Name = Constants.TELEPHONE, ResourceType = typeof(prgxResource))]
        [StringLength(20, ErrorMessage = null, ErrorMessageResourceName = Constants.TEL_NUMBER_LENGTH, ErrorMessageResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.TEL_NUMBER_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        [RegularExpression("([0-9, ,+-]+)", ErrorMessage = null, ErrorMessageResourceName = Constants.INVALID_TEL_NUMBER, ErrorMessageResourceType = typeof(prgxResource))]
        public string Telephone { get; set; }

        [EmailAddress(ErrorMessage = null, ErrorMessageResourceName = Constants.INVALID_EMAIL_ADDRESS, ErrorMessageResourceType = typeof(prgxResource))]
        [StringLength(50, MinimumLength = 5, ErrorMessage = null, ErrorMessageResourceName = Constants.EMAIL_ADDRESS_LENGTH, ErrorMessageResourceType = typeof(prgxResource))]
        [Display(Name = Constants.PRIMARY_EMAIL, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.EMAIL_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string PrimaryEmail { get; set; }

        [Display(Name = Constants.PRIMARY_FIRST_NAME, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.PRIMARY_FIRST_NAME_VALIDATION, ErrorMessageResourceType = typeof(prgxResource))]
        public string PrimaryFirstName { get; set; }

        [Display(Name = Constants.PRIMARY_LAST_NAME, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.PRIMARY_LAST_NAME_VALIDATION, ErrorMessageResourceType = typeof(prgxResource))]
        public string PrimaryLastName { get; set; }

        [Display(Name = Constants.JOB_TITLE, ResourceType = typeof(prgxResource))]
        public string JobTitle { get; set; }

        public Nullable<long> PrimaryContactPartyId { get; set; }
        public long OrganizationPartyId { get; set; }
        public bool IsActive { get; set; }
    }
}
