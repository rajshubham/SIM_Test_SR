using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.Domain.Model
{
    public class CampaignPreRegSupplier
    {
        public long PreRegSupplierId { get; set; }
        public long CampaignId { get; set; }
        [Display(Name = Constants.SUPPLIER_COMPANY_NAME)]
        public string SupplierName { get; set; }
        [Display(Name = Constants.LOGIN_ID)]
        public string LoginId { get; set; }
        public string Password { get; set; }
        public bool IsValid { get; set; }
        [Display(Name = Constants.INVALID_COMMENTS)]
        public string InvalidSupplierComments { get; set; }
        [Display(Name = Constants.FIRST_NAME)]
        public string FirstName { get; set; }
        [Display(Name = Constants.LAST_NAME)]
        public string LastName { get; set; }
        [Display(Name = Constants.JOB_TITLE)]
        public string JobTitle { get; set; }
        [Display(Name = Constants.ADDRESS_LINE_1)]
        public string AddressLine1 { get; set; }
        [Display(Name = Constants.ADDRESS_LINE_2)]
        public string AddressLine2 { get; set; }
        [Display(Name = Constants.CITY)]
        public string City { get; set; }
        [Display(Name = Constants.COUNTY_STATE)]
        public string State { get; set; }
        [Display(Name = Constants.COUNTRY)]
        public string CountryName { get; set; }
        public long Country { get; set; }
        [Display(Name = Constants.POSTAL_ZIPCODE)]
        public string ZipCode { get; set; }
        [Display(Name = Constants.TELEPHONE)]
        public string Telephone { get; set; }
        [Display(Name = Constants.REGISTRATION_NUMBER)]
        public string RegistrationNumber { get; set; }
        [Display(Name = Constants.VAT_NUMBER)]
        public string VatNumber { get; set; }
        [Display(Name = Constants.DUNS_NUMBER_STRING)]
        public string DunsNumber { get; set; }
        [Display(Name = Constants.PARENT)]
        public string UltimateParent { get; set; }
        public bool IsSubsidary { get; set; }
        public bool IsRegistered { get; set; }
        //[Display(Name = "FIT")]
        //public Nullable<bool> IsFITMappedToBuyer { get; set; }
        //[Display(Name = "HS")]
        //public Nullable<bool> IsHSMappedToBuyer { get; set; }
        //[Display(Name = "DS")]
        //public Nullable<bool> IsDSMappedToBuyer { get; set; }
        public string CampaignURL { get; set; }
        public string BuyerOrganization { get; set; }
        public string EmailContent { get; set; }
    }
}
