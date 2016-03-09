using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using prgxResource = PRGX.SIMTrax.Domain.Resource.ApplicationLanguage.PRGXResource;

namespace PRGX.SIMTrax.ViewModel
{
    public class Campaign
    {
        public long CampaignId { get; set; }

        [Display(Name = Constants.CAMPAIGN_NAME_DISPLAY, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.CAMPAIGN_NAME_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string CampaignName { get; set; }

        [Display(Name = Constants.BUYER_ORG_DISPLAY, ResourceType = typeof(prgxResource))]
        public Nullable<long> BuyerId { get; set; }

        [Display(Name = Constants.NO_OF_SUPPLIERS_DISPLAY, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.NO_OF_SUPPLIERS_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public int SupplierCount { get; set; }

        [Display(Name = Constants.MASTER_VENDOR_DISPLAY, ResourceType = typeof(prgxResource))]
        public Nullable<int> MasterVendor { get; set; }

        [Display(Name = Constants.CAMPAIGN_URL_DISPLAY, ResourceType = typeof(prgxResource))]
        public string CampaignUrl { get; set; }

        [Display(Name = Constants.WELCOME_MESSAGE_DISPLAY, ResourceType = typeof(prgxResource))]
        public string WelcomeMessage { get; set; }
        public long? CampaignMessageId { get; set; }

        [Display(Name = Constants.VOUCHER_DISPLAY, ResourceType = typeof(prgxResource))]
        public Nullable<long> VoucherId { get; set; }

        [Display(Name = Constants.NOTE_DISPLAY, ResourceType = typeof(prgxResource))]
        public string Notes { get; set; }

        [Display(Name = Constants.DATA_SOURCE_DISPLAY, ResourceType = typeof(prgxResource))]
        public string DataSource { get; set; }

        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.CAMPAIGN_TYPE_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        [Display(Name = Constants.CAMPAIGN_TYPE_DISPLAY, ResourceType = typeof(prgxResource))]
        public CampaignType CampaignType { get; set; }

        //[Required(ErrorMessage = "Select Page Template")]
        [Display(Name = Constants.PAGE_TEMPLATE_DISPLAY, ResourceType = typeof(prgxResource))]
        public Nullable<CampaignLandingTemplate> TemplateType { get; set; }

        [Display(Name = Constants.EMAIL_CONTENT_DISPLAY, ResourceType = typeof(prgxResource))]
        public string EmailContent { get; set; }
        public long? EmailTemplateId { get; set; }

        [Display(Name = Constants.LOGO, ResourceType = typeof(prgxResource)), DataType(DataType.Upload)]
        public HttpPostedFileBase CampaignLogo { get; set; }

        public string CampaignLogoFilePath { get; set; }
        public long? CampaignLogoDocumentId { get; set; }
        public string CampaignLogoFileName { get; set; }

        public CampaignStatus CampaignStatus { get; set; }

        [Display(Name = Constants.BUYER_ORG_DISPLAY, ResourceType = typeof(prgxResource))]
        public string BuyerOrganisation { get; set; }
        //public string VoucherCode { get; set; }
        public Nullable<long> AuditorId { get; set; }
        public string AssignedToAuditor { get; set; }

        [Display(Name = Constants.PRE_REG_FILE_DISPLAY, ResourceType = typeof(prgxResource)), DataType(DataType.Upload)]
        public HttpPostedFileBase PreRegFile { get; set; }

        public DateTime? CreatedDate { get; set; }
        public bool IsDownloaded { get; set; }
        public List<SelectListItem> CampaignTypeList { get; set; }
        public List<SelectListItem> BuyerList { get; set; }
        public List<SelectListItem> VoucherList { get; set; }
        public List<SelectListItem> PageTemplateTypeList { get; set; }
        public string CampaignTypeString
        {
            get
            {
                return CommonMethods.Description(CampaignType);
            }
        }
        public string CampaignStatusString
        {
            get
            {
                return CommonMethods.Description(CampaignStatus);
            }
        }
        public string CreatedDateString
        {
            get
            {
                if (CreatedDate.HasValue)
                    return CreatedDate.Value.ToShortDateString().ToString(Thread.CurrentThread.CurrentUICulture.DateTimeFormat);
                else
                    return string.Empty;
            }
        }
        public int CampaignTypeInt
        {
            get
            {
                return (int)CampaignType;
            }
        }
        public int CampaignStatusInt
        {
            get
            {
                return (int)CampaignStatus;
            }
        }
        //public int TemplateTypeInt { get; set; }

    }
}
