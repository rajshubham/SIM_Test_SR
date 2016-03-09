using EntityFramework.BulkInsert.Extensions;
using EntityFramework.Extensions;
using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Abstract;
using PRGX.SIMTrax.DAL.Repository.Context;
using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PRGX.SIMTrax.DAL.Repository.Repository
{
    public class CampaignInvitationRepository : GenericRepository<CampaignInvitation>, ICampaignInvitationRepository
    {
        public CampaignInvitationRepository(DbContext context) : base(context) { }

        public bool InsertListOfPreRegSupplier(List<CampaignInvitation> campaignPreRegSupplierList)
        {
            Logger.Info("CampaignInvitationRepository : InsertListOfPreRegSupplier() : Entering the method");
            var result = false;
            try
            {
                using (var ctx = new CampaignContext())
                {
                    using (var transactionScope = new TransactionScope())
                    {
                        foreach (var campaignPreRegSupplier in campaignPreRegSupplierList)
                        {

                            if (ctx.Parties.Where(t => t.PartyName.ToLower() == campaignPreRegSupplier.SupplierCompanyName.ToLower()).ToList().Count > 0)
                            {
                                campaignPreRegSupplier.InvalidComments = ReadResource.GetResource(Constants.ORGANISATION_ALREADY_EXISTS, ResourceType.Message);
                                campaignPreRegSupplier.IsRegistered = false;
                                campaignPreRegSupplier.IsValid = false;
                            }
                            else if (ctx.CampaignInvitations.Where(t => (t.SupplierCompanyName.ToLower() == campaignPreRegSupplier.SupplierCompanyName.ToLower() && t.RefCampaign != campaignPreRegSupplier.RefCampaign)).ToList().Count > 0)
                            {
                                campaignPreRegSupplier.InvalidComments = ReadResource.GetResource(Constants.SUPPLIER_REGISTERED_BY_OTHER_CAMPAIGN, ResourceType.Message);
                                campaignPreRegSupplier.IsRegistered = false;
                                campaignPreRegSupplier.IsValid = false;
                            }
                            else if (campaignPreRegSupplierList.Where(t => t.SupplierCompanyName.ToLower() == campaignPreRegSupplier.SupplierCompanyName.ToLower()
                                && t.EmailAddress == campaignPreRegSupplier.EmailAddress).ToList().Count > 1)
                            {
                                campaignPreRegSupplier.InvalidComments = ReadResource.GetResource(Constants.DUPLICATE_PRE_REG_SUPPLIER_RECORD, ResourceType.Message);
                                campaignPreRegSupplier.IsRegistered = false;
                                campaignPreRegSupplier.IsValid = false;
                            }
                            else
                            {
                                campaignPreRegSupplier.IsRegistered = false;
                                campaignPreRegSupplier.IsValid = true;
                            }
                        }
                        ctx.BulkInsert(campaignPreRegSupplierList);
                        ctx.SaveChanges();
                        transactionScope.Complete();
                    }
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("CampaignInvitationRepository : InsertListOfPreRegSupplier() : Caught an exception " + ex);
                throw;
            }
            Logger.Info("CampaignInvitationRepository : InsertListOfPreRegSupplier() : Exiting the method");
            return result;
        }

        public List<CampaignInvitation> GetPreRegSupplierInCampaign(int campaignId, string filterCriteria, out int total, int pageIndex, int size)
        {
            try
            {
                Logger.Info("CampaignPreRegSupplierRepository : GetPreRegSupplierInCampaign() : Entering the method");
                total = 0;

                int skipcount = (pageIndex - 1) * size;
                var preRegSupplierList = new List<CampaignInvitation>();
                var query = this.All().Include(x => x.Region).Include(x => x.BuyerCampaign).Include(x => x.BuyerCampaign.Buyer.Organization.Party).Include(x => x.BuyerCampaign.EmailTemplate).AsQueryable();
                if (filterCriteria != "")
                {
                    bool isValid = Convert.ToBoolean(filterCriteria);
                   query = query.Where(x => x.IsValid == isValid).AsQueryable();
                }
                query = query.Where(x => x.RefCampaign == campaignId).AsQueryable();
                total = query.Count();

                preRegSupplierList = query.OrderBy(u => u.Id).Skip(skipcount).Take(size).ToList();
                Logger.Info("CampaignInvitationRepository : GetPreRegSupplierInCampaign() : Exiting the method");
                return preRegSupplierList;
            }
            catch (Exception ex)
            {
                Logger.Error("CampaignInvitationRepository : GetPreRegSupplierInCampaign() : Caught an exception" + ex);
                throw;
            }
        }

        public bool UpdateListOfPreRegSupplier(List<CampaignInvitation> campaignPreRegSupplierList)
        {
            var result = false;
            try
            {
                Logger.Info("CampaignInvitationRepository : UpdateListOfPreRegSupplier() : Entering the method");
                using (var ctx = new CampaignContext())
                {
                    foreach (var campaignPreRegSupplier in campaignPreRegSupplierList)
                    {

                        if (ctx.Credentials.Where(t => t.LoginId == campaignPreRegSupplier.EmailAddress).ToList().Count > 0)
                        {
                            campaignPreRegSupplier.InvalidComments = ReadResource.GetResource(Constants.ORGANISATION_ALREADY_EXISTS, ResourceType.Message);
                            campaignPreRegSupplier.IsRegistered = false;
                            campaignPreRegSupplier.IsValid = false;
                        }
                        else if (ctx.CampaignInvitations.Where(t => (t.EmailAddress == campaignPreRegSupplier.EmailAddress && t.RefCampaign != campaignPreRegSupplier.RefCampaign) || (t.SupplierCompanyName == campaignPreRegSupplier.SupplierCompanyName && t.RefCampaign != campaignPreRegSupplier.RefCampaign)).ToList().Count > 0)
                        {
                            campaignPreRegSupplier.InvalidComments = ReadResource.GetResource(Constants.SUPPLIER_REGISTERED_BY_OTHER_CAMPAIGN, ResourceType.Message);
                            campaignPreRegSupplier.IsRegistered = false;
                            campaignPreRegSupplier.IsValid = false;
                        }
                        else if (campaignPreRegSupplierList.Where(t => t.SupplierCompanyName.ToLower() == campaignPreRegSupplier.SupplierCompanyName.ToLower()
                            || t.EmailAddress == campaignPreRegSupplier.EmailAddress).ToList().Count > 1)
                        {
                            campaignPreRegSupplier.InvalidComments = ReadResource.GetResource(Constants.DUPLICATE_PRE_REG_SUPPLIER_RECORD, ResourceType.Message);
                            campaignPreRegSupplier.IsRegistered = false;
                            campaignPreRegSupplier.IsValid = false;
                        }
                        else
                        {
                            campaignPreRegSupplier.IsRegistered = false;
                            campaignPreRegSupplier.IsValid = true;
                        }
                        //var filter = ctx.CampaignPreRegSuppliers.Where(item => item.PreRegSupplierId == campaignPreRegSupplier.PreRegSupplierId);
                        ctx.CampaignInvitations.Where(item => item.Id == campaignPreRegSupplier.Id).Update(i => new CampaignInvitation
                        {
                            AddressLine1 = campaignPreRegSupplier.AddressLine1,
                            AddressLine2 = campaignPreRegSupplier.AddressLine2,
                            RefCampaign = campaignPreRegSupplier.RefCampaign,
                            City = campaignPreRegSupplier.City,
                            RefCountry = campaignPreRegSupplier.RefCountry,
                            Identifier1 = campaignPreRegSupplier.Identifier1,
                            IdentifierType1 = campaignPreRegSupplier.IdentifierType1,
                            Identifier2 = campaignPreRegSupplier.Identifier2,
                            IdentifierType2 = campaignPreRegSupplier.IdentifierType2,
                            Identifier3 = campaignPreRegSupplier.Identifier3,
                            IdentifierType3 = campaignPreRegSupplier.IdentifierType3,
                            FirstName = campaignPreRegSupplier.FirstName,
                            InvalidComments = campaignPreRegSupplier.InvalidComments,
                            IsRegistered = campaignPreRegSupplier.IsRegistered,
                            IsValid = campaignPreRegSupplier.IsValid,
                            JobTitle = campaignPreRegSupplier.JobTitle,
                            LastName = campaignPreRegSupplier.LastName,
                            EmailAddress = campaignPreRegSupplier.EmailAddress,
                            UltimateParent = campaignPreRegSupplier.UltimateParent,
                            IsSubsidary = campaignPreRegSupplier.IsSubsidary,
                            State = campaignPreRegSupplier.State,
                            SupplierCompanyName = campaignPreRegSupplier.SupplierCompanyName,
                            Telephone = campaignPreRegSupplier.Telephone,
                            ZipCode = campaignPreRegSupplier.ZipCode,
                            RefLastUpdatedBy = campaignPreRegSupplier.RefLastUpdatedBy
                        });
                    }
                    Logger.Info("CampaignInvitationRepository : UpdateListOfPreRegSupplier() : Exiting the method");
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("CampaignInvitationRepository : UpdateListOfPreRegSupplier() : Caught an exception" + ex);
                throw;
            }
            return result;
        }

        public List<CampaignInvitation> GetPreRegSupplierListwithPasswordString(long campaignId)
        {
            Logger.Info("CampaignInvitationRepository : GetPreRegSupplierListwithPasswordString() : Entering the method");
            var supplierInvitationList = new List<CampaignInvitation>();
            try
            {
                supplierInvitationList = this.All().Include(x => x.BuyerCampaign.Buyer.Organization.Party).ToList();
            }
            catch (Exception ex)
            {
                Logger.Error("CampaignInvitationRepository : GetPreRegSupplierListWithPasswordString() : Caught an exception" + ex);
                throw;
            }
            Logger.Info("CampaignInvitationRepository : GetPreRegSupplierListwithPasswordString() : Exiting the method");
            return supplierInvitationList;
        }

        public CampaignInvitation GetCampaignInvitationRecord(long id)
        {
            try
            {
                Logger.Info("CampaignPreRegSupplierRepository : GetPreRegSupplierInCampaign() : Entering the method");

                var preRegSupplier = new CampaignInvitation();
                preRegSupplier = this.All().Include(x => x.Region).Include(x => x.BuyerCampaign).Include(x => x.BuyerCampaign.Buyer.Organization.Party).Include(x => x.BuyerCampaign.EmailTemplate).FirstOrDefault(x => x.Id == id);

                Logger.Info("CampaignInvitationRepository : GetPreRegSupplierInCampaign() : Exiting the method");
                return preRegSupplier;
            }
            catch (Exception ex)
            {
                Logger.Error("CampaignInvitationRepository : GetPreRegSupplierInCampaign() : Caught an exception" + ex);
                throw;
            }
        }
    }
}
