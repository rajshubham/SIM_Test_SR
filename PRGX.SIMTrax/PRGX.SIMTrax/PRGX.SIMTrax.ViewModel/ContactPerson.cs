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
    public class ContactPerson
    {
        public long Id { get; set; }
        [Display(Name = Constants.FIRST_NAME, ResourceType = typeof(prgxResource))]
        public string FirstName { get; set; }

        [Display(Name = Constants.LAST_NAME, ResourceType = typeof(prgxResource))]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = null, ErrorMessageResourceName = Constants.INVALID_EMAIL_ADDRESS, ErrorMessageResourceType = typeof(prgxResource))]
        [Display(Name = Constants.EMAIL, ResourceType = typeof(prgxResource))]
        [StringLength(50, MinimumLength = 5, ErrorMessage = null, ErrorMessageResourceName = Constants.EMAIL_ADDRESS_LENGTH, ErrorMessageResourceType = typeof(prgxResource))]
        public string Email { get; set; }

        [Display(Name = Constants.TEL_NUMBER, ResourceType = typeof(prgxResource))]
        [StringLength(20, ErrorMessage = null, ErrorMessageResourceName = Constants.TEL_NUMBER_LENGTH, ErrorMessageResourceType = typeof(prgxResource))]
        [RegularExpression("([0-9, ,+-]+)", ErrorMessage = null, ErrorMessageResourceName = Constants.INVALID_TEL_NUMBER, ErrorMessageResourceType = typeof(prgxResource))]
        public string Telephone { get; set; }

        [Display(Name = Constants.JOB_TITLE, ResourceType = typeof(prgxResource))]
        public string JobTitle { get; set; }

        public Nullable<short> ContactType { get; set; }
        public string Fax { get; set; }
        public long RefContactMethod { get; set; }
        public int AssignedCount { get; set; }
        public string MailingAddressValue { get; set; }
        public long SellerPartyId { get; set; }
        public long ContactPartyId { get; set; }
        public long RefEmail { get; set; }
        public long RefPhone { get; set; }
        public Nullable<long> RefAddressContactMethod { get; set; }
    }
}
