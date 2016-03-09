using PRGX.SIMTrax.Domain.Util;
using System.ComponentModel.DataAnnotations;
using prgxResource = PRGX.SIMTrax.Domain.Resource.ApplicationLanguage.PRGXResource;

namespace PRGX.SIMTrax.ViewModel
{
    public class CampaignLogin
    {
        public long CampaignId { get; set; }
        [Display(Name = Constants.LOGIN_ID, ResourceType = typeof(prgxResource))]
        public string LoginId { get; set; }
        public long BuyerId { get; set; }
        public string BuyerOrganisationName { get; set; }
        [Display(Name = Constants.REGISTRATION_CODE, ResourceType = typeof(prgxResource))]
        public string Password { get; set; }
    }
}
