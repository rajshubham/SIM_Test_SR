using PRGX.SIMTrax.DAL.Entity;
using System;
using System.Collections.Generic;
using PRGX.SIMTrax.Domain;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.Domain.Util;

namespace PRGX.SIMTrax.DAL.Repository.Abstract
{
    public interface IBuyerRepository : IGenericRepository<Buyer>
    {
        List<BuyerOrganization> GetBuyerOrganization(int status, long buyerRole, string fromdate, string toDate, out int total, int pageIndex, int pageSize, int sortDirection, string sortParameter, string buyerName = "");

        Buyer GetBuyerOrganizationDetailsByPartyId(long organizationPartyId);

        long GetBuyerPrimaryContactPartyId(long buyerPartyId);

        List<BuyerOrganization> GetNotActivatedBuyerOrganization(int pageIndex, int pageSize, int sortDirection, string sortParameter, out int total);

        List<SupplierDetails> GetSuppliers(BuyerSupplierSearchFilter model, long companyPartyId,long userPartyId,out int totalRecords);

        List<string> GetVerifiedBuyerNames(string buyerOrg);

        BuyerOrganization GetBuyerDetailsForDashboard(long partyId);

        List<ItemList> GetBuyersList();

        List<DiscountVoucher> GetAllVouchers(int currentPage, string sortParameter, int sortDirection, out int total, int count, long buyerPartyId = 0);

        List<BuyerCampaign> GetBuyerCampaignDetailsForDashboard(out int totalCampaigns, long partyId, int pageNumber, int sortDirection);
    }
}
