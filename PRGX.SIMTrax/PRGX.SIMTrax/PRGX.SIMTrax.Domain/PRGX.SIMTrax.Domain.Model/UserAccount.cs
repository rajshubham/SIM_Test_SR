using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using prgxResource = PRGX.SIMTrax.Domain.Resource.ApplicationLanguage.PRGXResource;

namespace PRGX.SIMTrax.Domain.Model
{
    public class UserAccount
    {
        public long UserId { get; set; }
        [Display(Name = Constants.USER_NAME, ResourceType = typeof(prgxResource))]
        public string UserName { get; set; }
        [Display(Name = Constants.LOGIN_ID_PLACEHOLDER, ResourceType = typeof(prgxResource))]
        public string LoginId { get; set; }
        [Display(Name = Constants.USER_TYPE, ResourceType = typeof(prgxResource))]
        public string UserType { get; set; }
        [Display(Name = Constants.LATEST_TERMS_VERSION, ResourceType = typeof(prgxResource))]
        public string LatestTermsVersion { get; set; }
        [Display(Name = Constants.LAST_LOGIN_DATE, ResourceType = typeof(prgxResource))]
        public DateTime? LastLogin { get; set; }
        public string LastLoginString {
            get
            {
                if (LastLogin.HasValue)
                    return LastLogin.Value.ToShortDateString().ToString(Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                else
                    return string.Empty;
            }
        }
        [Display(Name = Constants.SOURCE, ResourceType = typeof(prgxResource))]
        public string ProjectSource { get; set; }
        [Display(Name = Constants.STATUS, ResourceType = typeof(prgxResource))]
        public string ActiveStatus { get; set; }
        [Display(Name = Constants.ORIGINAL_TERMS_VERSION, ResourceType = typeof(prgxResource))]
        public string OriginalTermsVersion { get; set; }
        [Display(Name = Constants.LATEST_TERMS_ACCEPTED_DATE, ResourceType = typeof(prgxResource))]
        public DateTime? TermsAcceptedDate { get; set; }
        public string TermsAcceptedDateString
        {
            get
            {
                if (TermsAcceptedDate.HasValue)
                    return TermsAcceptedDate.Value.ToShortDateString().ToString(Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                else
                    return string.Empty;
            }
        }
    }
}
