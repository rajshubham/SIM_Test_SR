using PRGX.SIMTrax.DAL;
using PRGX.SIMTrax.DAL.Abstract;
using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.Domain.Util;
using PRGX.SIMTrax.ServiceFacade.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PRGX.SIMTrax.ServiceFacade
{
    public class MasterDataServiceFacade : IMasterDataServiceFacade
    {
        private readonly IMasterDataUow _masterDataUow;

        public MasterDataServiceFacade()
        {
            _masterDataUow = new MasterDataUow();
        }

        public SellerRegisterMasterData GetMasterDataForSellerRegistration(string regionCode)
        {
            try
            {
                Logger.Info("MasterDataServiceFacade : GetMasterDataForSellerRegistration() : Entering the method");

                SellerRegisterMasterData registerData = null;
                registerData = _masterDataUow.MasterDataValues.GetMasterDataValuesForSellerRegistration(regionCode);
                Logger.Info("MasterDataServiceFacade : GetMasterDataForSellerRegistration() : Entering the method");

                return registerData;
            }
            catch (Exception ex)
            {
                Logger.Error("MasterDataServiceFacade : GetMasterDataForSellerRegistration() : Caught an exception" + ex);
                throw;
            }
        }

        public List<ViewModel.IndustryCode> GetIndustryCodes(string regionIdentifier,int? ParentId = null, bool AllSICCodes = false)
        {
            Logger.Info("Service MasterDataServiceFacade : GetIndustryCodes(int? codeId) : Enter into Method");
            List<ViewModel.IndustryCode> IndustryCodes = null;
            try
            {
                var IndustryCodesDAL = _masterDataUow.IndustryCodeValues.GetIndustryCodes(regionIdentifier, ParentId, AllSICCodes);
                if (IndustryCodesDAL != null)
                {
                    IndustryCodes = IndustryCodesDAL.Select( v => new PRGX.SIMTrax.ViewModel.IndustryCode()
                    {
                        Id = v.Id,
                        RefParentId = v.RefParentId,
                        SectorName = v.SectorName,
                        CodeNumber = v.CodeNumber,
                        ChildCodes = v.IndustryCode1 != null ? v.IndustryCode1.Select(lst => new ViewModel.IndustryCode()
                        {
                            Id = lst.Id,
                            RefParentId = lst.RefParentId,
                            SectorName = lst.SectorName,
                            CodeNumber = lst.CodeNumber
                        }).ToList() : null
                    }).ToList();
                 
                }
            }
            catch (Exception e)
            {
                Logger.Error("Service MasterDataServiceFacade : GetIndustryCodes(int? codeId) : Caught an Error " + e);
                throw e;
            }
            Logger.Info("Service MasterDataServiceFacade : GetIndustryCodes(int? codeId) : Exit from Method");
            return IndustryCodes;
        }

        public List<ItemList> GetCountries()
        {
            Logger.Info("Service MasterDataServiceFacade : GetCountries : Enter into Method");
            var countryList = new List<ItemList>();
            try
            {
                countryList = _masterDataUow.Regions.All().Where(v => v.RegionType == Constants.COUNTRY_NAME).Select(v => new ItemList()
                {
                    Text = v.Name,
                    Value = v.Id,
                    Mnemonic = v.RegionType.Trim(),
                    OrderId = v.DisplayOrder
                }).OrderBy(v => v.OrderId).ToList();

            }
            catch (Exception e)
            {
                Logger.Error("Service MasterDataServiceFacade : GetCountries : Caught an Error " + e);
                throw e;
            }
            Logger.Info("Service MasterDataServiceFacade :GetCountries : Exit from Method");
            return countryList;
        }

        public BuyerRegisterMasterData GetMasterDataValuesForBuyerRegistration(string regionCode)
        {
            try
            {
                Logger.Info("MasterDataServiceFacade : GetMasterDataValuesForBuyerRegistration() : Entering the method");

                BuyerRegisterMasterData registerData = null;
                registerData = _masterDataUow.MasterDataValues.GetMasterDataValuesForBuyerRegistration(regionCode);

                Logger.Info("MasterDataServiceFacade : GetMasterDataValuesForBuyerRegistration() : Entering the method");
                return registerData;
            }
            catch (Exception ex)
            {
                Logger.Error("MasterDataServiceFacade : GetMasterDataValuesForBuyerRegistration() : Caught an exception" + ex);
                throw;
            }
        }

        public BuyerSupplierSearchFilter GetMasterValuesForSearchFilters(BuyerSupplierSearchFilter model)
        {
            try
            {
                Logger.Info("MasterDataService : HelperSearchSupplierFilter() : Entering the method");
                Logger.Info("MasterDataService : HelperSearchSupplierFilter() : Exiting the method");
                return _masterDataUow.MasterDataValues.GetMasterValuesForSearchFilters(model);
            }
            catch (Exception ex)
            {
                Logger.Error("MasterDataService : HelperSearchSupplierFilter() : Caught an exception" + ex);
                throw ex;
            }
        }
    }
}
