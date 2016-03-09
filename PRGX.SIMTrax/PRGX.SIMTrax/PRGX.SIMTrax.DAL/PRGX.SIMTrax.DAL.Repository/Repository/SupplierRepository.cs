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
    public class SupplierRepository : GenericRepository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(DbContext context) : base(context) { }

        public bool UpdateSellerDetails(Supplier seller, long partyId)
        {
            try
            {
                Logger.Info("SupplierRepository : UpdateSellerDetails() : Enter the method");
                var result = false;
                var sellerPM = this.All().Where(x => x.Organization.Id == partyId).FirstOrDefault();

                if (null != sellerPM)
                {
                    sellerPM.BusinessDescription = seller.BusinessDescription;
                    sellerPM.EstablishedYear = seller.EstablishedYear;
                    sellerPM.FacebookAccount = seller.FacebookAccount;
                    sellerPM.IsSubsidary = seller.IsSubsidary;
                    sellerPM.LinkedInAccount = seller.LinkedInAccount;
                    sellerPM.MaxContractValue = seller.MaxContractValue;
                    sellerPM.MinContractValue = seller.MinContractValue;
                    sellerPM.TradingName = seller.TradingName;
                    sellerPM.TwitterAccount = seller.TwitterAccount;
                    sellerPM.TypeOfSeller = seller.TypeOfSeller;
                    sellerPM.UltimateParent = seller.UltimateParent;
                    sellerPM.WebsiteLink = seller.WebsiteLink;
                    sellerPM.RefLastUpdatedBy = seller.RefLastUpdatedBy;
                    Update(sellerPM);
                    result = true;
                }
                Logger.Info("SupplierRepository : UpdateSellerDetails() : Exit the method");
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("SupplierRepository : UpdateSellerDetails() : Caught an error" + ex);
                throw;
            }
        }

        public ProfileSummary GetSellerProfilePercentage(long sellerPartyId, long sellerId, long organisationId)
        {
            try
            {
                ProfileSummary summaryData = new ProfileSummary();
                Logger.Info("SupplierRepository : GetSellerProfilePercentage() : Enter the method");
                using (var ctx = new SellerContext())
                {
                    var sellerDetails = ctx.Suppliers.FirstOrDefault(v => v.Organization.Id == sellerPartyId && v.Id == sellerId && v.Organization.Id == organisationId);
                    if (sellerDetails != null)
                    {
                        //Required Fields Count according Profile Sections
                        var capabilityFieldsCount = 8;
                        var marketingFieldsCount = 6;
                        var companyFieldsCount = 8;
                        var contactFieldCount = 5;
                        var addressFieldCount = 3;
                        var numberOfSections = 5;

                        //Capability Details Score
                        var capabilityCount = 0;
                        capabilityCount = (sellerDetails.Organization.BusinessSectorId != null) ? capabilityCount + 1 : capabilityCount;
                        capabilityCount = (sellerDetails.Organization.TurnOverSize != null) ? capabilityCount + 1 : capabilityCount;
                        capabilityCount = (sellerDetails.Organization.EmployeeSize != null) ? capabilityCount + 1 : capabilityCount;
                        capabilityCount = (!string.IsNullOrWhiteSpace(sellerDetails.MaxContractValue)) ? capabilityCount + 1 : capabilityCount;
                        capabilityCount = (!string.IsNullOrWhiteSpace(sellerDetails.MinContractValue)) ? capabilityCount + 1 : capabilityCount;
                        capabilityCount = (sellerDetails.Organization.Party.PartyRegionLinks.Any(v => v.LinkType.Trim() == Constants.PARTY_REGION_SALES_REGION)) ? capabilityCount + 1 : capabilityCount;
                        capabilityCount = (sellerDetails.Organization.Party.PartyRegionLinks.Any(v => v.LinkType.Trim() == Constants.PARTY_REGION_SERVICE_REGION)) ? capabilityCount + 1 : capabilityCount;
                        capabilityCount = (sellerDetails.Organization.IndustryCodeOrganizationLinks.Any()) ? capabilityCount + 1 : capabilityCount;
                        summaryData.CapabilityDetailsScore = (int)Math.Round(((double)(capabilityCount * 100) / (capabilityFieldsCount)));


                        //Marketing Details Score
                        var marketingCount = 0;
                        marketingCount = (!string.IsNullOrWhiteSpace(sellerDetails.BusinessDescription)) ? marketingCount + 1 : marketingCount;
                        marketingCount = (sellerDetails.Organization.RefLogoDocument != null) ? marketingCount + 1 : marketingCount;
                        marketingCount = (!string.IsNullOrWhiteSpace(sellerDetails.FacebookAccount)) ? marketingCount + 1 : marketingCount;
                        marketingCount = (!string.IsNullOrWhiteSpace(sellerDetails.WebsiteLink)) ? marketingCount + 1 : marketingCount;
                        marketingCount = (!string.IsNullOrWhiteSpace(sellerDetails.TwitterAccount)) ? marketingCount + 1 : marketingCount;
                        marketingCount = (!string.IsNullOrWhiteSpace(sellerDetails.LinkedInAccount)) ? marketingCount + 1 : marketingCount;
                        summaryData.MarketingDetailsScore = (int)Math.Round(((double)(marketingCount * 100) / (marketingFieldsCount)));

                        //Company Details Score
                        var companyCount = 0;
                        companyCount = (!string.IsNullOrWhiteSpace(sellerDetails.Organization.Party.PartyName)) ? companyCount + 1 : companyCount;
                        companyCount = (!string.IsNullOrWhiteSpace(sellerDetails.TradingName)) ? companyCount + 1 : companyCount;
                        companyCount = (sellerDetails.TypeOfSeller != null) ? companyCount + 1 : companyCount;
                        companyCount = (sellerDetails.IsSubsidary != null) ? companyCount + 1 : companyCount;
                        companyCount = (sellerDetails.Organization.Party.PartyIdentifiers.Any(v => v.PartyIdentifierType.IdentifierType.Trim() == Constants.IDENTIFIER_TYPE_REGISTRATION_NUMBER)) ? companyCount + 1 : companyCount;
                        companyCount = (!string.IsNullOrWhiteSpace(sellerDetails.EstablishedYear)) ? companyCount + 1 : companyCount;
                        summaryData.CompanyDetailsScore = (int)Math.Round(((double)((companyCount + 2) * 100) / (companyFieldsCount)));

                        //For AddressDetails
                        var addressTypeList = new List<Int16>();
                        addressTypeList.Add((Int16)AddressType.Primary);
                        addressTypeList.Add((Int16)AddressType.HeadQuarters);
                        addressTypeList.Add((Int16)AddressType.Registered);
                        var addressCount = ctx.Addresses.Count(v => v.ContactMethod.PartyContactMethodLinks.Any(c => c.RefParty == sellerPartyId) && addressTypeList.Contains(v.AddressType));
                        summaryData.AddressDetailsScore = (int)Math.Round(((double)(addressCount * 100) / (addressFieldCount)));

                        var contactsCount = ctx.ContactPersons.Count(v => v.Person.Party.PartyPartyLinks1.Any(c => c.RefLinkedParty == sellerPartyId && c.PartyPartyLinkType.Trim() == Constants.CONTACT_ORGANIZATION) &&
                        v.ContactType != null);
                        summaryData.ContactDetailsScore = (int)Math.Round(((double)(contactsCount * 100) / (contactFieldCount)));

                        summaryData.TotalScore = (int)Math.Round(((double)((summaryData.CompanyDetailsScore + summaryData.ContactDetailsScore + summaryData.MarketingDetailsScore + summaryData.CapabilityDetailsScore + summaryData.AddressDetailsScore)) / (numberOfSections)));

                    }
                }
                Logger.Info("SupplierRepository : GetSellerProfilePercentage() : Exit the method");
                return summaryData;
            }
            catch (Exception ex)
            {
                Logger.Error("SupplierRepository : GetSellerProfilePercentage() : Caught an exception" + ex);
                throw;
            }
        }


        public List<SupplierOrganization> GetSupplierOrganization(string fromdate, string toDate, out int total, int pageIndex, short source, int size, int sortDirection, long supplierId = 0, string supplierName = "", long status = (Int64)CompanyStatus.Started, string referrerName = "")
        {
            try
            {
                Logger.Info("SupplierRepository : GetSupplierOrganization() : Enter the method.");
                if (string.IsNullOrWhiteSpace(fromdate))
                    fromdate = DateTime.MinValue.ToString("dd-MMM-yyyy");
                if (string.IsNullOrWhiteSpace(toDate))
                    toDate = DateTime.MaxValue.ToString("dd-MMM-yyyy");
                var afterDate = DateTime.Parse(fromdate).Date;
                var beforeDate = DateTime.Parse(toDate).Date;
                total = 0;
                var supplierOrganizations = new List<SupplierOrganization>();
                var supplierList = new List<Supplier>();
                using (var ctx = new SellerContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = true;
                    var query = ctx.Suppliers.Where(elem => elem.CreatedOn >= afterDate && elem.CreatedOn <= beforeDate).AsQueryable();

                    if (!string.IsNullOrWhiteSpace(supplierName))
                    {
                        query = query.Where(elem => elem.Organization.Party.PartyName.ToLower().Contains(supplierName.ToLower()));
                    }
                    if (!string.IsNullOrWhiteSpace(referrerName))
                    {
                        query = query.Where(elem => elem.SupplierReferrers.Any(u => u.BuyerCampaign.Buyer.Organization.Party.PartyName.ToLower().Contains(referrerName.ToLower())));
                    }
                    if (supplierId != 0)
                    {
                        query = query.Where(elem => elem.Id == supplierId);
                    }
                    if (source == Convert.ToInt16(ProjectSource.SIM))
                    {
                        query = query.Where(elem => elem.Organization.Party.ProjectSource == (short)ProjectSource.SIM);
                    }
                    else if (source == Convert.ToInt16(ProjectSource.CIPS))
                    {
                        query = query.Where(elem => elem.Organization.Party.ProjectSource == (short)ProjectSource.CIPS);
                    }
                    switch ((CompanyStatus)status)
                    {
                        case CompanyStatus.Submitted:
                            query = query.Where(elem => elem.Organization.Status == (Int16)CompanyStatus.Submitted).AsQueryable(); ;
                            break;
                        case CompanyStatus.RegistrationVerified:
                            query = query.Where(elem => elem.Organization.Status == (Int16)CompanyStatus.RegistrationVerified).AsQueryable(); ;
                            break;
                        case CompanyStatus.ProfileVerified:
                            query = query.Where(elem => elem.Organization.Status == (Int16)CompanyStatus.ProfileVerified).AsQueryable(); ;
                            break;
                        case CompanyStatus.SanctionVerified:
                            query = query.Where(elem => elem.Organization.Status == (Int16)CompanyStatus.SanctionVerified).AsQueryable();
                            break;
                        default:
                            query = query.Where(elem => elem.Organization.Status != (Int16)CompanyStatus.Started).AsQueryable();
                            break;
                    }

                    total = query.Count();
                    int skipCount = 0;
                    if (pageIndex != int.MaxValue)
                    {
                        skipCount = Convert.ToInt32(pageIndex - 1) * size;
                    }
                    switch (sortDirection)
                    {
                        case 1:
                            supplierList = query.OrderBy(elem => elem.Organization.Party.PartyName).Skip(skipCount).Take(size).ToList();
                            break;
                        case 2:
                            supplierList = query.OrderByDescending(elem => elem.Organization.Party.PartyName).Skip(skipCount).Take(size).ToList();
                            break;
                        case 3:
                            supplierList = query.OrderByDescending(elem => elem.CreatedOn).Skip(skipCount).Take(size).ToList();
                            break;
                    }

                    foreach (var supplier in supplierList)
                    {
                        var supplierOrganization = new SupplierOrganization();
                        supplierOrganization.DetailsVerifiedDate = supplier.Organization.RegistrationVerifiedOn;
                        supplierOrganization.ProfileVerifiedDate = supplier.Organization.ProfileVerifiedOn;
                        supplierOrganization.ProjectSource = supplier.Organization.Party.ProjectSource.HasValue ? CommonMethods.Description((ProjectSource)supplier.Organization.Party.ProjectSource.Value) : string.Empty;
                        supplierOrganization.RegisteredDate = supplier.Organization.RegistrationSubmittedOn;
                        supplierOrganization.SignUpDate = supplier.Organization.CreatedOn;
                        supplierOrganization.SupplierId = supplier.Id;
                        supplierOrganization.SupplierOrganizationName = supplier.Organization.Party.PartyName;
                        supplierOrganization.SupplierStatus = supplier.Organization.Status;
                        supplierOrganization.SupplierUserId = supplier.Organization.Party.PartyPartyLinks.FirstOrDefault(p => p.PartyPartyLinkType.Trim().Equals(Constants.PRIMARY_ORGANIZATION)).Party1.Id;
                        var primaryContact = supplier.Organization.Party.PartyPartyLinks.FirstOrDefault(p => p.PartyPartyLinkType.Trim().Equals(Constants.CONTACT_ORGANIZATION) && p.Party1.Person.ContactPerson.ContactType == (short)ContactType.Primary);
                        if (null != primaryContact)
                        {
                            supplierOrganization.PrimaryContactName = primaryContact.Party1.PartyName;
                            supplierOrganization.PrimaryContactEmail = primaryContact.Party1.PartyContactMethodLinks.FirstOrDefault(m => m.ContactMethod.ContactMethodType.Trim().Equals(Constants.CONTACT_METHOD_EMAIL)) != null ? primaryContact.Party1.PartyContactMethodLinks.FirstOrDefault(m => m.ContactMethod.ContactMethodType.Trim().Equals(Constants.CONTACT_METHOD_EMAIL)).ContactMethod.Emails.FirstOrDefault().EmailAddress : string.Empty;
                        }
                        supplierOrganization.SupplierReferrers = supplier.SupplierReferrers != null ? supplier.SupplierReferrers.Select(elem => new Domain.Model.SupplierReferrer()
                        {
                            LandingReferrer = elem.LandingReferrer,
                            BuyerOrganizationId = elem.BuyerCampaign.RefBuyer.HasValue ? elem.BuyerCampaign.RefBuyer.Value : 0,
                            BuyerOrganizationName = elem.BuyerCampaign.Buyer != null ? elem.BuyerCampaign.Buyer.Organization.Party.PartyName : string.Empty
                        }).ToList() : null;
                        supplierOrganizations.Add(supplierOrganization);
                    }
                }
                Logger.Info("SupplierRepository : GetSupplierOrganization() : Exit the method.");
                return supplierOrganizations;
            }
            catch (Exception ex)
            {
                Logger.Error("SupplierRepository : GetSupplierOrganization() : Caught an exception" + ex);
                throw ex;
            }
        }

        public SupplierOrganization GetSupplierDetailsForDashboard(long supplierPartyId)
        {
            try
            {
                Logger.Info("SupplierRepository : GetSupplierDetailsForDashboard() : Enter the method.");
                var supplierOrganization = new SupplierOrganization();
                var supplier = new Supplier();
                using (var ctx = new SellerContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = true;
                    supplier = ctx.Suppliers.FirstOrDefault(elem => elem.Id == supplierPartyId);

                    if (null != supplier)
                    {
                        supplierOrganization.DetailsVerifiedDate = supplier.Organization.RegistrationVerifiedOn;
                        supplierOrganization.ProfileVerifiedDate = supplier.Organization.ProfileVerifiedOn;
                        supplierOrganization.ProjectSource = supplier.Organization.Party.ProjectSource.HasValue ? CommonMethods.Description((ProjectSource)supplier.Organization.Party.ProjectSource.Value) : string.Empty;
                        supplierOrganization.RegisteredDate = supplier.Organization.RegistrationSubmittedOn;
                        supplierOrganization.SignUpDate = supplier.Organization.CreatedOn;
                        supplierOrganization.SupplierId = supplier.Id;
                        supplierOrganization.SupplierOrganizationName = supplier.Organization.Party.PartyName;
                        supplierOrganization.SupplierStatus = supplier.Organization.Status;
                        supplierOrganization.SupplierUserId = supplier.Organization.Party.PartyPartyLinks.FirstOrDefault(p => p.PartyPartyLinkType.Trim().Equals(Constants.PRIMARY_ORGANIZATION)).Party1.Id;
                        var primaryContact = supplier.Organization.Party.PartyPartyLinks.FirstOrDefault(p => p.PartyPartyLinkType.Trim().Equals(Constants.CONTACT_ORGANIZATION) && p.Party1.Person.ContactPerson.ContactType == (short)ContactType.Primary);
                        if (null != primaryContact)
                        {
                            supplierOrganization.PrimaryContactName = primaryContact.Party1.PartyName;
                            supplierOrganization.PrimaryContactEmail = primaryContact.Party1.PartyContactMethodLinks.FirstOrDefault(m => m.ContactMethod.ContactMethodType.Trim().Equals(Constants.CONTACT_METHOD_EMAIL)) != null ? primaryContact.Party1.PartyContactMethodLinks.FirstOrDefault(m => m.ContactMethod.ContactMethodType.Trim().Equals(Constants.CONTACT_METHOD_EMAIL)).ContactMethod.Emails.FirstOrDefault().EmailAddress : string.Empty;
                        }
                    }
                }
                Logger.Info("SupplierRepository : GetSupplierDetailsForDashboard() : Exit the method.");
                return supplierOrganization;
            }
            catch (Exception ex)
            {
                Logger.Error("SupplierRepository : GetSupplierDetailsForDashboard() : Caught an exception" + ex);
                throw ex;
            }
        }

        public List<string> GetNotVerifiedSupplierNames(string supplierOrg)
        {
            try
            {
                Logger.Info("SupplierRepository : GetVerifiedBuyerNames() : Entering the method");
                List<string> supplierOrgList = null;
                using (var ctx = new SellerContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = true;

                    supplierOrgList = ctx.Suppliers.Where(u => u.Organization.Party.PartyName.ToLower().Contains(supplierOrg.ToLower()) && u.Organization.Status.Value != (short)CompanyStatus.Started).Select(u => u.Organization.Party.PartyName).Distinct().ToList();
                }
                Logger.Info("SupplierRepository : GetVerifiedBuyerNames() : Exiting the method");
                return supplierOrgList;
            }
            catch (Exception ex)
            {
                Logger.Error("SupplierRepository : GetVerifiedBuyerNames() : Caught an exception" + ex);
                throw ex;
            }
        }

        public List<string> GetSuppliersListForRegistration(string companyName)
        {
            List<string> supplierList = null;
            Logger.Info("SupplierRepository : GetSuppliersListForRegistration() : Enter into method");
            try
            {
                var prelist = new List<string>();
                using (var ctx = new SellerContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = true;
                    supplierList = ctx.Suppliers.Where(u => u.Organization.Party.PartyName.Contains(companyName)).Select(u => u.Organization.Party.PartyName).ToList();
                    prelist = ctx.CampaignInvitations.Where(u => u.SupplierCompanyName.Contains(companyName)).Select(u => u.SupplierCompanyName).ToList();
                }
                supplierList.AddRange(prelist);
                supplierList = supplierList.Distinct().ToList();
            }
            catch (Exception ex)
            {
                Logger.Error("SupplierRepository : GetSuppliersListForRegistration() : Caught an exception " + ex);
                throw ex;
            }
            Logger.Info("SupplierRepository : GetSuppliersListForRegistration() : Exit from method");
            return supplierList;
        }

        public List<SupplierOrganization> GetSuppliersForVerification(int pageNo, string sortParameter, int sortDirection, out int total, int sourceCheck, int viewOptions, int pageSize, string referrerName = "")
        {
            try
            {
                Logger.Info("SupplierRepository : GetSuppliersForVerification() : Enter  into method");

                var sortType = (SortOrder)sortDirection;
                var status = new List<Int16>();
                //var supplierProducts = new List<long>();
                //var publishedStatus = false;
                //var supplierProductStatusList = new List<Int16>();

                switch (viewOptions)
                {
                    case 1:
                        status.Add((Int16)CompanyStatus.Submitted);
                        status.Add((Int16)CompanyStatus.RegistrationVerified);
                        status.Add((Int16)CompanyStatus.ProfileVerified);
                        status.Add((Int16)CompanyStatus.SanctionVerified);
                        //supplierProducts.Add((Int16)Pillar.FinanceInsuranceTax);
                        //supplierProducts.Add((Int16)Pillar.HealthSafety);
                        //supplierProducts.Add((Int16)Pillar.DataSecurity);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.NotSubmitted);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.Submitted);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.PaymentDone);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.EvaluationStarted);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.Verified);
                        break;
                    case 2:
                        status.Add((Int16)CompanyStatus.Submitted);
                        status.Add((Int16)CompanyStatus.RegistrationVerified);
                        status.Add((Int16)CompanyStatus.ProfileVerified);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.NotSubmitted);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.Submitted);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.PaymentDone);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.EvaluationStarted);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.Verified);
                        //supplierProducts.Add((long)Pillar.FinanceInsuranceTax);
                        //supplierProducts.Add((long)Pillar.HealthSafety);
                        //supplierProducts.Add((long)Pillar.DataSecurity);
                        //publishedStatus = true;
                        break;
                    case 3:
                        status.Add((Int16)CompanyStatus.RegistrationVerified);
                        //status.Add((Int16)CompanyStatus.SanctionVerified);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.NotSubmitted);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.Submitted);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.PaymentDone);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.EvaluationStarted);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.Verified);
                        //supplierProducts.Add((long)Pillar.FinanceInsuranceTax);
                        //supplierProducts.Add((long)Pillar.HealthSafety);
                        //supplierProducts.Add((long)Pillar.DataSecurity);
                        //publishedStatus = true;
                        break;
                    case 4:
                        status.Add((Int16)CompanyStatus.Submitted);
                        status.Add((Int16)CompanyStatus.RegistrationVerified);
                        status.Add((Int16)CompanyStatus.ProfileVerified);
                        status.Add((Int16)CompanyStatus.SanctionVerified);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.PaymentDone);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.EvaluationStarted);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.Verified);
                        //supplierProducts.Add((Int16)Pillar.FinanceInsuranceTax);
                        break;
                    case 5:
                        status.Add((Int16)CompanyStatus.Submitted);
                        status.Add((Int16)CompanyStatus.RegistrationVerified);
                        status.Add((Int16)CompanyStatus.ProfileVerified);
                        status.Add((Int16)CompanyStatus.SanctionVerified);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.PaymentDone);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.EvaluationStarted);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.Verified);
                        //supplierProducts.Add((Int16)Pillar.HealthSafety);
                        break;
                    case 6:
                        status.Add((Int16)CompanyStatus.Submitted);
                        status.Add((Int16)CompanyStatus.RegistrationVerified);
                        status.Add((Int16)CompanyStatus.ProfileVerified);
                        status.Add((Int16)CompanyStatus.SanctionVerified);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.PaymentDone);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.EvaluationStarted);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.Verified);
                        //supplierProducts.Add((Int16)Pillar.DataSecurity);
                        break;
                }

                var supplierOrganizations = new List<SupplierOrganization>();
                List<Supplier> supplierPMList = null;
                var skipCount = (pageNo - 1) * pageSize;
                using (var ctx = new SellerContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = true;

                    var query = ctx.Suppliers.AsQueryable();

                    if (!string.IsNullOrWhiteSpace(referrerName))
                    {
                        query = query.Where(elem => elem.SupplierReferrers.Any(u => u.BuyerCampaign.Buyer.Organization.Party.PartyName.ToLower().Contains(referrerName.ToLower())));
                    }
                    if (sourceCheck == Convert.ToInt16(ProjectSource.SIM))
                    {
                        query = query.Where(elem => elem.Organization.Party.ProjectSource == (short)ProjectSource.SIM);
                    }
                    else if (sourceCheck == Convert.ToInt16(ProjectSource.CIPS))
                    {
                        query = query.Where(elem => elem.Organization.Party.ProjectSource == (short)ProjectSource.CIPS);
                    }
                    query = query.Where(v => status.Contains(v.Organization.Status.Value));
                    total = query.Count();
                    switch (sortParameter)
                    {
                        case "CompanyName":
                            switch (sortDirection)
                            {
                                case 1:
                                    supplierPMList = query.OrderBy(t => t.Organization.Party.PartyName).Skip(skipCount).Take(pageSize).ToList();
                                    break;
                                case 2:
                                    supplierPMList = query.OrderByDescending(t => t.Organization.Party.PartyName).Skip(skipCount).Take(pageSize).ToList();
                                    break;
                                case 3:
                                    supplierPMList = query.OrderBy(t => t.CreatedOn).Skip(skipCount).Take(pageSize).ToList();
                                    break;
                            }
                            break;
                        default:
                            supplierPMList = query.OrderBy(t => t.Organization.Party.PartyName).Skip(skipCount).Take(pageSize).ToList();
                            break;
                    }
                    foreach (var supplier in supplierPMList)
                    {
                        var supplierOrganization = new SupplierOrganization();
                        supplierOrganization.DetailsVerifiedDate = supplier.Organization.RegistrationVerifiedOn;
                        supplierOrganization.ProfileVerifiedDate = supplier.Organization.ProfileVerifiedOn;
                        supplierOrganization.ProjectSource = supplier.Organization.Party.ProjectSource.HasValue ? CommonMethods.Description((ProjectSource)supplier.Organization.Party.ProjectSource.Value) : string.Empty;
                        supplierOrganization.RegisteredDate = supplier.Organization.RegistrationSubmittedOn;
                        supplierOrganization.SignUpDate = supplier.Organization.CreatedOn;
                        supplierOrganization.SupplierId = supplier.Id;
                        supplierOrganization.SupplierOrganizationName = supplier.Organization.Party.PartyName;
                        supplierOrganization.SupplierStatus = supplier.Organization.Status;
                        supplierOrganization.SupplierUserId = supplier.Organization.Party.PartyPartyLinks.FirstOrDefault(p => p.PartyPartyLinkType.Trim().Equals(Constants.PRIMARY_ORGANIZATION)).Party1.Id;
                        var primaryContact = supplier.Organization.Party.PartyPartyLinks.FirstOrDefault(p => p.PartyPartyLinkType.Trim().Equals(Constants.CONTACT_ORGANIZATION) && p.Party1.Person.ContactPerson.ContactType == (short)ContactType.Primary);
                        if (null != primaryContact)
                        {
                            supplierOrganization.PrimaryContactName = primaryContact.Party1.PartyName;
                            supplierOrganization.PrimaryContactEmail = primaryContact.Party1.PartyContactMethodLinks.FirstOrDefault(m => m.ContactMethod.ContactMethodType.Trim().Equals(Constants.CONTACT_METHOD_EMAIL)) != null ? primaryContact.Party1.PartyContactMethodLinks.FirstOrDefault(m => m.ContactMethod.ContactMethodType.Trim().Equals(Constants.CONTACT_METHOD_EMAIL)).ContactMethod.Emails.FirstOrDefault().EmailAddress : string.Empty;
                        }
                        supplierOrganization.SupplierReferrers = supplier.SupplierReferrers != null ? supplier.SupplierReferrers.Select(elem => new Domain.Model.SupplierReferrer()
                        {
                            LandingReferrer = elem.LandingReferrer,
                            BuyerOrganizationId = elem.BuyerCampaign.RefBuyer.HasValue ? elem.BuyerCampaign.RefBuyer.Value : 0,
                            BuyerOrganizationName = elem.BuyerCampaign.Buyer != null ? elem.BuyerCampaign.Buyer.Organization.Party.PartyName : string.Empty
                        }).ToList() : null;
                        supplierOrganizations.Add(supplierOrganization);
                    }
                }
                Logger.Info("SupplierRepository : GetSuppliersForVerification() : Exit from method");
                return supplierOrganizations;
            }
            catch (Exception ex)
            {
                Logger.Error("SupplierRepository : GetSuppliersForVerification() : Caught an exception " + ex);
                throw;
            }
        }

        public List<SupplierCountBasedOnStage> GetSuppliersCountBasedOnStage(int sourceCheck, int viewOptions, string referrerName)
        {
            try
            {
                Logger.Info("SupplierRepository : GetSuppliersCountBasedOnStage() : Enter from method");
                var resultList = new List<SupplierCountBasedOnStage>();
                var checkModel = new SupplierCountBasedOnStage();
                var checkedModel = new SupplierCountBasedOnStage();
                var publishedModel = new SupplierCountBasedOnStage();
                //var supplierProducts = new List<long>();
                //var publishedStatus = false;
                //var supplierProductStatusList = new List<Int16>();
                var sourceCheckList = new List<long>();
                var status = new List<Int16>();
                switch (sourceCheck)
                {
                    case (Int16)ProjectSource.CIPS:
                        sourceCheckList.Add((long)ProjectSource.CIPS);
                        break;
                    case (Int16)ProjectSource.SIM:
                        sourceCheckList.Add((long)ProjectSource.SIM);
                        break;
                    default:
                        sourceCheckList.Add((long)ProjectSource.SIM);
                        sourceCheckList.Add((long)ProjectSource.CIPS);
                        break;
                }
                switch (viewOptions)
                {
                    case 1:
                        status.Add((Int16)CompanyStatus.Submitted);
                        status.Add((Int16)CompanyStatus.RegistrationVerified);
                        status.Add((Int16)CompanyStatus.ProfileVerified);
                        status.Add((Int16)CompanyStatus.SanctionVerified);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.NotSubmitted);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.Submitted);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.PaymentDone);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.EvaluationStarted);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.Verified);
                        break;
                    case 2:
                        status.Add((Int16)CompanyStatus.Submitted);
                        status.Add((Int16)CompanyStatus.RegistrationVerified);
                        status.Add((Int16)CompanyStatus.ProfileVerified);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.NotSubmitted);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.Submitted);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.PaymentDone);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.EvaluationStarted);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.Verified);
                        //publishedStatus = true;
                        break;
                    case 3:
                        status.Add((Int16)CompanyStatus.ProfileVerified);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.NotSubmitted);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.Submitted);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.PaymentDone);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.EvaluationStarted);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.Verified);
                        break;
                    case 4:
                    case 5:
                    case 6:
                        status.Add((Int16)CompanyStatus.Submitted);
                        status.Add((Int16)CompanyStatus.RegistrationVerified);
                        status.Add((Int16)CompanyStatus.ProfileVerified);
                        status.Add((Int16)CompanyStatus.SanctionVerified);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.PaymentDone);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.EvaluationStarted);
                        //supplierProductStatusList.Add((Int16)SupplierProductStatus.Verified);
                        break;
                }
                //switch (viewOptions)
                //{
                //    case 1:
                //    case 2:
                //    case 3:
                //        supplierProducts.Add((long)Pillar.FinanceInsuranceTax);
                //        supplierProducts.Add((long)Pillar.HealthSafety);
                //        supplierProducts.Add((long)Pillar.DataSecurity);
                //        break;
                //    case 4:
                //        supplierProducts.Add((Int16)Pillar.FinanceInsuranceTax);
                //        break;
                //    case 5:
                //        supplierProducts.Add((Int16)Pillar.HealthSafety);
                //        break;
                //    case 6:
                //        supplierProducts.Add((Int16)Pillar.DataSecurity);
                //        break;
                //}
                using (var ctx = new SellerContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = true;
                    var query = ctx.Suppliers.Where(u => status.Contains(u.Organization.Status.Value)
                    //&& u.SupplierProducts.Any(sp => supplierProductStatusList.Contains(sp.Status.Value) && supplierProducts.Contains(sp.ProductId)) 
                    && sourceCheckList.Contains(u.Organization.Party.ProjectSource.Value)).AsQueryable();
                    //if (publishedStatus)
                    //{
                    //    query = query.Where(v => v.IsPublished.Value != publishedStatus).AsQueryable();

                    //}

                    if (!string.IsNullOrWhiteSpace(referrerName))
                    {
                        query = query.Where(u => u.SupplierReferrers.Any(elem => elem.BuyerCampaign.Buyer.Organization.Party.PartyName.ToLower().Contains(referrerName.ToLower()))).AsQueryable();
                    }
                    // Check Stage
                    checkModel.Stage = "Check (Needs action)";
                    checkModel.DetailsScore = query.Count(u => u.Organization.Status < (Int16)CompanyStatus.RegistrationVerified);
                    checkModel.ProfileScore = query.Count(u => u.Organization.Status < (Int16)CompanyStatus.ProfileVerified);
                    checkModel.SanctionScore = query.Count(u => u.Organization.Status == (Int16)CompanyStatus.ProfileVerified);
                    //checkModel.FITScore = query.Count(u => u.SupplierProducts.Any(v => v.ProductId == (Int16)Pillar.FinanceInsuranceTax && v.Status == (Int16)SupplierProductStatus.PaymentDone));
                    //checkModel.HSScore = query.Count(u => u.SupplierProducts.Any(v => v.ProductId == (Int16)Pillar.HealthSafety && v.Status == (Int16)SupplierProductStatus.PaymentDone));
                    //checkModel.DSScore = query.Count(u => u.SupplierProducts.Any(v => v.ProductId == (Int16)Pillar.DataSecurity && v.Status == (Int16)SupplierProductStatus.PaymentDone));
                    resultList.Add(checkModel);
                    //Checked Stage
                    checkedModel.Stage = "Checked";
                    checkedModel.DetailsScore = query.Count(u => u.Organization.Status >= (Int16)CompanyStatus.RegistrationVerified);
                    checkedModel.ProfileScore = query.Count(u => u.Organization.Status >= (Int16)CompanyStatus.ProfileVerified);
                    checkedModel.SanctionScore = query.Count(u => u.Organization.Status >= (Int16)CompanyStatus.SanctionVerified);
                    //checkedModel.FITScore = query.Count(u => u.SupplierProducts.Any(v => v.ProductId == (Int16)Pillar.FinanceInsuranceTax && (v.Status == (Int16)SupplierProductStatus.EvaluationStarted || v.Status == (Int16)SupplierProductStatus.Verified)));
                    //checkedModel.HSScore = query.Count(u => u.SupplierProducts.Any(v => v.ProductId == (Int16)Pillar.HealthSafety && (v.Status == (Int16)SupplierProductStatus.EvaluationStarted || v.Status == (Int16)SupplierProductStatus.Verified)));
                    //checkedModel.DSScore = query.Count(u => u.SupplierProducts.Any(v => v.ProductId == (Int16)Pillar.DataSecurity && (v.Status == (Int16)SupplierProductStatus.EvaluationStarted || v.Status == (Int16)SupplierProductStatus.Verified)));
                    resultList.Add(checkedModel);
                    //Published Stage
                    publishedModel.Stage = "Published";
                    publishedModel.DetailsScore = query.Count(u => u.Organization.Status >= (Int16)CompanyStatus.ProfileVerified);
                    publishedModel.ProfileScore = publishedModel.DetailsScore;
                    publishedModel.SanctionScore = query.Count(u => u.Organization.Status >= (Int16)CompanyStatus.SanctionVerified);

                    //publishedModel.FITScore = query.Count(u => u.SupplierProducts.Any(v => v.ProductId == (Int16)Pillar.FinanceInsuranceTax && v.Status == (Int16)SupplierProductStatus.Published));
                    //publishedModel.HSScore = query.Count(u => u.SupplierProducts.Any(v => v.ProductId == (Int16)Pillar.HealthSafety && v.Status == (Int16)SupplierProductStatus.Published));
                    //publishedModel.DSScore = query.Count(u => u.SupplierProducts.Any(v => v.ProductId == (Int16)Pillar.DataSecurity && v.Status == (Int16)SupplierProductStatus.Published));
                    resultList.Add(publishedModel);

                }
                Logger.Info("SupplierRepository : GetSuppliersCountBasedOnStage() : Exit from method");
                return resultList;
            }
            catch(Exception ex)
            {
                Logger.Error("SupplierRepository : GetSuppliersCountBasedOnStage() : Caught an exception " + ex);
                throw;
            }
        }
    }
}
