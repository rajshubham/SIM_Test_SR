using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using prgxResource = PRGX.SIMTrax.Domain.Resource.ApplicationLanguage.PRGXResource;

namespace PRGX.SIMTrax.Domain.Model
{
    public class BuyerOrganization
    {
        public long BuyerId { get; set; }
        public long BuyerOrganizationId { get; set; }
        public long BuyerPartyId { get; set; }
        [Display(Name = Constants.BUYER_ORGANISATION, ResourceType = typeof(prgxResource))]
        public string BuyerOrganizationName { get; set; }
        [Display(Name = Constants.REQUEST, ResourceType = typeof(prgxResource))]
        public DateTime? CreatedDate { get; set; }
        [Display(Name = Constants.VERIFIED, ResourceType = typeof(prgxResource))]
        public DateTime? VerifiedDate { get; set; }
        [Display(Name = Constants.ACTIVATED, ResourceType = typeof(prgxResource))]
        public DateTime? ActivatedDate { get; set; }
        public long BuyerRoleId { get; set; }
        [Display(Name = Constants.ACCESS, ResourceType = typeof(prgxResource))]
        public string BuyerRoleName { get; set; }
        public long? BuyerStatus { get; set; }
        public string PrimaryEmail { get; set; }
        public string PrimaryContact { get; set; }
        [Display(Name = Constants.TERMS_ACCEPTED, ResourceType = typeof(prgxResource))]
        public DateTime? TermsAcceptedDate { get; set; }
        [Display(Name = Constants.STATUS, ResourceType = typeof(prgxResource))]
        public string BuyerStatusString
        {
            get
            {
                if (VerifiedDate == null)
                {
                    return "Registered";
                }

                else if (VerifiedDate != null && ActivatedDate == null)
                {
                    return "Verified";
                }
                else if (ActivatedDate != null)
                {
                    return "Activated";
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string CreatedDateString
        {
            get
            {
                if (CreatedDate.HasValue)
                    return CreatedDate.Value.ToShortDateString().ToString(Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                else
                    return string.Empty;
            }
        }
        public string VerifiedDateString
        {
            get
            {
                if (VerifiedDate.HasValue)
                    return VerifiedDate.Value.ToShortDateString().ToString(Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                else
                    return string.Empty;
            }
        }
        public string ActivatedDateString
        {
            get
            {
                if (ActivatedDate.HasValue)
                    return ActivatedDate.Value.ToShortDateString().ToString(Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                else
                    return string.Empty;
            }
        }
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
