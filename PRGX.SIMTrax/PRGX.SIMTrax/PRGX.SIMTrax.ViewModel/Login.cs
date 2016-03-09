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
    public class Login
    {
        [EmailAddress(ErrorMessage = null, ErrorMessageResourceName = Constants.INVALID_EMAIL_ADDRESS, ErrorMessageResourceType = typeof(prgxResource))]
        [StringLength(50, MinimumLength = 5, ErrorMessage = null, ErrorMessageResourceName = Constants.EMAIL_ADDRESS_LENGTH, ErrorMessageResourceType = typeof(prgxResource))]
        [Display(Name = Constants.EMAIL, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.EMAIL_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string LoginId { get; set; }

        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.PASSWORD_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        [DataType(DataType.Password)]
        [Display(Name = Constants.PASSWORD_MESSAGE, ResourceType = typeof(prgxResource))]
        public string Password { get; set; }
    }
}