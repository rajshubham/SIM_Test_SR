using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.Domain.Util;
using PRGX.SIMTrax.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.ServiceFacade.Abstract
{
    public interface ICampaignServiceFacade
    {
        Campaign GetCampaignInfo(long campaignId);

        bool AddOrUpdateBuyerCampaign(Campaign campaignModel, Document documentLogo, long auditorId);

        List<Campaign> GetBuyerCampaignDetailsForDashboard(out int totalCampaigns, long partyId, int pageNumber, int sortDirection);

        bool AssignCampaignToAuditor(long auditorId, int campaignId, long loggedInAuditor);

        List<Campaign> GetAssignedCampaigns(long auditorId, int index, out int total);

        List<Campaign> GetCampaignsAwaitingAction(out int total, int PageNo, long auditorId);

        bool UpdateCampaignPreRegFilePath(long campaignId, string filePath);

        bool InsertListOfPreRegSuppliers(List<CampaignPreRegSupplier> campaignPreRegSupplierDomain, long auditorId);

        bool UpdateListOfPreRegSupplier(List<CampaignPreRegSupplier> campaignPreRegSupplierList, long auditorId);

        List<CampaignPreRegSupplier> GetPreRegSupplierInCampaign(int campaignId, string filterCriteria, out int total, int pageIndex, int size);

        bool DeletePreRegSupplier(long preRegSupplierId);

        bool DeleteInvalidPreRegSupplierBasedOnCampaign(int campaignId);

        bool UpdateCampaignStatus(long campaignId, short campaignStatus);

        bool UpdateCampaignSupplierCount(int campaignId);

        List<Campaign> GetSubmittedOrApprovedCampaigns(short campaignStatus, int index, out int total);

        bool CompareSupplierCountAndMasterVendor(int campaignId);

        List<CampaignReleaseSupplier> GetPreRegSupplierListwithPasswordString(long campaignId);

        bool UpdatePreRegSupplierPassword(long preRegSupplierId, string password);

        bool UpdateCampaignDownloadStatus(long campaignId, bool isDownloaded);

        bool GetCampaignUrlSpecificForBuyer(string link, CampaignType campaignType);

        Campaign GetCampaignInfoBasedOnCampaignURL(string link);

        long ValidatePreRegSupplierCode(string loginId, string encryptedPassword, out bool isRegistered);

        SellerRegister GetPreRegSupplierDetails(long id);

        bool AddSupplierReferrer(long campaignId, long supplierId, bool landingReferrer);

        bool SetPreRegSupplierToRegistered(long id, long supplierId);

        CampaignPreRegSupplier GetPublicDataRecord(long id);

        List<SupplierReferrer> GetSupplierReferrerBuyerCampaignDetails(int pageNo, string buyerName, string campaignName, long supplierId, out int total);

        bool UpdateSupplierReferrerDetails(long supplierId, string[] assignReferrerCampaign, string[] removeReferrerCampaign, string[] primaryReferrerCampaign, long auditorId);
    }
}
