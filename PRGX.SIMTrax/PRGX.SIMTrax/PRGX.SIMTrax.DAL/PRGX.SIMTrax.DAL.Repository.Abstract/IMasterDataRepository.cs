using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.Domain.Model;

namespace PRGX.SIMTrax.DAL.Repository.Abstract
{
    public interface IMasterDataRepository : IGenericRepository<MasterData>
    {
        SellerRegisterMasterData GetMasterDataValuesForSellerRegistration(string regionCode);

        BuyerRegisterMasterData GetMasterDataValuesForBuyerRegistration(string regionCode);
        BuyerSupplierSearchFilter GetMasterValuesForSearchFilters(BuyerSupplierSearchFilter model);

    }
}
