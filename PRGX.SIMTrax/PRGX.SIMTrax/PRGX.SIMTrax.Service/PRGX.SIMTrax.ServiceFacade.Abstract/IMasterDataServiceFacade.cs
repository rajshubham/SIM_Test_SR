using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.Domain.Util;
using PRGX.SIMTrax.ViewModel;
using System.Collections.Generic;

namespace PRGX.SIMTrax.ServiceFacade.Abstract
{
    public interface IMasterDataServiceFacade
    {
        SellerRegisterMasterData GetMasterDataForSellerRegistration(string regionCode);

        List<IndustryCode> GetIndustryCodes(string regionIdentifier, int? ParentId = null, bool AllSICCodes = false);

        List<ItemList> GetCountries();

        BuyerRegisterMasterData GetMasterDataValuesForBuyerRegistration(string regionCode);

        BuyerSupplierSearchFilter GetMasterValuesForSearchFilters(BuyerSupplierSearchFilter model);
    }
}
