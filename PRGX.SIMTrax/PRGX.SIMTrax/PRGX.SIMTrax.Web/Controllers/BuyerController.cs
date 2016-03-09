using Newtonsoft.Json;
using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.Domain.Util;
using PRGX.SIMTrax.ServiceFacade;
using PRGX.SIMTrax.ServiceFacade.Abstract;
using PRGX.SIMTrax.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PRGX.SIMTrax.Web.Controllers
{
    public class BuyerController : BaseController
    {
        private readonly IMasterDataServiceFacade _masterDataService;
        private readonly IBuyerServiceFacade _buyerUserService;


        public BuyerController()
        {
            _masterDataService = new MasterDataServiceFacade();
            _buyerUserService = new BuyerServiceFacade();
        }

        public ActionResult Home()
        {
            return View();
        }

        public ActionResult Register()
        {
            try
            {
                var model = new BuyerRegister();
                var regionCode = Configuration.REGION_IDENTIFIER;
                var masterDataList = _masterDataService.GetMasterDataValuesForBuyerRegistration(regionCode);
                if (masterDataList != null)
                {
                    if (masterDataList.CountryList.FirstOrDefault(u => u.Text.ToLower() == regionCode.ToLower()) != null)
                    {
                        model.BuyerCountryList = new SelectList(masterDataList.CountryList.AsEnumerable(), "Value", "Text", masterDataList.CountryList.FirstOrDefault(u => u.Text.ToLower() == regionCode.ToLower()).Value).ToList();
                    }
                    else
                    {
                        model.BuyerCountryList = new SelectList(masterDataList.CountryList.AsEnumerable(), "Value", "Text").ToList();

                    }
                    model.BuyerNoOfEmployeesList = new SelectList(masterDataList.EmployeesNumberList.AsEnumerable(), "Value", "Text").ToList();
                    model.BuyerTurnOverList = new SelectList(masterDataList.TurnOverList.AsEnumerable(), "Value", "Text").ToList();
                    model.BuyerBusinessSectorList = masterDataList.BusinessSectorList.Select(v => new ItemList()
                    {
                        Value = v.Value,
                        Text = v.Text,
                        Mnemonic = v.Mnemonic,
                        OrderId = v.OrderId,
                        Description = ReadResource.GetResourceForGlobalization("MS_POP_UP_DESCRIPTION_" + v.Value, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture())
                    }).ToList();
                    
                    //model.RefRegionId = masterDataList.RefRegionId;
                }
                return View(model);
            }
            catch (Exception exception)
            {
                Logger.Error("BuyerController : Register() : Caught an exception " + exception);
                throw;
            }
        }

        [HttpPost]
        public JsonResult Register(BuyerRegister model)
        {
            try
            {
                var result = _buyerUserService.AddBuyerUser(model);
                return Json(result);
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerController : Register() : Caught an exception " + ex);
                throw;
            }
        }



        public ActionResult SupplierSearch(string id = "")
        {
            
                return View();
            
        }


        public JsonResult GetSearchFilterDropDowns()
        {
             var CompanyStatusList = new List<SelectListItem>();
            CompanyStatusList.Insert(0, new SelectListItem { Text = CommonMethods.Description(CompanyStatus.ProfileVerified), Value = Convert.ToString((Int16)CompanyStatus.ProfileVerified) });
            CompanyStatusList.Insert(1, new SelectListItem { Text = CommonMethods.Description(CompanyStatus.RegistrationVerified), Value = Convert.ToString((Int16)CompanyStatus.RegistrationVerified) });
            CompanyStatusList.Insert(2, new SelectListItem { Text = CommonMethods.Description(CompanyStatus.Submitted), Value = Convert.ToString((Int16)CompanyStatus.Submitted) });
            CompanyStatusList.Insert(3, new SelectListItem { Text = CommonMethods.Description(CompanyStatus.Started), Value = Convert.ToString((Int16)CompanyStatus.Started) });
            CompanyStatusList.Insert(4, new SelectListItem { Text = CommonMethods.Description(CompanyStatus.NotRegistered), Value = Convert.ToString((Int16)CompanyStatus.NotRegistered) });

           var regionCode = Configuration.REGION_IDENTIFIER;
            var masterDataList = _masterDataService.GetMasterDataForSellerRegistration(regionCode);
            var supplierTypeList = typeof(SupplierType).EnumDropDownList();
            supplierTypeList.RemoveAll(st => st.Text == "Both" || st.Text == "All");

            var BusinessSector = masterDataList.BusinessSectorList;
            var BusinessSectorList = new SelectList(BusinessSector.AsEnumerable(), "Value", "Text").ToList();

            var EmployeesNumberList = masterDataList.EmployeesNumberList;
            var NoOfEmployeesList = new SelectList(EmployeesNumberList.AsEnumerable(), "Value", "Text").ToList();

            var TurnOver = masterDataList.TurnOverList;
            var TurnOverList = new SelectList(TurnOver.AsEnumerable(), "Value", "Text").ToList();

          
            var companyType = masterDataList.CompanyTypeList;
            var companyTypeList = new SelectList(companyType.AsEnumerable(), "Value", "Text").ToList();

            return Json(new
            {
                CompanyStatusList = CompanyStatusList,
                supplierTypeList = supplierTypeList,
                BusinessSectorList = BusinessSectorList,
                NoOfEmployeesList = NoOfEmployeesList,
                TurnOverList = TurnOverList,
                CompanyTypeList = companyTypeList
            });
        }



        [HttpPost]
        public JsonResult GetSuppliers(BuyerSupplierSearchFilter model)
        {
            JsonResult json = null;
            try
            {
                int totalRecords = 0;
                var userPartyId = ((UserDetails)Session[Constants.SESSION_USER]).RefPersonPartyId;
                var companyPartyId = ((OrganizationDetail)Session[Constants.SESSION_ORGANIZATION]).RefPartyId;
               var suppliers = _buyerUserService.GetSuppliers(model, companyPartyId, userPartyId, out totalRecords);
                if (suppliers != null)
                {
                    json = Json(new { suppliers = suppliers, total = totalRecords });
                }
                return json;
            }
            catch (Exception exception)
            {
                Logger.Error("BuyerController : GetSuppliers() : Caught an exception " + exception);
                throw;
            }
        }

        public JsonResult GetSuppliersList(string text)
        {
            List<string> response = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(text))
                {
                    response = _buyerUserService.GetSuppliersListForIntellisense(text);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerController : GetSuppliersList() : Caught an exception " + ex);
                throw;
            }
            return Json(response);

        }

        public FileResult ExportSuppliersForBuyer(string val)
        {
            try
            {
                dynamic data = JsonConvert.DeserializeObject(val);

                BuyerSupplierSearchFilter model = data.ToObject<BuyerSupplierSearchFilter>();
                if (model != null)
                {
                    model.PageSize = int.MaxValue;
                    int totalRecords = 0;
                    var userPartyId = ((UserDetails)Session[Constants.SESSION_USER]).RefPersonPartyId;
                    var companyPartyId = ((OrganizationDetail)Session[Constants.SESSION_ORGANIZATION]).RefPartyId;
                    var filtersList = new List<ExcelDownloadFilterList>();
                    if (model.SupplierStatus != null && model.SupplierStatus.Count > 0)
                    {
                        var statusFilter = new ExcelDownloadFilterList();
                        statusFilter.FilterType = "Supplier Status";
                        foreach (var item in model.SupplierStatus)
                        {
                            statusFilter.FilterValue += CommonMethods.Description((CompanyStatus)item) + ",";
                        }
                        if (statusFilter.FilterValue != "")
                            statusFilter.FilterValue = statusFilter.FilterValue.Substring(0, statusFilter.FilterValue.LastIndexOf(","));
                        filtersList.Add(statusFilter);

                    }
                    if (model.SupplierType != null && model.SupplierType.Count > 0)
                    {
                        var supplierTypeFilter = new ExcelDownloadFilterList();
                        supplierTypeFilter.FilterType = "Supplier Type";
                        foreach (var item in model.SupplierType)
                        {
                            supplierTypeFilter.FilterValue += CommonMethods.Description((SupplierType)item) + ",";
                        }
                        if (supplierTypeFilter.FilterValue != "")
                            supplierTypeFilter.FilterValue = supplierTypeFilter.FilterValue.Substring(0, supplierTypeFilter.FilterValue.LastIndexOf(","));
                        filtersList.Add(supplierTypeFilter);

                    }
                    if ((model.Sector != null && model.Sector.Count > 0) || (model.EmployeeSize != null && model.EmployeeSize.Count > 0) || (model.TurnOver != null && model.TurnOver.Count > 0))
                    {
                        var dataModel = _masterDataService.GetMasterValuesForSearchFilters(model);
                        if (model.Sector != null && model.Sector.Count > 0)
                        {
                            var sectorTypeFilter = new ExcelDownloadFilterList();
                            sectorTypeFilter.FilterType = "Business Sector";
                            sectorTypeFilter.FilterValue = dataModel.SectorText;
                            filtersList.Add(sectorTypeFilter);

                        }
                        if (model.TypeOfCompany != null && model.TypeOfCompany.Count > 0)
                        {
                            var sectorTypeFilter = new ExcelDownloadFilterList();
                            sectorTypeFilter.FilterType = "Type Of Company";
                            sectorTypeFilter.FilterValue = dataModel.TypeOfCompanyText;
                            filtersList.Add(sectorTypeFilter);

                        }
                        if (model.EmployeeSize != null && model.EmployeeSize.Count > 0)
                        {
                            var sizeTypeFilter = new ExcelDownloadFilterList();
                            sizeTypeFilter.FilterType = "Employee Size";
                            sizeTypeFilter.FilterValue = dataModel.EmployeeSizeText;
                            filtersList.Add(sizeTypeFilter);

                        }
                        if (model.TurnOver != null && model.TurnOver.Count > 0)
                        {
                            var turnOverTypeFilter = new ExcelDownloadFilterList();
                            turnOverTypeFilter.FilterType = "Turn Over";
                            turnOverTypeFilter.FilterValue = dataModel.TurnOverText;
                            turnOverTypeFilter.FilterValue = turnOverTypeFilter.FilterValue.Replace("&pound;", "Pounds ");
                            turnOverTypeFilter.FilterValue = turnOverTypeFilter.FilterValue.Replace("&euro;", "Euros ");

                            filtersList.Add(turnOverTypeFilter);

                        }

                    }
                 
                    var suppliers = _buyerUserService.GetSuppliers(model, companyPartyId, userPartyId, out totalRecords);

                    string exclude = "BuyerId,IsFavourite,CreatedDate,IsTradingWith,StatusId,SectionSharedCount,RequestSupplierAnswerId,IsDiscussionStarted,AprrovedTypes,IsNotRegisteredSupplier,RegistrationStatus,CreatedDate,AddressLine1,AddressLine2,City,State,Country,PostCode,Registrationnumber,VATnumber,DUNSnumber,AnswersShared";
                    var file = CommonMethods.CreateDownloadExcel(suppliers, "", exclude, "Suppliers", "Suppliers", filtersList);
                    return File(file.GetBuffer(), "application/vnd.ms-excel", "SupplierList_" + DateTime.UtcNow.ToString("dd-MMM-yyyy") + ".xls");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerController : ExportSuppliersForBuyer() : Caught an exception " + ex);
                throw;
            }
            return null;
        }

    }
}
