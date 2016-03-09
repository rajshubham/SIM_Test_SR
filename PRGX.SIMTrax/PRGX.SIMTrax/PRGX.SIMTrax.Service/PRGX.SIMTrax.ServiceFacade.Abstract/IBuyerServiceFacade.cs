using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRGX.SIMTrax.Domain.Util;

namespace PRGX.SIMTrax.ServiceFacade.Abstract
{
    public interface IBuyerServiceFacade
    {
        bool AddBuyerUser(BuyerRegister buyerRegisterModel);

        List<BuyerOrganization> GetBuyerOrganizations(int status, string fromdate, string toDate, out int total, int pageIndex, long buyerRole, int pageSize, int sortDirection, string sortParameter, string buyerName = "");

        bool VerifyBuyerCompanyDetails(BuyerRegister buyerRegisterModel, long profileVerifiedBy);

        BuyerRegister GetBuyerOrganizationDetailsByPartyId(long organizationPartyId);

        bool ActivateBuyer(long buyerPartyId, long auditorPartyId, long roleId);

        List<BuyerOrganization> GetNotActivatedBuyerOrganization(int pageIndex, int pageSize, int sortDirection, string sortParameter, out int total);

        List<SupplierDetails> GetSuppliers(BuyerSupplierSearchFilter model, long companyPartyId, long userPartyId, out int totalRecords);

        List<string> GetSuppliersListForIntellisense(string text);

        bool ChangeAccessType(long buyerPartyId, long auditorPartyId, long roleId);

        List<string> GetVerifiedBuyerNames(string buyerOrg);

        BuyerOrganization GetBuyerDetailsForDashboard(long partyId);

        List<ItemList> GetBuyersList();

        bool AddorUpdateVoucher(Voucher voucher, long userId);

        bool IsVoucherAlreadyExists(string code);

        Voucher GetDiscountVoucherById(string voucherCode);

        List<Voucher> GetAllVouchers(int currentPage, string sortParameter, int sortDirection, out int total, int count, long buyerPartyId = 0);

        int GetMasterVendorValue(long buyerId);

        List<ItemList> GetVoucherListForBuyer(long buyerId);
    }
}
