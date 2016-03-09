using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.Domain.Model
{
    public class CampaignReleaseSupplier
    {
        [Display(Name = Constants.SUPPLIER_COMPANY_NAME)]
        public string SupplierName { get; set; }
        [Display(Name = Constants.LOGIN_ID)]
        public string LoginId { get; set; }
        [Display(Name = Constants.LANDING_PAGE_URL)]
        public string CampaignUrl { get; set; }
        [Display(Name = Constants.REGISTRATION_CODE)]
        public string RandomPasswordString { get; set; }
        [Display(Name = Constants.BUYER_ORGANISATION)]
        public string BuyerOrganisation { get; set; }
    }
}
