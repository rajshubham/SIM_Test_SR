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

namespace PRGX.SIMTrax.DAL.Repository.Repository
{
    public class BuyerCampaignRepository : GenericRepository<BuyerCampaign>, IBuyerCampaignRepository
    {
        public BuyerCampaignRepository(DbContext context) : base(context) { }

        public BuyerCampaign GetCampaignInfo(long campaignId, string campaignUrl = "")
        {
            try
            {
                Logger.Info("BuyerCampaignRepository : GetCampaignInfo() : Enter the method");

                var campaign = new BuyerCampaign();
                if (campaignId > 0)
                    campaign = this.All().Where(x => x.Id == campaignId).Include(x => x.Buyer.Organization.Party).Include(x => x.CampaignMessages).Include(x => x.Document).Include(x => x.EmailTemplate).FirstOrDefault();
                else if(!string.IsNullOrWhiteSpace(campaignUrl))
                    campaign = this.All().Where(x => x.CampaignUrl == campaignUrl).Include(x => x.Buyer.Organization.Party).Include(x => x.CampaignMessages).Include(x => x.Document).Include(x => x.EmailTemplate).FirstOrDefault();

                Logger.Info("BuyerCampaignRepository : GetCampaignInfo() : Enter the method");

                return campaign;

            }
            catch (Exception ex)
            {
                Logger.Error("BuyerCampaignRepository : GetCampaignInfo() : Caught an error" + ex);
                throw;
            }
        }

        public List<BuyerCampaign> GetAssignedCampaigns(long auditorId, int index, out int total)
        {
            Logger.Info("BuyerCampaignRepository : GetAssignedCampaigns() : Entering the method");
            var campaignList = new List<BuyerCampaign>();
            short campaignType = (short)CampaignType.NotRegistered;
            short campaignStatus = (short)CampaignStatus.Assigned;
            try
            {
                int pageSize = 5;
                int skipcount = (index - 1) * pageSize;

                var query = this.All().Include(PRGX => PRGX.Buyer.Organization.Party).Include(x => x.User.Person.Party).Where(item => (item.RefAuditorId == auditorId && item.CampaignType != campaignType && item.CampaignStatus == campaignStatus) || (item.RefCreatedBy == auditorId && item.CampaignStatus < campaignStatus && item.CampaignType != campaignType)).AsQueryable();
                total = query.Count();

                campaignList = query.OrderBy(u => u.CreatedOn).Skip(skipcount).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerCampaignRepository : GetAssignedCampaigns() : Caught an exception" + ex);
                throw ex;
            }
            Logger.Info("BuyerCampaignRepository : GetAssignedCampaigns() : Exiting the method");
            return campaignList;
        }

        public List<BuyerCampaign> GetCampaignsAwaitingAction(out int total, int PageNo, long auditorId)
        {
            Logger.Info("BuyerCampaignRepository : GetCampaignsAwaitingAction() : Enter into Method");
            var campaignListPM = new List<BuyerCampaign>();
            try
            {
                var pageSize = 5;
                var skipCount = pageSize * (PageNo - 1);
                int campaignSubmitted = (int)CampaignStatus.Submitted;

                var query = this.All().Include(PRGX => PRGX.Buyer.Organization.Party).Include(x => x.User.Person.Party).Where(u => u.RefCreatedBy == auditorId && u.RefAuditorId != auditorId && u.RefAuditorId != null && u.CampaignStatus < campaignSubmitted).AsQueryable();

                campaignListPM = query.OrderBy(u => u.CampaignName).Skip(skipCount).Take(pageSize).ToList();

                total = query.Count();
            }
            catch (Exception e)
            {
                Logger.Error("BuyerCampaignRepository : GetCampaignsAwaitingAction() : Caught an Error " + e);
                throw e;
            }
            Logger.Info("BuyerCampaignRepository : GetCampaignsAwaitingAction() : Exit from Method");
            return campaignListPM;
        }

        public List<BuyerCampaign> GetSubmittedOrApprovedCampaigns(short campaignStatus, int index, out int total)
        {
            Logger.Info("BuyerCampaignRepository : GetSubmittedOrApprovedCampaigns() : Entering the method");
            var campaignList = new List<BuyerCampaign>();
            short campaignType = (short)CampaignType.PreRegistered;
            int size = Convert.ToInt32(5);
            var skipCount = (index - 1) * size;
            try
            {
                total = this.All().Count(u => u.CampaignStatus == campaignStatus && u.CampaignType == campaignType);

                campaignList = this.All().Include(x => x.Buyer.Organization.Party).Include(x => x.User.Person.Party).Where(u => u.CampaignStatus == campaignStatus && u.CampaignType == campaignType).OrderBy(v => v.Id).Skip(skipCount).Take(size).ToList();
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerCampaignRepository : GetSubmittedOrApprovedCampaigns() : Caught an exception" + ex);
                throw ex;
            }
            Logger.Info("BuyerCampaignRepository : GetSubmittedOrApprovedCampaigns() : Exiting the method");
            return campaignList;
        }

        public List<Domain.Model.SupplierReferrer> GetSupplierReferrerBuyerCampaignDetails(int pageNo, string buyerName, string campaignName, long supplierId, out int total)
        {
            try
            {
                Logger.Info("BuyerCampaignRepository : GetSupplierReferrerBuyerCampaignDetails() : Enter into Method");

                var pageSize = 5;
                var skipCount = (pageNo - 1) * pageSize;
                total = 0;

                var notRegCampaign = (long)CampaignType.NotRegistered;
                var referrerList = new List<Domain.Model.SupplierReferrer>();
                using (var ctx = new CampaignContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = true;

                    var query = ctx.BuyerCampaigns.Where(b => b.CampaignType == notRegCampaign).AsQueryable();
                    if (!string.IsNullOrWhiteSpace(buyerName))
                        query = query.Where(x => x.Buyer.Organization.Party.PartyName.Contains(buyerName)).AsQueryable();
                    if (!string.IsNullOrWhiteSpace(campaignName))
                        query = query.Where(x => x.CampaignName.Contains(campaignName)).AsQueryable();

                    total = query.Count();
                    var campaignList = query.OrderByDescending(v => v.SupplierReferrers.Count(t => t.RefSupplier == supplierId)).Skip(skipCount).Take(pageSize).ToList();

                    referrerList = campaignList.Select(r => new Domain.Model.SupplierReferrer()
                    {
                        LandingReferrer = r.SupplierReferrers.FirstOrDefault(v => v.RefSupplier == supplierId) != null ? r.SupplierReferrers.FirstOrDefault(v => v.RefSupplier == supplierId).LandingReferrer : false,
                        CampaignName = r.CampaignName,
                        CampaignId = r.Id,
                        BuyerOrganizationId = r.RefBuyer.HasValue ? r.RefBuyer.Value : 0,
                        BuyerOrganizationName = r.Buyer != null ? r.Buyer.Organization.Party.PartyName : string.Empty,
                        IsSupplierReferred = r.SupplierReferrers.FirstOrDefault(u => u.RefSupplier == supplierId) != null ? true : false
                    }).OrderByDescending(u => u.IsSupplierReferred).OrderByDescending(u => u.LandingReferrer).ToList();
                }
                Logger.Info("BuyerCampaignRepository : GetSupplierReferrerBuyerCampaignDetails() : Exit from Method");
                return referrerList;
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerCampaignRepository : GetSupplierReferrerBuyerCampaignDetails() : Caught an Error " + ex);
                throw;
            }
        }
    }
}
