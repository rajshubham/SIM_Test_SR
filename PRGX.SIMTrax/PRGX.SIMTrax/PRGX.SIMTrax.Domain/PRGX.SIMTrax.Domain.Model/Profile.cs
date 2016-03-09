using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using prgxResource = PRGX.SIMTrax.Domain.Resource.ApplicationLanguage.PRGXResource;


namespace PRGX.SIMTrax.Domain.Model
{
     public class Profile
    {
        public Profile()
        {
            this.AddressDetails = new List<AddressDetails>();
            this.ContactDetails = new List<ContactDetails>();
            this.BankDetails = new List<BankDetails>();
            this.ReferenceDetails = new List<ReferenceDetails>();
        }

        public long SellerPartyId { get; set; }
        public string CompanyName { get; set; }

        [Display(Name = "NO_OF_EMPLOYEES", ResourceType = typeof(prgxResource))]

        public string CompanySize { get; set; }
        [Display(Name = "COMPANY_TURNOVER", ResourceType = typeof(prgxResource))]
        public Nullable<long> TurnOverSize { get; set; }
        public string TurnOver { get; set; }
        public Nullable<long> BusinessSectorId { get; set; }
        
        [Display(Name = "BUSINESS_SECTOR", ResourceType = typeof(prgxResource))]
        public string BusinessSector { get; set; }
        
        [Display(Name = "COMPANY_TYPE", ResourceType = typeof(prgxResource))]
        public string TypeOfCompany { get; set; }
        
        public string WebsiteLink { get; set; }
        public string BusinessDescription { get; set; }
        
        [Display(Name = "TRADING_NAME", ResourceType = typeof(prgxResource))]
        public string TradingName { get; set; }
        
        [Display(Name = "ESTABLISHED_YEAR", ResourceType = typeof(prgxResource))]
        public string EstablishedYear { get; set; }
        
        [Display(Name = "FACEBOOK_ACCOUNT", ResourceType = typeof(prgxResource))]
        public string FacebookAccount { get; set; }
        [Display(Name = "TWITTER_ACCOUNT", ResourceType = typeof(prgxResource))]
        public string TwitterAccount { get; set; }
        [Display(Name = "LINKEDIN_ACCOUNT", ResourceType = typeof(prgxResource))]
        public string LinkeldInAccount { get; set; }
        [Display(Name = "MAX_CONTRACT", ResourceType = typeof(prgxResource))]
        public string MaxContractValue { get; set; }
        
        [Display(Name = "MIN_CONTRACT", ResourceType = typeof(prgxResource))]
        public string MinContractValue { get; set; }
        public string CompanyLogoString { get; set; }
        public Nullable<short> Status { get; set; }
        public virtual List<AddressDetails> AddressDetails { get; set; }

        [Display(Name = "SERVICE_IN", ResourceType = typeof(prgxResource))]
        
        public string CompanyService { get; set; }
        [Display(Name = "SUBSIDIARIES_IN", ResourceType = typeof(prgxResource))]
        public string CompanySubsidiaries { get; set; }
        
        [Display(Name = "CUSTOMER_SECTOR", ResourceType = typeof(prgxResource))]
        public string CustomerSectors { get; set; }

        public virtual List<ContactDetails> ContactDetails { get; set; }
        public virtual List<BankDetails> BankDetails { get; set; }
        public virtual List<ReferenceDetails> ReferenceDetails { get; set; }
        public bool IsBuyer { get; set; }
        [Display(Name = "SUPPLIER_ID", ResourceType = typeof(prgxResource))]
        public string SupplierId { get; set; }
        public bool IsFavouriteSupplier { get; set; }
        public bool IsTradingSupplier { get; set; }
        public string CurrencyCodeHtml { get; set; }
    }
    public class BankDetails
    {
        public long Id { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string SwiftCode { get; set; }
        public string IBAN { get; set; }
        public string BankName { get; set; }
        public string Address { get; set; }
        public string PreferredMode { get; set; }
        public string BranchIdentifier { get; set; }
        public string CountryName { get; set; }
    }

    public class ContactDetails
    {
        public long Id { get; set; }
        public string Name { get; set; }


         public string Email { get; set; }

        public string Telephone { get; set; }

        public string JobTitle { get; set; }
        public Nullable<short> ContactType { get; set; }


    }

    public class ReferenceDetails
    {

        public long Id { get; set; }
        public string ClientName { get; set; }
        public string ContactName { get; set; }
        public long RefReferee { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string MailingAddress { get; set; }
        public string Fax { get; set; }
        public Nullable<bool> CanWeContact { get; set; }
        public string ClientRole { get; set; }
    }

    public class AddressDetails
    {
        public long Id { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Zone { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public short AddressType { get; set; }
        public string AddressTypeValue { get; set; }
        public string CountryName { get; set; }
        
        
    }


}
