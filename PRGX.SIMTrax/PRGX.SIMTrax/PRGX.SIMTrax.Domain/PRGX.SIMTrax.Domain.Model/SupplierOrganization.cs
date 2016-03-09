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
    public class SupplierOrganization
    {
        [Display(Name = Constants.SUPPLIER_ID, ResourceType = typeof(prgxResource))]
        public long SupplierId { get; set; }
        public long SupplierUserId { get; set; }
        [Display(Name = Constants.SUPPLIER_NAME, ResourceType = typeof(prgxResource))]
        public string SupplierOrganizationName { get; set; }
        [Display(Name = Constants.SIGN_UP, ResourceType = typeof(prgxResource))]
        public DateTime? SignUpDate { get; set; }
        [Display(Name = Constants.REGISTERED, ResourceType = typeof(prgxResource))]
        public DateTime? RegisteredDate { get; set; }
        [Display(Name = Constants.DETAILS_CHECK, ResourceType = typeof(prgxResource))]
        public DateTime? DetailsVerifiedDate { get; set; }
        [Display(Name = Constants.PROFILE_CHECK, ResourceType = typeof(prgxResource))]
        public DateTime? ProfileVerifiedDate { get; set; }
        public long? SupplierStatus { get; set; }
        [Display(Name = Constants.SOURCE, ResourceType = typeof(prgxResource))]
        public string ProjectSource { get; set; }
        [Display(Name = Constants.STATUS, ResourceType = typeof(prgxResource))]
        public string SupplierStatusString
        {
            get
            {
                if (SupplierStatus.HasValue)
                    return CommonMethods.Description((CompanyStatus)SupplierStatus.Value);
                else
                    return string.Empty;
            }
        }
        public string SignUpDateString
        {
            get
            {
                if (SignUpDate.HasValue)
                    return SignUpDate.Value.ToShortDateString().ToString(Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                else
                    return string.Empty;
            }
        }
        public string RegisteredDateString
        {
            get
            {
                if (RegisteredDate.HasValue)
                    return RegisteredDate.Value.ToShortDateString().ToString(Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                else
                    return string.Empty;
            }
        }
        public string DetailsVerifiedDateString
        {
            get
            {
                if (DetailsVerifiedDate.HasValue)
                    return DetailsVerifiedDate.Value.ToShortDateString().ToString(Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                else
                    return string.Empty;
            }
        }
        public string ProfileVerifiedDateString
        {
            get
            {
                if (ProfileVerifiedDate.HasValue)
                    return ProfileVerifiedDate.Value.ToShortDateString().ToString(Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                else
                    return string.Empty;
            }
        }

        public string PrimaryContactName { get; set; }
        public string PrimaryContactEmail { get; set; }
        [Display(Name = Constants.LANDING_PAGE_REFERRER)]
        public string LandingPageReferrer { get; set; }
        [Display(Name = Constants.OTHER_REFERRER)]
        public string OtherReferrer { get; set; }
        
        public List<SupplierReferrer> SupplierReferrers { get; set; }
    }

    public class SupplierReferrer
    {
        public long CampaignId { get; set; }
        public bool LandingReferrer { get; set; }
        public string CampaignName { get; set; }
        public long BuyerOrganizationId { get; set; }
        public string BuyerOrganizationName { get; set; }
        public bool IsSupplierReferred { get; set; }
    }
}
