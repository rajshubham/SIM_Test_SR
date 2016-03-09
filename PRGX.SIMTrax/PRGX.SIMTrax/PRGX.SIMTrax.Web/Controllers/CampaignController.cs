using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.Domain.Util;
using PRGX.SIMTrax.ServiceFacade;
using PRGX.SIMTrax.ServiceFacade.Abstract;
using PRGX.SIMTrax.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PRGX.SIMTrax.Web.Controllers
{
    public class CampaignController : BaseController
    {
        private readonly IBuyerServiceFacade _buyerServiceFacade;
        private readonly ICampaignServiceFacade _campaignServiceFacade;
        private readonly IMasterDataServiceFacade _masterDataServiceFacade;

        public CampaignController()
        {
            _buyerServiceFacade = new BuyerServiceFacade();
            _campaignServiceFacade = new CampaignServiceFacade();
            _masterDataServiceFacade = new MasterDataServiceFacade();
        }

        // GET: Campaign 
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateOrEditCampaign(long buyerId, string BreadCrumb, long? campaignId)
        {
            try
            {
                ViewBag.BreadCrumb = BreadCrumb;
                ViewBag.Objective = "Create";
                var campaignModel = new Campaign();
                if (campaignId != null)
                {
                    campaignModel = _campaignServiceFacade.GetCampaignInfo(Convert.ToInt32(campaignId));
                    buyerId = campaignModel.BuyerId.HasValue ? campaignModel.BuyerId.Value : (long)0;
                    campaignModel.CampaignLogoFilePath = (!string.IsNullOrEmpty(campaignModel.CampaignLogoFilePath)) ? Url.Content(Path.Combine(Configuration.DocumentFileUploadPath, campaignModel.CampaignLogoFilePath)) : null;
                    if (campaignModel.CampaignStatus != CampaignStatus.Release)
                    {
                        ViewBag.Objective = "Edit";
                    }
                    else
                    {
                        ViewBag.Objective = "View";
                    }
                }
                else
                {
                    campaignModel.BuyerId = buyerId;
                    campaignModel.CampaignStatus = CampaignStatus.None;
                }
                var campaignTypelist = typeof(CampaignType).EnumDropDownList();
                campaignModel.CampaignTypeList = new SelectList(campaignTypelist.AsEnumerable(), "Value", "Text").ToList();
                campaignModel.CampaignTypeList.Insert(0, new SelectListItem { Text = "--- Select  ---", Value = "" });

                var buyerList = _buyerServiceFacade.GetBuyersList();
                campaignModel.BuyerList = new SelectList(buyerList.AsEnumerable(), "Value", "Text", buyerId).ToList();
                campaignModel.BuyerList.Insert(0, new SelectListItem { Text = "--- Select ---", Value = "" });

                var pageTemplateTypeList = typeof(CampaignLandingTemplate).EnumDropDownList();
                campaignModel.PageTemplateTypeList = new SelectList(pageTemplateTypeList.AsEnumerable(), "Value", "Text").ToList();
                campaignModel.PageTemplateTypeList.Insert(0, new SelectListItem { Text = "--- Select  ---", Value = "" });

                var voucherList = _buyerServiceFacade.GetVoucherListForBuyer(Convert.ToInt64(buyerId));
                campaignModel.MasterVendor = _buyerServiceFacade.GetMasterVendorValue(buyerId);
                campaignModel.VoucherList = new SelectList(voucherList.AsEnumerable(), "Value", "Text").ToList();
                campaignModel.VoucherList.Insert(0, new SelectListItem { Text = "--- Select ---", Value = "" });
                return PartialView("Campaign/_CreateCampaign", campaignModel);
            }
            catch(Exception ex)
            {
                Logger.Error("CampaignController : CreateOrEditCampaign() : Caught an error" + ex);
                throw;
            }
        }

        [HttpPost]
        public ActionResult AddOrUpdateCampaign(Campaign campaign)
        {
            var result = false;
            try
            {
                //campaignVM.CampaignType = (CampaignType)campaignVM.CampaignTypeInt;
                if (ModelState.IsValid)
                {
                    var auditor = (UserDetails)Session[Constants.SESSION_USER];

                    var documentLogo = new Document();
                    if (null != campaign.CampaignLogo)
                    {
                        using (var binaryReader = new BinaryReader(campaign.CampaignLogo.InputStream))
                        {
                            if (CommonMethods.IsValidFileSignature(binaryReader.ReadBytes(16), campaign.CampaignLogo.FileName))
                            {
                                CommonMethods.DeleteDirectoryOnServer(Path.Combine(Server.MapPath(Configuration.DocumentFileUploadPath), campaign.CampaignName, "Logo"));
                                string folderPath = Path.Combine(campaign.CampaignName, "Logo");
                                CommonMethods.CreateFileFolder(Path.Combine(Configuration.DocumentFileUploadPath, folderPath));
                                string fileName = campaign.CampaignLogo.FileName.Substring(campaign.CampaignLogo.FileName.LastIndexOf("\\") + 1);
                                //    string hashFileName = CommonMethods.EncryptMD5Password(String.Concat(fileName.ToLower(), sellerPartyId));

                                var filePath = Path.Combine(Server.MapPath(Configuration.DocumentFileUploadPath), folderPath, fileName);
                                // to bring back the pointer to start of stream
                                binaryReader.BaseStream.Seek(0, SeekOrigin.Begin);
                                using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                                {
                                    int numBytesToRead = campaign.CampaignLogo.ContentLength;
                                    do
                                    {
                                        int length = numBytesToRead > 2048 ? 2048 : numBytesToRead;
                                        var fileData = new byte[length];
                                        int n = binaryReader.Read(fileData, 0, length);
                                        if (n == 0)
                                            break;
                                        fs.Write(fileData, 0, length);
                                        numBytesToRead -= n;
                                    } while (numBytesToRead > 0);
                                    fs.Close();
                                }
                                if (campaign.CampaignLogoDocumentId.HasValue)
                                {
                                    documentLogo.Id = campaign.CampaignLogoDocumentId.Value;
                                }
                                documentLogo.FileName = fileName;
                                documentLogo.FilePath = Path.Combine(folderPath, fileName);
                                documentLogo.ContentLength = campaign.CampaignLogo.ContentLength;
                                documentLogo.ContentType = campaign.CampaignLogo.ContentType;
                            }
                        }
                    }
                    else if (campaign.CampaignLogoDocumentId.HasValue)
                    {
                        documentLogo.Id = campaign.CampaignLogoDocumentId.Value;
                    }
                    result = _campaignServiceFacade.AddOrUpdateBuyerCampaign(campaign, documentLogo, auditor.UserId);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("CampaignController : Update() : Caught an exception " + ex);
                throw;
            }
            //return RedirectToAction("GetBuyerDashboard", "Admin", new { buyerPartyId = campaign.BuyerId });
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        public JsonResult GetVoucherList(long buyerId)
        {
            var masterVendor = 0;
            try
            {
                var voucherList = _buyerServiceFacade.GetVoucherListForBuyer(buyerId);
                var Vouchers = new SelectList(voucherList.AsEnumerable(), "Value", "Text").ToList();
                Vouchers.Insert(0, new SelectListItem { Text = "--- Select ---", Value = "0" });
                masterVendor = _buyerServiceFacade.GetMasterVendorValue(buyerId);

                return Json(new { result = Vouchers, masterVendor = masterVendor });
            }
            catch (Exception ex)
            {
                Logger.Error("CampaignController : GetVoucherList() : Caught an exception " + ex);
                throw;
            }
        }

        public ActionResult Verify(long campaignId)
        {
            try
            {
                var campaignModel = new Campaign();
                if (campaignId > 0)
                {
                    campaignModel = _campaignServiceFacade.GetCampaignInfo(campaignId);
                    campaignModel.CampaignLogoFilePath = (!string.IsNullOrEmpty(campaignModel.CampaignLogoFilePath)) ? Url.Content(Path.Combine(Configuration.DocumentFileUploadPath, campaignModel.CampaignLogoFilePath)) : null;
                }
                return View(campaignModel);
            }
            catch(Exception ex)
            {
                Logger.Error("CampaignController : Verify() : Caught an error" + ex);
                throw;
            }
        }

        [HttpGet]
        public FileResult DownloadTemplate()
        {
            string filePath = String.Empty;
            filePath = Server.MapPath(Path.Combine(Constants.CAMPAIGN_PRE_REG_SUPPLIER_SAMPLE_FILE_PATH));
            return File(filePath, "application/vnd.ms-excel", "SampleCampaignUpload.xls");
        }

        [HttpPost]
        public ActionResult VerifyCampaign(Campaign campaign)
        {
            try
            {
                if (null != campaign.PreRegFile)
                {
                    if (campaign.PreRegFile.ContentLength > 0)
                    {
                        string fileExtension = Path.GetExtension(campaign.PreRegFile.FileName);
                        if (fileExtension == ".xlsx" || fileExtension == ".xls")
                        {
                            string folderNameForCampaignId = "CampaignId_" + campaign.CampaignId;
                            string folderPath = Constants.CAMPAIGN_PRE_REG_SUPPLIER_FOLDER_PATH + folderNameForCampaignId + "\\";
                            CommonMethods.CreateFileFolder(folderPath);
                            string serverMapFolderPath = Server.MapPath(folderPath);
                            string FilePath = folderPath + campaign.PreRegFile.FileName;
                            string serverMappedFilePath = serverMapFolderPath + campaign.PreRegFile.FileName;
                            CommonMethods.CheckSaveDeleteFileOnServer(serverMappedFilePath);

                            // store path in database

                            _campaignServiceFacade.UpdateCampaignPreRegFilePath(campaign.CampaignId, FilePath);

                            var auditorId = ((UserDetails)Session[Constants.SESSION_USER]).UserId;

                            List<CampaignPreRegSupplier> campaignPreRegSupplierDomain = GetCampaignPreRegSupplierList(fileExtension, serverMappedFilePath, campaign.CampaignId, campaign.CampaignType);
                            if (0 == campaignPreRegSupplierDomain[0].PreRegSupplierId)
                            {
                                _campaignServiceFacade.InsertListOfPreRegSuppliers(campaignPreRegSupplierDomain, auditorId);
                            }
                            else
                            {
                                _campaignServiceFacade.UpdateListOfPreRegSupplier(campaignPreRegSupplierDomain, auditorId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("CampaignController : VerifyCampaign() : Caught an exception" + ex);
                throw ex;
            }
            return RedirectToAction("Verify", "Campaign", new { campaignId = campaign.CampaignId });
        }

        private List<CampaignPreRegSupplier> GetCampaignPreRegSupplierList(string fileExtension, string fileLocation, long campaignId, CampaignType campaignType)
        {
            var preRegCampaignSupplierList = CommonMethods.GetDataListFromExcel(fileExtension, fileLocation);
            CommonMethods.CheckSaveDeleteFileOnServer(fileLocation);
            List<CampaignPreRegSupplier> campaignSupplierList = new List<CampaignPreRegSupplier>();

            var countriesList = _masterDataServiceFacade.GetCountries();

            foreach (DataRow preRegCampaignSupplier in preRegCampaignSupplierList.Rows)
            {
                CampaignPreRegSupplier campaignSupplier = new CampaignPreRegSupplier();
                campaignSupplier.CampaignId = campaignId;
                if (!(string.IsNullOrWhiteSpace(preRegCampaignSupplier.ItemArray[0].ToString().Trim())))
                {
                    campaignSupplier.PreRegSupplierId = Convert.ToInt64(preRegCampaignSupplier.ItemArray[0].ToString().Trim());
                    campaignSupplier.InvalidSupplierComments = preRegCampaignSupplier.ItemArray[17].ToString().Trim();
                }
                if (string.IsNullOrWhiteSpace(preRegCampaignSupplier.ItemArray[1].ToString()))
                {
                    campaignSupplier.InvalidSupplierComments = ReadResource.GetResource(Constants.SUPPLIER_NAME_REQUIRED, ResourceType.Message);
                    campaignSupplier.IsValid = false;
                }
                else if (!string.IsNullOrWhiteSpace(preRegCampaignSupplier.ItemArray[2].ToString())
                    && CommonValidations.IsEmailValid(preRegCampaignSupplier.ItemArray[2].ToString()) == false)
                {
                    campaignSupplier.InvalidSupplierComments = ReadResource.GetResource(Constants.INVALID_EMAIL_ID, ResourceType.Message);
                    campaignSupplier.IsValid = false;
                }
                else
                {
                    campaignSupplier.InvalidSupplierComments = "";
                    campaignSupplier.IsValid = true;
                }
                campaignSupplier.SupplierName = preRegCampaignSupplier.ItemArray[1].ToString().Trim();
                campaignSupplier.LoginId = preRegCampaignSupplier.ItemArray[2].ToString().Trim();
                campaignSupplier.FirstName = preRegCampaignSupplier.ItemArray[3].ToString().Trim();
                campaignSupplier.LastName = preRegCampaignSupplier.ItemArray[4].ToString().Trim();
                campaignSupplier.JobTitle = preRegCampaignSupplier.ItemArray[5].ToString().Trim();
                campaignSupplier.AddressLine1 = preRegCampaignSupplier.ItemArray[6].ToString().Trim();
                campaignSupplier.AddressLine2 = preRegCampaignSupplier.ItemArray[7].ToString().Trim();
                campaignSupplier.City = preRegCampaignSupplier.ItemArray[8].ToString().Trim();
                campaignSupplier.State = preRegCampaignSupplier.ItemArray[9].ToString().Trim();
                long countryCode = 239;
                if (String.IsNullOrEmpty(preRegCampaignSupplier.ItemArray[10].ToString().Trim()) || !countriesList.Any(v => v.Text.Equals(preRegCampaignSupplier.ItemArray[10].ToString().Trim())))
                {
                    countryCode = 239;
                }
                else
                {
                    countryCode = countriesList.Any(x => x.Text.Equals(preRegCampaignSupplier.ItemArray[10].ToString().Trim())) ? countriesList.FirstOrDefault(x => x.Text.Equals(preRegCampaignSupplier.ItemArray[10].ToString().Trim())).Value : 239;
                }
                campaignSupplier.Country = countryCode;
                campaignSupplier.ZipCode = preRegCampaignSupplier.ItemArray[11].ToString().Trim();
                campaignSupplier.Telephone = preRegCampaignSupplier.ItemArray[12].ToString().Trim();
                campaignSupplier.RegistrationNumber = preRegCampaignSupplier.ItemArray[13].ToString().Trim();
                campaignSupplier.VatNumber = preRegCampaignSupplier.ItemArray[14].ToString().Trim();
                campaignSupplier.DunsNumber = preRegCampaignSupplier.ItemArray[15].ToString().Trim();
                if (String.IsNullOrWhiteSpace(preRegCampaignSupplier.ItemArray[16].ToString().Trim()))
                    campaignSupplier.IsSubsidary = false;
                else
                {
                    campaignSupplier.IsSubsidary = true;
                    campaignSupplier.UltimateParent = preRegCampaignSupplier.ItemArray[16].ToString().Trim();
                }
                //if (campaignType == CampaignType.PreRegistered || campaignType == CampaignType.PublicData)
                //{
                //    campaignSupplier.IsFITMappedToBuyer = preRegCampaignSupplier.ItemArray[18].ToString().ToLower().Trim() == "yes" ? true : false;
                //    campaignSupplier.IsHSMappedToBuyer = preRegCampaignSupplier.ItemArray[19].ToString().ToLower().Trim() == "yes" ? true : false;
                //    campaignSupplier.IsDSMappedToBuyer = preRegCampaignSupplier.ItemArray[20].ToString().ToLower().Trim() == "yes" ? true : false;
                //}
                campaignSupplierList.Add(campaignSupplier);
            }
            return campaignSupplierList;
        }

        [HttpPost]
        public JsonResult GetPreRegSupplier(string id, string filterCriteria, string index)
        {
            int total = 0;
            var listSuppliers = new List<CampaignPreRegSupplier>();
            int campaignId = Convert.ToInt32(id);
            int currentPage = Convert.ToInt32(index);
            int size = 10;
            try
            {
                listSuppliers = _campaignServiceFacade.GetPreRegSupplierInCampaign(campaignId, filterCriteria, out total, currentPage, size);
            }
            catch (Exception ex)
            {
                Logger.Error("CampaignController : GetPreRegSupplier() : Caught an exception" + ex);
                throw ex;
            }
            return Json(new { data = listSuppliers, total = total });
        }

        [HttpPost]
        public JsonResult DeletePreRegSupplier(string preRegSupplierId)
        {
            var result = false;
            try
            {
                if (preRegSupplierId != "")
                {
                    result = _campaignServiceFacade.DeletePreRegSupplier(Convert.ToInt64(preRegSupplierId));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { result = result });
        }

        public FileResult DownloadEditSuppliers(string id)
        {
            int total = 0;
            var campaignId = Convert.ToInt32(id);
            var listSuppliers = _campaignServiceFacade.GetPreRegSupplierInCampaign(campaignId, "", out total, 1, int.MaxValue);
            var campaignType = _campaignServiceFacade.GetCampaignInfo(campaignId).CampaignType;
            var file = CreatePreRegSupplierExcel(listSuppliers, campaignType);
            return File(file.GetBuffer(), "application/vnd.ms-excel", "EditPreRegSuppliers.xls");
        }

        private MemoryStream CreatePreRegSupplierExcel(List<CampaignPreRegSupplier> listCampaignPreRegSupplier, CampaignType campaignType)
        {
            var stream = new MemoryStream();
            try
            {
                var sourceFilePath = String.Empty;
                if (campaignType == CampaignType.PreRegistered)
                {
                    sourceFilePath = Server.MapPath(Constants.CAMPAIGN_PRE_REG_SUPPLIER_SAMPLE_FILE_PATH);
                }
                else if (campaignType == CampaignType.PublicData)
                {
                    sourceFilePath = Server.MapPath(Constants.CAMPAIGN_PUBLIC_DATA_SUPPLIER_SAMPLE_FILE_PATH);
                }
                //// check begin
                if (!String.IsNullOrWhiteSpace(sourceFilePath))
                {
                    string fileTemplate = sourceFilePath;
                    using (FileStream file = new FileStream(fileTemplate, FileMode.Open, FileAccess.Read))
                    {
                        HSSFWorkbook hssfWorkBook = new HSSFWorkbook(file);
                        ISheet mySheet = hssfWorkBook.GetSheet("Sheet1");
                        for (int i = 0; i < listCampaignPreRegSupplier.Count; i++)
                        {
                            IRow runningRow = mySheet.CreateRow(i + 1);
                            ICell runningCell = null;

                            string userComment = listCampaignPreRegSupplier[i].InvalidSupplierComments;
                            bool isCommentThere = !string.IsNullOrWhiteSpace(userComment);

                            //// create cell content
                            runningCell = runningRow.CreateCell(0);
                            runningCell.SetCellValue(listCampaignPreRegSupplier[i].PreRegSupplierId);
                            if (isCommentThere) runningCell.CellStyle = GetColoredCell(mySheet);

                            runningCell = runningRow.CreateCell(1);
                            runningCell.SetCellValue(listCampaignPreRegSupplier[i].SupplierName);
                            if (isCommentThere) runningCell.CellStyle = GetColoredCell(mySheet);

                            runningCell = runningRow.CreateCell(2);
                            runningCell.SetCellValue(listCampaignPreRegSupplier[i].LoginId);
                            if (isCommentThere) runningCell.CellStyle = GetColoredCell(mySheet);

                            runningCell = runningRow.CreateCell(3);
                            runningCell.SetCellValue(string.IsNullOrWhiteSpace(listCampaignPreRegSupplier[i].FirstName) ? "" : listCampaignPreRegSupplier[i].FirstName);
                            if (isCommentThere) runningCell.CellStyle = GetColoredCell(mySheet);

                            runningCell = runningRow.CreateCell(4);
                            runningCell.SetCellValue(string.IsNullOrWhiteSpace(listCampaignPreRegSupplier[i].LastName) ? "" : listCampaignPreRegSupplier[i].LastName);
                            if (isCommentThere) runningCell.CellStyle = GetColoredCell(mySheet);

                            runningCell = runningRow.CreateCell(5);
                            runningCell.SetCellValue(string.IsNullOrWhiteSpace(listCampaignPreRegSupplier[i].JobTitle) ? "" : listCampaignPreRegSupplier[i].JobTitle);
                            if (isCommentThere) runningCell.CellStyle = GetColoredCell(mySheet);

                            runningCell = runningRow.CreateCell(6);
                            runningCell.SetCellValue(string.IsNullOrWhiteSpace(listCampaignPreRegSupplier[i].AddressLine1) ? "" : listCampaignPreRegSupplier[i].AddressLine1);
                            if (isCommentThere) runningCell.CellStyle = GetColoredCell(mySheet);

                            runningCell = runningRow.CreateCell(7);
                            runningCell.SetCellValue(string.IsNullOrWhiteSpace(listCampaignPreRegSupplier[i].AddressLine2) ? "" : listCampaignPreRegSupplier[i].AddressLine2);
                            if (isCommentThere) runningCell.CellStyle = GetColoredCell(mySheet);

                            runningCell = runningRow.CreateCell(8);
                            runningCell.SetCellValue(string.IsNullOrWhiteSpace(listCampaignPreRegSupplier[i].City) ? "" : listCampaignPreRegSupplier[i].City);
                            if (isCommentThere) runningCell.CellStyle = GetColoredCell(mySheet);

                            runningCell = runningRow.CreateCell(9);
                            runningCell.SetCellValue(string.IsNullOrWhiteSpace(listCampaignPreRegSupplier[i].State) ? "" : listCampaignPreRegSupplier[i].State);
                            if (isCommentThere) runningCell.CellStyle = GetColoredCell(mySheet);

                            runningCell = runningRow.CreateCell(10);
                            runningCell.SetCellValue(listCampaignPreRegSupplier[i].CountryName);
                            if (isCommentThere) runningCell.CellStyle = GetColoredCell(mySheet);

                            runningCell = runningRow.CreateCell(11);
                            runningCell.SetCellValue(string.IsNullOrWhiteSpace(listCampaignPreRegSupplier[i].ZipCode) ? "" : listCampaignPreRegSupplier[i].ZipCode);
                            if (isCommentThere) runningCell.CellStyle = GetColoredCell(mySheet);

                            runningCell = runningRow.CreateCell(12);
                            runningCell.SetCellValue(string.IsNullOrWhiteSpace(listCampaignPreRegSupplier[i].Telephone) ? "" : listCampaignPreRegSupplier[i].Telephone);
                            if (isCommentThere) runningCell.CellStyle = GetColoredCell(mySheet);

                            runningCell = runningRow.CreateCell(13);
                            runningCell.SetCellValue(string.IsNullOrWhiteSpace(listCampaignPreRegSupplier[i].RegistrationNumber) ? "" : listCampaignPreRegSupplier[i].RegistrationNumber);
                            if (isCommentThere) runningCell.CellStyle = GetColoredCell(mySheet);

                            runningCell = runningRow.CreateCell(14);
                            runningCell.SetCellValue(string.IsNullOrWhiteSpace(listCampaignPreRegSupplier[i].VatNumber) ? "" : listCampaignPreRegSupplier[i].VatNumber);
                            if (isCommentThere) runningCell.CellStyle = GetColoredCell(mySheet);

                            runningCell = runningRow.CreateCell(15);
                            runningCell.SetCellValue(string.IsNullOrWhiteSpace(listCampaignPreRegSupplier[i].DunsNumber) ? "" : listCampaignPreRegSupplier[i].DunsNumber);
                            if (isCommentThere) runningCell.CellStyle = GetColoredCell(mySheet);

                            string parent = (string.IsNullOrWhiteSpace(listCampaignPreRegSupplier[i].UltimateParent)) ? "" : listCampaignPreRegSupplier[i].UltimateParent;
                            runningCell = runningRow.CreateCell(16);
                            runningCell.SetCellValue(parent);
                            if (isCommentThere) runningCell.CellStyle = GetColoredCell(mySheet);

                            runningCell = runningRow.CreateCell(17);
                            runningCell.SetCellValue(string.IsNullOrWhiteSpace(listCampaignPreRegSupplier[i].InvalidSupplierComments) ? "" : listCampaignPreRegSupplier[i].InvalidSupplierComments);
                            if (isCommentThere) runningCell.CellStyle = GetColoredCell(mySheet);

                            //if (campaignType == CampaignType.PreRegistered)
                            //{
                            //    runningCell = runningRow.CreateCell(18);
                            //    runningCell.SetCellValue(listCampaignPreRegSupplier[i].IsFITMappedToBuyer.HasValue ? (listCampaignPreRegSupplier[i].IsFITMappedToBuyer.Value ? "Yes" : "No") : String.Empty);
                            //    if (isCommentThere) runningCell.CellStyle = GetColoredCell(mySheet);

                            //    runningCell = runningRow.CreateCell(19);
                            //    runningCell.SetCellValue(listCampaignPreRegSupplier[i].IsHSMappedToBuyer.HasValue ? (listCampaignPreRegSupplier[i].IsHSMappedToBuyer.Value ? "Yes" : "No") : String.Empty);
                            //    if (isCommentThere) runningCell.CellStyle = GetColoredCell(mySheet);

                            //    runningCell = runningRow.CreateCell(20);
                            //    runningCell.SetCellValue(listCampaignPreRegSupplier[i].IsDSMappedToBuyer.HasValue ? (listCampaignPreRegSupplier[i].IsDSMappedToBuyer.Value ? "Yes" : "No") : String.Empty);
                            //    if (isCommentThere) runningCell.CellStyle = GetColoredCell(mySheet);
                            //}
                        }
                        hssfWorkBook.Write(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("CampaignController : CreatePreRegSupplierExcel() : Caught an exception" + ex);
                throw ex;
            }
            return stream;
        }

        private static ICellStyle GetColoredCell(ISheet sheet)
        {
            ICellStyle cellStyle = sheet.Workbook.CreateCellStyle();
            cellStyle.FillForegroundColor = HSSFColor.Red.Index;
            cellStyle.FillPattern = FillPattern.SolidForeground;
            return cellStyle;
        }

        [HttpPost]
        public JsonResult SubmitCampaign(string campaignId)
        {
            var result = false;
            try
            {
                if (campaignId != "")
                {
                    if (_campaignServiceFacade.DeleteInvalidPreRegSupplierBasedOnCampaign(Convert.ToInt32(campaignId)))
                    {
                        result = _campaignServiceFacade.UpdateCampaignStatus(Convert.ToInt32(campaignId), (short)CampaignStatus.Submitted);
                        if (result)
                        {
                            _campaignServiceFacade.UpdateCampaignSupplierCount(Convert.ToInt32(campaignId));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("CampaignController : SubmitCampaign() : Caught an exception" + ex);
                throw ex;
            }
            return Json(new { result = result });
        }


        public ActionResult LandingPage(string permalink)
        {
            try
            {
                var user = (Session[Constants.SESSION_USER] != null) ? (UserDetails)Session[Constants.SESSION_USER] : null;
                var company = (OrganizationDetail)Session[Constants.SESSION_ORGANIZATION];
                if (user != null && company != null)
                {
                    UserType type = (UserType)Convert.ToInt64(Session[Constants.SESSION_USER_TYPE]);
                    switch (type)
                    {
                        case UserType.Supplier:
                            if (company.Status >= (int)CompanyStatus.Submitted)
                            {
                                return RedirectToAction("Home", "Supplier");
                            }
                            else
                            {
                                return RedirectToAction("Register", "Supplier");
                            }
                        case UserType.Buyer:
                            return RedirectToAction("Home", "Buyer");
                        case UserType.Auditor:
                            return RedirectToAction("Home", "Admin");
                    }
                }
                var campaign = _campaignServiceFacade.GetCampaignInfoBasedOnCampaignURL(permalink);
                campaign.CampaignLogoFilePath = (!string.IsNullOrEmpty(campaign.CampaignLogoFilePath)) ? Url.Content(Path.Combine(Configuration.DocumentFileUploadPath, campaign.CampaignLogoFilePath)) : string.Empty;
                Session[Constants.SESSION_CAMPAIGN_ID] = campaign.CampaignId;
                TempData["BuyerOrganisation"] = campaign.BuyerOrganisation;
                Session[Constants.SESSION_BUYER_NAME] = campaign.BuyerOrganisation;
                TempData["WelcomeMessage"] = campaign.WelcomeMessage;
                TempData["CampaignLogo"] = campaign.CampaignLogoFilePath;
                TempData["CampaignName"] = campaign.CampaignName;
                if (campaign.TemplateType == CampaignLandingTemplate.PrgxTemplate)
                {
                    Session[Constants.SESSION_CAMPAIGN_LOGO] = null;
                    return View("LandingPage");
                }
                else
                {
                    Session[Constants.SESSION_CAMPAIGN_LOGO] = campaign.CampaignLogoFilePath;
                    return View("ClientLandingPage");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("CampaignController : LandingPage() : Caught an error");
                throw ex;
            }
        }

        public ActionResult Login(string permalink)
        {
            var campaignVM = new CampaignLogin();
            try
            {
                var campaign = _campaignServiceFacade.GetCampaignInfoBasedOnCampaignURL(permalink);
                campaign.CampaignLogoFilePath = (!string.IsNullOrEmpty(campaign.CampaignLogoFilePath)) ? Url.Content(Path.Combine(Configuration.DocumentFileUploadPath, campaign.CampaignLogoFilePath)) : string.Empty;

                TempData["BuyerOrganisation"] = campaign.BuyerOrganisation;
                TempData["WelcomeMessage"] = campaign.WelcomeMessage;
                TempData["CampaignLogo"] = campaign.CampaignLogoFilePath;
                TempData["CampaignName"] = campaign.CampaignName;

                campaignVM.BuyerId = campaign.BuyerId.HasValue ? campaign.BuyerId.Value : 0;
                campaignVM.BuyerOrganisationName = campaign.BuyerOrganisation;
                campaignVM.CampaignId = campaign.CampaignId;

                return View(campaignVM);
            }
            catch (Exception ex)
            {
                Logger.Error("CampaignController : Login() : Caught an exception" + ex);
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult Login(CampaignLogin campaignLogin)
        {
            var result = false;
            var redirectUrl = string.Empty;
            var message = string.Empty;
            long preRegId = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    var loginDomain = campaignLogin.LoginId.Substring(campaignLogin.LoginId.IndexOf('@'));
                    var encryptedPassword = CommonMethods.EncryptMD5Password(loginDomain.ToLower() + campaignLogin.Password.Trim());
                    bool isRegistered = false;
                    preRegId = _campaignServiceFacade.ValidatePreRegSupplierCode(campaignLogin.LoginId, encryptedPassword, out isRegistered);
                    if (preRegId != 0)
                    {
                        if (isRegistered == false)
                        {
                            Session[Constants.SESSION_CAMPAIGN_ID] = campaignLogin.CampaignId;
                            Session[Constants.SESSION_BUYER_NAME] = campaignLogin.BuyerOrganisationName;
                            redirectUrl = "/Supplier/Register";
                            Session[Constants.SESSION_PRE_REG_ID] = preRegId;
                            result = true;
                            message = string.Empty;
                        }
                        else if (isRegistered == true)
                        {
                            redirectUrl = "/Login";
                            result = true;
                            message = ReadResource.GetResourceForGlobalization(Constants.CAMPAIGN_ALREADY_REGISTERED_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
                        }
                    }
                    else
                    {
                        redirectUrl = string.Empty;
                        result = false;
                        message = ReadResource.GetResourceForGlobalization(Constants.CAMPAIGN_INCORRECT_REGISTRATION_CODE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("CampaignController : Login() : Caught an exception" + ex);
                throw ex;
            }
            return Json(new { result = result, redirectUrl = redirectUrl, message = message, preRegId = preRegId });
        }
    }
}