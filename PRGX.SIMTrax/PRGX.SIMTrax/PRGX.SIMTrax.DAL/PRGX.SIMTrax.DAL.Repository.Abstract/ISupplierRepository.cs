using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.Domain.Model;
using System.Collections.Generic;

namespace PRGX.SIMTrax.DAL.Repository.Abstract
{
    public interface ISupplierRepository : IGenericRepository<Supplier>
    {
        bool UpdateSellerDetails(Supplier seller, long partyId);

        ProfileSummary GetSellerProfilePercentage(long sellerPartyId, long sellerId, long organisationId);

        List<SupplierOrganization> GetSupplierOrganization(string fromdate, string toDate, out int total, int pageIndex, short source, int size, int sortDirection, long supplierId = 0, string supplierName = "", long status = 0, string referrerName = "");

        SupplierOrganization GetSupplierDetailsForDashboard(long supplierPartyId);

        List<string> GetNotVerifiedSupplierNames(string supplierOrg);

        List<string> GetSuppliersListForRegistration(string companyName);

        List<SupplierOrganization> GetSuppliersForVerification(int pageNo, string sortParameter, int sortDirection, out int total, int sourceCheck, int viewOptions, int pageSize, string referrerName = "");

        List<SupplierCountBasedOnStage> GetSuppliersCountBasedOnStage(int sourceCheck, int viewOptions, string referrerName);
    }
}
