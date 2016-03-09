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
    public class ResetPassword
    {
        public long UserId { set; get; }
        [EmailAddress(ErrorMessage = null, ErrorMessageResourceName = Constants.INVALID_EMAIL_ADDRESS, ErrorMessageResourceType = typeof(prgxResource))]
        [StringLength(50, MinimumLength = 5, ErrorMessage = null, ErrorMessageResourceName = Constants.EMAIL_ADDRESS_LENGTH, ErrorMessageResourceType = typeof(prgxResource))]
        [Display(Name = Constants.EMAIL, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.EMAIL_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string Email { get; set; }

        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.PASSWORD_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        [DataType(DataType.Password)]
        [StringLength(15, MinimumLength = 6, ErrorMessage = null, ErrorMessageResourceName = Constants.PASSWORD_LENGTH, ErrorMessageResourceType = typeof(prgxResource))]
        [RegularExpression("((?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#*$%_!&]).{6,15})", ErrorMessage = null, ErrorMessageResourceName = Constants.PASSWORD_MESSAGE, ErrorMessageResourceType = typeof(prgxResource))]
        [Display(Name = Constants.PASSWORD_MESSAGE, ResourceType = typeof(prgxResource))]
        public string Password { get; set; }

        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.PASSWORD_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        [DataType(DataType.Password)]
        [Display(Name = Constants.CONFIRM_PASSWORD_MESSAGE, ResourceType = typeof(prgxResource))]
        [RegularExpression("((?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#*$%_!&]).{6,15})", ErrorMessage = null, ErrorMessageResourceName = Constants.PASSWORD_MESSAGE, ErrorMessageResourceType = typeof(prgxResource))]
        [Compare("Password", ErrorMessage = null, ErrorMessageResourceName = Constants.COMPARE_PASSWORD_AND_CONFIRM_PASSWORD_FAIL, ErrorMessageResourceType = typeof(prgxResource))]
        public string ConfirmPassword { get; set; }
    }
}
