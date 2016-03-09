using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

using prgxResource = PRGX.SIMTrax.Domain.Resource.ApplicationLanguage.PRGXResource;

namespace PRGX.SIMTrax.ViewModel
{
    public class SellerRegister
    {

        public SellerRegister()
        {
            this.BusinessSectorList = new List<ItemList>();
            this.GeoGraphicSalesList = new List<SelectListItem>();
            this.GeoGraphicSuppList = new List<SelectListItem>();
            this.CompanyTypeList = new List<SelectListItem>();
            this.NoOfEmployeesList = new List<SelectListItem>();
            this.TurnOverList = new List<SelectListItem>();
            this.CountryList = new List<SelectListItem>();
            this.CustomerSectorList = new List<SelectListItem>();
            this.ContactDetails = new List<ContactDetails>();
            this.CompanyIndustryCodes = new List<ItemList>();
            this.CompanyServiceGeoRegions = new List<ItemList>();
            this.CompanySalesGeoRegions = new List<ItemList>();
        }
        public long SellerPartyId { get; set; }
        public long UserPartyId { get; set; }

        public bool IsCompanyDetailsSubmitted { get; set; }

        public short Source { get; set; }

        [Display(Name = Constants.FIRST_NAME, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.FIRST_NAME_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string FirstName { get; set; }

        [Display(Name = Constants.LAST_NAME, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.LAST_NAME_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string LastName { get; set; }


        [EmailAddress(ErrorMessage = null, ErrorMessageResourceName = Constants.INVALID_EMAIL_ADDRESS, ErrorMessageResourceType = typeof(prgxResource))]
        [StringLength(50, MinimumLength = 5, ErrorMessage = null, ErrorMessageResourceName = Constants.EMAIL_ADDRESS_LENGTH, ErrorMessageResourceType = typeof(prgxResource))]
        [Display(Name = Constants.EMAIL, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.EMAIL_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string Email { get; set; }


        [Display(Name = Constants.ORGANISATION_NAME, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.ORGANISATION_NAME_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string OrganisationName { get; set; }


        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.ORGANISATION_NAME_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string DummyOrganisationName { get; set; }

        [Display(Name = Constants.LOGO, ResourceType = typeof(prgxResource))]
        public HttpPostedFileBase logo { get; set; }

        public string LogoFilePath { get; set; }
        public long LogoDocumentId { get; set; }
        public string LogoFileName { get; set; }

        //[Display(Name = Constants.COMPANY_TYPE, ResourceType = typeof(prgxResource))]
        //[Required(ErrorMessage = null, ErrorMessageResourceName = Constants.COMPANY_TYPE_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        //public CompanyType CompanyType { get; set; }

        public long PrimaryAddressId { get; set; }

        [Display(Name = Constants.ADDRESS_LINE_1, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.ADDRESS_LINE_1_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string FirstAddressLine1 { get; set; }

        [Display(Name = Constants.ADDRESS_LINE_2, ResourceType = typeof(prgxResource))]
        public string FirstAddressLine2 { get; set; }

        [Display(Name = Constants.CITY, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.CITY_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string FirstAddressCity { get; set; }

        [Display(Name = Constants.COUNTY_STATE, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.COUNTY_STATE_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string FirstAddressState { get; set; }

        [Display(Name = Constants.COUNTRY, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.COUNTRY_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string FirstAddressCountry { get; set; }

        [Display(Name = Constants.POSTAL_ZIPCODE, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.POSTAL_ZIPCODE_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string FirstAddressPostalCode { get; set; }



        [Display(Name = Constants.NUMBER_OF_EMPLOYEES, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.NUMBER_OF_EMPLOYEES_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public long NumberOfEmployees { get; set; }

        [Display(Name = Constants.TURNOVER_MESSAGE, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.TURNOVER_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public long TurnOver { get; set; }

        [Display(Name = Constants.SECTOR_DESCRIPTION_MESSAGE, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.SECTOR_DESCRIPTION_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public long sector { get; set; }
        public string BusinessSectorDescription { get; set; }


        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }

        [Display(Name = Constants.CUSTOMER_SECTOR_LIST, ResourceType = typeof(prgxResource))]
        public List<SelectListItem> CustomerSectorList { get; set; }

        [Display(Name = Constants.GEOGRAPHIC_SALES_LIST, ResourceType = typeof(prgxResource))]
        public List<SelectListItem> GeoGraphicSalesList { get; set; }

        [Display(Name = Constants.GEOGRAPHIC_SUPPLIERS_LIST, ResourceType = typeof(prgxResource))]
        public List<SelectListItem> GeoGraphicSuppList { get; set; }
        public List<SelectListItem> CompanyTypeList { get; set; }

        [Display(Name = Constants.BUSINESS_DESCRIPTION, ResourceType = typeof(prgxResource))]
        [StringLength(1200)]
        public string BusinessDescription { get; set; }

        public long RegisteredAddressId { get; set; }

        [Display(Name = Constants.ADDRESS_LINE_1, ResourceType = typeof(prgxResource))]
        public string SecondAddressLine1 { get; set; }

        [Display(Name = Constants.ADDRESS_LINE_2, ResourceType = typeof(prgxResource))]
        public string SecondAddressLine2 { get; set; }

        [Display(Name = Constants.CITY, ResourceType = typeof(prgxResource))]
        public string SecondAddressCity { get; set; }

        [Display(Name = Constants.COUNTY_STATE, ResourceType = typeof(prgxResource))]
        public string SecondAddressState { get; set; }

        [Display(Name = Constants.COUNTRY, ResourceType = typeof(prgxResource))]
        public string SecondAddressCountry { get; set; }

        [Display(Name = Constants.POSTAL_ZIPCODE, ResourceType = typeof(prgxResource))]
        public string SecondAddressPostalCode { get; set; }

        [Display(Name = Constants.REM_ADDRESS_DIFFERENT, ResourceType = typeof(prgxResource))]
        [Required]
        public bool IsRemittanceAddressDifferent { get; set; }
        public long RemittanceAddressId { get; set; }

        [Display(Name = Constants.ADDRESS_LINE_1, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.ADDRESS_LINE_1_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string RemittanceAddressLine1 { get; set; }

        [Display(Name = Constants.ADDRESS_LINE_2, ResourceType = typeof(prgxResource))]
        public string RemittanceAddressLine2 { get; set; }

        [Display(Name = Constants.CITY, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.CITY_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string RemittanceAddressCity { get; set; }

        [Display(Name = Constants.COUNTY_STATE, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.COUNTY_STATE_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string RemittanceAddressState { get; set; }

        [Display(Name = Constants.COUNTRY, ResourceType = typeof(prgxResource))]
        public string RemittanceAddressCountry { get; set; }

        [Display(Name = Constants.POSTAL_ZIPCODE, ResourceType = typeof(prgxResource))]
        public string RemittanceAddressPostalCode { get; set; }

        [Display(Name = Constants.HQ_ADDRESS_DIFFERENT, ResourceType = typeof(prgxResource))]
        [Required]
        public bool IsHeadQuartersAddressDifferent { get; set; }
        public long HeadQuartersAddressId { get; set; }

        [Display(Name = Constants.ADDRESS_LINE_1, ResourceType = typeof(prgxResource))]
        public string HeadQuartersAddressLine1 { get; set; }

        [Display(Name = Constants.ADDRESS_LINE_2, ResourceType = typeof(prgxResource))]
        public string HeadQuartersAddressLine2 { get; set; }

        [Display(Name = Constants.CITY, ResourceType = typeof(prgxResource))]
        public string HeadQuartersAddressCity { get; set; }

        [Display(Name = Constants.COUNTY_STATE, ResourceType = typeof(prgxResource))]
        public string HeadQuartersAddressState { get; set; }

        [Display(Name = Constants.COUNTRY, ResourceType = typeof(prgxResource))]
        public string HeadQuartersAddressCountry { get; set; }

        [Display(Name = Constants.POSTAL_ZIPCODE, ResourceType = typeof(prgxResource))]
        public string HeadQuartersAddressPostalCode { get; set; }

        [Display(Name = Constants.WEBSITE_LINK, ResourceType = typeof(prgxResource))]
        [RegularExpression("([a-zA-Z 0-9 :/.&'-]+[.]+[a-zA-Z 0-9.&'-]+)", ErrorMessage = null, ErrorMessageResourceName = Constants.WEBSITE_LINK_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string WebsiteLink { get; set; }

        [Display(Name = Constants.REG_ADDRESS_DIFFERENT, ResourceType = typeof(prgxResource))]
        [Required]
        public bool IsAddressDifferent { get; set; }

        [Display(Name = Constants.HAVE_DUNS, ResourceType = typeof(prgxResource))]
        [Required]
        public bool HaveDuns { get; set; }

        [Display(Name = Constants.DUNS_NUMBER, ResourceType = typeof(prgxResource))]
        public string DUNSNumber { get; set; }

        [Display(Name = Constants.IS_VAT, ResourceType = typeof(prgxResource))]
        [Required]
        public bool IsVAT { get; set; }

        [Display(Name = Constants.VAT_NUMBER_MESSAGE, ResourceType = typeof(prgxResource))]
        public string VATNumber { get; set; }


        [Display(Name = Constants.TYPE_OF_COMPANY, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.TYPE_OF_COMPANY_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public int TypeOfCompany { get; set; }

        [Display(Name = Constants.TRADING, ResourceType = typeof(prgxResource))]
        public string Trading { get; set; }

        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.PASSWORD_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        [DataType(DataType.Password)]
        [StringLength(15, MinimumLength = 6, ErrorMessage = null, ErrorMessageResourceName = Constants.PASSWORD_LENGTH, ErrorMessageResourceType = typeof(prgxResource))]
        [RegularExpression("((?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#*$%_!&]).{6,15})", ErrorMessage = null, ErrorMessageResourceName = Constants.INVALID_PASSWORD_FORMAT, ErrorMessageResourceType = typeof(prgxResource))]
        [Display(Name = Constants.PASSWORD_MESSAGE, ResourceType = typeof(prgxResource))]
        public string Password { get; set; }

        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.PASSWORD_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        [DataType(DataType.Password)]
        [Display(Name = Constants.CONFIRM_PASSWORD_MESSAGE, ResourceType = typeof(prgxResource))]
        [RegularExpression("((?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#*$%_!&]).{6,15})", ErrorMessage = null, ErrorMessageResourceName = Constants.INVALID_PASSWORD_FORMAT, ErrorMessageResourceType = typeof(prgxResource))]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = null, ErrorMessageResourceName = Constants.COMPARE_PASSWORD_AND_CONFIRM_PASSWORD_FAIL, ErrorMessageResourceType = typeof(prgxResource))]
        public string ConfirmPassword { get; set; }

        [Display(Name = Constants.IS_EMPLOYEE_OF_COMPANY, ResourceType = typeof(prgxResource))]
        public bool IsEmplyeeOfCompany { get; set; }

        [Required]
        public bool IsAgreeOnTerms { get; set; }

        [Display(Name = Constants.IS_SUBSIDIARY_STATUS, ResourceType = typeof(prgxResource))]
        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.IS_SUBSIDIARY_STATUS_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public bool IsSubsidaryStatus { get; set; }

        [Display(Name = Constants.ULTIMATE_PARENT, ResourceType = typeof(prgxResource))]
        public string UltimateParent { get; set; }

        [Display(Name = Constants.PARENTS_DUNS_NUMBER, ResourceType = typeof(prgxResource))]
        public string ParentDunsNumber { get; set; }

        [Display(Name = Constants.COMPANY_REG_NUMBER, ResourceType = typeof(prgxResource))]
        public string CompanyRegistrationNumber { get; set; }

        [Required(ErrorMessage = null, ErrorMessageResourceName = Constants.COMPANY_YEAR_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        [Display(Name = Constants.COMPANY_YEAR, ResourceType = typeof(prgxResource))]
        public string CompanyYear { set; get; }

        [Display(Name = Constants.FIRM_STATUS, ResourceType = typeof(prgxResource))]
        public string FirmStatus { set; get; }

        [Display(Name = Constants.STATES_SELECTED, ResourceType = typeof(prgxResource))]
        public string StatesSelected { get; set; }


        public string SIC1 { get; set; }
        public string SIC2 { get; set; }
        public string SIC3 { get; set; }
        public string SIC4 { get; set; }
        public string SIC5 { get; set; }
        public bool IsSave { get; set; }
        public List<ItemList> BusinessSectorList { get; set; }
         public List<SelectListItem> CountryList { get; set; }
        public List<SelectListItem> NoOfEmployeesList { get; set; }
        public List<SelectListItem> TurnOverList { get; set; }
      


        public CompanyStatus Status { get; set; }
        [Display(Name = Constants.ORG_FACEBOOK_ACCOUNT, ResourceType = typeof(prgxResource))]
        public string OrganisationFacebookaccount { get; set; }

        [Display(Name = Constants.ORG_TWITTER_ACCOUNT, ResourceType = typeof(prgxResource))]
        public string OrganisationTwitteraccount { get; set; }

        [Display(Name = Constants.ORG_LINKEDIN_ACCOUNT, ResourceType = typeof(prgxResource))]
        public string OrganisationLinkedInaccount { get; set; }

        [Display(Name = Constants.MAX_CONTRACT_VALUE, ResourceType = typeof(prgxResource))]
        [RegularExpression("([0-9]+)", ErrorMessage = null, ErrorMessageResourceName = Constants.MAX_CONTRACT_VALUE_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string MaxContractValue { get; set; }

        [Display(Name = Constants.MIN_CONTRACT_VALUE, ResourceType = typeof(prgxResource))]
        [RegularExpression("([0-9]+)", ErrorMessage = null, ErrorMessageResourceName = Constants.MIN_CONTRACT_VALUE_ERROR, ErrorMessageResourceType = typeof(prgxResource))]
        public string MinContractValue { get; set; }
        public long RefRegionId { get; set; }
        public long TermsOfUseId { get; set; }
        public bool IsPreRegistered { get; set; }
        public List<ItemList> IdentifierTypeList { get; set; }
        public List<ContactDetails> ContactDetails { get; set; }
        [Display(Name = Constants.JOB_TITLE, ResourceType = typeof(prgxResource))]
        public string JobTitle { get; set; }

        public List<ItemList> CompanyIndustryCodes { get; set; }
        public List<ItemList> CompanyServiceGeoRegions { get; set; }
        public List<ItemList> CompanySalesGeoRegions { get; set; }
    }
    public class ContactDetails
    {
        public ContactType ContactType { set; get; }
        public long ContactId { set; get; }

        [Display(Name = Constants.FIRST_NAME, ResourceType = typeof(prgxResource))]
        public string FirstName { get; set; }

        [Display(Name = Constants.LAST_NAME, ResourceType = typeof(prgxResource))]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = null, ErrorMessageResourceName = Constants.INVALID_EMAIL_ADDRESS, ErrorMessageResourceType = typeof(prgxResource))]
        [Display(Name = Constants.EMAIL, ResourceType = typeof(prgxResource))]
        [StringLength(50, MinimumLength = 5, ErrorMessage = null, ErrorMessageResourceName = Constants.EMAIL_ADDRESS_LENGTH, ErrorMessageResourceType = typeof(prgxResource))]
        public string  Email { get; set; }

        [Display(Name = Constants.TEL_NUMBER, ResourceType = typeof(prgxResource))]
        [StringLength(20, ErrorMessage = null, ErrorMessageResourceName = Constants.TEL_NUMBER_LENGTH, ErrorMessageResourceType = typeof(prgxResource))]
        [RegularExpression("([0-9, ,+-]+)", ErrorMessage = null, ErrorMessageResourceName = Constants.INVALID_TEL_NUMBER, ErrorMessageResourceType = typeof(prgxResource))]
        public string Telephone { get; set; }

        [Display(Name = Constants.JOB_TITLE, ResourceType = typeof(prgxResource))]
        public string JobTitle { get; set; }
    }


}