using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.ViewModel;
using System.Collections.Generic;

namespace PRGX.SIMTrax.ServiceFacade.Abstract
{
    public interface IPartyServiceFacade
    {
        bool IsOrganisationExists(string organisationName);

        bool UpdateSellerPartyDetails(SellerRegister registerModel);

        bool UpdatePartyContactDetails(SellerRegister registerModel);

        OrganizationDetail GetOrganizationDetail(long organizationPartyId);

        SellerRegister GetCompanyDetailsByPartyId(long partyId);

        SellerRegister GetCapabilityDetailsByPartyId(long partyId);

        SellerRegister GetMarketingDetailsByPartyId(long partyId);
        bool AddOrUpdateSellerLogoDetails(Document document,long sellerPartyId);
        bool SaveCompanyDetails(SellerRegister model);

        bool SaveCapabilityDetails(SellerRegister model);

        bool SaveMarketingDetails(SellerRegister model);

        List<long> GetIndustryCodesByOrganisationPartyId(long sellerPartyId);

        SellerRegister GetSellerOrganizationDetailsByPartyId(long organizationPartyId);
        long AddOrUpdateAddressList(Address address, long sellerPartyId, long sellerUserPartyId);

        ContactPerson GetContactByRoleAndPartyId(long sellerPartyId,int contactType);
        bool AddOrUpdateContactsList(ContactPerson contact,long SellerPartyId,long sellerUserPartyId);
        List<Address> GetAddressDetailsByPartyId(long partyId);
        List<ContactPerson> GetContactDetailsByPartyId(long partyId);
        List<Invitee> GetReferenceDetailsBySellerId(long sellerId);
        List<BankAccount> GetBankDetailsByOrganisationId(long organisationId);
        bool AddOrUpdateReferenceDetails(Invitee referenceDetail, long sellerId,long sellerUserPartyId);
        bool AddOrUpdateBankDetails(BankAccount bankDetail, long organisationId, long sellerUserPartyId);

        bool DeleteAddressById(long addressId);
        int BuyersAssignedToContact(long contactPartyId);
        bool DeleteContactById(long contactPartyId,long sellerPartyId);
        bool DeleteReferenceById(long referenceId);
        bool DeleteBankAccountById(long bankId);
        List<BuyerSupplierReferenceList> BuyerSupplierReferenceList(int pageNo, string sortParameter, int sortDirection, string buyerName, long sellerId, long referenceId, out int totalRecords);
        bool AddOrRemoveBuyerSupplierReferenceDetails(bool isAdd, long buyerId, long referecneId);
        List<BuyerSupplierAddressList> BuyerSupplierAddressList(int pageNo, string sortParameter, int sortDirection, string buyerName, long sellerPartyId, long addressId, out int totalRecords);
        bool AddOrRemoveBuyerSupplierAddressDetails(bool isAdd, long buyerPartyId, long refContactMethodId);
        List<BuyerSupplierBankAccount> BuyerSupplierBankList(int pageNo, string sortParameter, int sortDirection, string buyerName, long organisationId, long bankId, out int totalRecords);
        bool AddOrRemoveBuyerSupplierBankDetails(bool isAdd, long buyerPartyId, long bankId);
        int BuyersAssignedToAddress(long addressId);
        ProfileSummary GetSellerProfilePercentage(long sellerPartyId, long sellerId, long organisationId);
        Address GetAddressDetailsByContactMethodId(long contactMethodId);
        List<BuyerSupplierContacts> BuyerSupplierContactsList(int pageNo, string sortParameter, int sortDirection, string buyerName, long sellerId, long contactPartyId, out int totalRecords);
        bool AddOrUpdateBuyerContacts(bool isAssigned, long buyerPartyId, long contactPartyId, int role, long sellerPartyId, long sellerUserPartyId);


        Profile SellerProfileDetails(long sellerPartyId, long buyerPartyId,long buyerUserPartyId);

        bool AddOrRemoveFavouriteSupplier(long buyerUserPartyId,long supplierPartyId,bool  isAdd);
        bool AddOrRemoveTradingSupplier(long buyerUserPartyId, long buyerSellerPartyId, long supplierPartyId, bool isAdd);
        long GetCompanyPartyIdBasedOnCompanyName(string companyName);
        List<SupplierOrganization> GetSupplierOrganization(string fromdate, string toDate, out int total, int pageIndex, short source, int size, int sortDirection, long supplierId = 0, string supplierName = "", long status = 0, string referrerName = "");

        SupplierOrganization GetSupplierDetailsForDashboard(long supplierPartyId);

        List<string> GetNotVerifiedSupplierNames(string supplierOrg);

        List<string> GetSuppliersListForRegistration(string companyName);

        bool CheckwhetherSupplierNameExists(string companyName, out bool IsNotRegistered, out long campaignPublicDataId);

        List<SupplierOrganization> GetSuppliersForVerification(int pageNo, string sortParameter, int sortDirection, out int total, int sourceCheck, int viewOptions, int pageSize, string referrerName = "");

        List<SupplierCountBasedOnStage> GetSuppliersCountBasedOnStage(int sourceCheck, int viewOptions, string referrerName);
    }
}