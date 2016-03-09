using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prgxResource = PRGX.SIMTrax.Domain.Resource.ApplicationLanguage.PRGXResource;
using System.Web.Mvc;
using System.Threading;

namespace PRGX.SIMTrax.ViewModel
{
    public class Voucher
    {
        public long VoucherId { get; set; }

        [Display(Name = Constants.PROMOTION_CODE_DISPLAY, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.PROMOTION_CODE_VALIDATION, ErrorMessageResourceType = typeof(prgxResource))]
        public string PromotionalCode { get; set; }

        [Display(Name = Constants.DISCOUNT_PERCENT_DISPLAY, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.DISCOUNT_PERCENT_VALIDATION, ErrorMessageResourceType = typeof(prgxResource))]
        [Range(0, 100, ErrorMessage = null, ErrorMessageResourceName = Constants.DISCOUNT_PERCENT_RANGE, ErrorMessageResourceType = typeof(prgxResource))]
        public decimal DiscountPercent { get; set; }

        [Display(Name = Constants.PROMOTION_START_DATE_DISPLAY, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.PROMOTION_START_DATE_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public DateTime? PromotionStartDate { get; set; }

        [Display(Name = Constants.PROMOTION_END_DATE_DISPLAY, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.PROMOTION_END_DATE_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public DateTime? PromotionEndDate { get; set; }

        [Display(Name = Constants.MAP_BUYER_DISPLAY, ResourceType = typeof(prgxResource))]
        public bool MapBuyer { get; set; }

        [Display(Name = Constants.SELECT_BUYER_ERROR, ResourceType = typeof(prgxResource))]
        public long? BuyerPartyId { get; set; }
        
        public List<SelectListItem> BuyerList { get; set; }
        public string BuyerName { get; set; }
        public string FormattedPromotionStartDate
        {
            get
            {
                if (PromotionStartDate.HasValue)
                    return PromotionStartDate.Value.ToShortDateString().ToString(Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                else
                    return string.Empty;
            }
        }
        public string FormattedPromotionEndDate
        {
            get
            {
                if (PromotionEndDate.HasValue)
                    return PromotionEndDate.Value.ToShortDateString().ToString(Thread.CurrentThread.CurrentCulture.DateTimeFormat);
                else
                    return string.Empty;
            }
        }
    }
}
