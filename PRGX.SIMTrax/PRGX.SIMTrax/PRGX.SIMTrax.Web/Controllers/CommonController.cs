using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.Domain.Util;
using PRGX.SIMTrax.ServiceFacade;
using PRGX.SIMTrax.ServiceFacade.Abstract;
using PRGX.SIMTrax.Web.Models.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;

namespace PRGX.SIMTrax.Web.Controllers
{
    public class CommonController : BaseController
    {
        private readonly IPartyServiceFacade _partyService;
        private readonly IBuyerServiceFacade _buyerService;

        public CommonController()
        {
            _partyService = new PartyServiceFacade();
            _buyerService = new BuyerServiceFacade();
        }

        public ActionResult Error(string message, bool displayDescription)
        {
            try
            {
                ViewBag.Message = message;
                ViewBag.displayDescription = true;
                return View();
            }
            catch (Exception)
            {
                Logger.Error("CommonController : Error(string message) : Caught an error");
                throw;
            }
        }

        public ActionResult SupplierProfile(long sellerPartyId = 0)
        {
            try
            {
                //companyName = companyName.Replace('-', '.');
                if (Session[Constants.SESSION_USER] != null)
                {
                    var companyDetails = new Profile();
                    //var organization = (Session[Constants.SESSION_ORGANIZATION] != null) ? (OrganizationDetail)Session[Constants.SESSION_ORGANIZATION] : null;
                    //sellerPartyId = (organization != null) ? organization.RefPartyId : 0;
                    //companyDetails = _partyService.SellerProfileDetails(sellerPartyId, 20103);
                    return View(companyDetails);
                }
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                Logger.Info("CommonController : SellerProfile() : Caught an exception");
                throw ex;
            }
        }

        public JsonResult ProfileDetails(long sellerPartyId)
        {
            long buyerPartyId = 0;
            long buyerUserPartyId = 0;
            var companyDetails = new Profile();
            var jsonResult = new JsonResult();
            try
            {
                if (((UserType)Convert.ToInt64(Session[Constants.SESSION_USER_TYPE]) == UserType.Buyer || (UserType)Convert.ToInt64(Session[Constants.SESSION_USER_TYPE]) == UserType.AdminBuyer))
                {
                    buyerPartyId = ((OrganizationDetail)Session[Constants.SESSION_ORGANIZATION]).RefPartyId;
                   buyerUserPartyId = ((UserDetails)Session[Constants.SESSION_USER]).RefPersonPartyId;
                }
                if (sellerPartyId == 0)
                {
                    if (((UserType)Convert.ToInt64(Session[Constants.SESSION_USER_TYPE]) == UserType.Buyer || (UserType)Convert.ToInt64(Session[Constants.SESSION_USER_TYPE]) == UserType.AdminBuyer))
                    {
                        return Json(new { companyExists = false, companyDetails = "", redirectUrl = "/Buyer/Home" });
                    }
                    else if (((UserType)Convert.ToInt64(Session[Constants.SESSION_USER_TYPE]) == UserType.Supplier || (UserType)Convert.ToInt64(Session[Constants.SESSION_USER_TYPE]) == UserType.AdminSupplier))

                    {
                        return Json(new { companyExists = false, companyDetails = "", redirectUrl = "/Supplier/Home" });
                    }
                    else
                    {
                        return Json(new { companyExists = false, companyDetails = "", redirectUrl = "/Auditor/Home" });
                    }
                }
                companyDetails= _partyService.SellerProfileDetails(sellerPartyId, buyerPartyId, buyerUserPartyId);
                companyDetails.CurrencyCodeHtml = ReadResource.GetResourceForGlobalization(Constants.CURRENCY_CODE_HTML, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                companyDetails.CompanyLogoString = (!string.IsNullOrEmpty(companyDetails.CompanyLogoString)) ? Url.Content(Path.Combine(Domain.Util.Configuration.DocumentFileUploadPath, companyDetails.CompanyLogoString)) : null;

                jsonResult = Json(new { companyExists = true, companyDetails = companyDetails, redirectUrl = "" }, JsonRequestBehavior.AllowGet);
                     jsonResult.MaxJsonLength = int.MaxValue;
            }
            catch (Exception ex)
            {
                Logger.Error("CommonController : ProfileDetails(): Caught an exception " + ex);
                throw;
            }
            return jsonResult;
        }

        public JsonResult GetVerificationStatus(long sellerPartyId)
        {
            var profileScoreCount = new List<ComplianceChart>();
            var complianceListEnum = CommonMethods.EnumDropDownList(typeof(ComplianceScoreCategory));
            foreach(var item in complianceListEnum)
            {
                var chart = new ComplianceChart();
                chart.CategoryId =(Int16)item.Value;
                chart.MissingCount = 0;
                chart.VerifiedCount = 0;
                chart.NotVerifiedCount = 0;
                chart.MissingPercentage =0;
                chart.NotVerifiedPercentage = 0;
                chart.VerifiedPercentage = 0;
                    chart.Status = Convert.ToInt16(CompanyStatus.Submitted);
                    chart.StatusValue = "Not Started";
                profileScoreCount.Add(chart);
            }
            string lastSubmittedDate = null;
         
            return Json(new { scoreCount = profileScoreCount, lastSubmittedDate = lastSubmittedDate });
        }


        public JsonResult AddOrRemoveFavouriteSupplier(bool isAdd, long supplierPartyId)
        {
            try
            {
                var buyerUserPartyId = ((UserDetails)Session[Constants.SESSION_USER]).RefPersonPartyId;
                var result = _partyService.AddOrRemoveFavouriteSupplier(buyerUserPartyId, supplierPartyId, isAdd);
                var message = "";
                if (result)
                {
                    if (isAdd)
                    {
                        message = ReadResource.GetResourceForGlobalization(Constants.FAVOURITE_SUPPLIER_ADDED_SUCCESSFULLY, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                    }
                    else
                    {
                        message = ReadResource.GetResourceForGlobalization(Constants.FAVOURITE_SUPPLIER_REMOVED_SUCCESSFULLY, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                    }
                }
                else
                {
                    message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                }

                return Json(new { result = true, message = message, isAdd = isAdd });
            }
            catch (Exception ex)
            {
                Logger.Error("CommonController : AddOrRemoveFavouriteSupplier(): Caught an exception " + ex);
                throw;
            }
        }

        public JsonResult AddOrRemoveTradingSupplier(bool isAdd, long supplierPartyId)
        {
            try
            {
                var buyerUserPartyId = ((UserDetails)Session[Constants.SESSION_USER]).RefPersonPartyId;
                var buyerPartyId = ((OrganizationDetail)Session[Constants.SESSION_ORGANIZATION]).RefPartyId;
                var result = _partyService.AddOrRemoveTradingSupplier(buyerUserPartyId, buyerPartyId, supplierPartyId, isAdd);
                var message = "";
                if (result)
                {
                    if (isAdd)
                    {
                        message = ReadResource.GetResourceForGlobalization(Constants.TRADING_SUPPLIER_ADDED_SUCCESSFULLY, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                    }
                    else
                    {
                        message = ReadResource.GetResourceForGlobalization(Constants.TRADING_SUPPLIER_REMOVED_SUCCESSFULLY, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                    }
                }
                else
                {
                    message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                }

                return Json(new { result = true, message = message, isAdd = isAdd });
            }
            catch (Exception ex)
            {
                Logger.Error("CommonController : AddOrRemoveTradingSupplier(): Caught an exception " + ex);
                throw;
            }
        }

        public ActionResult DownloadProfile(long supplierPartyId)
        {
            try
            {
                if (supplierPartyId > 0)
                {
                    var model = new List<ComplianceChart>();
                    var complianceListEnum = CommonMethods.EnumDropDownList(typeof(ComplianceScoreCategory));
                    foreach (var item in complianceListEnum)
                    {
                        var chart = new ComplianceChart();
                        chart.CategoryId = (Int16)item.Value;
                        chart.MissingCount = 0;
                        chart.VerifiedCount = 0;
                        chart.NotVerifiedCount = 0;
                        chart.MissingPercentage = 0;
                        chart.NotVerifiedPercentage = 0;
                        chart.VerifiedPercentage = 0;
                        chart.Status = Convert.ToInt16(CompanyStatus.Submitted);
                        chart.StatusValue = "Not Started";
                        model.Add(chart);
                    }
                    var list = new DownloadProfile();
                    Color noneColor = ColorTranslator.FromHtml("#D2D2D2");
                    Color missingColor = ColorTranslator.FromHtml(Constants.SelfDeclaredColor);
                    Color verifiedColor = ColorTranslator.FromHtml(Constants.VerifiedColor);
                    Color notVerifiedColor = ColorTranslator.FromHtml(Constants.FlaggedColor);
                    string specificFolder = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "Suppliers_Profile_Download" + supplierPartyId + "_" + DateTime.UtcNow.ToString("dd_MM_yyyy_H_mm_ss"));
                    if (!Directory.Exists(specificFolder))
                    {
                        Directory.CreateDirectory(specificFolder);
                    }
                    for (int i = 0, j = 0; i < model.Count; i++, j++)
                    {
                        var item = model[i];
                        Chart chart = new Chart();
                        chart.ChartAreas.Add(new ChartArea("ChartArea1"));
                        chart.Series.Add(new Series("Data"));
                        chart.Width = 150;
                        chart.Height = 150;
                        //     chart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;  
                        chart.Series["Data"].CustomProperties = "DoughnutRadius=50,PieStartAngle=270";
                        chart.Series["Data"].ChartType = SeriesChartType.Doughnut;
                        var divHtml = "";
                        if (item.IsDataPresent)
                        {
                            var BoldFont = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);
                            chart.Series["Data"].Points.AddXY((item.NotVerifiedPercentage > 0) ? Convert.ToString(item.NotVerifiedPercentage) + "%" : "", item.NotVerifiedPercentage);
                            chart.Series["Data"].Points[0].Color = notVerifiedColor;
                            chart.Series["Data"].Points[0].Font = BoldFont;
                            //chart.Series["Data"].Points[0].LabelForeColor = System.Drawing.Color.White;
                            chart.Series["Data"].Points.AddXY((item.MissingPercentage > 0) ? Convert.ToString(item.MissingPercentage) + "%" : "", item.MissingPercentage);
                            chart.Series["Data"].Points[1].Color = missingColor;
                            chart.Series["Data"].Points[1].Font = BoldFont;
                            chart.Series["Data"].Points.AddXY((item.VerifiedPercentage > 0) ? Convert.ToString(item.VerifiedPercentage) + "%" : "", item.VerifiedPercentage);
                            chart.Series["Data"].Points[2].Color = verifiedColor;
                            chart.Series["Data"].Points[2].LabelForeColor = System.Drawing.Color.White;
                            chart.Series["Data"].Points[2].Font = BoldFont;

                            divHtml += "<div style=\"width:180px;line-height:20px;background-color:" + Constants.FlaggedColor + ";text-align:center;color:black;margin:auto\">" + item.NotVerifiedCount + ((item.NotVerifiedCount != 1) ? " answers flagged</div>" : " answer flagged</div>");
                            divHtml += "<div style=\"width:180px;line-height:20px;background-color:" + Constants.SelfDeclaredColor + ";text-align:center;margin:auto\">" + item.MissingCount + ((item.MissingCount != 1) ? " answers self declared</div>" : " answer self declared</div>");
                            divHtml += "<div style=\"width:180px;line-height:20px;background-color:" + Constants.VerifiedColor + ";text-align:center;color:white;margin:auto\">" + item.VerifiedCount + ((item.VerifiedCount != 1) ? " answers verified</div>" : " answer verified</div>");
                            divHtml += "<div style=\"width:200px;line-height:20px;padding-top:10px;float:left;text-align:center;\">Verified on " + item.FormattedVerified + "</div>";
                        }
                        else
                        {
                            chart.Series["Data"].Points.AddXY("", 100);
                            chart.Series["Data"].Points.AddXY("", 0);
                            chart.Series["Data"].Points[0].Color = noneColor;
                            divHtml = "<div style=\"line-height:68px;color:#D2D2D2;text-align:center;\">";
                            divHtml += "<span style=\"vertical-align:middle\">Verification Not Started</span></div>";

                        }
                        chart.SaveImage(specificFolder + "/" + item.CategoryId + ".png");
                        var helper = new ChartImages();
                        helper.DivHtml = divHtml;
                        helper.Category = item.CategoryId;
                        helper.IsPublished = item.IsDataPresent;
                        using (Image image = Image.FromFile(specificFolder + "/" + item.CategoryId + ".png"))
                        {
                            using (MemoryStream m = new MemoryStream())
                            {
                                image.Save(m, image.RawFormat);
                                byte[] imageBytes = m.ToArray();
                                // Convert byte[] to Base64 String
                                string base64String = Convert.ToBase64String(imageBytes);
                                helper.ChartBaseString = "data:;base64," + base64String;
                            }
                        }
                        list.Charts.Add(helper);
                    }

                    Directory.Delete(specificFolder, true);
                    var companyDetails = _partyService.SellerProfileDetails(supplierPartyId, 0, 0);
                    companyDetails.CurrencyCodeHtml = ReadResource.GetResourceForGlobalization(Constants.CURRENCY_CODE_HTML, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                    if (companyDetails.SellerPartyId > 0)
                    {
                        list.GeneralInformation = companyDetails.Mapping();
                        var logoPath = (!string.IsNullOrEmpty(companyDetails.CompanyLogoString)) ? Url.Content(Path.Combine(Domain.Util.Configuration.DocumentFileUploadPath, companyDetails.CompanyLogoString)) : null;
                        if (!string.IsNullOrWhiteSpace(logoPath))
                        {
                            using (Image image = Image.FromFile(Server.MapPath(logoPath)))
                            {
                                using (MemoryStream m = new MemoryStream())
                                {
                                    image.Save(m, image.RawFormat);
                                    byte[] imageBytes = m.ToArray();
                                    // Convert byte[] to Base64 String
                                    string base64String = Convert.ToBase64String(imageBytes);
                                    list.GeneralInformation.LogoBaseString = "data:;base64," + base64String;
                                }
                            }
                        }
                    }
                    list.CompanyName = list.GeneralInformation.CompanyName;
                    var encodedCompanyName = HttpUtility.UrlEncode(list.CompanyName);
                    var customSwitches = string.Format("--print-media-type --allow {0} --header-html {0} --allow {1} --footer-html {1} --header-spacing 10  --footer-spacing 10 ",
                    Url.Action("Header", "Common", new { companyName = encodedCompanyName }, "http"), Url.Action("Footer", "Common", new { area = "" }, "http"));
                    if ((ConfigurationManager.AppSettings["Environment"] != null) && (ConfigurationManager.AppSettings["Environment"].ToUpper() == "PROD"))
                    {
                        customSwitches = string.Format("--print-media-type --allow {0} --header-html {0} --allow {1} --footer-html {1} --header-spacing 10  --footer-spacing 10 ",
                     "http://localhost:8082/Common/Header/?companyName=" + encodedCompanyName, "http://localhost:8082/Common/Footer");

                    }
                    ViewBag.CompanyName = list.CompanyName;
                    return new Rotativa.ViewAsPdf("DownloadProfile", list)
                    {
                        FileName = list.CompanyName.Replace(" ", String.Empty) + "_Profile_" + DateTime.Now.ToString("dd-MM-yyyy") + ".pdf",
                        PageMargins = { Left = 0, Right = 0 },
                        CustomSwitches = customSwitches
                    };
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("CommonController : DownloadProfile() : Caught an exception" + ex);
                throw;
            }
        }

        [AllowAnonymous]
        public ActionResult Header(string companyName)
        {
            var oragnisation = new OrganizationDetail();

            oragnisation.OrganizationName = HttpUtility.UrlDecode(companyName);
            return View(oragnisation);
        }

        [AllowAnonymous]
        public ActionResult Footer()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetCompanyPartyIdBasedOnCompanyName(string companyName)
        {
            try
            {
                long companyId = 0;
                companyId = _partyService.GetCompanyPartyIdBasedOnCompanyName(companyName);
                return Json(new { CompanyId = companyId });
            }
            catch (Exception ex)
            {
                Logger.Error("CommonController : GetCompanyIdBasedOnCompanyName() : Caught an exception " + ex);
                throw;
            }
        }

        public JsonResult GetBuyerName(string text)
        {
            List<string> response = null;
            try
            {
                response = _buyerService.GetVerifiedBuyerNames(text);
            }
            catch (Exception ex)
            {
                Logger.Error("CommonController : GetBuyerName() : Caught an exception " + ex);
                throw;
            }
            return Json(response);
        }

        [HttpPost]
        public JsonResult GetSuppliersForIntellisense(string text)
        {
            List<string> supplierList = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(text))
                    supplierList = _partyService.GetNotVerifiedSupplierNames(text);
            }
            catch (Exception ex)
            {
                Logger.Error("CommonController : GetSuppliersForIntellisense(): Caught an error" + ex);
                throw;
            }
            return Json(supplierList);
        }
    }
}