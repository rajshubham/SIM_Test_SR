using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Abstract;
using PRGX.SIMTrax.DAL.Repository.Context;
using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;

namespace PRGX.SIMTrax.DAL.Repository.Repository
{
    public class BuyerRepository : GenericRepository<Buyer>, IBuyerRepository
    {
        public BuyerRepository(DbContext context) : base(context) { }

        public List<BuyerOrganization> GetBuyerOrganization(int status, long buyerRole, string fromdate, string toDate, out int total, int pageIndex, int pageSize, int sortDirection, string sortParameter, string buyerName = "")
        {
            try
            {
                Logger.Info("BuyerRepository : GetBuyerOrganization() : Enter the method.");
                if (string.IsNullOrWhiteSpace(fromdate))
                    fromdate = DateTime.MinValue.ToString("dd-MMM-yyyy");
                if (string.IsNullOrWhiteSpace(toDate))
                    toDate = DateTime.MaxValue.ToString("dd-MMM-yyyy");
                var afterDate = DateTime.Parse(fromdate);
                var beforeDate = DateTime.Parse(toDate);
                int size = pageSize;
                total = 0;
                short submittedRegistration = (short)CompanyStatus.Submitted;
                short verifiedRegistration = (short)CompanyStatus.ProfileVerified;

                var buyerOrganizations = new List<BuyerOrganization>();
                var buyers = new List<Buyer>();
                using (var ctx = new BuyerContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = true;
                    var query = ctx.Buyers.Where(elem => elem.CreatedOn >= afterDate && elem.CreatedOn <= beforeDate).AsQueryable();

                    if (buyerRole > 0)
                    {
                        query = query.Where(b => b.Organization.Party.PartyPartyLinks.FirstOrDefault(p => p.PartyPartyLinkType.Trim().Equals(Constants.PRIMARY_ORGANIZATION)).Party1.Person.User.UserRoleLinks.FirstOrDefault().RefRole == buyerRole).AsQueryable();
                    }
                    if (!string.IsNullOrWhiteSpace(buyerName))
                    {
                        query = query.Where(elem => elem.Organization.Party.PartyName.ToLower().Contains(buyerName.ToLower())).AsQueryable();
                    }

                    switch ((BuyerOrganisationStatus)status)
                    {
                        case BuyerOrganisationStatus.All:
                            query = query.AsQueryable();
                            break;
                        case BuyerOrganisationStatus.SubmittedRegistration:
                            query = query.Where(elem => elem.Organization.Status == submittedRegistration).AsQueryable();
                            break;
                        case BuyerOrganisationStatus.VerifiedRegistration:
                            query = query.Where(elem => !elem.ActivationDate.HasValue && elem.Organization.Status == verifiedRegistration).AsQueryable();
                            break;
                        case BuyerOrganisationStatus.VerifiedAndActivated:
                            query = query.Where(elem => elem.ActivationDate.HasValue && elem.Organization.Status == verifiedRegistration).AsQueryable();
                            break;
                    }
                    buyers = query.ToList();
                    total = query.Count();
                    int skipCount = Convert.ToInt32(pageIndex - 1) * size;
                    switch (sortDirection)
                    {
                        case 1:
                            buyers = query.OrderBy(elem => elem.Organization.Party.PartyName).Skip(skipCount).Take(size).ToList();
                            break;
                        case 2:
                            buyers = query.OrderByDescending(elem => elem.Organization.Party.PartyName).Skip(skipCount).Take(size).ToList();
                            break;
                        case 3:
                            buyers = query.OrderByDescending(elem => elem.CreatedOn).Skip(skipCount).Take(size).ToList();
                            break;
                    }
                    foreach (var buyer in buyers)
                    {
                        var buyerOrg = new BuyerOrganization();
                        buyerOrg.ActivatedDate = buyer.ActivationDate;
                        buyerOrg.BuyerId = buyer.Id;
                        buyerOrg.BuyerOrganizationId = buyer.Id;
                        buyerOrg.BuyerOrganizationName = buyer.Organization.Party.PartyName;
                        buyerOrg.BuyerPartyId = buyer.Organization.Id;
                        buyerOrg.CreatedDate = buyer.Organization.CreatedOn;
                        buyerOrg.VerifiedDate = buyer.Organization.ProfileVerifiedOn;
                        buyerOrg.BuyerStatus = buyer.Organization.Status.HasValue ? buyer.Organization.Status.Value : 0;

                        var contactMethodParties = buyer.Organization.Party.PartyPartyLinks.Where(x => x.PartyPartyLinkType.Trim().Equals(Constants.CONTACT_ORGANIZATION) && x.RefLinkedParty == buyer.Organization.Id).Select(p => p.Party1.Id).ToList();

                        var primaryContact = ctx.ContactPersons.FirstOrDefault(x => contactMethodParties.Contains(x.Person.Id) && x.ContactType == (Int16)ContactType.Primary);

                        if (null != primaryContact)
                        {
                            var primaryContactMethodLink = primaryContact.Person.Party.PartyContactMethodLinks.Where(x => x.ContactMethod.ContactMethodType.Trim().Equals(Constants.CONTACT_METHOD_EMAIL)).FirstOrDefault();
                            buyerOrg.PrimaryEmail = null != primaryContactMethodLink ? primaryContactMethodLink.ContactMethod.Emails.FirstOrDefault().EmailAddress : string.Empty;

                        }
                        buyerOrg.PrimaryContact = null != primaryContact ? primaryContact.Person.Party.PartyName : string.Empty;

                        var primaryUser = buyer.Organization.Party.PartyPartyLinks.FirstOrDefault(p => p.PartyPartyLinkType.Trim().Equals(Constants.PRIMARY_ORGANIZATION)).Party1.Person.User;

                        if (null != primaryUser.AcceptedTermsOfUses.FirstOrDefault())
                        {
                            buyerOrg.TermsAcceptedDate = primaryUser.AcceptedTermsOfUses.FirstOrDefault().AcceptedDate;
                        }


                        if (primaryUser.UserRoleLinks.Any())
                        {
                            buyerOrg.BuyerRoleId = buyer.Organization.Party.PartyPartyLinks.FirstOrDefault(p => p.PartyPartyLinkType.Trim().Equals(Constants.PRIMARY_ORGANIZATION)).Party1.Person.User.UserRoleLinks.FirstOrDefault().RefRole;
                            buyerOrg.BuyerRoleName = buyer.Organization.Party.PartyPartyLinks.FirstOrDefault(p => p.PartyPartyLinkType.Trim().Equals(Constants.PRIMARY_ORGANIZATION)).Party1.Person.User.UserRoleLinks.FirstOrDefault().Role.Name;
                        }
                        else
                        {
                            buyerOrg.BuyerRoleId = 0;
                            buyerOrg.BuyerRoleName = string.Empty;
                        }

                        buyerOrganizations.Add(buyerOrg);
                    }
                }
                Logger.Info("BuyerRepository : GetBuyerOrganization() : Exit the method.");
                return buyerOrganizations;
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerRepository : GetBuyerOrganization() : Caught an exception" + ex);
                throw ex;
            }
        }


        public Buyer GetBuyerOrganizationDetailsByPartyId(long organizationPartyId)
        {
            try
            {
                Buyer details = null;
                Logger.Info("BuyerRepository : GetBuyerOrganizationDetailsByPartyId() : Enter the method");

                details = All().Include("Organization").Include("Organization.Party").Include("Organization.Party.PartyPartyLinks").Include("Organization.Party.PartyPartyLinks.Party1").Include("Organization.Party.PartyPartyLinks.Party1.Person").Include("Organization.Party.PartyPartyLinks.Party1.Person.User").Include("Organization.Party.PartyPartyLinks.Party1.Person.ContactPerson").Include("Organization.Party.PartyPartyLinks.Party1.Person.User.Credentials").Include("Organization.Party.PartyPartyLinks.Party1.PartyContactMethodLinks.ContactMethod").Include("Organization.Party.PartyContactMethodLinks.ContactMethod.Addresses").Include("Organization.Party.PartyPartyLinks.Party1.PartyContactMethodLinks.ContactMethod.Emails").Include("Organization.Party.PartyPartyLinks.Party1.PartyContactMethodLinks.ContactMethod.Phones").FirstOrDefault(v => v.Organization.Id == organizationPartyId);

                Logger.Info("BuyerRepository : GetBuyerOrganizationDetailsByPartyId() : Exit the method");
                return details;
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerRepository : GetBuyerOrganizationDetailsByPartyId() : Caught an exception" + ex);
                throw;
            }
        }

        public long GetBuyerPrimaryContactPartyId(long buyerPartyId)
        {
            try
            {
                Logger.Info("BuyerRepository : GetBuyerPrimaryContactPartyId() : Enter the method");
                long primaryContactPartyId = 0;
                using (var ctx = new BuyerContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = true;

                    var contactMethodParties = ctx.PartyPartyLinks.Where(x => x.PartyPartyLinkType.Trim().Equals(Constants.CONTACT_ORGANIZATION) && x.RefLinkedParty == buyerPartyId).Select(p => p.Party1.Id).ToList();

                    var primaryContact = ctx.ContactPersons.FirstOrDefault(x => contactMethodParties.Contains(x.Person.Id) && x.ContactType == (Int16)ContactType.Primary);

                    if (null != primaryContact)
                    {
                        primaryContactPartyId = primaryContact.Person.Party.Id;
                    }
                }

                Logger.Info("BuyerRepository : GetBuyerPrimaryContactPartyId() : Exit the method");
                return primaryContactPartyId;
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerRepository : GetBuyerPrimaryContactPartyId() : Caught an exception" + ex);
                throw;
            }
        }

        public List<BuyerOrganization> GetNotActivatedBuyerOrganization(int pageIndex, int pageSize, int sortDirection, string sortParameter, out int total)
        {
            try
            {
                Logger.Info("BuyerRepository : GetNotActivatedBuyerOrganization() : Enter the method.");
                int size = pageSize;
                total = 0;

                var buyerOrganizations = new List<BuyerOrganization>();
                var buyers = new List<Buyer>();
                using (var ctx = new BuyerContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = true;
                    var query = ctx.Buyers.Where(b => !b.ActivationDate.HasValue).AsQueryable();

                    buyers = query.ToList();
                    total = query.Count();
                    int skipCount = Convert.ToInt32(pageIndex - 1) * size;
                    switch (sortDirection)
                    {
                        case 1:
                            buyers = query.OrderBy(elem => elem.Organization.Party.PartyName).Skip(skipCount).Take(size).ToList();
                            break;
                        case 2:
                            buyers = query.OrderByDescending(elem => elem.Organization.Party.PartyName).Skip(skipCount).Take(size).ToList();
                            break;
                        case 3:
                            buyers = query.OrderByDescending(elem => elem.CreatedOn).Skip(skipCount).Take(size).ToList();
                            break;
                    }
                    foreach (var buyer in buyers)
                    {
                        var buyerOrg = new BuyerOrganization();
                        buyerOrg.ActivatedDate = buyer.ActivationDate;
                        buyerOrg.BuyerId = buyer.Id;
                        buyerOrg.BuyerOrganizationId = buyer.Id;
                        buyerOrg.BuyerOrganizationName = buyer.Organization.Party.PartyName;
                        buyerOrg.BuyerPartyId = buyer.Organization.Id;
                        buyerOrg.CreatedDate = buyer.Organization.CreatedOn;
                        buyerOrg.VerifiedDate = buyer.Organization.ProfileVerifiedOn;
                        buyerOrg.BuyerStatus = buyer.Organization.Status.HasValue ? buyer.Organization.Status.Value : 0;

                        var contactMethodParties = buyer.Organization.Party.PartyPartyLinks.Where(x => x.PartyPartyLinkType.Trim().Equals(Constants.CONTACT_ORGANIZATION) && x.RefLinkedParty == buyer.Organization.Id).Select(p => p.Party1.Id).ToList();

                        var primaryContact = ctx.ContactPersons.FirstOrDefault(x => contactMethodParties.Contains(x.Person.Id) && x.ContactType == (Int16)ContactType.Primary);

                        if (null != primaryContact)
                        {
                            var primaryContactMethodLink = primaryContact.Person.Party.PartyContactMethodLinks.Where(x => x.ContactMethod.ContactMethodType.Trim().Equals(Constants.CONTACT_METHOD_EMAIL)).FirstOrDefault();
                            buyerOrg.PrimaryEmail = null != primaryContactMethodLink ? primaryContactMethodLink.ContactMethod.Emails.FirstOrDefault().EmailAddress : string.Empty;
                        }

                        buyerOrg.PrimaryContact = null != primaryContact ? primaryContact.Person.Party.PartyName : string.Empty;

                        var primaryUser = buyer.Organization.Party.PartyPartyLinks.FirstOrDefault(p => p.PartyPartyLinkType.Trim().Equals(Constants.PRIMARY_ORGANIZATION)).Party1.Person.User;

                        if (null != primaryUser.AcceptedTermsOfUses.FirstOrDefault())
                        {
                            buyerOrg.TermsAcceptedDate = primaryUser.AcceptedTermsOfUses.FirstOrDefault().AcceptedDate;
                        }

                        if (primaryUser.UserRoleLinks.Any())
                        {
                            buyerOrg.BuyerRoleId = buyer.Organization.Party.PartyPartyLinks.FirstOrDefault(p => p.PartyPartyLinkType.Trim().Equals(Constants.PRIMARY_ORGANIZATION)).Party1.Person.User.UserRoleLinks.FirstOrDefault().RefRole;
                            buyerOrg.BuyerRoleName = buyer.Organization.Party.PartyPartyLinks.FirstOrDefault(p => p.PartyPartyLinkType.Trim().Equals(Constants.PRIMARY_ORGANIZATION)).Party1.Person.User.UserRoleLinks.FirstOrDefault().Role.Name;
                        }
                        else
                        {
                            buyerOrg.BuyerRoleId = 0;
                            buyerOrg.BuyerRoleName = string.Empty;
                        }

                        buyerOrganizations.Add(buyerOrg);
                    }
                }
                Logger.Info("BuyerRepository : GetNotActivatedBuyerOrganization() : Exit the method.");
                return buyerOrganizations;
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerRepository : GetNotActivatedBuyerOrganization() : Caught an exception" + ex);
                throw ex;
            }
        }


        public List<SupplierDetails> GetSuppliers(BuyerSupplierSearchFilter model, long companyPartyId, long userPartyId, out int totalRecords)
        {
            try
            {
                Logger.Info("BuyerRepository : GetSuppliers() : Enter the method");
                var supplierDetailList = new List<SupplierDetails>();
                totalRecords = 0;
                using (var ctx = new SellerContext())
                {
                    var supplierParties = ctx.Parties.Where(v => v.PartyType.Trim() == Constants.PARTY_TYPE_ORGANIZATION && v.Organization.OrganizationType.Trim().Equals(Constants.ORGANIZATION_TYPE_SELLER)).AsQueryable();

                    if (model.SupplierStatus != null && model.SupplierStatus.Count > 0)
                    {
                        supplierParties = supplierParties.Where(v => model.SupplierStatus.Contains(v.Organization.Status.Value)).AsQueryable();
                    }
                    if (!string.IsNullOrWhiteSpace(model.SupplierName))
                    {
                        supplierParties = supplierParties.Where(v => v.PartyName.ToLower().Trim().Contains(model.SupplierName.ToLower().Trim())).AsQueryable();

                    }
                    if (model.SupplierType != null && model.SupplierType.Count > 0)
                    {
                        if (model.SupplierType.Contains((Int16)SupplierType.Favourite))
                        {
                            supplierParties = supplierParties.Where(v => v.PartyPartyLinks.Any(c => c.PartyPartyLinkType.Trim() == Constants.BUYER_FAVOURITE_SUPPLIER && c.RefParty == userPartyId)).AsQueryable();
                        }
                        if (model.SupplierType.Contains((Int16)SupplierType.TradingWith))
                        {
                            supplierParties = supplierParties.Where(v => v.PartyPartyLinks.Any(c => c.PartyPartyLinkType.Trim() == Constants.BUYER_TRADING_SUPPLIER && c.RefParty == companyPartyId)).AsQueryable();
                        }
                    }
                    if (model.Sector != null && model.Sector.Count > 0)
                    {
                        supplierParties = supplierParties.Where(v => model.Sector.Contains(v.Organization.BusinessSectorId.Value)).AsQueryable();
                    }
                    if (model.EmployeeSize != null && model.EmployeeSize.Count > 0)
                    {
                        supplierParties = supplierParties.Where(v => model.EmployeeSize.Contains(v.Organization.EmployeeSize.Value)).AsQueryable();
                    }
                    if (model.TurnOver != null && model.TurnOver.Count > 0)
                    {
                        supplierParties = supplierParties.Where(v => model.TurnOver.Contains(v.Organization.TurnOverSize.Value)).AsQueryable();
                    }
                    if (model.TypeOfCompany != null && model.TypeOfCompany.Count > 0)
                    {
                        supplierParties = supplierParties.Where(v => model.TypeOfCompany.Contains(v.Organization.Supplier.TypeOfSeller.Value)).AsQueryable();
                    }
                    var finalSuppliers = supplierParties;
                    var skipCount = (model.PageNo - 1) * model.PageSize;
                    totalRecords = finalSuppliers.Count();
                    var supplierList = new List<Party>();
                    switch (model.SortParameter)
                    {
                        case "CompanyName":
                            switch (model.SortDirection)
                            {
                                case (Int16)SortOrder.Asc:
                                    supplierList = finalSuppliers.OrderBy(u => u.PartyName).Skip(skipCount).Take(model.PageSize).ToList();
                                    break;
                                case (Int16)SortOrder.Desc:
                                    supplierList = finalSuppliers.OrderByDescending(u => u.PartyName).Skip(skipCount).Take(model.PageSize).ToList();
                                    break;

                            }
                            break;
                        case "Status":
                            switch (model.SortDirection)
                            {
                                case (Int16)SortOrder.Asc:
                                    supplierList = finalSuppliers.OrderBy(u => u.Organization.Status.Value).Skip(skipCount).Take(model.PageSize).ToList();
                                    break;
                                case (Int16)SortOrder.Desc:
                                    supplierList = finalSuppliers.OrderByDescending(u => u.Organization.Status.Value).Skip(skipCount).Take(model.PageSize).ToList();
                                    break;

                            }
                            break;
                        default:
                            supplierList = finalSuppliers.OrderBy(u => u.CreatedOn).Skip(skipCount).Take(model.PageSize).ToList();
                            break;
                    }
                    supplierDetailList = supplierList.Select(v => new SupplierDetails()
                    {
                        BuyerId = companyPartyId,
                        CompanyId = v.Id,
                        CompanyName = v.PartyName,
                        IsFavourite = (v.PartyPartyLinks.FirstOrDefault(c => c.RefParty == userPartyId && c.PartyPartyLinkType.Trim() == Constants.BUYER_FAVOURITE_SUPPLIER) != null) ? true : false,
                        IsTradingWith = (v.PartyPartyLinks.FirstOrDefault(c => c.RefParty == companyPartyId && c.PartyPartyLinkType.Trim() == Constants.BUYER_TRADING_SUPPLIER) != null) ? true : false,
                        StatusId = v.Organization.Status,
                        Status = v.Organization.Status != null ? CommonMethods.Description((CompanyStatus)Convert.ToInt16(v.Organization.Status.Value)) : CommonMethods.Description(CompanyStatus.Started),
                        CreatedDate = v.CreatedOn,
                        IsNotRegisteredSupplier = false,
                        AddressLine1 = v.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS) != null && v.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS).ContactMethod.Addresses.FirstOrDefault() != null ?
                        v.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS).ContactMethod.Addresses.FirstOrDefault().Line1 : string.Empty,
                        AddressLine2 = v.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS) != null && v.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS).ContactMethod.Addresses.FirstOrDefault() != null ?
                        v.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS).ContactMethod.Addresses.FirstOrDefault().Line2 : string.Empty,
                        City = v.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS) != null && v.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS).ContactMethod.Addresses.FirstOrDefault() != null ?
                        v.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS).ContactMethod.Addresses.FirstOrDefault().City : string.Empty,
                        State = v.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS) != null && v.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS).ContactMethod.Addresses.FirstOrDefault() != null ?
                        v.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS).ContactMethod.Addresses.FirstOrDefault().State : string.Empty,
                        Country = v.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS) != null && v.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS).ContactMethod.Addresses.FirstOrDefault() != null ?
                        v.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS).ContactMethod.Addresses.FirstOrDefault().Region.Name : string.Empty,
                        PostCode = v.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS) != null && v.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS).ContactMethod.Addresses.FirstOrDefault() != null ?
                        v.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS).ContactMethod.Addresses.FirstOrDefault().ZipCode : string.Empty,
                        Registrationnumber = v.PartyIdentifiers.FirstOrDefault(c => c.PartyIdentifierType.IdentifierType.Trim() == Constants.IDENTIFIER_TYPE_REGISTRATION_NUMBER) != null ?
                         v.PartyIdentifiers.FirstOrDefault(c => c.PartyIdentifierType.IdentifierType.Trim() == Constants.IDENTIFIER_TYPE_REGISTRATION_NUMBER).IdentifierNumber : string.Empty,
                        DUNSnumber = v.PartyIdentifiers.FirstOrDefault(c => c.PartyIdentifierType.IdentifierType.Trim() == Constants.IDENTIFIER_TYPE_DUNS_NUMBER) != null ?
                         v.PartyIdentifiers.FirstOrDefault(c => c.PartyIdentifierType.IdentifierType.Trim() == Constants.IDENTIFIER_TYPE_DUNS_NUMBER).IdentifierNumber : string.Empty,
                        VATnumber = v.PartyIdentifiers.FirstOrDefault(c => c.PartyIdentifierType.IdentifierType.Trim() == Constants.IDENTIFIER_TYPE_VAT_NUMBER) != null ?
                         v.PartyIdentifiers.FirstOrDefault(c => c.PartyIdentifierType.IdentifierType.Trim() == Constants.IDENTIFIER_TYPE_VAT_NUMBER).IdentifierNumber : string.Empty,
                        StartedDate = (v.CreatedOn != null) ? v.CreatedOn.ToShortDateString().ToString(Thread.CurrentThread.CurrentCulture.DateTimeFormat) : "-",
                        ProfileCreatedDate = (v.Organization.RegistrationSubmittedOn != null) ? v.Organization.RegistrationSubmittedOn.Value.ToShortDateString().ToString(Thread.CurrentThread.CurrentCulture.DateTimeFormat) : "-"

                    }).ToList();
                }
                Logger.Info("BuyerRepository : GetSuppliers() : Exit the method");
                return supplierDetailList;
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerRepository : GetSuppliers() : Caught an error" + ex);
                throw;
            }
        }

        public List<string> GetVerifiedBuyerNames(string buyerOrg)
        {
            try
            {
                Logger.Info("BuyerRepository : GetVerifiedBuyerNames() : Entering the method");
                List<string> buyerOrgList = null;
                using (var ctx = new BuyerContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = true;

                    buyerOrgList = ctx.Buyers.Where(u => u.Organization.Party.PartyName.ToLower().Contains(buyerOrg.ToLower())).Select(u => u.Organization.Party.PartyName).Distinct().ToList();
                }
                Logger.Info("BuyerRepository : GetVerifiedBuyerNames() : Exiting the method");
                return buyerOrgList;
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerRepository : GetVerifiedBuyerNames() : Caught an exception" + ex);
                throw ex;
            }
        }

        public BuyerOrganization GetBuyerDetailsForDashboard(long partyId)
        {
            try
            {
                Logger.Info("BuyerRepository : GetBuyerDetailsForDashboard() : Enter the method.");
                var buyerOrg = new BuyerOrganization();
                using (var ctx = new BuyerContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = true;
                    var buyer = ctx.Buyers.Where(elem => elem.Organization.Party.Id == partyId).FirstOrDefault();
                    if (null != buyer)
                    {
                        buyerOrg.ActivatedDate = buyer.ActivationDate;
                        buyerOrg.BuyerId = buyer.Id;
                        buyerOrg.BuyerOrganizationId = buyer.Id;
                        buyerOrg.BuyerOrganizationName = buyer.Organization.Party.PartyName;
                        buyerOrg.BuyerPartyId = buyer.Organization.Id;
                        buyerOrg.CreatedDate = buyer.Organization.CreatedOn;
                        buyerOrg.VerifiedDate = buyer.Organization.ProfileVerifiedOn;
                        buyerOrg.BuyerStatus = buyer.Organization.Status.HasValue ? buyer.Organization.Status.Value : 0;

                        var contactMethodParties = buyer.Organization.Party.PartyPartyLinks.Where(x => x.PartyPartyLinkType.Trim().Equals(Constants.CONTACT_ORGANIZATION) && x.RefLinkedParty == buyer.Organization.Id).Select(p => p.Party1.Id).ToList();

                        var primaryContact = ctx.ContactPersons.FirstOrDefault(x => contactMethodParties.Contains(x.Person.Id) && x.ContactType == (Int16)ContactType.Primary);

                        if (null != primaryContact)
                        {
                            var primaryContactMethodLink = primaryContact.Person.Party.PartyContactMethodLinks.Where(x => x.ContactMethod.ContactMethodType.Trim().Equals(Constants.CONTACT_METHOD_EMAIL)).FirstOrDefault();
                            buyerOrg.PrimaryEmail = null != primaryContactMethodLink ? primaryContactMethodLink.ContactMethod.Emails.FirstOrDefault().EmailAddress : string.Empty;

                        }
                        buyerOrg.PrimaryContact = null != primaryContact ? primaryContact.Person.Party.PartyName : string.Empty;

                        var primaryUser = buyer.Organization.Party.PartyPartyLinks.FirstOrDefault(p => p.PartyPartyLinkType.Trim().Equals(Constants.PRIMARY_ORGANIZATION)).Party1.Person.User;

                        if (null != primaryUser.AcceptedTermsOfUses.FirstOrDefault())
                        {
                            buyerOrg.TermsAcceptedDate = primaryUser.AcceptedTermsOfUses.FirstOrDefault().AcceptedDate;
                        }

                        if (primaryUser.UserRoleLinks.Any())
                        {
                            buyerOrg.BuyerRoleId = buyer.Organization.Party.PartyPartyLinks.FirstOrDefault(p => p.PartyPartyLinkType.Trim().Equals(Constants.PRIMARY_ORGANIZATION)).Party1.Person.User.UserRoleLinks.FirstOrDefault().RefRole;
                            buyerOrg.BuyerRoleName = buyer.Organization.Party.PartyPartyLinks.FirstOrDefault(p => p.PartyPartyLinkType.Trim().Equals(Constants.PRIMARY_ORGANIZATION)).Party1.Person.User.UserRoleLinks.FirstOrDefault().Role.Name;
                        }
                        else
                        {
                            buyerOrg.BuyerRoleId = 0;
                            buyerOrg.BuyerRoleName = string.Empty;
                        }
                    }
                }
                Logger.Info("BuyerRepository : GetBuyerDetailsForDashboard() : Exit the method.");
                return buyerOrg;
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerRepository : GetBuyerDetailsForDashboard() : Caught an exception" + ex);
                throw ex;
            }
        }

        public List<ItemList> GetBuyersList()
        {
            Logger.Info("BuyerRepository : GetBuyersList() : Entering the method");
            var buyerList = new List<ItemList>();
            try
            {
                using (var ctx = new BuyerContext())
                {
                    buyerList = ctx.Buyers.Select(c => new ItemList()
                    {
                        Text = c.Organization.Party.PartyName,
                        Value = c.Id
                    }).ToList();
                }
                Logger.Info("BuyerRepository : GetBuyersList() : Exiting the method");
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerRepository : GetBuyersList() : Caught an exception" + ex);
                throw ex;
            }
            return buyerList;
        }

        public List<DiscountVoucher> GetAllVouchers(int currentPage, string sortParameter, int sortDirection, out int total, int count, long buyerPartyId = 0)
        {
            Logger.Info("BuyerRepository : GetAllVouchers() : Entering the method");
            var voucherList = new List<DiscountVoucher>();
            var skipCount = (currentPage - 1) * count;
            try
            {
                using (var ctx = new BuyerContext())
                {
                    var query = ctx.DiscountVouchers.Include(dv => dv.Buyer.Organization.Party).AsQueryable();
                    if (buyerPartyId > 0)
                        query = query.Where(v => v.RefBuyer == buyerPartyId);
                    voucherList = query.ToList();
                    total = voucherList.Count();
                    switch (sortDirection)
                    {
                        case 1:
                            voucherList = voucherList.OrderBy(elem => elem.PromotionalCode).Skip(skipCount).Take(count).ToList();
                            break;
                        case 2:
                            voucherList = voucherList.OrderByDescending(elem => elem.PromotionalCode).Skip(skipCount).Take(count).ToList();
                            break;
                        case 3:
                            voucherList = voucherList.OrderByDescending(elem => elem.PromotionStartDate).Skip(skipCount).Take(count).ToList();
                            break;
                    }
                }
                Logger.Info("BuyerRepository : GetBuyersList() : Exiting the method");
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerRepository : GetBuyersList() : Caught an exception" + ex);
                throw ex;
            }
            return voucherList;
        }

        public List<BuyerCampaign> GetBuyerCampaignDetailsForDashboard(out int totalCampaigns, long partyId, int pageNumber, int sortDirection)
        {
            Logger.Info("BuyerRepository : GetBuyerCampaignDetailsForDashboard() : Enter into method");
            var campaignDetails = new List<BuyerCampaign>();
            try
            {
                int pageSize = 5;
                int skipcount = (pageNumber - 1) * pageSize;
                using (var ctx = new BuyerContext())
                {
                    var query = ctx.BuyerCampaigns.Where(u => u.RefBuyer == partyId).AsQueryable();
                    totalCampaigns = query.Count();
                    switch (sortDirection)
                    {
                        case 1:
                            campaignDetails = query.OrderBy(u => u.CampaignName).Skip(skipcount).Take(pageSize).ToList();
                            break;
                        case 2:
                            campaignDetails = query.OrderByDescending(u => u.CampaignName).Skip(skipcount).Take(pageSize).ToList();
                            break;
                        case 3:
                            campaignDetails = query.OrderBy(u => u.CreatedOn).Skip(skipcount).Take(pageSize).ToList();
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Info("BuyerRepository : GetBuyerCampaignDetailsForDashboard() : Caught an exception " + ex);
                throw;
            }
            Logger.Info("BuyerRepository : GetBuyerCampaignDetailsForDashboard() : Exit from method");
            return campaignDetails;
        }
    }
}
