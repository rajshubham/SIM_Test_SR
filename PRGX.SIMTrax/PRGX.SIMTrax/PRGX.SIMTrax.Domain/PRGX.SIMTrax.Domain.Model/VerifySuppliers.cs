using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.Domain.Model
{
    public class VerifySuppliers
    {
        [Display(Name = Constants.SUPPLIER_NAME)]
        public string SupplierName { get; set; }
        [Display(Name = Constants.SUPPLIER_ID)]
        public long SupplierId { get; set; }
        [Display(Name = Constants.DETAILS)]
        public string Details { get; set; }
        [Display(Name = Constants.DETAILS_VERIFIED_DATE)]
        public DateTime? DetailsDate { get; set; }
        [Display(Name = Constants.PROFILE)]
        public string Profile { get; set; }
        [Display(Name = Constants.PROFILE_VERIFIED_DATE)]
        public DateTime? ProfileDate { get; set; }
        [Display(Name = Constants.SANCTION)]
        public string Sanction { get; set; }
        [Display(Name = Constants.SANCTION_VERIFIED_DATE)]
        public DateTime? SanctionDate { get; set; }
        //[Display(Name = "FIT")]
        //public string FIT { get; set; }
        //[Display(Name = "FIT {Verified Date}")]
        //public Nullable<DateTime> FITDate { get; set; }
        //[Display(Name = "H&S")]
        //public string HS { get; set; }
        //[Display(Name = "H&S {Verified Date}")]
        //public DateTime? HSDate { get; set; }
        //[Display(Name = "DS")]
        //public string DS { get; set; }
        //[Display(Name = "DS {Verified Date}")]
        //public DateTime? DSDate { get; set; }
        [Display(Name = Constants.LANDING_PAGE_REFERRER)]
        public string LandingPageReferrer { get; set; }
        [Display(Name = Constants.OTHER_REFERRER)]
        public string OtherReferrer { get; set; }
    }
}
