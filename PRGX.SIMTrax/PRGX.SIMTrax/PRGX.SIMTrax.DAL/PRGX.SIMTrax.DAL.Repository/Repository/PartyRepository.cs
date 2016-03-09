using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Abstract;
using PRGX.SIMTrax.DAL.Repository.Context;
using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PRGX.SIMTrax.DAL.Repository.Repository
{
    public class PartyRepository : GenericRepository<Party>, IPartyRepository
    {
        public PartyRepository(DbContext context) : base(context) { }

        public bool IsOrganisationExists(string organisationName)
        {
            try
            {
                Logger.Info("PartyRepository : IsOrganisationExists() : Enter the method");

                var result = false;
                var organisation = this.All().AsQueryable().FirstOrDefault(v => v.PartyName.ToLower() == organisationName.ToLower() && v.PartyType.Trim() == Constants.PARTY_TYPE_ORGANIZATION);
                if (organisation != null)
                {
                    result = true;
                }
                Logger.Info("PartyRepository : IsOrganisationExists() : Exit the method");

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyRepository : IsOrganisationExists() : Caught an exception" + ex);
                throw;
            }
        }

        public Party GetCompanyDetailsByPartyId(long organizationPartyId)
        {
            try
            {
                Party details = null;
                Logger.Info("PartyRepository : GetCompanyDetailsByPartyId() : Enter the method");
                details = All().Include("Organization").Include("Organization.Supplier").Include("PartyIdentifiers").Include("PartyIdentifiers.PartyIdentifierType").FirstOrDefault(v => v.Id == organizationPartyId && v.PartyType.Trim() == Constants.PARTY_TYPE_ORGANIZATION);
                Logger.Info("PartyRepository : GetCompanyDetailsByPartyId() : Exit the method");
                return details;
            }
            catch (Exception ex)
            {

                Logger.Error("PartyRepository : GetCompanyDetailsByPartyId() : Caught an exception" + ex);
                throw;
            }
        }
        public Party GetCapabilityDetailsByPartyId(long organizationPartyId)
        {
            try
            {
                Party details = null;
                Logger.Info("PartyRepository : GetCapabilityDetailsByPartyId() : Enter the method");
                details = All().Include("Organization").Include("Organization.Supplier").Include("Organization.IndustryCodeOrganizationLinks")
                    .Include("Organization.IndustryCodeOrganizationLinks.IndustryCode").Include("PartyRegionLinks").Include("PartyRegionLinks.Region").FirstOrDefault(v => v.Id == organizationPartyId && v.PartyType.Trim() == Constants.PARTY_TYPE_ORGANIZATION);
                Logger.Info("PartyRepository : GetCapabilityDetailsByPartyId() : Exit the method");
                return details;
            }
            catch (Exception ex)
            {

                Logger.Error("PartyRepository : GetCapabilityDetailsByPartyId() : Caught an exception" + ex);
                throw;
            }
        }
        public Party GetMarketingDetailsByPartyId(long organizationPartyId)
        {
            try
            {
                Party details = null;
                Logger.Info("PartyRepository : GetMarketingDetailsByPartyId() : Enter the method");
                details = All().Include("Organization").Include("Organization.Supplier").Include("Organization.Document").FirstOrDefault(v => v.Id == organizationPartyId && v.PartyType.Trim() == Constants.PARTY_TYPE_ORGANIZATION);
                Logger.Info("PartyRepository : GetMarketingDetailsByPartyId() : Exit the method");
                return details;
            }
            catch (Exception ex)
            {

                Logger.Error("PartyRepository : GetMarketingDetailsByPartyId() : Caught an exception" + ex);
                throw;
            }
        }
        

        public List<long> GetIndustryCodesByOrganisationPartyId(long sellerPartyId)
        {
            try
            {
                List<long> industryCodes = new List<long>();
                Logger.Info("PartyRepository : GetIndustryCodesByOrganisationPartyId() : Enter the method");
                var party = All().Include("Organization").Include("Organization.IndustryCodeOrganizationLinks").FirstOrDefault(v => v.Id == sellerPartyId && v.PartyType.Trim() == Constants.PARTY_TYPE_ORGANIZATION);
                if (party != null)
                {
                    industryCodes = (party.Organization != null) ?
                        party.Organization.IndustryCodeOrganizationLinks.Select(v => v.RefIndustryCode).ToList()
                        : new List<long>();
                }
                Logger.Info("PartyRepository : GetIndustryCodesByOrganisationPartyId() : Exit the method");
                return industryCodes;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyRepository : GetIndustryCodesByOrganisationPartyId() : Caught an exception" + ex);
                throw;
            }
        }

        public Party GetSellerOrganizationDetailsByPartyId(long organizationPartyId)
        {
            try
            {
                Party details = null;
                Logger.Info("PartyRepository : GetSellerOrganizationDetailsByPartyId() : Enter the method");

                details = All().Include("Organization").Include("Organization.Supplier").Include("Organization.IndustryCodeOrganizationLinks")
                    .Include("Organization.IndustryCodeOrganizationLinks.IndustryCode").Include("PartyRegionLinks")
                    .Include("PartyRegionLinks.Region").Include("PartyContactMethodLinks").Include("PartyContactMethodLinks.ContactMethod").Include("PartyContactMethodLinks.ContactMethod.Addresses").Include("PartyIdentifiers").Include("PartyIdentifiers.PartyIdentifierType")
                    //.Include("ContactMethods.Emails").Include("ContactMethods.Phones").Include("ContactMethods.Phones.ContactPersons").Include("ContactMethods.Emails.ContactPersons")
                    .FirstOrDefault(v => v.Id == organizationPartyId && v.PartyType.Trim() == Constants.PARTY_TYPE_ORGANIZATION);

                Logger.Info("PartyRepository : GetSellerOrganizationDetailsByPartyId() : Exit the method");
                return details;
            }
            catch (Exception ex)
            {

                Logger.Error("PartyRepository : GetSellerOrganizationDetailsByPartyId() : Caught an exception" + ex);
                throw;
            }
        }

        public Profile SellerProfileDetails(long sellerPartyId, long buyerPartyId,long buyerUserPartyId)
        {
            try
            {
                Logger.Info("PartyRepository : SellerProfileDetails() : Enter the method");
                var companyDetails = new Profile();
                using (var ctx = new SellerContext())
                {
                    var party = ctx.Parties.FirstOrDefault(v => v.Id == sellerPartyId);
                    if (party != null)
                    {
                        companyDetails.SupplierId = "SIM - " + party.Id;
                        companyDetails.SellerPartyId = party.Id;
                        companyDetails.CompanyName = party.PartyName;
                        var organisation = party.Organization;
                        var mnemonics = new string[] { Constants.ORG_EMP_BAND, Constants.ORG_TURNOVER, Constants.ORG_BIZ_SECT, Constants.TYPE_COMPANY };
                        var masterDataList = ctx.MasterDatas.Include("MasterDataType").Where(v => mnemonics.Contains(v.MasterDataType.Mnemonic)).ToList();
                        if (organisation != null)
                        {
                            companyDetails.BusinessSector = (masterDataList.FirstOrDefault(v => v.Id == organisation.BusinessSectorId) != null) ?
                                ((masterDataList.FirstOrDefault(v => v.Id == organisation.BusinessSectorId).Value.Trim() != "Not Sure") ?
                                masterDataList.FirstOrDefault(v => v.Id == organisation.BusinessSectorId).Value : organisation.BusinessSectorDescription) : string.Empty;
                            companyDetails.CompanySize = masterDataList.FirstOrDefault(v => v.Id == organisation.EmployeeSize) != null ?
                                masterDataList.FirstOrDefault(v => v.Id == organisation.EmployeeSize).Value : string.Empty;
                            companyDetails.TurnOver = masterDataList.FirstOrDefault(v => v.Id == organisation.TurnOverSize) != null ?
                                masterDataList.FirstOrDefault(v => v.Id == organisation.TurnOverSize).Value : string.Empty;
                            companyDetails.CompanyLogoString = (organisation.Document != null) ? organisation.Document.FilePath : string.Empty; ;
                            
                            var customerSectorsList = ctx.IndustryCodeOrganizationLinks.Where(v => v.RefOrganization == organisation.Id).Select(v => v.IndustryCode).ToList();
                            foreach (var item in customerSectorsList)
                            {
                                companyDetails.CustomerSectors += item.SectorName + "; ";
                            }
                            if (companyDetails.CustomerSectors != "")
                            {
                                companyDetails.CustomerSectors = companyDetails.CustomerSectors.Substring(0, companyDetails.CustomerSectors.Length - 2);
                            }
                            var regionsList = ctx.PartyRegionLinks.Where(v => v.RefParty == sellerPartyId && (v.LinkType.Trim() == Constants.PARTY_REGION_SALES_REGION || v.LinkType.Trim() == Constants.PARTY_REGION_SERVICE_REGION)).ToList();
                            var serviceRegionList = regionsList.Where(v => v.LinkType.Trim() == Constants.PARTY_REGION_SERVICE_REGION).Select(v => v.Region.Name).ToList();
                            foreach (var item in serviceRegionList)
                            {
                                companyDetails.CompanyService += item + "; ";
                            }
                            if (companyDetails.CompanyService != "")
                            {
                                companyDetails.CompanyService = companyDetails.CompanyService.Substring(0, companyDetails.CompanyService.Length - 2);
                            }
                            var subsidaryRegionList = regionsList.Where(v => v.LinkType.Trim() == Constants.PARTY_REGION_SALES_REGION).Select(v => v.Region.Name).ToList();
                            foreach (var item in subsidaryRegionList)
                            {
                                companyDetails.CompanySubsidiaries += item + "; ";
                            }
                            if (companyDetails.CompanySubsidiaries != "")
                            {
                                companyDetails.CompanySubsidiaries = companyDetails.CompanySubsidiaries.Substring(0, companyDetails.CompanySubsidiaries.Length - 2);
                            }
                            var seller = organisation.Supplier;
                            if (seller != null)
                            {
                                companyDetails.TradingName = seller.TradingName;
                                companyDetails.FacebookAccount = seller.FacebookAccount;
                                companyDetails.TwitterAccount = seller.TwitterAccount;
                                companyDetails.LinkeldInAccount = seller.LinkedInAccount;
                                companyDetails.WebsiteLink = seller.WebsiteLink;
                                companyDetails.EstablishedYear = seller.EstablishedYear;
                                companyDetails.MaxContractValue = seller.MaxContractValue;
                                companyDetails.MinContractValue = seller.MinContractValue;
                                companyDetails.BusinessDescription = seller.BusinessDescription;
                            }
                            if (buyerPartyId > 0)
                            {
                                var assignedContactParties = ctx.Parties.Where(v => v.PartyPartyLinks1.Any(c => c.PartyPartyLinkType.Trim() == Constants.CONTACT_BUYER && c.RefLinkedParty == buyerPartyId) && v.PartyPartyLinks1.Any(c => c.PartyPartyLinkType.Trim() == Constants.CONTACT_ORGANIZATION && c.RefLinkedParty == sellerPartyId)).ToList();
                                foreach (var item in assignedContactParties)
                                {
                                    var contactDetails = new Domain.Model.ContactDetails();
                                    var person = item.Person;
                                    var contactPerson = person.ContactPerson;
                                    var email = item.PartyContactMethodLinks.FirstOrDefault(v => v.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_EMAIL).ContactMethod.Emails.FirstOrDefault();
                                    var Phone = item.PartyContactMethodLinks.FirstOrDefault(v => v.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_PHONE).ContactMethod.Phones.FirstOrDefault(v => v.Type.Trim() == Constants.PHONE_TYPE_TELEPHONE);
                                    contactDetails.Name = person.FirstName + " " + person.LastName;
                                    contactDetails.Telephone = Phone.PhoneNumber;
                                    contactDetails.Email = email.EmailAddress;
                                    contactDetails.JobTitle = contactPerson.JobTitle;
                                    contactDetails.ContactType = (item.PartyPartyLinks1.FirstOrDefault(v => v.PartyPartyLinkType.Trim() == Constants.CONTACT_BUYER) != null) ?
                                     Convert.ToInt16(item.PartyPartyLinks1.FirstOrDefault(v => v.PartyPartyLinkType.Trim() == Constants.CONTACT_BUYER).PartyPartyLinkSubType) : (short)0;
                                    companyDetails.ContactDetails.Add(contactDetails);
                                }
                            }
                            var contactParties = ctx.Parties.Where(v => v.PartyPartyLinks1.Any(c => c.PartyPartyLinkType.Trim() == Constants.CONTACT_ORGANIZATION && c.RefLinkedParty == sellerPartyId)).ToList();
                            foreach (var item in contactParties)
                            {
                                var contactDetails = new Domain.Model.ContactDetails();
                                var person = item.Person;
                                var contactPerson = person.ContactPerson;
                                var email = item.PartyContactMethodLinks.FirstOrDefault(v => v.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_EMAIL).ContactMethod.Emails.FirstOrDefault();
                                var Phone = item.PartyContactMethodLinks.FirstOrDefault(v => v.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_PHONE).ContactMethod.Phones.FirstOrDefault(v => v.Type.Trim() == Constants.PHONE_TYPE_TELEPHONE);
                                contactDetails.Name = person.FirstName + " " + person.LastName;
                                contactDetails.Telephone = Phone.PhoneNumber;
                                contactDetails.Email = email.EmailAddress;
                                contactDetails.ContactType = contactPerson.ContactType;
                                contactDetails.JobTitle = contactPerson.JobTitle;
                                companyDetails.ContactDetails.Add(contactDetails);
                            }

                            var addresses = new List<Address>();
                            if (buyerPartyId > 0)
                                addresses = ctx.Addresses.Where(v => v.ContactMethod.LegalEntityProfiles.Any(c => c.RefPartyId == buyerPartyId) && v.ContactMethod.PartyContactMethodLinks.Any(c => c.RefParty == sellerPartyId)).ToList();
                            else
                                addresses = ctx.Addresses.Where(v => v.ContactMethod.PartyContactMethodLinks.Any(c => c.RefParty == sellerPartyId)).ToList();
                            companyDetails.AddressDetails = addresses.Select(u => new Domain.Model.AddressDetails()
                            {
                                Id = u.Id,
                                Line1 = u.Line1,
                                Line2 = u.Line2,
                                City = u.City,
                                State = u.State,
                                ZipCode = u.ZipCode,
                                AddressType = u.AddressType,
                                CountryName = u.Region.Name,
                                AddressTypeValue = (u.AddressType > 0) ? CommonMethods.Description((AddressType)(u.AddressType)) : ""
                            }).ToList();
                            if (buyerPartyId > 0)
                            {
                                companyDetails.IsBuyer = true;
                                var favouriteLink = ctx.PartyPartyLinks.FirstOrDefault(v => v.RefParty == buyerUserPartyId && v.RefLinkedParty == sellerPartyId && v.PartyPartyLinkType.Trim() == Constants.BUYER_FAVOURITE_SUPPLIER);
                                companyDetails.IsFavouriteSupplier = (favouriteLink != null) ? true : false;
                                var tradingLink = ctx.PartyPartyLinks.FirstOrDefault(v => v.RefParty == buyerPartyId && v.RefLinkedParty == sellerPartyId && v.PartyPartyLinkType.Trim() == Constants.BUYER_TRADING_SUPPLIER);
                                companyDetails.IsTradingSupplier = (tradingLink != null) ? true : false;

                                var inviteeList = ctx.Invitees.Where(v => v.Supplier1.Organization.Party.Id == sellerPartyId && v.BuyerSupplierReferences.Any(c => c.Buyer.Organization.Party.Id == buyerPartyId)).ToList();
                                companyDetails.ReferenceDetails = inviteeList.Select(u => new Domain.Model.ReferenceDetails()
                                {
                                    Id = u.Id,
                                    ClientName = u.ClientName.Trim(),
                                    ContactName = u.ContactName.Trim(),
                                    JobTitle = u.JobTitle.Trim(),
                                    Email = u.Email.Trim(),
                                    MailingAddress = u.MailingAddress.Trim(),
                                    Fax = u.Fax,
                                    CanWeContact = u.CanWeContact,
                                    ClientRole = u.ClientRole.Trim(),
                                }).ToList();
                                var bankDetails = ctx.BankAccounts.Where(v => v.LegalEntity.Organizations.Any(k => k.Party.Id == sellerPartyId) && v.LegalEntityProfiles.Any(c => c.RefPartyId == buyerPartyId)).ToList();
                                companyDetails.BankDetails = bankDetails.Select(u => new Domain.Model.BankDetails()
                                {

                                    Id = u.Id,
                                    AccountName = u.AccountName.Trim(),
                                    AccountNumber = u.AccountNumber.Trim(),
                                    SwiftCode = u.SwiftCode.Trim(),
                                    BranchIdentifier = (u.BranchIdentifier != null) ? u.BranchIdentifier.Trim() : "",
                                    IBAN = u.IBAN.Trim(),
                                    BankName = u.BankName.Trim(),
                                    Address = u.Address.Trim(),
                                    CountryName = u.Region.Name,
                                    PreferredMode = u.PreferredMode.Trim(),
                                }).ToList();
                            }
                        }
                    }
                }
                Logger.Info("PartyRepository : SellerProfileDetails() : Exit the method");
                return companyDetails;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyRepository : SellerProfileDetails() : Caught an exception" + ex);
                throw;
            }
        }

    }
}
