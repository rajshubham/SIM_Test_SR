using PRGX.SIMTrax.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository.Abstract
{
    public interface IBuyerCampaignRepository : IGenericRepository<BuyerCampaign>
    {
        BuyerCampaign GetCampaignInfo(long campaignId, string campaignUrl = "");

        List<BuyerCampaign> GetAssignedCampaigns(long auditorId, int index, out int total);

        List<BuyerCampaign> GetCampaignsAwaitingAction(out int total, int PageNo, long auditorId);

        List<BuyerCampaign> GetSubmittedOrApprovedCampaigns(short campaignStatus, int index, out int total);

        List<Domain.Model.SupplierReferrer> GetSupplierReferrerBuyerCampaignDetails(int pageNo, string buyerName, string campaignName, long supplierId, out int total);
    }
}
