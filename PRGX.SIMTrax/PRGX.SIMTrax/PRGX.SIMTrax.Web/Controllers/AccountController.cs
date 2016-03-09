using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.Domain.Util;
using PRGX.SIMTrax.ServiceFacade;
using PRGX.SIMTrax.ServiceFacade.Abstract;
using PRGX.SIMTrax.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PRGX.SIMTrax.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IMasterDataServiceFacade _masterDataService;
        private readonly IUserServiceFacade _userServiceFacade;
        private readonly IEmailServiceFacade _emailServiceFacade;
        private readonly IPartyServiceFacade _partyService;
        private readonly IRoleServiceFacade _roleService;
        private readonly ICampaignServiceFacade _campaignServiceFacade;

        public AccountController()
        {
            _masterDataService = new MasterDataServiceFacade();
            _userServiceFacade = new UserServiceFacade();
            _emailServiceFacade = new EmailServiceFacade();
            _partyService = new PartyServiceFacade();
            _roleService = new RoleServiceFacade();
            _campaignServiceFacade = new CampaignServiceFacade();
        }

        public JsonResult GetIndustryCodes()
        {
            List<TreeStructure> treeViewVM = new List<TreeStructure>();
            try
            {
                var regionCode = Configuration.REGION_IDENTIFIER;
                var IndustryCodeDM = new List<IndustryCode>();
                IndustryCodeDM = _masterDataService.GetIndustryCodes(regionCode,null, true);
                if (IndustryCodeDM != null && IndustryCodeDM.Count > 0)
                {
                    foreach (var item in IndustryCodeDM)
                    {
                        if (item.RefParentId == null)
                        {
                            var childNodes = IndustryCodeDM.Where(a => a.RefParentId == item.Id).ToList();
                            var mainNode = new TreeStructure();
                            mainNode.attr = new Attributes() { id = item.Id.ToString(), IsChecked = false };
                            mainNode.data = item.SectorName + " (" + item.CodeNumber.ToString() + ")";
                            mainNode.state = childNodes != null && childNodes.Count > 0 ? "closed" : null;
                            if (childNodes != null && childNodes.Count > 0)
                            {
                                var nodes = new List<TreeStructure>();
                                AddChildNodes(childNodes, IndustryCodeDM, nodes);
                                mainNode.children = nodes;
                            }
                            treeViewVM.Add(mainNode);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Error("AccountController : GetIndustryCodes() : Caught an error " + exception);
                throw;
            }
            return Json(treeViewVM);
        }

        private void AddChildNodes(List<IndustryCode> IndustryCodeDM, List<IndustryCode> AllNodes, List<TreeStructure> nodes)
        {
            try
            {
                foreach (var item in IndustryCodeDM)
                {
                    var childNodes = AllNodes.Where(a => a.RefParentId == item.Id).ToList();
                    var node = new TreeStructure();
                    node.attr = new Attributes() { id = item.Id.ToString(), IsChecked = false };
                    node.data = item.SectorName + " (" + item.CodeNumber.ToString() + ")";
                    node.state = childNodes != null && childNodes.Count > 0 ? "closed" : null;
                    if (childNodes != null && childNodes.Count > 0)
                    {
                        var subNodes = new List<TreeStructure>();
                        AddChildNodes(childNodes, AllNodes, subNodes);
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

        public JsonResult AddSellerUserDetails(SellerRegister model, int CampaignId, int CampaignSupplierId)
        {
            try
            {
                var result = false;
                var message = string.Empty;
                long sellerPartyId = 0;
                long userPartyId = 0;
                var decryptedPassword = DecryptStringAES(model.Password);
                model.Password = CommonMethods.EncryptMD5Password(model.Email.ToLower() + decryptedPassword.Trim());
                model.Status = CompanyStatus.Started;
                result = _userServiceFacade.AddUser(model, out sellerPartyId, out userPartyId);

                if (Session[Constants.SESSION_CAMPAIGN_ID] != null)
                {
                    var campaignId = Convert.ToInt64(Session[Constants.SESSION_CAMPAIGN_ID].ToString());
                    var addReferrer = _campaignServiceFacade.AddSupplierReferrer(campaignId, sellerPartyId, true);
                    if (addReferrer)
                    {
                        var campaign = _campaignServiceFacade.GetCampaignInfo(campaignId);
                        //var buyerSupplierMapping = new BuyerSupplierMapping()
                        //{
                        //    BuyerId = Convert.ToInt64(campaign.BuyerId),
                        //    SupplierId = Convert.ToInt64(companyId)
                        //};
                        //var createdRecord = _buyerSupplierMappingService.InsertBuyerSupplierMapping(buyerSupplierMapping);
                        if (Session[Constants.SESSION_PRE_REG_ID] != null)
                        {
                            var preRegId = Convert.ToInt64(Session[Constants.SESSION_PRE_REG_ID]);
                            _campaignServiceFacade.SetPreRegSupplierToRegistered(preRegId, sellerPartyId);

                            var preRegSupplierRecord = _campaignServiceFacade.GetPreRegSupplierDetails(preRegId);
                            preRegSupplierRecord.IdentifierTypeList = model.IdentifierTypeList;
                            preRegSupplierRecord.SellerPartyId = sellerPartyId;
                            preRegSupplierRecord.UserPartyId = userPartyId;
                            preRegSupplierRecord.Status = model.Status;
                            preRegSupplierRecord.RefRegionId = model.RefRegionId;
                            _partyService.UpdateSellerPartyDetails(preRegSupplierRecord);

                            //var supplierProducts = _supplierProductService.GetSupplierProduct(companyId, SupplierProductStatus.NotSubmitted);
                            //foreach (var supplierProduct in supplierProducts)
                            //{
                            //    if ((preRegSupplierRecord.IsFITMappedToBuyer.Value && supplierProduct.ProductId == (short)Pillar.FinanceInsuranceTax) || ((preRegSupplierRecord.IsHSMappedToBuyer.Value && supplierProduct.ProductId == (short)Pillar.HealthSafety)) || ((preRegSupplierRecord.IsDSMappedToBuyer.Value && supplierProduct.ProductId == (short)Pillar.DataSecurity)))
                            //    {
                            //        var buyerSupplierProductMapping = new BuyerSupplierProductMapping
                            //        {
                            //            BuyerSupplierMappingId = createdRecord.Id,
                            //            ProductId = supplierProduct.ProductId,
                            //            SupplierProductId = supplierProduct.SupplierProductId,
                            //        };
                            //        _buyerSupplierProductMappingService.Add(buyerSupplierProductMapping, supplierProduct.ProductId);
                            //    }
                            //}
                        }
                        else
                        {
                            // For self registered campaign we are checking whether there is any default product mapped 
                            //to the buyer or not and if it is then mapping those products to supplier.
                            //var buyerCompanyId = campaign.BuyerId;
                            //var defaultProductService = new BuyerDefaultProductsService();
                            //var buyerDefaultProducts = defaultProductService.GetDefaultProductsOfBuyer((long)buyerCompanyId);
                            //if (buyerDefaultProducts != null)
                            //{
                            //    List<SupplierProduct> supplierProducts = _supplierProductService.GetSupplierProduct(companyId, SupplierProductStatus.NotSubmitted);
                            //    foreach (var product in buyerDefaultProducts)
                            //    {
                            //        var supplierProductId = supplierProducts.Any(a => a.ProductId == product.ProductId) ?
                            //            supplierProducts.Where(a => a.ProductId == product.ProductId).First().SupplierProductId : 0;
                            //        if (supplierProductId > 0)
                            //        {
                            //            var buyerSupplierProductMapping = new BuyerSupplierProductMapping
                            //            {
                            //                BuyerSupplierMappingId = createdRecord.Id,
                            //                ProductId = product.ProductId,
                            //                SupplierProductId = supplierProductId
                            //            };
                            //            _buyerSupplierProductMappingService.Add(buyerSupplierProductMapping, supplierProductId);
                            //        }
                            //    }
                            //}
                        }
                        if (campaign.BuyerId.HasValue)
                            _partyService.AddOrRemoveTradingSupplier(userPartyId, campaign.BuyerId.Value, sellerPartyId, true);
                        //_companyService.AddAnswerPermissionsForInvitedSuppliers(companyId, Convert.ToInt64(campaign.BuyerId));
                    }
                    Session[Constants.SESSION_CAMPAIGN_ID] = null;
                }
                else
                {
                    if(CampaignId > 0 && CampaignSupplierId > 0)
                    {
                        var campaign = _campaignServiceFacade.GetCampaignInfo(CampaignId);
                        if (campaign.BuyerId.HasValue)
                        {
                            //var buyerSupplierMapping = new BuyerSupplierMapping()
                            //{
                            //    BuyerId = Convert.ToInt64(campaign.BuyerId),
                            //    SupplierId = Convert.ToInt64(companyId)
                            //};
                            //var buyerSupplierMap = _buyerSupplierMappingService.InsertBuyerSupplierMapping(buyerSupplierMapping);

                            var addReferrer = _campaignServiceFacade.AddSupplierReferrer((long)CampaignId, sellerPartyId, true);

                            _partyService.AddOrRemoveTradingSupplier(userPartyId, campaign.BuyerId.Value, sellerPartyId, true);
                            //_companyService.AddAnswerPermissionsForInvitedSuppliers(companyId, Convert.ToInt64(campaign.BuyerId));

                            _campaignServiceFacade.SetPreRegSupplierToRegistered((long)CampaignSupplierId, sellerPartyId);
                            var preRegSupplierRecord = _campaignServiceFacade.GetPreRegSupplierDetails((long)CampaignSupplierId);
                            preRegSupplierRecord.IdentifierTypeList = model.IdentifierTypeList;
                            preRegSupplierRecord.SellerPartyId = sellerPartyId;
                            preRegSupplierRecord.UserPartyId = userPartyId;
                            preRegSupplierRecord.Status = model.Status;
                            preRegSupplierRecord.RefRegionId = model.RefRegionId;
                            //Save pre reg data in main company record.
                            _partyService.UpdateSellerPartyDetails(preRegSupplierRecord);

                            //var supplierProducts = _supplierProductService.GetSupplierProduct(companyId, SupplierProductStatus.NotSubmitted);
                            //foreach (var supplierProduct in supplierProducts)
                            //{
                            //    if ((preRegSupplierRecord.IsFITMappedToBuyer.Value && supplierProduct.ProductId == (short)Pillar.FinanceInsuranceTax)
                            //        || ((preRegSupplierRecord.IsHSMappedToBuyer.Value && supplierProduct.ProductId == (short)Pillar.HealthSafety))
                            //        || ((preRegSupplierRecord.IsDSMappedToBuyer.Value && supplierProduct.ProductId == (short)Pillar.DataSecurity)))
                            //    {
                            //        var buyerSupplierProductMapping = new BuyerSupplierProductMapping
                            //        {
                            //            BuyerSupplierMappingId = buyerSupplierMap.Id,
                            //            ProductId = supplierProduct.ProductId,
                            //            SupplierProductId = supplierProduct.SupplierProductId,
                            //        };
                            //        _buyerSupplierProductMappingService.Add(buyerSupplierProductMapping, supplierProduct.ProductId);
                            //    }
                            //}
                        }
                    }
                }

                if (result)
                {
                    var userDetails = _userServiceFacade.GetUserDetailsByOrganisationPartyId(sellerPartyId);
                    Session[Constants.SESSION_USER] = userDetails;
                    Session[Constants.SESSION_LOGIN_ID] = userDetails.LoginId;
                    Session[Constants.SESSION_ORGANIZATION] = _partyService.GetOrganizationDetail(userDetails.RefOrganisationPartyId);
                    Session[Constants.SESSION_USER_TYPE] = userDetails.UserType;

                    SendMail(model.Email);

                    return Json(new { result = result, message = ReadResource.GetResourceForGlobalization(Constants.ACCOUNT_CREATED_SUCCESSFULLY, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()), SellerPartyId = sellerPartyId, UserPartyId = userPartyId });
                }
                else
                    return Json(new { result = result, message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()) });
            }
            catch (Exception exception)
            {
                Logger.Error("AccountController : AddSellerUserDetails() : Caught an error " + exception);
                throw;
            }
        }

        private void SendMail(string loginId)
        {
            long localeId = 0;
            var emailTemplate = _emailServiceFacade.GetEmailMessage(Constants.USER_ACCOUNT_DETAILS.ToString(), localeId, loginId);

            if (Constants.IS_EMAIL_ON)
            {
                try
                {
                    // get the application path.
                    string appFilePhysicalPath = HttpContext.Request.PhysicalApplicationPath;
                    string emailSIMLogoUrl = appFilePhysicalPath + @"\Content\Images\logo\siteLogo.png";
                    string emailPRGXLogoUrl = appFilePhysicalPath + @"\Content\Images\logo\simTraxFooterLogoForEmail.png";
                    _emailServiceFacade.SendEmail(loginId, string.Empty, string.Empty, emailTemplate.Subject, emailTemplate.Content, emailSIMLogoUrl, emailPRGXLogoUrl, "", "", appFilePhysicalPath);
                }
                catch
                {
                }
            }
        }

        public JsonResult GetSuppliersListForRegistration(string text)
        {
            List<string> response = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(text))
                {
                    response = _partyService.GetSuppliersListForRegistration(text);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("AccountController : GetSuppliersListForRegistration() : Caught an exception " + ex);
                throw;
            }
            return Json(response);
        }

        [HttpPost]
        public JsonResult CheckwhetherSupplierNameExists(string organisationName)
        {
            var IsAlreadyRegistered = false;
            var IsNotRegistered = false;
            var NoRecord = false;
            long publicDataId = 0;
            try
            {

                IsAlreadyRegistered = _partyService.CheckwhetherSupplierNameExists(organisationName, out IsNotRegistered, out publicDataId);
            }
            catch (Exception ex)
            {
                Logger.Error("AccountController : IsOrganisationExists() : Caught an exception " + ex);
                throw;
            }
            return Json(new { IsAlreadyRegistered = IsAlreadyRegistered, IsNotRegistered = IsNotRegistered, NoRecord = NoRecord, publicDataId = publicDataId, message = ReadResource.GetResourceForGlobalization(Constants.ORGANISATION_ALREADY_EXISTS, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()) });
        }

        public JsonResult IsEmailExists(string email)
        {
            try
            {
                var result = false;
                var message = string.Empty;
                result = _userServiceFacade.IsEmailExists(email);
                if (result)
                    message = ReadResource.GetResourceForGlobalization(Constants.USER_ALREADY_EXISTS, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
                return Json(new { result = result, message = message });

            }
            catch (Exception exception)
            {
                Logger.Error("AccountController : IsEmailExists() : Caught an error " + exception);
                throw;
            }
        }

        public JsonResult IsOrganisationExists(string organisationName)
        {
            try
            {
                var result = false;
                var message = string.Empty;
                result = _partyService.IsOrganisationExists(organisationName);
                if (result)
                    message = ReadResource.GetResourceForGlobalization(Constants.ORGANISATION_ALREADY_EXISTS, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
                return Json(new { result = result, message = message });

            }
            catch (Exception exception)
            {

                Logger.Error("AccountController : IsOrganisationExists() : Caught an error " + exception);
                throw;
            }
        }
        
        public static string DecryptStringAES(string cipherText)
        {
            var keybytes = Encoding.UTF8.GetBytes("8080808080808080");
            var iv = Encoding.UTF8.GetBytes("8080808080808080");

            var encrypted = Convert.FromBase64String(cipherText);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
            return string.Format(decriptedFromJavascript);
        }
        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.  
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold  
            // the decrypted text.  
            string plaintext = null;

            // Create an RijndaelManaged object  
            // with the specified key and IV.  
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings  
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.  
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption.  
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream  
                                // and place them in a string.  
                                plaintext = srDecrypt.ReadToEnd();

                            }

                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }

        public ActionResult Login()
        {
            try
            {
                var user = (Session[Constants.SESSION_USER] != null) ? (UserDetails)Session[Constants.SESSION_USER] : null;
                if (user != null)
                {
                    var result = _userServiceFacade.UpdateUserLastLoginDate(user.UserId, user.LoginId);
                    UserType type = (UserType)Convert.ToInt64(Session[Constants.SESSION_USER_TYPE]);
                    Session[Constants.SESSION_LOGIN_ID] = user.LoginId;
                    var organization = (OrganizationDetail)Session[Constants.SESSION_ORGANIZATION];
                    if (!(user.UserType == (long)UserType.AdminAuditor || user.UserType == (long)UserType.Auditor))
                    {
                        if (!_userServiceFacade.HasUserAcceptedLatestTermsOfUse(user.UserId))
                        {
                            TempData["TermsAcceptMessage"] = "New terms of use - Please accept below to continue";
                            return RedirectToAction("TermsOfUse", "Account");
                        }
                    }
                    switch (type)
                    {
                        case UserType.Supplier:
                        case UserType.AdminSupplier:
                            if (organization.Status >= (short)CompanyStatus.Submitted)
                                return RedirectToAction("Home", "Supplier");
                            else
                                return RedirectToAction("Register", "Supplier");
                        case UserType.Buyer:
                        case UserType.AdminBuyer:
                            var buyerPermissionList = new List<ItemList>();
                            if (Session[Constants.SESSION_BUYER_ACCESS_PERMISSIONS] != null)
                            {
                                buyerPermissionList = (List<ItemList>)Session[Constants.SESSION_BUYER_ACCESS_PERMISSIONS];
                            }
                            else
                            {
                                buyerPermissionList = _roleService.GetUserPermissionBasedOnUserId((long)user.UserId);
                                Session[Constants.SESSION_BUYER_ACCESS_PERMISSIONS] = buyerPermissionList;
                            }
                            if ((buyerPermissionList.Find(u => u.Value == (long)BuyerPermissions.DashboardCompliance) != null) || (buyerPermissionList.Find(u => u.Value == (long)BuyerPermissions.DashboardOnboarding) != null))
                            {
                                return RedirectToAction("Home", "Buyer");
                            }
                            else if ((buyerPermissionList.Find(u => u.Value == (long)BuyerPermissions.Search) != null))
                            {
                                return RedirectToAction("SupplierSearch", "Buyer");
                            }
                            else if ((buyerPermissionList.Find(u => u.Value == (long)BuyerPermissions.KeyQuestions) != null) || (buyerPermissionList.Find(u => u.Value == (long)BuyerPermissions.References) != null) || (buyerPermissionList.Find(u => u.Value == (long)BuyerPermissions.RiskAnalysis) != null))
                            {
                                return RedirectToAction("Reports", "Buyer");
                            }
                            else if (buyerPermissionList.Find(u => u.Value == (long)BuyerPermissions.Inbox) != null)
                            {
                                return RedirectToAction("Inbox", "Buyer");
                            }
                            else
                            {
                                return RedirectToAction("Logout", "Account");
                            }
                        case UserType.Auditor:
                        case UserType.AdminAuditor:
                            var auditorPermissionList = new List<ItemList>();
                            if (Session[Constants.SESSION_AUDITOR_ACCESS_PERMISSIONS] != null)
                            {
                                auditorPermissionList = (List<ItemList>)Session[Constants.SESSION_AUDITOR_ACCESS_PERMISSIONS];
                            }
                            else
                            {
                                auditorPermissionList = _roleService.GetUserPermissionBasedOnUserId((long)user.UserId);
                                Session[Constants.SESSION_AUDITOR_ACCESS_PERMISSIONS] = auditorPermissionList;
                            }
                            return RedirectToAction("Home", "Admin");
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                Logger.Error("AccountController : Login() : Caught an exception " + ex);
                throw;
            }
        }

        [HttpPost]
        public JsonResult Login(Login model)
        {
            try
            {
                Logger.Info("AccountController : Login():" + model.LoginId + " : Click Login Time:" + DateTime.UtcNow);
                var message = string.Empty;
                var password = DecryptStringAES(model.Password);
                var encryptedPassword = CommonMethods.EncryptMD5Password(string.Concat(model.LoginId.ToLower(), password.Trim()));
                //Get the user details from database
                var isLocked = false;
                var lockCount = (!string.IsNullOrWhiteSpace(Configuration.AccountLockCount)) ? Convert.ToInt32(Configuration.AccountLockCount) : 10;
                var timeSpanLimit = (!string.IsNullOrWhiteSpace(Configuration.AccountLockTime)) ? Convert.ToInt32(Configuration.AccountLockTime) : 60;
                var userId = _userServiceFacade.GetUserIdFromCredentials(model.LoginId, encryptedPassword, lockCount, timeSpanLimit, out isLocked);
                var redirectUrl = string.Empty;
                if (userId > 0)
                {
                    var result = _userServiceFacade.UpdateUserLastLoginDate(userId, model.LoginId);

                    //var exitingRecord = _userServiceFacade.GetLoginInfoFromCommonDB(Guid.Empty, model.LoginId);
                    //CommonLogin login = null;
                    //if (exitingRecord != null && exitingRecord.Id != Guid.Empty)
                    //{
                    //    login = new CommonLogin()
                    //    {
                    //        Id = exitingRecord.Id,
                    //        LoginId = model.Email,
                    //        SIMTraxLogOff = false
                    //    };
                    //}
                    //else
                    //{
                    //    ///Putting the LoginId in CommonDataBase
                    //    login = new CommonLogin()
                    //    {
                    //        Id = Guid.NewGuid(),
                    //        LoginId = model.Email,
                    //        SIMTraxLogOff = false,
                    //        CIPSLogOff = true
                    //    };
                    //}
                    //_userService.AddLoginUserInCommonDB(login);
                    //FormsAuthentication.SetAuthCookie(login.Id.ToString(), true);

                    var user = PutUserDetailsInSession(userId);
                    if (user.NeedPasswordChange)
                    {
                        redirectUrl = "/change-password";
                        return Json(new { valid = true, redirectUrl = redirectUrl, IsTemparory = false });
                    }
                    var organization = (OrganizationDetail)Session[Constants.SESSION_ORGANIZATION];
                    if (!(user.UserType == (long)UserType.AdminAuditor || user.UserType == (long)UserType.Auditor))
                    {
                        if (!_userServiceFacade.HasUserAcceptedLatestTermsOfUse(user.UserId))
                        {
                            TempData["TermsAcceptMessage"] = "New terms of use - Please accept below to continue";
                            redirectUrl = "/terms-of-use";
                            return Json(new { valid = true, redirectUrl = redirectUrl });
                        }
                    }
                    switch ((UserType)user.UserType)
                    {
                        case UserType.Supplier:
                        case UserType.AdminSupplier:
                            if (organization.Status >= (int)CompanyStatus.Submitted)
                            {
                                redirectUrl = "/supplier-home";
                            }
                            else
                            {
                                redirectUrl = "/Supplier/Register";
                            }
                            break;
                        case UserType.Buyer:
                        case UserType.AdminBuyer:
                            redirectUrl = GetBuyerRedirectUrl();
                            break;
                        case UserType.AdminAuditor:
                        case UserType.Auditor:
                            var auditorPermissionList = (Session[Constants.SESSION_AUDITOR_ACCESS_PERMISSIONS] != null) ? ((List<ItemList>)Session[Constants.SESSION_AUDITOR_ACCESS_PERMISSIONS]) : _roleService.GetUserPermissionBasedOnUserId((long)user.UserId);
                            Session[Constants.SESSION_AUDITOR_ACCESS_PERMISSIONS] = auditorPermissionList;
                            redirectUrl = "/Admin/Home";
                            break;
                    }
                    Session[Constants.SESSION_USER_TYPE] = user.UserType;
                    Logger.Info("AccountController : Login():" + model.LoginId + " :  Login Response Time:" + DateTime.UtcNow);
                    return Json(new { valid = true, redirectUrl = redirectUrl, IsTemparory = false });
                }
                if (isLocked)
                    message = ReadResource.GetResourceForGlobalization(Constants.ACCOUNT_LOCKED, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
                else
                    message = ReadResource.GetResourceForGlobalization(Constants.INVALID_USER_DETAILS, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
                return Json(new { valid = false, message = message });
            }
            catch (Exception ex)
            {
                Logger.Error("AccountController : Login() : Caught an exception " + ex);
                throw;
            }
        }

        private string GetBuyerRedirectUrl()
        {
            var redirectUrl = "/Buyer/Home";
            try
            {
                var userId = ((UserDetails)(Session[Constants.SESSION_USER])).UserId;
                var buyerPermissionList = new List<ItemList>();
                if (Session[Constants.SESSION_BUYER_ACCESS_PERMISSIONS] != null)
                {
                    buyerPermissionList = (List<ItemList>)Session[Constants.SESSION_BUYER_ACCESS_PERMISSIONS];
                }
                else
                {
                    buyerPermissionList = _roleService.GetUserPermissionBasedOnUserId(userId);
                    Session[Constants.SESSION_BUYER_ACCESS_PERMISSIONS] = buyerPermissionList;
                }
                if ((buyerPermissionList.Find(u => u.Value == (long)BuyerPermissions.DashboardCompliance) != null) || (buyerPermissionList.Find(u => u.Value == (long)BuyerPermissions.DashboardOnboarding) != null))
                {
                    redirectUrl = "/Buyer/Home";
                }
                else if ((buyerPermissionList.Find(u => u.Value == (long)BuyerPermissions.Search) != null))
                {
                    redirectUrl = "/Buyer/SupplierSearch";

                }
                else if ((buyerPermissionList.Find(u => u.Value == (long)BuyerPermissions.KeyQuestions) != null) || (buyerPermissionList.Find(u => u.Value == (long)BuyerPermissions.References) != null) || (buyerPermissionList.Find(u => u.Value == (long)BuyerPermissions.RiskAnalysis) != null))
                {
                    redirectUrl = "/Buyer/Reports";
                }
                else if (buyerPermissionList.Find(u => u.Value == (long)BuyerPermissions.Inbox) != null)
                {
                    redirectUrl = "/Buyer/Inbox";
                }
            }
            catch (Exception ex)
            {
                Logger.Error("AccountController : GetBuyerRedirectUrl() : Caught an exception " + ex);
                throw;
            }
            return redirectUrl;
        }

        private UserDetails PutUserDetailsInSession(long userId)
        {
            try
            {
                var userDetails = new UserDetails();
                if (userId > 0)
                {
                    userDetails = _userServiceFacade.GetUserDetailByUserId(userId);
                    Session[Constants.SESSION_USER] = userDetails;
                    Session[Constants.SESSION_LOGIN_ID] = userDetails.LoginId;
                    Session[Constants.SESSION_ORGANIZATION] = _partyService.GetOrganizationDetail(userDetails.RefOrganisationPartyId);

                    Session[Constants.SESSION_USER_TYPE] = userDetails.UserType;
                    //Session[Constants.SESSION_COMPANY_ID] = user.CompanyId;
                    Session[Constants.SESSION_BUYER_ACCESS_PERMISSIONS] = null;
                    if (userDetails.UserType == (long)UserType.AdminBuyer || userDetails.UserType == (long)UserType.Buyer)
                    {
                        //Session[Constants.BUYER_QUESTION_SETS] = _clientService.GetQuestionSetsForBuyer((long)user.CompanyId);
                        Session[Constants.SESSION_BUYER_ACCESS_PERMISSIONS] = _roleService.GetUserPermissionBasedOnUserId(userId);
                    }
                }
                return userDetails;
            }
            catch (Exception e)
            {
                Logger.Error("AccountController : PutUserDetailsInSession() : Caught an Error " + e);
                throw e;
            }
        }

        [HttpPost]
        public JsonResult AcceptTerms(long userId, long latestTermsOfUseId)
        {
            var redirectUrl = "/Account/Login";
            try
            {
                var result = _userServiceFacade.UpdateAcceptedTermsOfUse(userId, latestTermsOfUseId);
                var user = (Session[Constants.SESSION_USER] != null) ? (UserDetails)Session[Constants.SESSION_USER] : null;
                if (user != null && result)
                {
                    var organization = (OrganizationDetail)Session[Constants.SESSION_ORGANIZATION];
                    switch ((UserType)user.UserType)
                    {
                        case UserType.Supplier:
                        case UserType.AdminSupplier:
                            if (organization.Status >= (int)CompanyStatus.Submitted)
                            {
                                redirectUrl = "/supplier-home";
                            }
                            else
                            {
                                redirectUrl = "/Supplier/Register";
                            }
                            break;
                        case UserType.Buyer:
                        case UserType.AdminBuyer:
                            redirectUrl = "/Buyer/Home";
                            break;
                        case UserType.AdminAuditor:
                        case UserType.Auditor:
                            redirectUrl = "/Admin/Home";
                            break;
                    }
                    Session[Constants.SESSION_USER_TYPE] = user.UserType;
                }
                else
                {
                    redirectUrl = "/Account/Login";
                }
            }
            catch (Exception ex)
            {
                Logger.Error("AcccountController : AcceptTerms() : Caught an exception" + ex);
            }
            return Json(new { redirectUrl = redirectUrl });
        }

        public ActionResult ResetPassword()
        {
            var password = new ChangePassword();
            var user = (UserDetails)Session[Constants.SESSION_USER];
            password.Email = user.LoginId;
            password.Password = user.Password;
            return View(password);
        }

        [HttpPost]
        public JsonResult ChangePassword(ChangePassword passwordVM)
        {
            try
            {
                var success = false;
                var redirectUrl = "/Account/Logout";
                var encryptedPassword = CommonMethods.EncryptMD5Password(passwordVM.Email.ToLower() + passwordVM.NewPassword.Trim());
                var user = (UserDetails)Session[Constants.SESSION_USER];
                success = _userServiceFacade.UpdatePassword(passwordVM.Email.Trim(), encryptedPassword, user.UserId, false);
                
                if (success)
                {
                    var company = (OrganizationDetail)Session[Constants.SESSION_ORGANIZATION];
                    if (!(user.UserType == (long)UserType.AdminAuditor || user.UserType == (long)UserType.Auditor))
                    {
                        if (!_userServiceFacade.HasUserAcceptedLatestTermsOfUse(user.UserId))
                        {
                            TempData["TermsAcceptMessage"] = "New terms of use - Please accept below to continue";
                            redirectUrl = "/terms-of-use";
                            return Json(new { success = true, redirectUrl = redirectUrl });
                        }
                    }
                    switch ((UserType)user.UserType)
                    {
                        case UserType.Supplier:
                        case UserType.AdminSupplier:
                            if (company.Status >= (int)CompanyStatus.Submitted)
                            {
                                redirectUrl = "/supplier-home";
                            }
                            else
                            {
                                redirectUrl = "/Supplier/Register";
                            }
                            break;
                        case UserType.Buyer:
                        case UserType.AdminBuyer:
                            redirectUrl = "/Buyer/Home";
                            break;
                        case UserType.Auditor:
                        case UserType.AdminAuditor:
                            redirectUrl = "/Admin/Home";
                            break;
                    }
                    Session[Constants.SESSION_USER_TYPE] = user.UserType;
                }
                Session[Constants.SESSION_USER] = _userServiceFacade.GetUserDetailByUserId(user.UserId);
                return Json(new { success = success, redirectUrl = redirectUrl });
            }
            catch (Exception ex)
            {
                Logger.Error("AccountController : ChangePassword() : Caught an Error " + ex);
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult OldPasswordValidationForReset(string oldPassword)
        {
            var result = false;
            try
            {
                var user = (UserDetails)Session[Constants.SESSION_USER];
                var encryptedOldPassword = CommonMethods.EncryptMD5Password(user.LoginId.ToLower() + oldPassword.Trim());
                if (encryptedOldPassword == user.Password)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("AccountController : OldPasswordValidationForReset() : Caught an exception" + ex);
            }
            return Json(new { result = result });
        }

        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        public ActionResult TermsOfUse()
        {
            TempData["TermsOfUseId"] = _userServiceFacade.GetLatestTermsOfUseId();
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        public ActionResult FrequentlyAskedQuestions()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            try
            {
                //LogOffUser();
                //DeleteLoginInfo();
                Session.Contents.RemoveAll();
                Session.Abandon();
                Session.Clear();
                Response.ExpiresAbsolute = DateTime.Now;
                Response.Expires = 0;
                Response.CacheControl = "no-cache";
                //var singleSignOn = Request.Cookies["SIMTraxSSO"];
                //if (singleSignOn != null)
                //{
                //    var id = new Guid(FormsAuthentication.Decrypt(singleSignOn.Value).Name);
                //    _userService.DeleteLoginDetailsFromCommonDB(id);
                //}
                //FormsAuthentication.SignOut();
                return RedirectToAction("Login", "Account");
            }
            catch (Exception e)
            {
                Logger.Error("AccountController : Logout() : Caught an Error " + e);
                throw e;
            }
        }

        public JsonResult TriggerNewPassword(string email)
        {
            try
            {
                var IsEmailExists = false;
                var userId = _userServiceFacade.ValidateUserLoginId(email);
                var IsSent = false;
                if (userId > 0)
                {
                    IsEmailExists = true;
                    var randomPassword = CommonMethods.GetUniqueKey(new Random());
                    var encryptedPassword = CommonMethods.Encrypt(email.ToLower() + randomPassword.Trim());

                    var temporaryUrl = new TemporaryPasswordUrl();
                    temporaryUrl = _userServiceFacade.GetTemporaryPasswordUrl(userId);
                    var encodedEncryptedPassword = HttpUtility.UrlEncode(encryptedPassword);
                    var url = "";
                    if (temporaryUrl.TemporaryPasswordUrlId == 0)
                    {
                        temporaryUrl.UserId = userId;
                        temporaryUrl.Token = encryptedPassword;
                        temporaryUrl.CreatedDate = DateTime.UtcNow;
                        temporaryUrl.ModifiedDate = DateTime.UtcNow;
                        if ((Configuration.Environment != null) && (Configuration.Environment.ToUpper() == "PROD"))
                        {
                            url = "https://sim.prgx.com/Account/PasswordReset/?token=" + encodedEncryptedPassword;
                        }
                        else
                        {
                            url = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, "/") + "Account/PasswordReset/?token=" + encodedEncryptedPassword;
                        }
                        temporaryUrl.PasswordURL = url;
                    }
                    else
                    {
                        temporaryUrl.ModifiedDate = DateTime.UtcNow;
                        url = temporaryUrl.PasswordURL;
                    }
                    var IsTemporaryUrlUpdated = _userServiceFacade.AddorUpdateTemporaryUrl(temporaryUrl);
                    if (IsTemporaryUrlUpdated)
                    {
                        //Send the Mail to User
                        IsSent = SendMailToUserForForgotPassword(email, url);
                    }
                }
                return Json(new { EmailExists = IsEmailExists, IsSent = IsSent });

            }
            catch (Exception ex)
            {
                Logger.Error("AccountController : TriggerNewPassword() : Caught an Error " + ex);
                throw ex;
            }
        }

        private bool SendMailToUserForForgotPassword(string email, string url)
        {
            var result = false;
            try
            {
                var localeId = 0;
                var emailMessage = _emailServiceFacade.GetEmailMessage(Constants.FORGOT_PASSWORD, localeId, url);
                if (Constants.IS_EMAIL_ON)
                {
                    // get the application path.
                    string appFilePhysicalPath = HttpContext.Request.PhysicalApplicationPath;
                    string emailSIMLogoUrl = appFilePhysicalPath + @"\Content\Images\logo\siteLogo.png";
                    string emailPRGXLogoUrl = appFilePhysicalPath + @"\Content\Images\logo\simTraxFooterLogoForEmail.png";
                    _emailServiceFacade.SendEmail(email, string.Empty, string.Empty, emailMessage.Subject, emailMessage.Content, emailSIMLogoUrl, emailPRGXLogoUrl, "", "", appFilePhysicalPath);

                    result = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("AccountController : SendMailToUserForForgotPassword() : Caught an Error " + ex);
                throw ex;
            }
            return result;
        }

        public ActionResult PasswordReset(string token)
        {
            try
            {
                var decodedToken = HttpUtility.UrlDecode(token);
                long userId = 0;
                string email = "";
                userId = _userServiceFacade.IsTemporaryUrlValid(token, out email);
                var changepassword = new ResetPassword();
                changepassword.UserId = userId;
                changepassword.Email = email;
                return View(changepassword);
            }
            catch (Exception ex)
            {
                Logger.Error("AccountController : PasswordReset() : Caught an exception" + ex);
                throw;
            }
        }

        public JsonResult ResetPasswordFromUrl(ResetPassword resetpassword)
        {
            try
            {
                var success = false;
                var encryptedPassword = CommonMethods.EncryptMD5Password(resetpassword.Email.ToLower() + resetpassword.Password.Trim());
                success = _userServiceFacade.UpdatePassword(resetpassword.Email, encryptedPassword, resetpassword.UserId, false);
                if (success)
                {
                    var emailMessage = _emailServiceFacade.GetEmailMessage(Constants.RESET_PASSWORD_CONFIRMATION);
                    if (Constants.IS_EMAIL_ON)
                    {
                        // get the application path.
                        string appFilePhysicalPath = HttpContext.Request.PhysicalApplicationPath;
                        string emailSIMLogoUrl = appFilePhysicalPath + @"\Content\Images\logo\siteLogo.png";
                        string emailPRGXLogoUrl = appFilePhysicalPath + @"\Content\Images\logo\simTraxFooterLogoForEmail.png";
                        _emailServiceFacade.SendEmail(resetpassword.Email, string.Empty, string.Empty, emailMessage.Subject, emailMessage.Content, emailSIMLogoUrl, emailPRGXLogoUrl, "", "", appFilePhysicalPath);

                    }
                }
                return Json(new { success = success });
            }
            catch (Exception ex)
            {
                Logger.Error("AccountController : ResetPasswordFromUrl() : Caught an Error " + ex);
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult GetPublicDataRecord(long publicDataId)
        {
            var publicDataRecord = new CampaignPreRegSupplier();
            try
            {
                publicDataRecord = _campaignServiceFacade.GetPublicDataRecord(publicDataId);
            }
            catch (Exception ex)
            {
                Logger.Error("AccountController : GetPublicDataRecord() : Caught an exception " + ex);
                throw;
            }
            return Json(new { publicDataRecord = publicDataRecord });
        }
    }
}