using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using prgxResource = PRGX.SIMTrax.Domain.Resource.ApplicationLanguage.PRGXResource;

namespace PRGX.SIMTrax.ViewModel
{
    public class BuyerRegister
    {
        public long BuyerPartyId { get; set; }
        public long UserPartyId { get; set; }
        public long ContactPersonId { get; set; }

        [Display(Name = Constants.FIRST_NAME, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.FIRST_NAME_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string BuyerFirstName { get; set; }

        [Display(Name = Constants.LAST_NAME, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.LAST_NAME_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string BuyerLastName { get; set; }

        [EmailAddress(ErrorMessage = null, ErrorMessageResourceName = Constants.INVALID_EMAIL_ADDRESS, ErrorMessageResourceType = typeof(prgxResource))]
        [StringLength(50, MinimumLength = 5, ErrorMessage = null, ErrorMessageResourceName = Constants.EMAIL_ADDRESS_LENGTH, ErrorMessageResourceType = typeof(prgxResource))]
        [Display(Name = Constants.EMAIL, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.EMAIL_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string BuyerEmail { get; set; }

        [Display(Name = Constants.ORGANISATION_NAME_BUYER, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.ORGANISATION_NAME_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string BuyerOrganisationName { get; set; }

        [Display(Name = Constants.ADDRESS_LINE_1, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.ADDRESS_LINE_1_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string BuyerFirstAddressLine1 { get; set; }

        [Display(Name = Constants.ADDRESS_LINE_2, ResourceType = typeof(prgxResource))]
        public string BuyerFirstAddressLine2 { get; set; }

        [Display(Name = Constants.CITY, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.CITY_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string BuyerFirstAddressCity { get; set; }

        [Display(Name = Constants.COUNTY_STATE, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.COUNTY_STATE_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string BuyerFirstAddressState { get; set; }

        [Display(Name = Constants.COUNTRY, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.COUNTRY_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string BuyerFirstAddressCountry { get; set; }

        [Display(Name = Constants.POSTAL_ZIPCODE, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.POSTAL_ZIPCODE_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string BuyerFirstAddressPostalCode { get; set; }

        //[Display(Name = Constants.FIRST_NAME, ResourceType = typeof(prgxResource))]
        //[Required(ErrorMessage = null, ErrorMessageResourceName = Constants.FIRST_NAME_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        //public string BuyerContactFirstName { get; set; }

        //[Display(Name = Constants.LAST_NAME, ResourceType = typeof(prgxResource))]
        //[Required(ErrorMessage = null, ErrorMessageResourceName = Constants.LAST_NAME_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        //public string BuyerContactLastName { get; set; }

        //[EmailAddress(ErrorMessage = null, ErrorMessageResourceName = Constants.INVALID_EMAIL_ADDRESS, ErrorMessageResourceType = typeof(prgxResource))]
        //[StringLength(50, MinimumLength = 5, ErrorMessage = null, ErrorMessageResourceName = Constants.EMAIL_ADDRESS_LENGTH, ErrorMessageResourceType = typeof(prgxResource))]
        //[Display(Name = Constants.EMAIL, ResourceType = typeof(prgxResource))]
        //[Required(ErrorMessage = null, ErrorMessageResourceName = Constants.EMAIL_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        //public string BuyerContactEmail { get; set; }

        [Display(Name = Constants.TEL_NUMBER, ResourceType = typeof(prgxResource))]
        [StringLength(20, ErrorMessage = null, ErrorMessageResourceName = Constants.TEL_NUMBER_LENGTH, ErrorMessageResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.TEL_NUMBER_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        [RegularExpression("([0-9, ,+-]+)", ErrorMessage = null, ErrorMessageResourceName = Constants.INVALID_TEL_NUMBER, ErrorMessageResourceType = typeof(prgxResource))]
        public string BuyerTelephone { get; set; }

        [Display(Name = Constants.JOB_TITLE, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.JOB_TITLE_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string BuyerJobTitle { get; set; }

        [Display(Name = Constants.NUMBER_OF_EMPLOYEES, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.NUMBER_OF_EMPLOYEES_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public long BuyerNumberOfEmployees { get; set; }

        [Display(Name = Constants.TURNOVER_MESSAGE, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.TURNOVER_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public long BuyerTurnOver { get; set; }

        [Display(Name = Constants.SECTOR_DESCRIPTION_MESSAGE, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.INDUSTRY_SECTOR_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public long BuyerSector { get; set; }

        //public bool IsSave { get; set; }
        public List<ItemList> BuyerBusinessSectorList { get; set; }
        public string BusinessSectorDescription { get; set; }
        public List<SelectListItem> BuyerCountryList { get; set; }
        public List<SelectListItem> BuyerNoOfEmployeesList { get; set; }
        public List<SelectListItem> BuyerTurnOverList { get; set; }
        //public UserType BuyerUserType { get; set; }
        //public DateTime BuyerCreatedDate { get; set; }
        //public long BuyerCreatedBy { get; set; }
        //public long RefRegionId { get; set; }
    }
}
