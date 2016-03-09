using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Abstract;
using PRGX.SIMTrax.DAL.Repository.Context;
using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository.Repository
{
    public class MasterDataRepository : GenericRepository<MasterData>, IMasterDataRepository
    {
        public MasterDataRepository(DbContext context) : base(context) { }

        /// <summary>
        /// Getting MasterDataValuesForRegistraionDropDowns
        /// </summary>
        /// <returns></returns>
        public SellerRegisterMasterData GetMasterDataValuesForSellerRegistration(string regionCode)
        {
            try
            {
                Logger.Info("MasterDataRepository : GetMasterDataValuesForSellerRegistration() : Entering the method");
                var masterList = new SellerRegisterMasterData();
                var mnemonics = new string[] {  Constants.ORG_EMP_BAND, Constants.ORG_TURNOVER,  Constants.ORG_BIZ_SECT,Constants.TYPE_COMPANY };

                var query = this.All().AsQueryable();
                 var list =  query.Include("MasterDataType").Where(v => mnemonics.Contains(v.MasterDataType.Mnemonic)).Select(v => new ItemList()
                {
                    Text = v.Value,
                    Value = v.Id,
                    Description = v.Description,
                    Mnemonic = v.MasterDataType.Mnemonic.Trim(),
                     OrderId = v.OrderId
                 }).OrderBy(v => v.OrderId).ToList();

                masterList.EmployeesNumberList = list.Where(u => u.Mnemonic == Constants.ORG_EMP_BAND).ToList();
                masterList.TurnOverList = list.Where(u => u.Mnemonic == Constants.ORG_TURNOVER).ToList();
                masterList.BusinessSectorList = list.Where(u => u.Mnemonic == Constants.ORG_BIZ_SECT).ToList();
                masterList.CompanyTypeList = list.Where(u => u.Mnemonic == Constants.TYPE_COMPANY).ToList();
                using (var ctx = new MasterDataContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    var regionMnemonics = new string[] { Constants.SALES_REGION, Constants.SERVICE_REGION,Constants.COUNTRY_NAME };
                    masterList.RefRegionId = (ctx.Regions.FirstOrDefault(v => v.Name.ToLower() == regionCode.ToLower()) != null) ?
                        ctx.Regions.FirstOrDefault(v => v.Name.ToLower() == regionCode.ToLower()).Id : 0;
                    if(masterList.RefRegionId > 0)
                    {
                        masterList.IdentifierTypeList = ctx.PartyIdentifierTypes.Where(x => x.RefRegion == masterList.RefRegionId).
                            Select(v => new ItemList()
                            {
                                Text = v.IdentifierType,
                                Value = v.Id
                            }).ToList();

                    }
                    masterList.TermsOfUseId = ctx.TermsOfUses.OrderByDescending(v => v.CreatedOn).FirstOrDefault().Id;
                    var regionList = ctx.Regions.Where(v => regionMnemonics.Contains(v.RegionType)).Select(v => new ItemList()
                    {
                        Text = v.Name,
                        Value = v.Id,
                        Mnemonic = v.RegionType.Trim(),
                        OrderId = v.DisplayOrder
                    }).OrderBy(v => v.OrderId).ToList();
                    masterList.CountryList = regionList.Where(u => u.Mnemonic == Constants.COUNTRY).ToList();
                    masterList.GeographicSalesList = regionList.Where(u => u.Mnemonic == Constants.SALES_REGION).ToList();
                    masterList.GeographicServiceList = regionList.Where(u => u.Mnemonic == Constants.SERVICE_REGION).ToList();
                }
                Logger.Info("MasterDataRepository : GetMasterDataValuesForSellerRegistration() : Exit the method");
                return masterList;
            }
            catch (Exception ex)
            {
                Logger.Error("MasterDataRepository : GetMasterDataValuesForSellerRegistration() : Caught an exception" + ex);
                throw;
            }
        }

        public BuyerRegisterMasterData GetMasterDataValuesForBuyerRegistration(string regionCode)
        {
            try
            {
                Logger.Info("MasterDataRepository : GetMasterDataValuesForBuyerRegistration() : Entering the method");
                var masterList = new BuyerRegisterMasterData();
                var mnemonics = new string[] { Constants.ORG_EMP_BAND, Constants.ORG_TURNOVER, Constants.ORG_BIZ_SECT};

                var query = this.All().AsQueryable();
                var list = query.Include("MasterDataType").Where(v => mnemonics.Contains(v.MasterDataType.Mnemonic)).Select(v => new ItemList()
                {
                    Text = v.Value,
                    Value = v.Id,
                    Description = v.Description,
                    Mnemonic = v.MasterDataType.Mnemonic.Trim(),
                    OrderId = v.OrderId
                }).OrderBy(v => v.OrderId).ToList();

                masterList.EmployeesNumberList = list.Where(u => u.Mnemonic == Constants.ORG_EMP_BAND).ToList();
                masterList.TurnOverList = list.Where(u => u.Mnemonic == Constants.ORG_TURNOVER).ToList();
                masterList.BusinessSectorList = list.Where(u => u.Mnemonic == Constants.ORG_BIZ_SECT).ToList();

                using (var ctx = new MasterDataContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    var regionMnemonics = new string[] { Constants.COUNTRY_NAME };
                    //masterList.RefRegionId = (ctx.Regions.FirstOrDefault(v => v.Name.ToLower() == regionCode.ToLower()) != null) ?
                        //ctx.Regions.FirstOrDefault(v => v.Name.ToLower() == regionCode.ToLower()).Id : 0;

                    var regionList = ctx.Regions.Where(v => regionMnemonics.Contains(v.RegionType)).Select(v => new ItemList()
                    {
                        Text = v.Name,
                        Value = v.Id,
                        Mnemonic = v.RegionType.Trim(),
                        OrderId = v.DisplayOrder
                    }).OrderBy(v => v.OrderId).ToList();
                    masterList.CountryList = regionList.Where(u => u.Mnemonic == Constants.COUNTRY).ToList();
                }
                Logger.Info("MasterDataRepository : GetMasterDataValuesForBuyerRegistration() : Exit the method");
                return masterList;
            }
            catch (Exception ex)
            {
                Logger.Error("MasterDataRepository : GetMasterDataValuesForBuyerRegistration() : Caught an exception" + ex);
                throw;
            }
        }

        public BuyerSupplierSearchFilter GetMasterValuesForSearchFilters(BuyerSupplierSearchFilter model)
        {
            Logger.Info("MasterDataRepository : GetMasterValuesForSearchFilters() : Entering the method");

            var newModel = new BuyerSupplierSearchFilter();
            try
            {
                using (var ctx = new MasterDataContext())
                {
                    if (model.Sector != null && model.Sector.Count > 0)
                    {
                        var sectorMasterList = ctx.MasterDatas.Where(u => model.Sector.Contains(u.Id)).Select(v => v.Value).ToList();
                        if (sectorMasterList.Count > 0)
                        {
                            foreach (var item in sectorMasterList)
                            {
                                newModel.SectorText += item + ",";
                            }
                            if (newModel.SectorText != "")
                                newModel.SectorText = newModel.SectorText.Substring(0, newModel.SectorText.LastIndexOf(","));

                        }
                    }
                    if (model.TypeOfCompany != null && model.TypeOfCompany.Count > 0)
                    {
                        var companyTypeMasterList = ctx.MasterDatas.Where(u => model.TypeOfCompany.Contains(u.Id)).Select(v => v.Value).ToList();
                        if (companyTypeMasterList.Count > 0)
                        {
                            foreach (var item in companyTypeMasterList)
                            {
                                newModel.TypeOfCompanyText += item + ",";
                            }
                            if (newModel.TypeOfCompanyText != "")
                                newModel.TypeOfCompanyText = newModel.TypeOfCompanyText.Substring(0, newModel.TypeOfCompanyText.LastIndexOf(","));

                        }
                    }
                    if (model.TurnOver != null && model.TurnOver.Count > 0)
                    {
                        var turnOverMasterList = ctx.MasterDatas.Where(u => model.TurnOver.Contains(u.Id)).Select(v => v.Value).ToList();
                        if (turnOverMasterList.Count > 0)
                        {
                            foreach (var item in turnOverMasterList)
                            {
                                newModel.TurnOverText += item + ",";
                            }
                            if (newModel.TurnOverText != "")
                                newModel.TurnOverText = newModel.TurnOverText.Substring(0, newModel.TurnOverText.LastIndexOf(","));

                        }
                    }
                    if (model.EmployeeSize != null && model.EmployeeSize.Count > 0)
                    {
                        var sizeMasterList = ctx.MasterDatas.Where(u => model.EmployeeSize.Contains(u.Id)).Select(v => v.Value).ToList();
                        if (sizeMasterList.Count > 0)
                        {
                            foreach (var item in sizeMasterList)
                            {
                                newModel.EmployeeSizeText += item + ",";
                            }
                            if (newModel.EmployeeSizeText != "")
                                newModel.EmployeeSizeText = newModel.EmployeeSizeText.Substring(0, newModel.EmployeeSizeText.LastIndexOf(","));

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Error("MasterDataRepository : GetMasterValuesForSearchFilters() : Caught an exception" + ex);
                throw;
            }
            Logger.Info("MasterDataRepository : GetMasterValuesForSearchFilters() : Exiting the method");

            return newModel;
        }
    }
}
