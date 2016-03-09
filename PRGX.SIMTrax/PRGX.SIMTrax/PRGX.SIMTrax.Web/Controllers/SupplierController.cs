using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.Domain.Util;
using PRGX.SIMTrax.ServiceFacade;
using PRGX.SIMTrax.ServiceFacade.Abstract;
using PRGX.SIMTrax.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PRGX.SIMTrax.Web.Controllers
{
    public class SupplierController : BaseController
    {
        private readonly IMasterDataServiceFacade _masterDataService;
        private readonly IPartyServiceFacade _partyService;
        private readonly IEmailServiceFacade _emailserviceFacade;
        private readonly IUserServiceFacade _userServiceFacade;
        private readonly ICampaignServiceFacade _campaignServiceFacade;

        public SupplierController()
        {
            _masterDataService = new MasterDataServiceFacade();
            _partyService = new PartyServiceFacade();
            _emailserviceFacade = new EmailServiceFacade();
            _userServiceFacade = new UserServiceFacade();
            _campaignServiceFacade = new CampaignServiceFacade();
        }

        public ActionResult Register()
        {
            try
            {
                var model = new SellerRegister();

                var regionCode = Configuration.REGION_IDENTIFIER;
                var PrimaryContact = new ViewModel.ContactDetails();
                PrimaryContact.ContactType = ContactType.Primary;
                var RemittanceContact = new ViewModel.ContactDetails();
                RemittanceContact.ContactType = ContactType.Accounts;
                var SustainabilityContact = new ViewModel.ContactDetails();
                SustainabilityContact.ContactType = ContactType.Sustainability;
                var HsContact = new ViewModel.ContactDetails();
                HsContact.ContactType = ContactType.HS;
                var ProcurementContact = new ViewModel.ContactDetails();
                ProcurementContact.ContactType = ContactType.Procurement;

                var user = (Session[Constants.SESSION_USER] != null) ? (UserDetails)Session[Constants.SESSION_USER] : null;
                model.IsPreRegistered = false;

                if (Session[Constants.SESSION_PRE_REG_ID] != null)
                {
                    var id = Convert.ToInt64(Session[Constants.SESSION_PRE_REG_ID]);
                    model = _campaignServiceFacade.GetPreRegSupplierDetails(id);

                    if (!model.ContactDetails.Any(c => c.ContactType == ContactType.Primary))
                        model.ContactDetails.Add(PrimaryContact);
                    if (!model.ContactDetails.Any(c => c.ContactType == ContactType.Accounts))
                        model.ContactDetails.Add(RemittanceContact);
                    if (!model.ContactDetails.Any(c => c.ContactType == ContactType.Sustainability))
                        model.ContactDetails.Add(SustainabilityContact);
                    if (!model.ContactDetails.Any(c => c.ContactType == ContactType.HS))
                        model.ContactDetails.Add(HsContact);
                    if (!model.ContactDetails.Any(c => c.ContactType == ContactType.Procurement))
                        model.ContactDetails.Add(ProcurementContact);
                }
                if (Session[Constants.SESSION_ORGANIZATION] != null)
                {
                    model = _partyService.GetSellerOrganizationDetailsByPartyId(user.RefOrganisationPartyId);

                    if (!model.ContactDetails.Any(c => c.ContactType == ContactType.Primary))
                        model.ContactDetails.Add(PrimaryContact);
                    if (!model.ContactDetails.Any(c => c.ContactType == ContactType.Accounts))
                        model.ContactDetails.Add(RemittanceContact);
                    if (!model.ContactDetails.Any(c => c.ContactType == ContactType.Sustainability))
                        model.ContactDetails.Add(SustainabilityContact);
                    if (!model.ContactDetails.Any(c => c.ContactType == ContactType.HS))
                        model.ContactDetails.Add(HsContact);
                    if (!model.ContactDetails.Any(c => c.ContactType == ContactType.Procurement))
                        model.ContactDetails.Add(ProcurementContact);
                }
                else
                {
                    model.ContactDetails.Add(PrimaryContact);
                    model.ContactDetails.Add(RemittanceContact);
                    model.ContactDetails.Add(SustainabilityContact);
                    model.ContactDetails.Add(HsContact);
                    model.ContactDetails.Add(ProcurementContact);
                }

                if (!string.IsNullOrWhiteSpace(model.FirstAddressLine1))
                {
                    model.IsCompanyDetailsSubmitted = true;
                }
                else
                {
                    model.IsCompanyDetailsSubmitted = false;
                }

                ///We Should Add Primary Contact first to make sure that it will be mandatory
                ///

                var masterDataList = _masterDataService.GetMasterDataForSellerRegistration(regionCode);

                if (masterDataList != null)
                {
                    if (masterDataList.CountryList.FirstOrDefault(u => u.Text.ToLower() == regionCode.ToLower()) != null)
                    {
                        model.CountryList = new SelectList(masterDataList.CountryList.AsEnumerable(), "Value", "Text", masterDataList.CountryList.FirstOrDefault(u => u.Text.ToLower() == regionCode.ToLower()).Value).ToList();
                    }
                    else
                    {
                        model.CountryList = new SelectList(masterDataList.CountryList.AsEnumerable(), "Value", "Text").ToList();

                    }
                    model.NoOfEmployeesList = new SelectList(masterDataList.EmployeesNumberList.AsEnumerable(), "Value", "Text").ToList();
                    model.NoOfEmployeesList.Insert(0, new SelectListItem { Text = "--- Select ---", Value = "" });
                    model.TurnOverList = new SelectList(masterDataList.TurnOverList.AsEnumerable(), "Value", "Text").ToList();
                    model.TurnOverList.Insert(0, new SelectListItem { Text = "--- Select  ---", Value = "" });
                    model.BusinessSectorList = masterDataList.BusinessSectorList.Select(v => new ItemList()
                    {
                        Value = v.Value,
                        Text = v.Text,
                        Mnemonic = v.Mnemonic,
                        OrderId = v.OrderId,
                        Description = ReadResource.GetResourceForGlobalization("MS_POP_UP_DESCRIPTION_"+v.Value, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture())
                    }).ToList();

                    
                    model.GeoGraphicSalesList = new SelectList(masterDataList.GeographicSalesList.AsEnumerable(), "Value", "Text").ToList();
                    model.GeoGraphicSuppList = new SelectList(masterDataList.GeographicServiceList.AsEnumerable(), "Value", "Text").ToList();
                    model.CompanyTypeList = new SelectList(masterDataList.CompanyTypeList.AsEnumerable(), "Value", "Text").ToList();
                    model.CompanyTypeList.Insert(0, new SelectListItem { Text = "--- Select  ---", Value = "" });
                    model.RefRegionId = masterDataList.RefRegionId;
                    model.TermsOfUseId = masterDataList.TermsOfUseId;
                    model.IdentifierTypeList = masterDataList.IdentifierTypeList;
                }
                return View(model);
            }
            catch (Exception ex)
            {
                Logger.Error("SellerController : Register() : Caught an error " + ex);
                throw;
            }
        }

        public JsonResult AddOrUpdateSellerOrganisationDetails(SellerRegister model)
        {
            try
            {
                var result = true;
                var message = string.Empty;
                long sellerPartyId = model.SellerPartyId;
                model.Status = CompanyStatus.Started;
                result = _partyService.UpdateSellerPartyDetails(model);
                if (result)
                    return Json(new { result = result, message = ReadResource.GetResourceForGlobalization(Constants.GENERAL_INFORMATION_SAVED_SUCCESFULLY, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()), SellerPartyId = sellerPartyId});
                else
                    return Json(new { result = result, message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()) });

            }
            catch (Exception exception)
            {
                Logger.Error("AccountController : AddSellerOrganisationDetails() : Caught an error " + exception);
                throw;
            }
        }

        public JsonResult AddSellerContactDetails(SellerRegister model)
        {
            try
            {
                var result = true;
                var message = string.Empty;
                model.Status = CompanyStatus.Submitted;
                result = _partyService.UpdatePartyContactDetails(model);
                var redirectUrl = "/Account/Login";
                if (result)
                {
                    long localeId = 0;
                    
                    var userDetails = _userServiceFacade.GetUserDetailsByOrganisationPartyId(model.SellerPartyId);
                    if (userDetails != null)
                    {
                        PutSupplierDetailsInSession(userDetails);
                        Session[Constants.SESSION_SUBMIT_VERIFICATION] = ReadResource.GetResource(Constants.SUBMIT_FOR_VERIFICATION, ResourceType.Message);
                        redirectUrl = "/supplier-home";
                        SendRegistrationMailToSupplier(userDetails, localeId);
                    }
                    return Json(new { result = result, message = ReadResource.GetResourceForGlobalization(Constants.CONTACTS_SAVED_SUCCESFULLY, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()), redirectUrl= redirectUrl });
                }
                else
                    return Json(new { result = result, message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()), redirectUrl= redirectUrl });

            }
            catch (Exception exception)
            {
                Logger.Error("SellerController : AddSellerContactDetails() : Caught an error " + exception);
                throw;
            }

        }

        private bool PutSupplierDetailsInSession(UserDetails user)
        {
            try
            {
                bool result = true;
                if (null != user)
                {
                    Session[Constants.SESSION_USER] = user;
                    Session[Constants.SESSION_ORGANIZATION] = _partyService.GetOrganizationDetail(user.RefOrganisationPartyId);
                    Session[Constants.SESSION_USER_TYPE] = user.UserType;
                    Session[Constants.SESSION_LOGIN_ID] = user.LoginId;
                }
                else
                {
                    result = false;
                }
                return result;
            }
            catch (Exception e)
            {
                Logger.Error("SellerController : PutUserDetailsInSession() : Caught an Error " + e);
                throw e;
            }
        }

        private void SendRegistrationMailToSupplier(UserDetails userDetails, long localeId)
        {
            var emailMessage = _emailserviceFacade.GetEmailMessage(Constants.SUBMITTED_FOR_VERIFICATION.ToString(), localeId, userDetails.LoginId);

            if (Constants.IS_EMAIL_ON)
            {
                try
                {
                    // get the application path.
                    string appFilePhysicalPath = HttpContext.Request.PhysicalApplicationPath;
                    string emailSIMLogoUrl = appFilePhysicalPath + @"\Content\Images\logo\siteLogo.png";
                    string emailPRGXLogoUrl = appFilePhysicalPath + @"\Content\Images\logo\simTraxFooterLogoForEmail.png";
                    var emailSubject = emailMessage.Subject + " - " + userDetails.OrganisationName;
                    _emailserviceFacade.SendEmail(userDetails.LoginId, string.Empty, string.Empty, emailMessage.Subject, emailMessage.Content, emailSIMLogoUrl, emailPRGXLogoUrl, "", "", appFilePhysicalPath);
                    SendNotificationMailToAuditor(userDetails, localeId);
                }
                catch (Exception ex)
                {
                    Logger.Error("SupplierController : SendRegistrationMailToSupplier() : Caught an Error " + ex);
                }
            }
        }

        private void SendNotificationMailToAuditor(UserDetails userDetails,long localeId)
        {
            var emailMessage = _emailserviceFacade.GetEmailMessage(Constants.ADMIN_NOTIFICATION_SUPPLIER_REGISTERED.ToString(),localeId,
                userDetails.OrganisationName, userDetails.FirstName, userDetails.LastName, userDetails.LoginId, userDetails.PrimaryContactTelephone, userDetails.PrimaryContactJobTitle);

            if (Constants.IS_EMAIL_ON)
            {
                try
                {
                    // get the application path.
                    string appFilePhysicalPath = HttpContext.Request.PhysicalApplicationPath;
                    string emailSIMLogoUrl = appFilePhysicalPath + @"\Content\Images\logo\siteLogo.png";
                    string emailPRGXLogoUrl = appFilePhysicalPath + @"\Content\Images\logo\simTraxFooterLogoForEmail.png";
                    var emailSubject = emailMessage.Subject + " - " + userDetails.OrganisationName;
                    _emailserviceFacade.SendEmail("support@sim.prgx.com", string.Empty, string.Empty, emailSubject, emailMessage.Content, emailSIMLogoUrl, emailPRGXLogoUrl, "", "", appFilePhysicalPath);
                }
                catch (Exception ex)
                {
                    Logger.Error("SupplierController : SendNotificationMailToAuditor() : Caught an Error " + ex);
                }
            }
        }

        public ActionResult Home()
        {
            if (Session[Constants.SESSION_SUBMIT_VERIFICATION] != null)
            {
                string message = (string)Session[Constants.SESSION_SUBMIT_VERIFICATION];
                TempData["SubmitMessage"] = message;
                Session.Remove(Constants.SESSION_SUBMIT_VERIFICATION);
            }
            return View();
        }

        public ActionResult GeneralInformationAndContacts()
        {
            try
            {

                var model = new SellerRegister();
                var regionCode = Configuration.REGION_IDENTIFIER;


                var masterDataList = _masterDataService.GetMasterDataForSellerRegistration(regionCode);

                if (masterDataList != null)
                {
                    if (masterDataList.CountryList.FirstOrDefault(u => u.Text.ToLower() == regionCode.ToLower()) != null)
                    {
                        model.CountryList = new SelectList(masterDataList.CountryList.AsEnumerable(), "Value", "Text", masterDataList.CountryList.FirstOrDefault(u => u.Text.ToLower() == regionCode.ToLower()).Value).ToList();
                    }
                    else
                    {
                        model.CountryList = new SelectList(masterDataList.CountryList.AsEnumerable(), "Value", "Text").ToList();

                    }
                    model.NoOfEmployeesList = new SelectList(masterDataList.EmployeesNumberList.AsEnumerable(), "Value", "Text").ToList();
                    model.NoOfEmployeesList.Insert(0, new SelectListItem { Text = "--- Select ---", Value = "" });
                    model.TurnOverList = new SelectList(masterDataList.TurnOverList.AsEnumerable(), "Value", "Text").ToList();
                    model.TurnOverList.Insert(0, new SelectListItem { Text = "--- Select  ---", Value = "" });
                    model.BusinessSectorList = masterDataList.BusinessSectorList.Select(v => new ItemList()
                    {
                        Value = v.Value,
                        Text = v.Text,
                        Mnemonic = v.Mnemonic,
                        OrderId = v.OrderId,
                        Description = ReadResource.GetResourceForGlobalization("MS_POP_UP_DESCRIPTION_" + v.Value, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture())
                    }).ToList();


                    model.GeoGraphicSalesList = new SelectList(masterDataList.GeographicSalesList.AsEnumerable(), "Value", "Text").ToList();
                    model.GeoGraphicSuppList = new SelectList(masterDataList.GeographicServiceList.AsEnumerable(), "Value", "Text").ToList();
                    model.CompanyTypeList = new SelectList(masterDataList.CompanyTypeList.AsEnumerable(), "Value", "Text").ToList();
                    model.CompanyTypeList.Insert(0, new SelectListItem { Text = "--- Select  ---", Value = "" });
                    model.RefRegionId = masterDataList.RefRegionId;
                    model.TermsOfUseId = masterDataList.TermsOfUseId;
                    model.IdentifierTypeList = masterDataList.IdentifierTypeList;
                }
                return View(model);
            }
            catch (Exception ex)
            {
                Logger.Error("SellerController : Register() : Caught an error " + ex);
                throw;
            };
        }
        
        #region Edit Profile
        public JsonResult GetCompanyDetailsByPartyId()
        {
            try
            {
                var organization = (Session[Constants.SESSION_ORGANIZATION] != null) ? (OrganizationDetail)Session[Constants.SESSION_ORGANIZATION] : null; 
                var sellerPartyId = (organization != null)?organization.RefPartyId:0;
                var sellerData = _partyService.GetCompanyDetailsByPartyId(sellerPartyId);
                return Json(sellerData);
            }
            catch (Exception ex)
            {
                Logger.Error("SellerController : GetCompanyDetailsByPartyId() : Caught an Error " + ex);
                throw;
            }
        }

        public JsonResult SaveCompanyDetails(SellerRegister model)
        {
            var result = false;
            string message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
            result = _partyService.SaveCompanyDetails(model);
            if (result)
                message = ReadResource.GetResourceForGlobalization(Constants.COMPANY_DETAILS_SUCCESS, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
            return Json(new { result = result, message = message });
        }

        public JsonResult GetCapabilityDetailsByPartyId()
        {
            try
            {
                var organization = (Session[Constants.SESSION_ORGANIZATION] != null) ? (OrganizationDetail)Session[Constants.SESSION_ORGANIZATION] : null;
                var sellerPartyId = (organization != null) ? organization.RefPartyId : 0;
                var sellerData = _partyService.GetCapabilityDetailsByPartyId(sellerPartyId);
                return Json(sellerData);
            }
            catch (Exception ex)
            {
                Logger.Error("SellerController : GetCapabilityDetailsByPartyId() : Caught an Error " + ex);
                throw;
            }
        }
        public JsonResult SaveCapabilityDetails(SellerRegister model)
        {
            var result = false;
            string message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
            result = _partyService.SaveCapabilityDetails(model);
            if (result)
                message = ReadResource.GetResourceForGlobalization(Constants.CAPABILITY_DETAILS_SUCCESS, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
            return Json(new { result = result, message = message });
        }
        public JsonResult GetAddressDetailsByPartyId()
        {
            try
            {
                var organization = (Session[Constants.SESSION_ORGANIZATION] != null) ? (OrganizationDetail)Session[Constants.SESSION_ORGANIZATION] : null;
                var sellerPartyId = (organization != null) ? organization.RefPartyId : 0;
                var addressList = _partyService.GetAddressDetailsByPartyId(sellerPartyId);
                var addressTypeList = new List<Int16>();
                addressTypeList.Add((Int16)AddressType.Primary);
                addressTypeList.Add((Int16)AddressType.HeadQuarters);
                addressTypeList.Add((Int16)AddressType.Registered);
                var generalAddressList = addressList.Where(u => addressTypeList.Contains(u.AddressType)).ToList();
                var additionalAddressList = addressList.Where(u => !addressTypeList.Contains(u.AddressType)).OrderBy(u => u.AddressType).ToList();
                var existingGeneralAddresTypes = addressList.Select(u => u.AddressType).ToList();
                var nonExistingGeneralAddressTypes = addressTypeList.Except(existingGeneralAddresTypes);
                var jsonResult = Json(new { generalAddressList = generalAddressList, additionalAddressList = additionalAddressList, nonExistingGeneralAddressTypes = nonExistingGeneralAddressTypes }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                Logger.Error("SupplierController : GetCompanyAddressInformation() : Caught an exception " + ex);
                throw;
            }
        }
        public JsonResult GetContactDetailsByPartyId()
        {
            try
            {
                var organization = (Session[Constants.SESSION_ORGANIZATION] != null) ? (OrganizationDetail)Session[Constants.SESSION_ORGANIZATION] : null;
                var sellerPartyId = (organization != null) ? organization.RefPartyId : 0;
                var model = _partyService.GetContactDetailsByPartyId(sellerPartyId);
                var generalContacts = model.Where(u => u.ContactType != null).ToList();
                var additionalContacts = model.Where(u => u.ContactType == null).ToList();
                var jsonResult = Json(new { generalContacts = generalContacts, additionalContacts = additionalContacts, sellerPartyId = sellerPartyId }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                Logger.Error("SupplierController : GetCompanyContactInformation() : Caught an exception " + ex);
                throw;
            }
        }

        public JsonResult GetCountries()
        {
            try
            {
                var data = _masterDataService.GetCountries();
                var jsonResult = Json(data);
                return jsonResult;
            }
            catch (Exception ex)
            {
                Logger.Error("SupplierController : GetCountries() : Caught an exception " + ex);
                throw;
            }
        }

        public JsonResult GetMarketingDetailsByPartyId()
        {
            try
            {
                var organization = (Session[Constants.SESSION_ORGANIZATION] != null) ? (OrganizationDetail)Session[Constants.SESSION_ORGANIZATION] : null;
                var sellerPartyId = (organization != null) ? organization.RefPartyId : 0;
                var sellerData = _partyService.GetMarketingDetailsByPartyId(sellerPartyId);
                sellerData.LogoFilePath = (!string.IsNullOrEmpty(sellerData.LogoFilePath)) ? Url.Content(Path.Combine(Configuration.DocumentFileUploadPath, sellerData.LogoFilePath)) : null;
                return Json(sellerData);
            }
            catch (Exception ex)
            {
                Logger.Error("SellerController : GetMarketingDetailsByPartyId() : Caught an Error " + ex);
                throw;
            }
        }
        public JsonResult GetReferenceDetailsBySellerId()
        {
            try
            {
                var organization = (Session[Constants.SESSION_ORGANIZATION] != null) ? (OrganizationDetail)Session[Constants.SESSION_ORGANIZATION] : null;
                var sellerId = (organization != null) ? organization.RefSellerId : 0;
                var sellerData = _partyService.GetReferenceDetailsBySellerId(sellerId);
                return Json(sellerData);
            }
            catch (Exception ex)
            {
                Logger.Error("SellerController : GetReferenceDetailsByPartyId() : Caught an Error " + ex);
                throw;
            }
        }

        [HttpPost]
        public JsonResult GetBankDetailsByOrganisationId()
        {
            try
            {
                var organization = (Session[Constants.SESSION_ORGANIZATION] != null) ? (OrganizationDetail)Session[Constants.SESSION_ORGANIZATION] : null;
                var organizationId = (organization != null) ? organization.OrganizationId : 0;
                var sellerData = _partyService.GetBankDetailsByOrganisationId(organizationId);
                return Json(sellerData);
            }
            catch (Exception ex)
            {
                Logger.Error("SellerController : GetReferenceDetailsByPartyId() : Caught an Error " + ex);
                throw;
            }
        }

        [HttpPost]
        public JsonResult SaveMarketingDetails(SellerRegister model)
        {
            var result = false;
            object responseToView = null;
            try
            {
               
                      result = _partyService.SaveMarketingDetails(model);
                if (result)
                        responseToView = new { success = result, message = ReadResource.GetResourceForGlobalization(Constants.MARKETING_DETAILS_SUCCESS, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()) };
                    else
                        responseToView = new { success = result, message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()) };
            }
            catch (Exception ex)
            {
                Logger.Error("SellerController : SaveMarketingDetails() : Caught an exception " + ex);
                throw;
            }
            if (Request.IsAjaxRequest() == false && (!string.IsNullOrWhiteSpace(Request.Form["oldBrowser"])))
            {
                return Json(responseToView, "text/html");
            }
            else
            {
                return Json(responseToView, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult UploadSellerLogo()
        {
            object responseToView = null;
            string message = Constants.DEFAULT_ERROR_MESSAGE;
            bool result = false;
            try
            {
                if (Request.Files.Count > 0  && Convert.ToBoolean(Request.Form["IsLogo"]))
                {
                  
                    using (var binaryReader = new BinaryReader(Request.Files[0].InputStream))
                    {
                        result = CommonMethods.IsValidFileSignature(binaryReader.ReadBytes(16), Request.Files[0].FileName);
                        var organization = (Session[Constants.SESSION_ORGANIZATION] != null) ? (OrganizationDetail)Session[Constants.SESSION_ORGANIZATION] : null;
                        var sellerPartyId = (organization != null) ? organization.RefPartyId : 0;
                        if (result)
                        {
                            CommonMethods.DeleteDirectoryOnServer(Path.Combine(Server.MapPath(Configuration.DocumentFileUploadPath),sellerPartyId.ToString(), "Logo"));
                            string folderPath = Path.Combine(sellerPartyId.ToString(), "Logo");
                            CommonMethods.CreateFileFolder(Path.Combine(Configuration.DocumentFileUploadPath, folderPath));
                            string fileName = Request.Files[0].FileName.Substring(Request.Files[0].FileName.LastIndexOf("\\") + 1);
                        //    string hashFileName = CommonMethods.EncryptMD5Password(String.Concat(fileName.ToLower(), sellerPartyId));

                            var filePath = Path.Combine(Server.MapPath(Configuration.DocumentFileUploadPath), folderPath, fileName);
                            // to bring back the pointer to start of stream
                            binaryReader.BaseStream.Seek(0, SeekOrigin.Begin);
                            using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                            {
                                int numBytesToRead = Request.Files[0].ContentLength;
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
                            var document = new Document()
                            {
                                FileName = fileName,
                                FilePath = Path.Combine(folderPath, fileName),
                                ContentLength = Request.Files[0].ContentLength,
                                ContentType = Request.Files[0].ContentType
                            };
                            result = _partyService.AddOrUpdateSellerLogoDetails(document,sellerPartyId);
                            responseToView = new { result = result,message=message};
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("SellerController : UploadSellerLogo(): Caught an error" + ex);
                throw;
            }
            if (Request.IsAjaxRequest() == false && (!string.IsNullOrWhiteSpace(Request.Form["oldBrowser"])))
            {
                return Json(responseToView, "text/html");
            }
            else
            {
                return Json(responseToView, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult AddOrUpdateAddressList(string model)
        {
            try
            {
                JavaScriptSerializer objJavascript = new JavaScriptSerializer();
                Address addressDetails = objJavascript.Deserialize<Address>(model);
                var result = false;
                var message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                var organization = (Session[Constants.SESSION_ORGANIZATION] != null) ? (OrganizationDetail)Session[Constants.SESSION_ORGANIZATION] : null;
                var sellerPartyId = (organization != null) ? organization.RefPartyId : 0;
                var sellerUserPartyId = ((UserDetails)Session[Constants.SESSION_USER]).RefPersonPartyId;
               var addressID  = _partyService.AddOrUpdateAddressList(addressDetails, sellerPartyId, sellerUserPartyId);
                if (addressID  > 0)
                {
                    result = true;
                    if (addressDetails.Id > 0)
                    {
                        message = ReadResource.GetResourceForGlobalization(Constants.UPDATE_ADDRESS_SUCCESS, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                    }
                    else
                    {
                        message = ReadResource.GetResourceForGlobalization(Constants.ADD_ADDRESS_SUCCESS, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();

                    }
                }
                return Json(new { result = result, message = message });

            }
            catch (Exception ex)
            {
                Logger.Error("SupplierController : AddOrUpdateAddressList() : Caught an Error " + ex);
                throw;
            }
        }
     
        public JsonResult DeleteAddressById(long addressId)
        {
            try
            {

                var result = false;
                var message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                if (addressId > 0)
                {
                    result = _partyService.DeleteAddressById(addressId);
                }
                if(result)
                {
                    message = ReadResource.GetResourceForGlobalization(Constants.DELETE_ADDRESS_SUCCESS, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                }
                return Json(new { result = result, message = message });
            }
            catch (Exception ex)
            {
                Logger.Error("SupplierController : DeleteAddress() : Caught an exception " + ex);
                throw;
            }
        }

        [HttpPost]
        public JsonResult GetAddressDetailsByContactMethodId(long contactMethodId)
        {
            try
            {

                Address data = null;
                if (contactMethodId > 0)
                {
                    data = _partyService.GetAddressDetailsByContactMethodId(contactMethodId);
                }
                return Json(data);
            }
            catch (Exception ex)
            {
                Logger.Error("SellerController : GetAddressDetailsByAddressId() : Caught an exception " + ex);
                throw;
            }
        }

        [HttpPost]
        public JsonResult AddOrUpdateContactsList(string model, string addressModel)
        {
            try
            {
                JavaScriptSerializer objJavascript = new JavaScriptSerializer();
                ContactPerson contactDetails = objJavascript.Deserialize<ContactPerson>(model);
                var organization = (Session[Constants.SESSION_ORGANIZATION] != null) ? (OrganizationDetail)Session[Constants.SESSION_ORGANIZATION] : null;
                var sellerPartyId = (organization != null) ? organization.RefPartyId : 0;
                var sellerUserPartyId = ((UserDetails)Session[Constants.SESSION_USER]).RefPersonPartyId;
                contactDetails.SellerPartyId = sellerPartyId;
                long refAddressContactMethod = 0;
                if (addressModel != "null")
                {
                    Address addressDetails = objJavascript.Deserialize<Address>(addressModel);
                    addressDetails.SellerPartyId = sellerPartyId;
                    refAddressContactMethod = _partyService.AddOrUpdateAddressList(addressDetails, sellerPartyId,sellerUserPartyId);
                    contactDetails.RefAddressContactMethod = refAddressContactMethod;
                }
                var result = false;
                var IsRoleAlreadyExists = false;
                var ExistingRoleUser = "";
                if (contactDetails.ContactType > 0)
                {
                   var ContactWithRole = _partyService.GetContactByRoleAndPartyId(sellerPartyId, Convert.ToInt32(contactDetails.ContactType));
                    if (ContactWithRole != null && contactDetails.Id != ContactWithRole.Id)
                    {
                        IsRoleAlreadyExists = true;
                        ExistingRoleUser = ContactWithRole.FirstName + " " + ContactWithRole.LastName;
                    }
                    if (ContactWithRole != null && contactDetails.Id > 0 && contactDetails.Id == ContactWithRole.Id)
                    {
                        IsRoleAlreadyExists = false;
                    }
                }
                if (contactDetails.ContactType == 0) { contactDetails.ContactType = null; }
                if (!IsRoleAlreadyExists)
                {
                
                   result = _partyService.AddOrUpdateContactsList(contactDetails,sellerPartyId,sellerUserPartyId);
                }
                var message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                if (result)
                {
                    if (contactDetails.Id > 0)
                    {
                        message = ReadResource.GetResourceForGlobalization(Constants.UPDATE_CONTACT_SUCCESS, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                    }
                    else
                    {
                        message = ReadResource.GetResourceForGlobalization(Constants.ADD_CONTACT_SUCCESS, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                    }
                }
                return Json(new { IsRoleAlreadyExists = IsRoleAlreadyExists, ExistingRoleUser = ExistingRoleUser, result = result, refAddressContactMethod = refAddressContactMethod, message= message });

            }
            catch (Exception ex)
            {
                Logger.Error("SupplierController : AddOrUpdateContactsList() : Caught an Error " + ex);
                throw;
            }
        }

        public JsonResult AddOrUpdateReferenceDetails(string model)
        {
            try
            {
                JavaScriptSerializer objJavascript = new JavaScriptSerializer();
                Invitee referenceDetails = objJavascript.Deserialize<Invitee>(model);
               var result = false;
                var organization = (Session[Constants.SESSION_ORGANIZATION] != null) ? (OrganizationDetail)Session[Constants.SESSION_ORGANIZATION] : null;
                var sellerId = (organization != null) ? organization.RefSellerId : 0;
                var sellerUserPartyId = ((UserDetails)Session[Constants.SESSION_USER]).RefPersonPartyId;

                if (sellerId > 0)
                result = _partyService.AddOrUpdateReferenceDetails(referenceDetails, sellerId,sellerUserPartyId);
                var message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                if (result)
                {
                    if (referenceDetails.Id > 0)
                    {
                        message = ReadResource.GetResourceForGlobalization(Constants.UPDATE_REFERENCE_DETAILS_SUCCESS, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                    }
                    else
                    {
                        message = ReadResource.GetResourceForGlobalization(Constants.ADD_REFERENCE_DETAILS_SUCCESS, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                    }
                }
                return Json(new { result = result, message= message});
            }
            catch (Exception ex)
            {
                Logger.Error("SellerController : AddOrUpdateReferenceDetails() : Caught an exception " + ex);
                throw;
            }
        }

        public JsonResult UpdateMappingContactTypes(string model)
        {
            try
            {
                JavaScriptSerializer objJavascript = new JavaScriptSerializer();
                ContactPerson contactDetails = objJavascript.Deserialize<ContactPerson>(model);
                var organization = (Session[Constants.SESSION_ORGANIZATION] != null) ? (OrganizationDetail)Session[Constants.SESSION_ORGANIZATION] : null;
                var sellerPartyId = (organization != null) ? organization.RefPartyId : 0;
                var sellerUserPartyId = ((UserDetails)Session[Constants.SESSION_USER]).RefPersonPartyId;
                contactDetails.SellerPartyId = sellerPartyId;
                var result = false;
                if (contactDetails.ContactType == 0) { contactDetails.ContactType = null; }

                result = _partyService.AddOrUpdateContactsList(contactDetails, sellerPartyId,sellerUserPartyId);
                var message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                if (result)
                {
                    if (contactDetails.Id > 0)
                    {
                        message = ReadResource.GetResourceForGlobalization(Constants.UPDATE_CONTACT_SUCCESS, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                    }
                    else
                    {
                        message = ReadResource.GetResourceForGlobalization(Constants.ADD_CONTACT_SUCCESS, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                    }
                }
                return Json(new { result = result,message = message});
            }
            catch (Exception ex)
            {
                Logger.Error("SupplierController : MappingContacts() : Caught an Error " + ex);
                throw;
            }
        }

        public JsonResult BuyersAssignedToContact(long contactPartyId)
        {
            try
            {
                var count = 0;
                if (contactPartyId > 0)
                {
                    count = _partyService.BuyersAssignedToContact(contactPartyId);
                }
                return Json(count);
            }
            catch (Exception ex)
            {
                Logger.Error("SellerController : BuyersAssignedToContact() : Caught an exception " + ex);
                throw;
            }
        }
        public JsonResult DeleteContactById(long contactPartyId,long contactId)
        {
            try
            {

                var result = false;
                var message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                var organization = (Session[Constants.SESSION_ORGANIZATION] != null) ? (OrganizationDetail)Session[Constants.SESSION_ORGANIZATION] : null;
                var sellerPartyId = (organization != null) ? organization.RefPartyId : 0;

                if (contactPartyId  > 0 && sellerPartyId> 0)
                {
                    result = _partyService.DeleteContactById(contactPartyId, sellerPartyId);
                }
                if (result)
                {
                    message = ReadResource.GetResourceForGlobalization(Constants.IS_DELETED_SUCCESSFULLY, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                }
                return Json(new { result = result, message= message});
            }
            catch (Exception ex)
            {
                Logger.Error("SellerController : DeleteContactById() : Caught an exception " + ex);
                throw;
            }
        }
        public JsonResult DeleteReferenceById(long referenceId)
        {
            try
            {

                var result = false;
                var message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                if (referenceId > 0)
                {
                    result = _partyService.DeleteReferenceById(referenceId);
                }
                if (result)
                {
                    message = ReadResource.GetResourceForGlobalization(Constants.IS_DELETED_SUCCESSFULLY, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                }
                return Json(new { result = result, message = message });
            }
            catch (Exception ex)
            {
                Logger.Error("SellerController : DeleteReferenceById() : Caught an exception " + ex);
                throw;
            }
        }
        public JsonResult DeleteBankAccountById(long bankId)
        {
            try
            {

                var result = false;
                var message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                if (bankId > 0)
                {
                    result = _partyService.DeleteBankAccountById(bankId);
                }
                if (result)
                {
                    message = ReadResource.GetResourceForGlobalization(Constants.IS_DELETED_SUCCESSFULLY, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                }
                return Json(new { result = result, message = message });
            }
            catch (Exception ex)
            {
                Logger.Error("SellerController : DeleteReferenceById() : Caught an exception " + ex);
                throw;
            }
        }
        public JsonResult AddOrUpdateBankDetails(string model)
        {
            try
            {
                JavaScriptSerializer objJavascript = new JavaScriptSerializer();
                BankAccount bankDetails = objJavascript.Deserialize<BankAccount>(model);
                var result = false;
                var organization = (Session[Constants.SESSION_ORGANIZATION] != null) ? (OrganizationDetail)Session[Constants.SESSION_ORGANIZATION] : null;
                var organisationId = (organization != null) ? organization.OrganizationId : 0;
                var sellerUserPartyId = ((UserDetails)Session[Constants.SESSION_USER]).RefPersonPartyId;
                if (organisationId > 0)
                    result = _partyService.AddOrUpdateBankDetails(bankDetails, organisationId,sellerUserPartyId);
                var message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                if (result)
                {
                    if (bankDetails.Id > 0)
                    {
                        message = ReadResource.GetResourceForGlobalization(Constants.UPDATE_BANK_DETAILS_SUCCESS, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                    }
                    else
                    {
                        message = ReadResource.GetResourceForGlobalization(Constants.ADD_BANK_DETAILS_SUCCESS, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()).ToString();
                    }
                }
                return Json(new { result = result, message = message });
            }
            catch (Exception ex)
            {
                Logger.Error("SellerController : AddOrUpdateBankDetails() : Caught an exception " + ex);
                throw;
            }
        }

        [HttpPost]
        public JsonResult GetSellerProfilePercentage()
        {
            try
            {
                var sellerData = new ProfileSummary();
                var organization = (Session[Constants.SESSION_ORGANIZATION] != null) ? (OrganizationDetail)Session[Constants.SESSION_ORGANIZATION] : null;
                if (organization == null)
                    return Json(sellerData);
                var organizationId =  organization.OrganizationId;
                var sellerPartyId = organization.RefPartyId;
                var sellerId = organization.RefSellerId;
                sellerData = _partyService.GetSellerProfilePercentage(sellerPartyId,sellerId,organizationId);
                return Json(sellerData);
            }
            catch (Exception ex)
            {
                Logger.Error("SellerController : GetSellerProfilePercentage() : Caught an Error " + ex);
                throw;
            }
        }
        public JsonResult BuyersAssignedToAddress(long addressId, bool checkRemittance = false)
        {
            try
            {
                var count = 0;
                if (checkRemittance)
                {
                    count = _partyService.BuyersAssignedToAddress(addressId);
                }
                return Json(count);
            }
            catch (Exception ex)
            {
                Logger.Error("SellerController : BuyersAssignedToAddress() : Caught an exception " + ex);
                throw;
            }
        }

        public JsonResult BuyerSupplierReferenceList(int pageNo, string sortParameter, int sortDirection, string buyerName, long referenceId)
        {
            try
            {
                var totalRecords = 0;
                var organization = (Session[Constants.SESSION_ORGANIZATION] != null) ? (OrganizationDetail)Session[Constants.SESSION_ORGANIZATION] : null;
                var sellerId = organization.RefSellerId;
                var buyerList = _partyService.BuyerSupplierReferenceList(pageNo, sortParameter, sortDirection, buyerName, sellerId, referenceId, out totalRecords);
                return Json(new { data = buyerList, totalRecords = totalRecords });
            }
            catch (Exception ex)
            {
                Logger.Error("SupplierController : BuyerSupplierReferenceList() : Caught an exception " + ex);
                throw;
            }
        }

        public JsonResult AddOrRemoveBuyerSupplierReferenceDetails(bool isAdd, long buyerId, long referenceId)
        {
            try
            {
                var result = false;
                var message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
                result = _partyService.AddOrRemoveBuyerSupplierReferenceDetails(isAdd, buyerId, referenceId);
                if (isAdd)
                    return Json(new { result = result, message = ReadResource.GetResourceForGlobalization(Constants.ASSIGN_REFERENCE_SUCCESS, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()) });
                else
                    return Json(new { result = result, message = ReadResource.GetResourceForGlobalization(Constants.UNASSIGN_REFERENCE_SUCCESS, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()) });

            }
            catch (Exception ex)
            {
                Logger.Error("SellerController : AddOrRemoveBuyerSupplierReferenceDetails() : Caught an exception " + ex);
                throw;
            }
        }
        public JsonResult BuyerSupplierAddressDetails(int pageNo, string sortParameter, int sortDirection, string buyerName, long addressId)
        {
            try
            {
                var totalRecords = 0;
                var organization = (Session[Constants.SESSION_ORGANIZATION] != null) ? (OrganizationDetail)Session[Constants.SESSION_ORGANIZATION] : null;
                var sellerPartyId = organization.RefPartyId;
                var buyerList = _partyService.BuyerSupplierAddressList(pageNo, sortParameter, sortDirection, buyerName, sellerPartyId, addressId, out totalRecords);
                return Json(new { data = buyerList, totalRecords = totalRecords });
            }
            catch (Exception ex)
            {
                Logger.Error("SellerController : BuyerSupplierAddressList() : Caught an exception " + ex);
                throw;
            }
        }

        public JsonResult AddOrRemoveBuyerSupplierAddressDetails(bool isAdd, long buyerPartyId, long refContactMethodId)
        {
            try
            {
                var result = false;
                var message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
                result = _partyService.AddOrRemoveBuyerSupplierAddressDetails(isAdd, buyerPartyId, refContactMethodId);
                if (isAdd)
                    return Json(new { result = result, message = ReadResource.GetResourceForGlobalization(Constants.ASSIGN_ADDRESS_SUCCESS, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()) });
                else
                    return Json(new { result = result, message = ReadResource.GetResourceForGlobalization(Constants.UNASSIGN_ADDRESS_SUCCESS, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()) });

            }
            catch (Exception ex)
            {
                Logger.Error("SellerController : AddOrRemoveBuyerSupplierAddressDetails() : Caught an exception " + ex);
                throw;
            }
        }

        public JsonResult BuyerSupplierBankList(int pageNo, string sortParameter, int sortDirection, string buyerName, long bankId)
        {
            try
            {
                var totalRecords = 0;
                var organization = (Session[Constants.SESSION_ORGANIZATION] != null) ? (OrganizationDetail)Session[Constants.SESSION_ORGANIZATION] : null;
                var organisationId = organization.OrganizationId;
                var buyerList = _partyService.BuyerSupplierBankList(pageNo, sortParameter, sortDirection, buyerName, organisationId, bankId, out totalRecords);
                return Json(new { data = buyerList, totalRecords = totalRecords });
            }
            catch (Exception ex)
            {
                Logger.Error("SupplierController : BuyerSupplierBankList() : Caught an exception " + ex);
                throw;
            }
        }
        public JsonResult AddOrRemoveBuyerSupplierBankDetails(bool isAdd, long buyerPartyId, long bankId)
        {
            try
            {
                var result = false;
                var message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
                result = _partyService.AddOrRemoveBuyerSupplierBankDetails(isAdd, buyerPartyId, bankId);
                if (isAdd)
                    return Json(new { result = result, message = ReadResource.GetResourceForGlobalization(Constants.ASSIGN_BANK_DETAILS_SUCCESS, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()) });
                else
                    return Json(new { result = result, message = ReadResource.GetResourceForGlobalization(Constants.UNASSIGN_BANK_DETAILS_SUCCESS, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()) });

            }
            catch (Exception ex)
            {
                Logger.Error("SellerController : AddOrRemoveBuyerSupplierBankDetails() : Caught an exception " + ex);
                throw;
            }
        }

        public JsonResult BuyerSupplierContactsList(int pageNo, string sortParameter, int sortDirection, string buyerName, long contactPartyId)
        {
            try
            {
                var totalRecords = 0;
                var organization = (Session[Constants.SESSION_ORGANIZATION] != null) ? (OrganizationDetail)Session[Constants.SESSION_ORGANIZATION] : null;
                var sellerId = organization.RefSellerId;
                var buyerList = _partyService.BuyerSupplierContactsList(pageNo, sortParameter, sortDirection, buyerName, sellerId, contactPartyId, out totalRecords);
                return Json(new { data = buyerList, totalRecords = totalRecords });
            }
            catch (Exception ex)
            {
                Logger.Error("SupplierController : BuyerSupplierContactsList() : Caught an exception " + ex);
                throw;
            }
        }

        public JsonResult AddOrUpdateBuyerContacts(bool isAssigned, long buyerPartyId, long contactPartyId,int role)
        {
            try
            {
                var result = false;
                var message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
                var organization = (Session[Constants.SESSION_ORGANIZATION] != null) ? (OrganizationDetail)Session[Constants.SESSION_ORGANIZATION] : null;
                var sellerPartyId = organization.RefPartyId;
                var sellerUserPartyId = ((UserDetails)Session[Constants.SESSION_USER]).RefPersonPartyId;

                result = _partyService.AddOrUpdateBuyerContacts(isAssigned, buyerPartyId, contactPartyId,role, sellerPartyId, sellerUserPartyId);
                return Json(new { result = result, message = ReadResource.GetResourceForGlobalization(Constants.SAVED_SUCCESSFULLY, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (System.Globalization.CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()) });
             
            }
            catch (Exception ex)
            {
                Logger.Error("SellerController : AddOrRemoveBuyerSupplierReferenceDetails() : Caught an exception " + ex);
                throw;
            }
        }

        #endregion

        public JsonResult GetIndustryCodesofSellerWithValues()
        {
            List<TreeStructure> treeViewVM = new List<TreeStructure>();
            try
            {
                var organization = (Session[Constants.SESSION_ORGANIZATION] != null) ? (OrganizationDetail)Session[Constants.SESSION_ORGANIZATION] : null;
                var sellerPartyId = (organization != null) ? organization.RefPartyId : 0;
                var selectedIndustryCodeValues = _partyService.GetIndustryCodesByOrganisationPartyId(sellerPartyId);
                var regionCode = Configuration.REGION_IDENTIFIER;
                var IndustryCodeDM = new List<IndustryCode>();
                IndustryCodeDM = _masterDataService.GetIndustryCodes(regionCode, null, true);
                if (IndustryCodeDM != null && IndustryCodeDM.Count > 0)
                {
                    foreach (var item in IndustryCodeDM)
                    {
                        if (item.RefParentId == null)
                        {
                            var childNodes = IndustryCodeDM.Where(a => a.RefParentId == item.Id).ToList();
                            var mainNode = new TreeStructure();
                            mainNode.attr = new Attributes() { id = item.Id.ToString(), IsChecked = selectedIndustryCodeValues.Exists(v => v == item.Id) };
                            mainNode.data = item.SectorName + " (" + item.CodeNumber.ToString() + ")";
                            mainNode.state = childNodes != null && childNodes.Count > 0 ? "closed" : null;
                            if (childNodes != null && childNodes.Count > 0)
                            {
                                var nodes = new List<TreeStructure>();
                                AddChildNodes(childNodes, IndustryCodeDM, nodes, selectedIndustryCodeValues);
                                mainNode.children = nodes;
                            }
                            treeViewVM.Add(mainNode);
                        }
                    }
                }

            }
            catch (Exception exception)
            {
                Logger.Error("SupplierController : GetParentSICCodes() : Caught an error " + exception);
                throw;
            }
            return Json(treeViewVM);
        }

        private void AddChildNodes(List<IndustryCode> IndustryCodeDM, List<IndustryCode> AllNodes, List<TreeStructure> nodes, List<long> selectedIndustryCodeValues)
        {
            try
            {
                foreach (var item in IndustryCodeDM)
                {
                    var childNodes = AllNodes.Where(a => a.RefParentId == item.Id).ToList();
                    var node = new TreeStructure();
                    node.attr = new Attributes() { id = item.Id.ToString(), IsChecked = selectedIndustryCodeValues.Exists(v => v == item.Id) };
                    node.data = item.SectorName + " (" + item.CodeNumber.ToString() + ")";
                    node.state = childNodes != null && childNodes.Count > 0 ? "closed" : null;
                    if (childNodes != null && childNodes.Count > 0)
                    {
                        var subNodes = new List<TreeStructure>();
                        AddChildNodes(childNodes, AllNodes, subNodes, selectedIndustryCodeValues);
                        node.children = subNodes;
                    }
                    nodes.Add(node);
                }
            }
            catch (Exception exception)
            {
                Logger.Error("AccountController : AddChildNodes() : Caught an error " + exception);
                throw;
            }
        }
    }
}