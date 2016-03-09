using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.Domain.Util;
using PRGX.SIMTrax.ServiceFacade;
using PRGX.SIMTrax.ServiceFacade.Abstract;
using PRGX.SIMTrax.ViewModel;
using PRGX.SIMTrax.Web.Models.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PRGX.SIMTrax.Web.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IRoleServiceFacade _roleServiceFacade;
        private readonly IBuyerServiceFacade _buyerServiceFacade;
        private readonly IMasterDataServiceFacade _masterDataService;
        private readonly IUserServiceFacade _userServiceFacade;
        private readonly IEmailServiceFacade _emailServiceFacade;
        private readonly IPartyServiceFacade _partyServiceFacade;
        private readonly ICampaignServiceFacade _campaignServiceFacade;

        public AdminController()
        {
            _roleServiceFacade = new RoleServiceFacade();
            _buyerServiceFacade = new BuyerServiceFacade();
            _masterDataService = new MasterDataServiceFacade();
            _userServiceFacade = new UserServiceFacade();
            _emailServiceFacade = new EmailServiceFacade();
            _partyServiceFacade = new PartyServiceFacade();
            _campaignServiceFacade = new CampaignServiceFacade();
        }

        public ActionResult Home()
        {
            return View();
        }

        public ActionResult BuyerOrganisation()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetBuyerOrganisations(int pageSize, int currentPage, int sortDirection, string sortParameter, string firstName, int status, string fromDate, string toDate, long buyerAccess = 0)
        {
            var buyerList = new List<BuyerOrganization>();
            int total = 0;
            try
            {
                buyerList = _buyerServiceFacade.GetBuyerOrganizations(status, fromDate, toDate, out total, currentPage, buyerAccess, pageSize, sortDirection, sortParameter, firstName);
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : GetBuyerOrganisations() : Caught an exception " + ex);
                throw;
            }
            var data = new { result = buyerList, total = total };
            return Json(data);
        }

        [HttpPost]
        public JsonResult GetBuyerAccessList()
        {
            try
            {
                var buyerParentRole = CommonMethods.Description(RoleType.BuyerAccess);
                var accessList = _roleServiceFacade.GetRoles(buyerParentRole);
                return Json(new { accessList = accessList });
            }
            catch (Exception ex)
            {

                Logger.Error("AdminController : GetBuyerAccessList() : Caught an exception " + ex);
                throw;
            }
        }

        public ActionResult GetBuyerInformationForVerification(long buyerPartyId, string breadCrumb)
        {
            var model = new BuyerRegister();
            var regionCode = Configuration.REGION_IDENTIFIER;
            var masterDataList = _masterDataService.GetMasterDataValuesForBuyerRegistration(regionCode);
            if (masterDataList != null)
            {
                model = _buyerServiceFacade.GetBuyerOrganizationDetailsByPartyId(buyerPartyId);
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
                    Description = ReadResource.GetResourceForGlobalization("MS_POP_UP_DESCRIPTION_" + v.Value, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture())
                }).ToList();
            }
            model.BuyerPartyId = buyerPartyId;
            ViewBag.BreadCrumb = breadCrumb;
            return PartialView("Admin/_VerifyBuyer", model);
        }

        [HttpPost]
        public JsonResult CompleteBuyerVerification(BuyerRegister model)
        {
            var result = false;
            object response = null;
            try
            {
                var loginUser = (UserDetails)Session[Constants.SESSION_USER];
                result = _buyerServiceFacade.VerifyBuyerCompanyDetails(model, loginUser.RefPersonPartyId);
                if (result)
                {
                    response = new { result = result, message = ReadResource.GetResourceForGlobalization(Constants.BUYER_VERIFIED_SUCCESSFULLY, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Domain.Util.Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()) };
                }
                else
                {
                    response = new { result = result, message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()) };
                }

            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : CompleteBuyerVerification() : Caught an exception " + ex);
                throw;
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ActivateBuyer(string buyerPartyId, long roleId)
        {
            object response = null;
            var result = false;
            var message = string.Empty;
            long buyerParty = 0;
            try
            {
                if (roleId > 0)
                {
                    var loginUser = (UserDetails)Session[Constants.SESSION_USER];
                    if (!string.IsNullOrWhiteSpace(buyerPartyId))
                    {
                        buyerParty = Convert.ToInt64(buyerPartyId);
                    }
                    if (buyerParty > 0)
                    {
                        result = _buyerServiceFacade.ActivateBuyer(buyerParty, loginUser.RefPersonPartyId, roleId);
                        if (result)
                        {
                            var randomPassword = CommonMethods.GetUniqueKey(new Random());
                            var user = _userServiceFacade.UpdatePasswordBasedOnOrganizationPartyId(buyerParty, randomPassword, true);
                            SendMailToBuyerForVerification(buyerParty, randomPassword);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : ActivateBuyer() : Caught an error " + ex);
                throw;
            }
            if (result)
                response = new { success = result, message = ReadResource.GetResourceForGlobalization(Constants.BUYER_ACTIVATED_SUCCESSFULLY_SUCCESS_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()) };
            else
                response = new { success = result, message = ReadResource.GetResourceForGlobalization(Constants.ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()) };

            return Json(response);
        }

        private void SendMailToBuyerForVerification(long buyerOrganizationPartyId, string randomPassword)
        {
            long localeId = 0;
            var user = _userServiceFacade.GetUserDetailsByOrganisationPartyId(buyerOrganizationPartyId);
            var emailMessage = _emailServiceFacade.GetEmailMessage(Constants.BUYER_VERIFIED, localeId, user.FirstName, user.LoginId, randomPassword);
            if (Constants.IS_EMAIL_ON)
            {
                try
                {
                    string appFilePhysicalPath = HttpContext.Request.PhysicalApplicationPath;
                    string emailSIMLogoUrl = appFilePhysicalPath + @"\Content\Images\logo\siteLogo.png";
                    string emailPRGXLogoUrl = appFilePhysicalPath + @"\Content\Images\logo\simTraxFooterLogoForEmail.png";
                    _emailServiceFacade.SendEmail(user.LoginId, string.Empty, string.Empty, emailMessage.Subject, emailMessage.Content, emailSIMLogoUrl, emailPRGXLogoUrl, "", "", appFilePhysicalPath);
                }
                catch
                {
                }
            }
        }

        [HttpPost]
        public JsonResult GetBuyersForVerification(int pageNo, string sortParameter, int sortDirection, string buyerName, int pageSize)
        {
            var buyerList = new List<BuyerOrganization>();
            int total = 0;
            try
            {
                buyerList = _buyerServiceFacade.GetNotActivatedBuyerOrganization(pageNo, pageSize, sortDirection, sortParameter, out total);
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : GetBuyersForVerification() : Caught an exception " + ex);
                throw;
            }
            var data = new { result = buyerList, totalRecords = total };
            return Json(data);
        }

        [HttpPost]
        public JsonResult CheckPermission(string permission)
        {
            bool hasPermission = false;
            var enumValue = Enum.Parse(typeof(AuditorRoles), permission);
            try
            {
                if (enumValue != null && Session[Constants.SESSION_USER] != null)
                {
                    var userType = ((UserDetails)(Session[Constants.SESSION_USER])).UserType;
                    if (userType == (Int64)UserType.AdminAuditor)
                    {
                        hasPermission = true;
                    }
                    else if (userType == (Int64)UserType.Auditor)
                    {
                        var permissionId = Convert.ToInt32(enumValue);
                        var userId = ((UserDetails)(Session[Constants.SESSION_USER])).UserId;
                        hasPermission = _roleServiceFacade.CheckAuditorPermission(permissionId, userId);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : CheckPermission() : Caught an error" + ex);
            }
            return Json(new { result = hasPermission });
        }

        public ActionResult DefineAccessType()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAllRoleDetails(int type, int pageSize, int index)
        {
            var roleList = new List<AccessType>();
            var total = 0;
            try
            {
                roleList = _roleServiceFacade.GetAllAccessTypes(type, pageSize, index, out total);
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : GetAllRoleDetails() : Caught an error" + ex);
            }
            return Json(new { roles = roleList, total = total });
        }

        [HttpPost]
        public ActionResult GetCreateRoleForm(int roleId, int existingRoleId)
        {
            var role = new Role();
            try
            {
                var auditorRoles = typeof(AuditorRoles).EnumDropDownList();
                role.UserPermission = auditorRoles.Where(a => a.Value <= (int)AuditorRoles.DeleteAuditor || a.Value == (int)AuditorRoles.CreateUser).Select(p => new RolePermission()
                {
                    PermissionId = (int)p.Value,
                    Description = p.Text
                }).ToList();

                role.QuestionnairePermission = auditorRoles.Where(a => a.Value >= (int)AuditorRoles.CreateQuestionnaireSection && a.Value <= (int)AuditorRoles.PublishQuestionnaireSection).Select(p => new RolePermission()
                {
                    PermissionId = (int)p.Value,
                    Description = p.Text
                }).ToList();

                role.BuyerPermission = auditorRoles.Where(a => a.Value >= (int)AuditorRoles.CreateEditCampaign && a.Value <= (int)AuditorRoles.BuyerSupplierAssignProduct).Select(p => new RolePermission()
                {
                    PermissionId = (int)p.Value,
                    Description = p.Text
                }).ToList();

                role.SupplierPermission = auditorRoles.Where(a => a.Value >= (int)AuditorRoles.AddReferrer && a.Value <= (int)AuditorRoles.PublishSupplier).Select(p => new RolePermission()
                {
                    PermissionId = (int)p.Value,
                    Description = p.Text
                }).ToList();

                role.FinancePermission = auditorRoles.Where(a => a.Value >= (int)AuditorRoles.CreateVoucher && a.Value <= (int)AuditorRoles.AuthoriseTransaction).Select(p => new RolePermission()
                {
                    PermissionId = (int)p.Value,
                    Description = p.Text
                }).ToList();

                role.NavigatePermission = auditorRoles.Where(a => a.Value >= (int)AuditorRoles.Users && a.Value <= (int)AuditorRoles.Finance).Select(p => new RolePermission()
                {
                    PermissionId = (int)p.Value,
                    Description = p.Text
                }).ToList();
            }
            catch (Exception exception)
            {
                Logger.Error("AdminController :GetCreateRoleForm() : Caught an exception " + exception);
                throw;
            }
            return PartialView("Admin/_CreateRole", role);
        }

        public JsonResult ChangeBuyerAccessType(long buyerPartyId, long roleId)
        {
            object response = null;
            var result = false;
            var message = string.Empty;
            try
            {
                if (roleId > 0)
                {
                    var loginUser = (UserDetails)Session[Constants.SESSION_USER];
                    result = _buyerServiceFacade.ChangeAccessType(buyerPartyId, loginUser.RefPersonPartyId, roleId);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : ChangeBuyerAccessType() : Caught an error " + ex);
                throw;
            }
            if (result)
                response = new { success = result, message = ReadResource.GetResourceForGlobalization(Constants.BUYER_ACCESSTYPE_CHANGED_SUCCESSFULLY, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()) };
            else
                response = new { success = result, message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()) };

            return Json(response);
        }

        public ActionResult ManageUsers()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAllUsers(string loginId, string userName, int userType, string status, short source, int currentPage, int pageSize, int sortDirection)
        {
            var users = new List<UserAccount>();
            int total = 0;
            try
            {
                users = _userServiceFacade.GetAllUsers(loginId, userName, userType, status, source, out total, currentPage, pageSize, sortDirection);
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : GetAllUsersAccountStatus() : Caught an exception" + ex);
            }
            return Json(new { usersList = users, total = total });
        }

        [HttpPost]
        public JsonResult ChangeUserPassword(User passwordUpdate)
        {
            object data = null;
            var result = false;
            try
            {
                var encryptedPassword = CommonMethods.EncryptMD5Password(passwordUpdate.LoginId.ToLower() + passwordUpdate.Password.Trim());
                result = _userServiceFacade.UpdatePassword(passwordUpdate.LoginId, encryptedPassword, passwordUpdate.UserId, true);
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : ChangeUserPassword(): Caught an error " + ex);
                throw;
            }
            if (result)
                data = new { result = result, message = ReadResource.GetResourceForGlobalization(Constants.CHANGED_PASSWORD_SUCCESSFULLY, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()) };
            else
                data = new { result = result, message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()) };

            return Json(data);
        }

        public JsonResult DeleteAuditor(string userId)
        {
            var response = false;
            object jsonresult = null;
            string message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
            try
            {
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    _roleServiceFacade.DeleteUserRoleLinks(Convert.ToInt64(userId));
                    response = _userServiceFacade.DeleteUser(Convert.ToInt64(userId));
                    if (response)
                    {
                        message = ReadResource.GetResourceForGlobalization(Constants.USER_DELETED, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
                    }
                    else
                    {
                        message = ReadResource.GetResourceForGlobalization(Constants.CAMPAIGNS_ASSIGNED_TO_USER, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
                    }
                    jsonresult = new { success = response, message = message };
                }
            }
            catch (Exception exception)
            {
                Logger.Error("AdminController : DeleteAuditor(string userId): Caught an error " + exception);
                throw exception;
            }
            return Json(jsonresult);
        }

        [HttpPost]
        public JsonResult EditUserProfile(string userId)
        {
            var users = new User();
            try
            {
                if (!string.IsNullOrWhiteSpace(userId))
                    users = _userServiceFacade.GetUserViewModelByUserId(Convert.ToInt64(userId));
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : EditUserProfile(): Caught an error " + ex);
                throw;
            }
            var result = new { users = users, message = ReadResource.GetResourceForGlobalization(Constants.ERROR_TO_LOAD, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()) };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateUserProfile(User userDetails)
        {
            object data = null;
            var result = false;
            try
            {
                var auditorId = ((UserDetails)Session[Constants.SESSION_USER]).UserId;
                result = _userServiceFacade.UpdateUserProfile(userDetails, auditorId);
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : UpdateUserProfile(): Caught an error " + ex);
                throw;
            }
            if (result)
                data = new { result = result, message = ReadResource.GetResourceForGlobalization(Constants.SAVED_SUCCESSFULLY, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()) };
            else
                data = new { result = result, message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()) };
            return Json(data);
        }

        public ActionResult GetCreateAuditorTemplate(string userId)
        {
            var auditor = new AuditorUser();
            try
            {
                //TODO For Edit Tamplate
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    var user = _userServiceFacade.GetUserViewModelByUserId(Convert.ToInt64(userId));
                    var selectedRoles = string.Join(",", _roleServiceFacade.GetRolesForUser(Convert.ToInt64(userId)));
                    auditor.Roles = _roleServiceFacade.GetRoles(CommonMethods.Description(RoleType.AuditorRole));
                    auditor.Id = user.UserId;
                    auditor.Email = user.LoginId;
                    auditor.FirstName = user.FirstName;
                    auditor.LastName = user.LastName;
                    auditor.Password = user.Password;
                    auditor.UserType = (UserType)user.UserType;
                    auditor.IsActive = user.IsActive;
                    auditor.SelectedRoles = selectedRoles;
                    auditor.ConfirmPassword = user.Password;
                    auditor.OrganizationPartyId = user.OrganizationPartyId;
                }
                //TODO :: for create
                else
                {
                    var role = _roleServiceFacade.GetRoles(CommonMethods.Description(RoleType.AuditorRole));
                    if (role != null)
                    {
                        auditor.Roles = role;
                        auditor.UserType = UserType.Auditor;
                        auditor.OrganizationPartyId = ((UserDetails)Session[Constants.SESSION_USER]).RefOrganisationPartyId;
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Error("AdminController : GetCreateAuditorTemplate() : Caught an exception " + exception);
                throw;
            }
            return PartialView("Admin/_CreateAuditor", auditor);
        }

        public ActionResult AddOrUpdateAuditorUser(AuditorUser auditorUserModel)
        {
            #region Variable Declaration
            object jsonresult = null;
            string message = Constants.DEFAULT_ERROR_MESSAGE;
            var result = false;
            #endregion
            try
            {
                if (ModelState.IsValid)
                {
                    var selectedRoles = new List<long>();
                    if (!string.IsNullOrEmpty(auditorUserModel.SelectedRoles))
                    {
                        var selectedRolesArr = auditorUserModel.SelectedRoles.Split(',');
                        foreach (var role in selectedRolesArr)
                        {
                            if (!string.IsNullOrWhiteSpace(role))
                                selectedRoles.Add(Convert.ToInt64(role));
                        }
                    }
                    //TODO :: For Edit
                    var loggedInUserId = ((UserDetails)(Session[Constants.SESSION_USER])).UserId;
                    if (auditorUserModel.Id > 0)
                    {
                        auditorUserModel.Id = _userServiceFacade.UpdateUser(auditorUserModel.Mapping(), loggedInUserId);
                        result = _roleServiceFacade.AddOrUpdateUserRoleLinks(auditorUserModel.Id, selectedRoles);
                    }
                    //TODO :: For Create
                    else
                    {
                        auditorUserModel.Id = _userServiceFacade.AddNewUser(auditorUserModel.Mapping(), loggedInUserId);
                        result = _roleServiceFacade.AddOrUpdateUserRoleLinks(auditorUserModel.Id, selectedRoles);
                    }
                    //TODO :: Prepare a response
                    if (result)
                    {
                        message = auditorUserModel.Id > 0 ? ReadResource.GetResourceForGlobalization(Constants.UPDATE_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()) : ReadResource.GetResourceForGlobalization(Constants.AUDITOR_CREATED_SUCCESS, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
                    }
                }
                else
                {
                    message = ReadResource.GetResourceForGlobalization(Constants.VALIDATION_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
                }

                jsonresult = new { success = result, message = message };
            }
            catch (Exception exception)
            {
                Logger.Error("AdminController : AddOrUpdateAuditorUser : Caught an exception " + exception);
                throw;
            }
            return Json(jsonresult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAuditorDetails(long userId)
        {
            #region Variables Declaration
            AuditorUser auditorModel = new AuditorUser();
            object jsonResult = null;
            #endregion
            try
            {
                var user = _userServiceFacade.GetUserViewModelByUserId(Convert.ToInt64(userId));
                var selectedRoles = string.Join(",", _roleServiceFacade.GetRolesForUser(Convert.ToInt64(userId)));
                auditorModel.Roles = _roleServiceFacade.GetRoles(CommonMethods.Description(RoleType.AuditorRole));
                auditorModel.Id = user.UserId;
                auditorModel.Email = user.LoginId;
                auditorModel.FirstName = user.FirstName;
                auditorModel.LastName = user.LastName;
                auditorModel.Password = user.Password;
                auditorModel.UserType = (UserType)user.UserType;
                auditorModel.IsActive = user.IsActive;
                auditorModel.SelectedRoles = selectedRoles;
                auditorModel.ConfirmPassword = user.Password;
                auditorModel.OrganizationPartyId = user.OrganizationPartyId;
                jsonResult = new { auditorModel = auditorModel };
            }
            catch (Exception exception)
            {
                Logger.Error("AdminController : GetUser(long userId) : Caught an exception " + exception);
                throw;
            }
            return Json(jsonResult);
        }

        [HttpPost]
        public ActionResult UpdateAuditorRoles(string userId, string selectedRoles)
        {
            #region Variables Declaration
            object jsonResult = null;
            string message = Constants.DEFAULT_ERROR_MESSAGE;
            var result = false;
            #endregion
            try
            {
                if (!string.IsNullOrWhiteSpace(userId) && !string.IsNullOrWhiteSpace(selectedRoles))
                {
                    var selectedRoleIds = new List<long>();
                    var selectedRolesArr = selectedRoles.Split(',');
                    foreach (var role in selectedRolesArr)
                    {
                        if (!string.IsNullOrWhiteSpace(role))
                            selectedRoleIds.Add(Convert.ToInt64(role));
                    }
                    result = _roleServiceFacade.AddOrUpdateUserRoleLinks(Convert.ToInt64(userId), selectedRoleIds);

                    //TODO :: Prepare response
                    if (result)
                    {
                        message = ReadResource.GetResourceForGlobalization(Constants.UPDATE_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
                    }
                }
                else
                {
                    message = ReadResource.GetResourceForGlobalization(Constants.VALIDATION_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
                }
                jsonResult = new { success = result, message = message };
            }
            catch (Exception exception)
            {
                Logger.Error("AdminController : UpdateAuditorRoles() : Caught an exception " + exception);
                throw exception;
            }
            return Json(jsonResult);
        }


        public FileResult ManageUsersDownload(string loginId, string userName, int userType, string status, short source)
        {
            try
            {
                int total = 0;
                var userList = _userServiceFacade.GetAllUsers(loginId, userName, userType, status, source, out total, 1, int.MaxValue, 1);
                string filterValue = String.Empty;
                var filtersList = new List<ExcelDownloadFilterList>();
                if ((ProjectSource)source != ProjectSource.Both)
                {
                    filterValue = ((ProjectSource)source).Description();
                }
                else
                {
                    foreach (var value in Enum.GetValues(typeof(ProjectSource)).Cast<ProjectSource>().Where(v => !v.Equals((ProjectSource)ProjectSource.Both)).Select(v => v.Description()).ToList())
                    {
                        filterValue = filterValue + value + ", ";
                    }
                    filterValue = filterValue.Substring(0, filterValue.LastIndexOf(", "));
                }
                if (!String.IsNullOrEmpty(filterValue))
                {
                    filtersList.Add(new ExcelDownloadFilterList()
                    {
                        FilterType = "Source",
                        FilterValue = filterValue
                    });
                }
                string usertype = String.Empty;
                if ((UserType)userType != UserType.None)
                {
                    usertype = ((UserType)userType).Description();
                }

                if (!String.IsNullOrEmpty(usertype))
                {
                    filtersList.Add(new ExcelDownloadFilterList()
                    {
                        FilterType = "User Type",
                        FilterValue = usertype
                    });
                }
                if (!String.IsNullOrEmpty(userName))
                {
                    filtersList.Add(new ExcelDownloadFilterList()
                    {
                        FilterType = "User Name",
                        FilterValue = userName
                    });
                }
                if (!String.IsNullOrEmpty(loginId))
                {
                    filtersList.Add(new ExcelDownloadFilterList()
                    {
                        FilterType = "Login Id",
                        FilterValue = loginId
                    });
                }
                if (!String.IsNullOrEmpty(status))
                {
                    filtersList.Add(new ExcelDownloadFilterList()
                    {
                        FilterType = "Status",
                        FilterValue = status
                    });
                }
                string include = "FirstName,LastName,LoginId,ProjectSource,UserType,OriginalTermsVersion,TermsAcceptedDate,LatestTermsVersion,LastLoginDate,ActiveStatus";
                string exclude = "";
                var file = CommonMethods.CreateDownloadExcel(userList, include, exclude, "Report", "Manage Users Reports", filtersList);
                return File(file.GetBuffer(), "application/vnd.ms-excel", "ManageUsersReports.xls");
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : ManageUsersDownload() : Caught an error" + ex);
                throw;
            }
        }

        public JsonResult GetUserEmail(string text)
        {
            List<string> response = null;
            try
            {
                response = _userServiceFacade.GetUserEmail(text);

            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : GetUserEmail() : Caught an exception " + ex);
                throw;
            }
            return Json(response);
        }

        public JsonResult GetUserName(string text)
        {
            List<string> response = null;
            try
            {
                response = _userServiceFacade.GetUserName(text);

            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : GetUserName() : Caught an exception " + ex);
                throw;
            }
            return Json(response);
        }

        public FileResult BuyerOrganizationExport(string buyerName, string fromDate, string toDate, int status, long access)
        {
            try
            {
                var total = 0;
                var buyerOrgList = _buyerServiceFacade.GetBuyerOrganizations(status, fromDate, toDate, out total, 1, access, int.MaxValue, 1, string.Empty, buyerName);
                string filterValue = String.Empty;
                var filtersList = new List<ExcelDownloadFilterList>();
                if ((BuyerOrganisationStatus)status != BuyerOrganisationStatus.All)
                {
                    filterValue = ((CompanyStatus)status).Description();
                }
                else
                {
                    foreach (var value in Enum.GetValues(typeof(BuyerOrganisationStatus)).Cast<BuyerOrganisationStatus>().Where(v => !v.Equals((BuyerOrganisationStatus)BuyerOrganisationStatus.All)).Select(v => v.Description()).ToList())
                    {
                        filterValue = filterValue + value + ", ";
                    }
                    filterValue = filterValue.Substring(0, filterValue.LastIndexOf(", "));
                }
                if (!String.IsNullOrEmpty(filterValue))
                {
                    filtersList.Add(new ExcelDownloadFilterList()
                    {
                        FilterType = "Status",
                        FilterValue = filterValue
                    });
                }
                if (access != null)
                {
                    if (access == 1)
                        filterValue = "Basic View";
                    if (access == 2)
                        filterValue = "Standard View";
                    if (access == 3)
                        filterValue = "Premium View";
                    if (access == 0)
                        filterValue = "All";
                    filtersList.Add(new ExcelDownloadFilterList()
                    {
                        FilterType = "Access",
                        FilterValue = filterValue
                    });
                }
                if (!String.IsNullOrEmpty(buyerName))
                {
                    filtersList.Add(new ExcelDownloadFilterList()
                    {
                        FilterType = "Buyer Organization",
                        FilterValue = buyerName
                    });
                }
                if (!String.IsNullOrEmpty(fromDate))
                {
                    filtersList.Add(new ExcelDownloadFilterList()
                    {
                        FilterType = "From Date",
                        FilterValue = fromDate
                    });
                }
                if (!String.IsNullOrEmpty(toDate))
                {
                    filtersList.Add(new ExcelDownloadFilterList()
                    {
                        FilterType = "To Date",
                        FilterValue = toDate
                    });
                }
                string include = "BuyerOrganizationName,CreatedDate,VerifiedDate,ActivatedDate,BuyerRoleName,TermsAcceptedDate,BuyerStatusString";
                var file = CommonMethods.CreateDownloadExcel(buyerOrgList, include, "", "Report", "Buyer Organisation Reports", filtersList);
                return File(file.GetBuffer(), "application/vnd.ms-excel", "BuyerOrganisationReports.xls");
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : BuyerOrgsDownload() : Caught an error" + ex);
                throw;
            }
        }

        public ActionResult GetBuyerDashboard(long buyerPartyId)
        {
            ViewBag.BuyerPartyId = buyerPartyId;
            if (this.Request.IsAjaxRequest())
            {
                return PartialView("Admin/_BuyerDashboard");
            }
            else
            {
                return View("Admin/_BuyerDashboard");
            }
        }

        public JsonResult GetBuyerDetailsForDashboard(long partyId)
        {
            var buyerDetails = new BuyerOrganization();
            try
            {
                buyerDetails = _buyerServiceFacade.GetBuyerDetailsForDashboard(partyId);
            }
            catch (Exception ex)
            {
                Logger.Info("AdminController : GetBuyerDetailsForDashboard() : Caught an exception " + ex);
                throw;
            }
            var data = new { result = buyerDetails };
            return Json(data);
        }

        public JsonResult GetBuyerUserDetailsForDashboard(long buyerPartyId, int pageNumber, int sortDirection)
        {
            var userDetails = new List<UserAccount>();
            var totalUsers = 0;
            try
            {
                userDetails = _userServiceFacade.GetUserDetailsForDashboard(buyerPartyId, out totalUsers, pageNumber, sortDirection);
            }
            catch (Exception ex)
            {
                Logger.Info("AdminController : GetBuyerUserDetailsForDashboard() : Caught an exception " + ex);
                throw;
            }
            var result = new { userDetails = userDetails, totalUsers = totalUsers };
            return Json(result);
        }

        public ActionResult CreateUserForBuyerCompany(long buyerPartyId, string BreadCrumb, long UserId)
        {
            ViewBag.BuyerPartyId = buyerPartyId;
            ViewBag.BreadCrumb = BreadCrumb;
            ViewBag.Objective = "Create";
            var model = new User();
            model.OrganizationPartyId = buyerPartyId;
            model.UserType = UserType.Buyer;
            if (UserId != 0)
            {
                model = _userServiceFacade.GetUserViewModelByUserId(UserId);
                ViewBag.Objective = "Edit";
            }
            return PartialView("Admin/_CreateUser", model);
        }

        public ActionResult CreateUserForSupplierCompany(long supplierPartyId, string BreadCrumb, long UserId)
        {
            ViewBag.SupplierPartyId = supplierPartyId;
            ViewBag.BreadCrumb = BreadCrumb;
            ViewBag.Objective = "Create";
            var model = new User();
            model.OrganizationPartyId = supplierPartyId;
            model.UserType = UserType.Supplier;
            if (UserId != 0)
            {
                model = _userServiceFacade.GetUserViewModelByUserId(UserId);
                ViewBag.Objective = "Edit";
            }
            return PartialView("Admin/_CreateUser", model);
        }

        [HttpPost]
        public JsonResult AddOrEditUserForParty(User model)
        {
            bool success = false;
            object jsonResult = null;
            var message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
            bool isUpdate = false;
            try
            {
                UserDetails user = (UserDetails)Session[Constants.SESSION_USER];
                if (model.UserId > 0)
                {
                    ModelState.Remove("Password");
                    ModelState.Remove("ConfirmPassword");
                }
                if (user != null)
                {
                    if (model.UserType == UserType.Buyer && model.IsAdminUser)
                    {
                        model.UserType = UserType.AdminBuyer;
                    }
                    else if (model.UserType == UserType.Supplier && model.IsAdminUser)
                    {
                        model.UserType = UserType.AdminSupplier;
                    }
                    else if (model.UserType == UserType.AdminBuyer && !model.IsAdminUser)
                    {
                        model.UserType = UserType.Buyer;
                    }
                    else if (model.UserType == UserType.AdminSupplier && !model.IsAdminUser)
                    {
                        model.UserType = UserType.Supplier;
                    }
                    if (model.UserId > 0)
                    {
                        success = _userServiceFacade.UpdateUser(model, user.UserId) > 0 ? true : false;
                        message = ReadResource.GetResourceForGlobalization(Constants.USER_UPDATED_SUCCESSFULLY, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
                        isUpdate = true;
                    }
                    else
                    {
                        success = _userServiceFacade.AddNewUser(model, user.UserId) > 0 ? true : false;
                        message = ReadResource.GetResourceForGlobalization(Constants.USER_ADDED_SUCCESSFULLY, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
                    }
                    jsonResult = new { success = success, message = message, isUpdate = isUpdate };
                }
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : AddOrEditUserForParty() : Caught an exception " + ex);
                throw;
            }
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SupplierOrganisations()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetSupplierOrganisation(int status, string fromDate, string toDate, string index, string supplierName, short source, string supplierId, string referrerName, int count, int sortDirection)
        {
            int currentPage = Convert.ToInt32(index);
            int total = 0;
            long id = 0;
            bool valid = long.TryParse(supplierId, out id);
            try
            {
                var supplierList = _partyServiceFacade.GetSupplierOrganization(fromDate, toDate, out total, currentPage, source, count, sortDirection, id, supplierName, status, referrerName);
                return Json(new { result = supplierList, total = total });
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : GetSupplierOrganisation() : Caught an exception" + ex);
                throw;
            }
        }

        public FileResult SupplierOrganizationExport(int status, string fromDate, string toDate, string supplierName, string supplierId, string referrerName, short source)
        {
            var companyStatus = (CompanyStatus)status;
            int total = 0;
            long id = 0;
            bool valid = long.TryParse(supplierId, out id);
            try
            {

                var supplierList = _partyServiceFacade.GetSupplierOrganization(fromDate, toDate, out total, 1, source, int.MaxValue, 1, id, supplierName, status, referrerName);
                string filterValue = String.Empty;
                var filtersList = new List<ExcelDownloadFilterList>();
                if ((CompanyStatus)status != CompanyStatus.Started)
                {
                    filterValue = ((CompanyStatus)status).Description();
                }
                else
                {
                    foreach (var value in Enum.GetValues(typeof(CompanyStatus)).Cast<CompanyStatus>().Where(v => !v.Equals((CompanyStatus)CompanyStatus.Started)).Select(v => v.Description()).ToList())
                    {
                        filterValue = filterValue + value + ", ";
                    }
                    filterValue = filterValue.Substring(0, filterValue.LastIndexOf(", "));
                }
                if (!String.IsNullOrEmpty(filterValue))
                {
                    filtersList.Add(new ExcelDownloadFilterList()
                    {
                        FilterType = "Status",
                        FilterValue = filterValue
                    });
                }
                string filtervalues = String.Empty;
                if ((ProjectSource)source != ProjectSource.Both)
                {
                    filtervalues = ((ProjectSource)source).Description();
                }
                else
                {
                    foreach (var value in Enum.GetValues(typeof(ProjectSource)).Cast<ProjectSource>().Where(v => !v.Equals((ProjectSource)ProjectSource.Both)).Select(v => v.Description()).ToList())
                    {
                        filtervalues = filtervalues + value + ", ";
                    }
                    filtervalues = filtervalues.Substring(0, filtervalues.LastIndexOf(", "));
                }
                if (!String.IsNullOrEmpty(filtervalues))
                {
                    filtersList.Add(new ExcelDownloadFilterList()
                    {
                        FilterType = "Source",
                        FilterValue = filtervalues
                    });
                }
                if (!String.IsNullOrEmpty(supplierId))
                {
                    filtersList.Add(new ExcelDownloadFilterList()
                    {
                        FilterType = "Supplier ID",
                        FilterValue = supplierId
                    });
                }
                if (!String.IsNullOrEmpty(fromDate))
                {
                    filtersList.Add(new ExcelDownloadFilterList()
                    {
                        FilterType = "From Date",
                        FilterValue = fromDate
                    });
                }
                if (!String.IsNullOrEmpty(toDate))
                {
                    filtersList.Add(new ExcelDownloadFilterList()
                    {
                        FilterType = "To Date",
                        FilterValue = toDate
                    });
                }
                if (!String.IsNullOrEmpty(supplierName))
                {
                    filtersList.Add(new ExcelDownloadFilterList()
                    {
                        FilterType = "Company Name",
                        FilterValue = supplierName
                    });
                }
                if (!String.IsNullOrEmpty(referrerName))
                {
                    filtersList.Add(new ExcelDownloadFilterList()
                    {
                        FilterType = "Referrer Name",
                        FilterValue = referrerName
                    });
                }
                foreach (var item in supplierList)
                {
                    item.OtherReferrer = string.Join("; ", item.SupplierReferrers.Where(u => u.LandingReferrer != true).Select(u => u.BuyerOrganizationName).ToList());
                    //foreach (var otherReferrerName in refferes)
                    //{
                    //    item.OtherReferres += otherReferrerName + "; ";
                    //}

                    item.LandingPageReferrer = item.SupplierReferrers.FirstOrDefault(u => u.LandingReferrer == true) != null ? 
                        item.SupplierReferrers.FirstOrDefault(u => u.LandingReferrer == true).BuyerOrganizationName : string.Empty;

                }
                string include = "SupplierId,SupplierOrganizationName,SignUpDate,RegisteredDate,DetailsVerifiedDate,ProfileVerifiedDate,SupplierStatusString,ProjectSource,LandingPageReferrer,OtherReferrer";
                var file = CommonMethods.CreateDownloadExcel(supplierList, include, "", "Supplier Organisations", "Supplier Organisation Reports", filtersList);
                return File(file.GetBuffer(), "application/vnd.ms-excel", "SupplierOrganisationsReport.xls");
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : SupplierOrganizationExport() : Caught an error" + ex);
                throw;
            }
        }

        public ActionResult GetSupplierDashboard(long supplierPartyId)
        {
            ViewBag.SupplierPartyId = supplierPartyId;
            if (this.Request.IsAjaxRequest())
            {
                return PartialView("Admin/_SupplierDashboard");
            }
            else
            {
                return View("Admin/_SupplierDashboard");
            }
        }

        public JsonResult GetSupplierDetailsForDashboard(long partyId)
        {
            var supplierDetails = new SupplierOrganization();
            try
            {
                supplierDetails = _partyServiceFacade.GetSupplierDetailsForDashboard(partyId);
            }
            catch (Exception ex)
            {
                Logger.Info("AdminController : GetBuyerDetailsForDashboard() : Caught an exception " + ex);
                throw;
            }
            var data = new { result = supplierDetails };
            return Json(data);
        }

        public JsonResult GetSupplierUserDetailsForDashboard(long supplierPartyId, int pageNumber, int sortDirection)
        {
            var userDetails = new List<UserAccount>();
            var totalUsers = 0;
            try
            {
                userDetails = _userServiceFacade.GetUserDetailsForDashboard(supplierPartyId, out totalUsers, pageNumber, sortDirection);
            }
            catch (Exception ex)
            {
                Logger.Info("AdminController : GetBuyerUserDetailsForDashboard() : Caught an exception " + ex);
                throw;
            }
            var result = new { userDetails = userDetails, totalUsers = totalUsers };
            return Json(result);
        }

        public JsonResult AddorUpdateBuyerRole(long roleId, string roleName, string roleDescription, long[] permissions)
        {
            try
            {
                var result = false;
                List<long> permissionList = new List<long>();
                if (permissions != null)
                {
                    permissionList = permissions.Cast<long>().ToList();
                    var userId = ((UserDetails)Session[Constants.SESSION_USER]).UserId;
                    // existing role id for buyer role = 4
                    result = _roleServiceFacade.AddorUpdateRole(roleId, roleName, roleDescription, permissionList, userId, 4);
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : AddorUpdateBuyerRole() : Caught an error" + ex);
                throw;
            }
        }

        public JsonResult AddorUpdateAuditorRole(Role auditorRole)
        {
            var message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
            object jsonResult = null;
            bool result = false;
            try
            {
                if (ModelState.IsValid)
                {
                    var permissions = new List<long>();
                    permissions.AddRange(auditorRole.UserPermission.Where(elem => elem.IsChecked == true).Select(x => x.PermissionId).ToList());
                    permissions.AddRange(auditorRole.QuestionnairePermission.Where(elem => elem.IsChecked == true).Select(x => x.PermissionId).ToList());
                    permissions.AddRange(auditorRole.BuyerPermission.Where(elem => elem.IsChecked == true).Select(x => x.PermissionId).ToList());
                    permissions.AddRange(auditorRole.SupplierPermission.Where(elem => elem.IsChecked == true).Select(x => x.PermissionId).ToList());
                    permissions.AddRange(auditorRole.FinancePermission.Where(elem => elem.IsChecked == true).Select(x => x.PermissionId).ToList());
                    permissions.AddRange(auditorRole.NavigatePermission.Where(elem => elem.IsChecked == true).Select(x => x.PermissionId).ToList());

                    var userId = ((UserDetails)Session[Constants.SESSION_USER]).UserId;
                    // existing role id for auditor role= 5
                    result = _roleServiceFacade.AddorUpdateRole(auditorRole.Id, auditorRole.Name, auditorRole.Description, permissions, userId, 5);
                    if (auditorRole.Id > 0 && result)
                    {
                        message = ReadResource.GetResourceForGlobalization(Constants.UPDATE_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
                    }
                    else if (auditorRole.Id == 0 && result)
                    {
                        message = ReadResource.GetResourceForGlobalization(Constants.ROLE_CREATED, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
                    }

                    //TODO :: Response to view
                    jsonResult = new { success = result, message = message };
                }
                else
                {
                    message = ReadResource.GetResourceForGlobalization(Constants.VALIDATION_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
                    jsonResult = new { success = result, message = message };
                }
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : AddorUpdateAuditorRole() : Caught an error" + ex);
                throw;
            }
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBuyerRoleBasedOnRoleId(long roleId)
        {
            try
            {
                var buyerRole = new Role();
                if (roleId > 0)
                {
                    buyerRole = _roleServiceFacade.GetRoleDetails(roleId);
                }
                return Json(buyerRole);
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : GetBuyerRoleBasedOnRoleId() : Caught an error" + ex);
                throw;
            }
        }

        public ActionResult GetAuditorRoleBasedOnRoleId(int roleId)
        {
            Role role = new Role();
            try
            {
                role = _roleServiceFacade.GetRoleDetails(roleId);

                var auditorRoles = typeof(AuditorRoles).EnumDropDownList();
                role.UserPermission = auditorRoles.Where(a => a.Value <= (int)AuditorRoles.DeleteAuditor || a.Value == (int)AuditorRoles.CreateUser).Select(p => new RolePermission()
                {
                    PermissionId = (int)p.Value,
                    Description = p.Text,
                    IsChecked = role.RolePermissions.Any(elem => elem.PermissionId == (int)p.Value) ? role.RolePermissions.FirstOrDefault(elem => elem.PermissionId == (int)p.Value).IsChecked : false,
                    RoleId = role.RolePermissions.Any(elem => elem.PermissionId == (int)p.Value) ? role.Id : 0,
                    Id = role.RolePermissions.Any(elem => elem.PermissionId == (int)p.Value) ? role.RolePermissions.FirstOrDefault(elem => elem.PermissionId == (int)p.Value).Id : 0
                }).ToList();

                role.QuestionnairePermission = auditorRoles.Where(a => a.Value >= (int)AuditorRoles.CreateQuestionnaireSection && a.Value <= (int)AuditorRoles.PublishQuestionnaireSection).Select(p => new RolePermission()
                {
                    PermissionId = (int)p.Value,
                    Description = p.Text,
                    IsChecked = role.RolePermissions.Any(elem => elem.PermissionId == (int)p.Value) ? role.RolePermissions.FirstOrDefault(elem => elem.PermissionId == (int)p.Value).IsChecked : false,
                    RoleId = role.RolePermissions.Any(elem => elem.PermissionId == (int)p.Value) ? role.Id : 0,
                    Id = role.RolePermissions.Any(elem => elem.PermissionId == (int)p.Value) ? role.RolePermissions.FirstOrDefault(elem => elem.PermissionId == (int)p.Value).Id : 0
                }).ToList();

                role.BuyerPermission = auditorRoles.Where(a => a.Value >= (int)AuditorRoles.CreateEditCampaign && a.Value <= (int)AuditorRoles.BuyerSupplierAssignProduct).Select(p => new RolePermission()
                {
                    PermissionId = (int)p.Value,
                    Description = p.Text,
                    IsChecked = role.RolePermissions.Any(elem => elem.PermissionId == (int)p.Value) ? role.RolePermissions.FirstOrDefault(elem => elem.PermissionId == (int)p.Value).IsChecked : false,
                    RoleId = role.RolePermissions.Any(elem => elem.PermissionId == (int)p.Value) ? role.Id : 0,
                    Id = role.RolePermissions.Any(elem => elem.PermissionId == (int)p.Value) ? role.RolePermissions.FirstOrDefault(elem => elem.PermissionId == (int)p.Value).Id : 0
                }).ToList();

                role.SupplierPermission = auditorRoles.Where(a => a.Value >= (int)AuditorRoles.AddReferrer && a.Value <= (int)AuditorRoles.PublishSupplier).Select(p => new RolePermission()
                {
                    PermissionId = (int)p.Value,
                    Description = p.Text,
                    IsChecked = role.RolePermissions.Any(elem => elem.PermissionId == (int)p.Value) ? role.RolePermissions.FirstOrDefault(elem => elem.PermissionId == (int)p.Value).IsChecked : false,
                    RoleId = role.RolePermissions.Any(elem => elem.PermissionId == (int)p.Value) ? role.Id : 0,
                    Id = role.RolePermissions.Any(elem => elem.PermissionId == (int)p.Value) ? role.RolePermissions.FirstOrDefault(elem => elem.PermissionId == (int)p.Value).Id : 0
                }).ToList();

                role.FinancePermission = auditorRoles.Where(a => a.Value >= (int)AuditorRoles.CreateVoucher && a.Value <= (int)AuditorRoles.AuthoriseTransaction).Select(p => new RolePermission()
                {
                    PermissionId = (int)p.Value,
                    Description = p.Text,
                    IsChecked = role.RolePermissions.Any(elem => elem.PermissionId == (int)p.Value) ? role.RolePermissions.FirstOrDefault(elem => elem.PermissionId == (int)p.Value).IsChecked : false,
                    RoleId = role.RolePermissions.Any(elem => elem.PermissionId == (int)p.Value) ? role.Id : 0,
                    Id = role.RolePermissions.Any(elem => elem.PermissionId == (int)p.Value) ? role.RolePermissions.FirstOrDefault(elem => elem.PermissionId == (int)p.Value).Id : 0
                }).ToList();

                role.NavigatePermission = auditorRoles.Where(a => a.Value >= (int)AuditorRoles.Users && a.Value <= (int)AuditorRoles.Finance).Select(p => new RolePermission()
                {
                    PermissionId = (int)p.Value,
                    Description = p.Text,
                    IsChecked = role.RolePermissions.Any(elem => elem.PermissionId == (int)p.Value) ? role.RolePermissions.FirstOrDefault(elem => elem.PermissionId == (int)p.Value).IsChecked : false,
                    RoleId = role.RolePermissions.Any(elem => elem.PermissionId == (int)p.Value) ? role.Id : 0,
                    Id = role.RolePermissions.Any(elem => elem.PermissionId == (int)p.Value) ? role.RolePermissions.FirstOrDefault(elem => elem.PermissionId == (int)p.Value).Id : 0
                }).ToList();
            }
            catch (Exception exception)
            {
                Logger.Error(" AdminController :  GetAuditorRoleBasedOnRoleId : Caught an error" + exception);
                throw;
            }
            return PartialView("Admin/_CreateRole", role);
        }

        public JsonResult DeleteRole(int roleId)
        {
            string message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
            object response = null;
            try
            {
                var result = _roleServiceFacade.DeleteRole(roleId);
                if (result)
                {
                    response = new { success = result, message = ReadResource.GetResourceForGlobalization(Constants.ROLE_DELETED, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()) };
                }
            }
            catch (Exception exception)
            {
                Logger.Error("AdminController : DeleteRole(int roleId) : Caught an exception " + exception);
                throw;
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateOrUpdateVoucherForBuyerCompany(long? buyerPartyId, string BreadCrumb, string voucherCode)
        {
            var model = new Voucher();

            if (voucherCode != null)
            {
                model = _buyerServiceFacade.GetDiscountVoucherById(voucherCode);
                if (model.PromotionStartDate.HasValue)
                    model.PromotionStartDate = model.PromotionStartDate.Value.Date;
                if (model.PromotionEndDate.HasValue)
                    model.PromotionEndDate = model.PromotionEndDate.Value.Date;
            }
            else
            {
                model.BuyerPartyId = (buyerPartyId != null) ? buyerPartyId : 0;
            }
            var list = _buyerServiceFacade.GetBuyersList();
            model.BuyerList = new SelectList(list.AsEnumerable(), "Value", "Text", model.BuyerPartyId.HasValue ? model.BuyerPartyId.Value : 0).ToList();
            model.BuyerList.Insert(0, new SelectListItem { Text = "--- Select  ---", Value = "0" });

            model.MapBuyer = true;
            ViewBag.BreadCrumb = BreadCrumb;
            return PartialView("Admin/_CreateVoucher", model);
        }

        public JsonResult AddorUpdateVoucher(long VoucherId, string PromotionalCode, decimal DiscountPercent, string PromotionalStartDate, string PromotionalEndDate, long BuyerPartyId)
        {
            try
            {
                var result = false;
                var voucher = new Voucher();
                voucher.VoucherId = VoucherId;
                voucher.PromotionalCode = PromotionalCode;
                voucher.DiscountPercent = DiscountPercent;
                voucher.PromotionStartDate = DateTime.ParseExact(PromotionalStartDate, "ddMMyyyy", CultureInfo.InvariantCulture);
                voucher.PromotionEndDate = DateTime.ParseExact(PromotionalEndDate, "ddMMyyyy", CultureInfo.InvariantCulture);
                if (BuyerPartyId > 0)
                {
                    voucher.BuyerPartyId = BuyerPartyId;
                }
                if (voucher != null)
                {
                    var user = (UserDetails)Session[Constants.SESSION_USER];
                    result = _buyerServiceFacade.AddorUpdateVoucher(voucher, user.UserId);
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : AddorUpdateVoucher(): Caught an exception" + ex);
                throw;
            }
        }

        public JsonResult IsVoucherAlreadyExists(string code)
        {
            try
            {
                var result = false;
                result = _buyerServiceFacade.IsVoucherAlreadyExists(code);
                return Json(result);
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : IsVoucherAlreadyExists() : Caught an exception " + ex);
                throw;
            }
        }

        public ActionResult Voucher()
        {
            return View();
        }

        public JsonResult GetAllVouchers(int currentPage, string sortParameter, int sortDirection, int count)
        {
            try
            {
                int total = 0;
                var vouchersList = _buyerServiceFacade.GetAllVouchers(currentPage, sortParameter, sortDirection, out total, count);
                return Json(new { vouchersList = vouchersList, total = total });
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : GetVoucherDetails(): Caught an exception" + ex);
                throw;
            }
        }

        public JsonResult GetVoucherDetailsForDashboard(long BuyerPartyId, int pageNumber, int sortDirection)
        {
            var voucherDetails = new List<Voucher>();
            int total = 0;
            try
            {
                voucherDetails = _buyerServiceFacade.GetAllVouchers(pageNumber, string.Empty, sortDirection, out total, 5, BuyerPartyId);
            }
            catch (Exception ex)
            {
                Logger.Info("AdminController : GetBuyerDetailsForDashboard() : Caught an exception " + ex);
                throw;
            }
            return Json(new { voucherDetails = voucherDetails, total = total });
        }

        public JsonResult GetBuyerCampaignDetailsForDashboard(long partyId, int pageNumber, int sortDirection)
        {
            var campaignDetails = new List<Campaign>();
            var totalCampaigns = 0;

            try
            {
                campaignDetails = _campaignServiceFacade.GetBuyerCampaignDetailsForDashboard(out totalCampaigns, partyId, pageNumber, sortDirection);
            }
            catch (Exception ex)
            {
                Logger.Info("AdminController : GetBuyerCampaignDetailsForDashboard() : Caught an exception " + ex);
                throw;
            }
            var result = new { campaignDetails = campaignDetails, totalCampaigns = totalCampaigns };
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetAssignCampaignAuditorList()
        {
            var auditorList = new List<ItemList>();
            try
            {
                auditorList = _roleServiceFacade.GetUserListByPermission((long)AuditorRoles.VerifyCampaign);
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : GetAssignCampaignAuditorList() : Caught an exception" + ex);
                throw;
            }
            return Json(new { result = auditorList });
        }

        [HttpPost]
        public JsonResult AssignCampaignToAuditor(string auditorId, string campaignId)
        {
            var result = false;
            var message = ReadResource.GetResourceForGlobalization(Constants.CAMPAIGN_ASSIGNED_TO_AUDITOR_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
            try
            {
                var user = (UserDetails)Session[Constants.SESSION_USER];
                result = _campaignServiceFacade.AssignCampaignToAuditor(Convert.ToInt64(auditorId), Convert.ToInt32(campaignId), user.UserId);
                message = ReadResource.GetResourceForGlobalization(Constants.CAMPAIGN_ASSIGNED_TO_AUDITOR_SUCCESS_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : AssignCampaignToAuditor() : Caught an exception" + ex);
                throw;
            }
            return Json(new { result = result, message = message });
        }

        [HttpPost]
        public JsonResult GetAssignedCampaigns(string currPage, string auditorId)
        {
            var campaignList = new List<Campaign>();
            int total = 0;
            int currentPage = 1;
            long auditorID = 0;
            try
            {
                if (long.TryParse(auditorId, out auditorID) && int.TryParse(currPage, out currentPage))
                {
                    campaignList = _campaignServiceFacade.GetAssignedCampaigns(auditorID, currentPage, out total);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : GetAssignedCampaigns() : Caught an exception" + ex);
                throw ex;
            }
            return Json(new { data = campaignList, total = total });
        }

        public JsonResult GetCampaignsAwaitingAction(int PageNo, long auditorId)
        {
            var campaignList = new List<Campaign>();
            int total = 0;
            try
            {
                campaignList = _campaignServiceFacade.GetCampaignsAwaitingAction(out total, PageNo, auditorId);
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : GetAssignedCampaigns() : Caught an exception" + ex);
                throw ex;
            }
            return Json(new { data = campaignList, total = total });
        }

        [HttpPost]
        public JsonResult GetSubmittedCampaigns(string currPage)
        {
            var campaignList = new List<Campaign>();
            int total = 0;
            int currentPage = 1;
            try
            {
                if (int.TryParse(currPage, out currentPage))
                {
                    campaignList = _campaignServiceFacade.GetSubmittedOrApprovedCampaigns((short)CampaignStatus.Submitted, currentPage, out total);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : GetSubmittedCampaigns() : Caught an exception" + ex);
            }
            return Json(new { totalRecords = total, campaigns = campaignList });
        }

        [HttpPost]
        public JsonResult GetApprovedCampaigns(string currPage)
        {
            var campaignList = new List<Campaign>();
            int total = 0;
            int currentPage = 1;
            try
            {
                if (int.TryParse(currPage, out currentPage))
                {
                    campaignList = _campaignServiceFacade.GetSubmittedOrApprovedCampaigns((short)CampaignStatus.Approved, currentPage, out total);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : GetApprovedCampaigns() : Caught an exception" + ex);
            }
            return Json(new { totalRecords = total, campaigns = campaignList });
        }

        public FileResult WorkSheet(string campaignId)
        {
            try
            {
                int total = 0;
                int id = 0;
                if (!string.IsNullOrWhiteSpace(campaignId))
                {
                    id = Convert.ToInt32(campaignId);

                }
                var supplierList = _campaignServiceFacade.GetPreRegSupplierInCampaign(id, "", out total, 1, int.MaxValue);
                string exclude = "PreRegSupplierId, CampaignId, Password, IsValid, InvalidSupplierComments, Country, IsSubsidary, IsRegistered, CampaignURL";
                var file = CommonMethods.CreateDownloadExcel(supplierList, "", exclude, "Suppliers List", "Pre reg Suppliers");
                return File(file.GetBuffer(), "application/vnd.ms-excel", "Worksheet.xls");
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : WorkSheet(string campaignId) : Caught an error" + ex);
                throw;
            }
        }

        [HttpPost]
        public JsonResult ApproveCampaign(string campaignId)
        {
            var result = false;
            var message = "";
            int id = 0;
            try
            {
                if (int.TryParse(campaignId, out id))
                {
                    if (_campaignServiceFacade.CompareSupplierCountAndMasterVendor(id))
                    {
                        result = _campaignServiceFacade.UpdateCampaignStatus(id, (short)CampaignStatus.Approved);
                        message = ReadResource.GetResourceForGlobalization(Constants.CAMPAIGN_APPROVED_SUCCESS_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
                    }
                    else
                    {
                        message = ReadResource.GetResourceForGlobalization(Constants.UPDATE_MASTER_VENDOR_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : ApproveCampaign() : Caught an error" + ex);
                throw;
            }
            return Json(new { result = result, message = message });
        }

        [HttpPost]
        public JsonResult RevertCampaignToAssign(int campaignId)
        {
            var message = ReadResource.GetResourceForGlobalization(Constants.REVERT_CAMPAIGN_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
            var result = false;
            try
            {
                result = _campaignServiceFacade.UpdateCampaignStatus(campaignId, (short)CampaignStatus.Assigned);
                message = ReadResource.GetResourceForGlobalization(Constants.REVERT_CAMPAIGN_SUCCESS_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : RevertCampaignToAssign() : Caught an error" + ex);
                throw;
            }
            return Json(new { result = result, message = message });
        }

        [HttpGet]
        public FileResult GetCampaignReleaseSuppliers(string campaignId)
        {
            try
            {
                int id = 0;
                if (!string.IsNullOrWhiteSpace(campaignId))
                {
                    id = Convert.ToInt32(campaignId);

                }
                var supplierList = _campaignServiceFacade.GetPreRegSupplierListwithPasswordString(id);
                var file = CommonMethods.CreateDownloadExcel(supplierList, "", "", "Suppliers", "Pre reg Suppliers");
                return File(file.GetBuffer(), "application/vnd.ms-excel", "Suppliers.xls");
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : SuppliersList(string campaignId) : Caught an error" + ex);
                throw;
            }
        }

        [HttpPost]
        public JsonResult RevertCampaignToSubmitted(int campaignId)
        {
            var message = ReadResource.GetResourceForGlobalization(Constants.REVERT_CAMPAIGN_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
            var result = false;
            try
            {
                result = _campaignServiceFacade.UpdateCampaignStatus(campaignId, (short)CampaignStatus.Submitted);
                if (result)
                {
                    _campaignServiceFacade.UpdateCampaignDownloadStatus(campaignId, false);
                }
                message = ReadResource.GetResourceForGlobalization(Constants.REVERT_CAMPAIGN_SUCCESS_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : RevertCampaignToSubmitted() : Caught an error" + ex);
                throw;
            }
            return Json(new { result = result, message = message });
        }

        [HttpPost]
        public JsonResult ReleaseCampaign(string campaignId, string isDownloaded)
        {
            var result = false;
            var message = ReadResource.GetResourceForGlobalization(Constants.CAMPAIGN_RELEASED_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
            int id = 0;
            try
            {
                if (int.TryParse(campaignId, out id))
                {
                    if (Convert.ToBoolean(isDownloaded))
                    {
                        result = _campaignServiceFacade.UpdateCampaignStatus(id, (short)CampaignStatus.Release);
                        message = ReadResource.GetResourceForGlobalization(Constants.CAMPAIGN_RELEASED_SUCCESS_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
                    }
                    else
                    {
                        var baseUrl = CommonMethods.GetBaseUrl();
                        UpdatePasswordAndSendMailDelegateInvoke(id, baseUrl);
                        result = _campaignServiceFacade.UpdateCampaignStatus(id, (short)CampaignStatus.Release);
                        message = ReadResource.GetResourceForGlobalization(Constants.CAMPAIGN_RELEASED_SUCCESS_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : ReleaseCampaign() : Caught an error" + ex);
                throw;
            }
            return Json(new { result = result, message = message });
        }

        delegate void DelegateUpdatePasswordAndSendMail(int campaignId, string baseUrl);

        public void UpdatePasswordAndSendMailDelegateInvoke(int campaignId, string baseUrl)
        {
            DelegateUpdatePasswordAndSendMail sendMailDelegate;
            sendMailDelegate = new DelegateUpdatePasswordAndSendMail(UpdatePasswordAndSendMail);
            sendMailDelegate.BeginInvoke(campaignId, baseUrl, null, null);
        }

        private void UpdatePasswordAndSendMail(int campaignId, string baseUrl)
        {
            int total = 0;
            try
            {
                var supplierList = _campaignServiceFacade.GetPreRegSupplierInCampaign(campaignId, "", out total, 1, int.MaxValue);
                foreach (var supplier in supplierList)
                {
                    var emailId = string.Empty;
                    var toEmailId = string.Empty;
                    toEmailId = supplier.LoginId;
                    emailId = supplier.LoginId;
                    Random random = new Random();
                    var randomPassword = CommonMethods.GetUniqueKey(random);   // return this to user by E-mail
                    var domainSupplier = supplier.LoginId.Substring(supplier.LoginId.IndexOf('@'));
                    var encryptedRandomPassword = CommonMethods.EncryptMD5Password(domainSupplier.ToLower() + randomPassword.Trim());
                    _campaignServiceFacade.UpdatePreRegSupplierPassword(supplier.PreRegSupplierId, encryptedRandomPassword);
                    var landingPageUrl = baseUrl + supplier.CampaignURL;
                    SendMailToSupplier(toEmailId, emailId, randomPassword, landingPageUrl, supplier.EmailContent, supplier.BuyerOrganization);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : UpdatePasswordAndSendMail() : Caught an error" + ex);
            }
        }

        private void SendMailToSupplier(string toEmailId, string emailId, string randomPassword, string landingPageURL, string emailContent, string buyerOrg)
        {
            long localeId = 0;
            var FAQlink = string.Empty;
            switch (Configuration.Environment)
            {
                case "DEV":
                    FAQlink = "http://localhost:63378/FAQ";
                    break;
                case "UAT":
                    FAQlink = "http://88.208.220.112:8087/FAQ";
                    break;
                case "PROD":
                    FAQlink = "https://sim.prgx.com/FAQ";
                    break;
            }
            var emailMessage = _emailServiceFacade.GetEmailMessage(Constants.PRE_REG_CAMPAIGN_RELEASE, localeId, buyerOrg, landingPageURL, emailId, randomPassword, FAQlink);
            if (Constants.IS_EMAIL_ON)
            {
                try
                {
                    // get the application path.
                    string appFilePhysicalPath = HttpContext.Request.PhysicalApplicationPath;
                    string emailSIMLogoUrl = appFilePhysicalPath + @"\Content\Images\logo\siteLogo.png";
                    string emailPRGXLogoUrl = appFilePhysicalPath + @"\Content\Images\logo\simTraxFooterLogoForEmail.png";
                    _emailServiceFacade.SendEmail(toEmailId, string.Empty, string.Empty, emailMessage.Subject, emailMessage.Content, emailSIMLogoUrl, emailPRGXLogoUrl, "", "", appFilePhysicalPath);
                }
                catch
                {
                }
            }
        }
        
        [HttpPost]
        public JsonResult GetSupplierReferrerBuyerCampaignDetails(int pageNo, string buyerName, string campaignName, long supplierId)
        {
            var referrers = new List<SupplierReferrer>();
            var total = 0;
            try
            {
                referrers = _campaignServiceFacade.GetSupplierReferrerBuyerCampaignDetails(pageNo, buyerName, campaignName, supplierId, out total).ToList();
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : GetSupplierReferrerBuyerCampaignDetails() : Caught an exception " + ex);
                throw;
            }
            var result = new { referrers = referrers, total = total };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddOrUpdateSupplierReferrer(long supplierId, string[] assignReferrerCampaign, string[] removeReferrerCampaign, string[] primaryReferrerCampaign)
        {
            object data = null;
            var result = false;
            try
            {
                var auditorId = ((UserDetails)Session[Constants.SESSION_USER]).UserId;
                result = _campaignServiceFacade.UpdateSupplierReferrerDetails(supplierId, assignReferrerCampaign, removeReferrerCampaign, primaryReferrerCampaign, auditorId);
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : AddOrUpdateSupplierReferrer() : Caught an error" + ex);
                throw;
            }
            if (result)
                data = new { result = result, message = ReadResource.GetResourceForGlobalization(Constants.SAVED_SUCCESSFULLY, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()) };
            else
                data = new { result = result, message = ReadResource.GetResourceForGlobalization(Constants.DEFAULT_ERROR_MESSAGE, (Session[Constants.USER_PREFERENCE_CULTURE] != null) ? (CultureInfo)Session[Constants.USER_PREFERENCE_CULTURE] : ResolveCulture()) };
            return Json(data);
        }

        [HttpPost]
        public JsonResult GetSuppliersForVerification(int pageNo, string sortParameter, int sortDirection, int sourceCheck, int viewOptions, string referrerName, int pageSize)
        {
            var model = new List<SupplierOrganization>();
            int total = 0;
            try
            {
                model = _partyServiceFacade.GetSuppliersForVerification(pageNo, sortParameter, sortDirection, out total, sourceCheck, viewOptions, pageSize, referrerName);
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : GetSuppliersForVerification() : Caught an exception " + ex);
                throw;
            }
            var jsonResult = Json(new { data = model, totalRecords = total }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetSuppliersCountBasedOnStage(int sourceCheck, int viewOptions, string referrerName)
        {
            var model = new List<SupplierCountBasedOnStage>();
            try
            {
                model = _partyServiceFacade.GetSuppliersCountBasedOnStage(sourceCheck, viewOptions, referrerName);
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : GetSuppliersCountBasedOnStage() : Caught an exception " + ex);
                throw;
            }
            var jsonResult = Json(new { data = model }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public FileResult SupplierInfoDownload(int sourceCheck, int viewOptions, string referrerName)
        {
            var total = 0;
            try
            {
                var VerifySuppliersList = _partyServiceFacade.GetSuppliersForVerification(1, string.Empty, 1, out total, sourceCheck, viewOptions, int.MaxValue, referrerName);
                string filterValue = String.Empty;
                var filtersList = new List<ExcelDownloadFilterList>();
                string filtervalues = String.Empty;
                if (!String.IsNullOrEmpty(referrerName))
                {
                    filtersList.Add(new ExcelDownloadFilterList()
                    {
                        FilterType = "Referrer Name",
                        FilterValue = referrerName.ToString()
                    });
                }

                if ((ProjectSource)sourceCheck != ProjectSource.Both)
                {
                    filtervalues = ((ProjectSource)sourceCheck).Description();
                }
                else
                {
                    foreach (var value in Enum.GetValues(typeof(ProjectSource)).Cast<ProjectSource>().Where(v => !v.Equals((ProjectSource)ProjectSource.Both)).Select(v => v.Description()).ToList())
                    {
                        filtervalues = filtervalues + value + ", ";
                    }
                    filtervalues = filtervalues.Substring(0, filtervalues.LastIndexOf(", "));
                }
                if (!String.IsNullOrEmpty(filtervalues))
                {
                    filtersList.Add(new ExcelDownloadFilterList()
                    {
                        FilterType = "Source",
                        FilterValue = filtervalues
                    });
                }
                if (viewOptions == 1)
                {
                    filtersList.Add(new ExcelDownloadFilterList()
                    {
                        FilterType = "View Options",
                        FilterValue = "All"
                    });

                }
                else if (viewOptions == 2)
                {
                    filtersList.Add(new ExcelDownloadFilterList()
                    {
                        FilterType = "View Options",
                        FilterValue = "Profile"
                    });

                }
                else if (viewOptions == 3)
                {
                    filtersList.Add(new ExcelDownloadFilterList()
                    {
                        FilterType = "View Options",
                        FilterValue = "Sanction"
                    });

                }
                else if (viewOptions == 4)
                {
                    filtersList.Add(new ExcelDownloadFilterList()
                    {
                        FilterType = "View Options",
                        FilterValue = "FIT"
                    });
                }

                else if (viewOptions == 5)
                {
                    filtersList.Add(new ExcelDownloadFilterList()
                    {
                        FilterType = "View Options",
                        FilterValue = "HS"
                    });
                }

                else if (viewOptions == 6)
                {
                    filtersList.Add(new ExcelDownloadFilterList()
                    {
                        FilterType = "View Options",
                        FilterValue = "DS"
                    });

                }
                
                var verifyList = new List<VerifySuppliers>();
                foreach (var item in VerifySuppliersList)
                {
                    var listItem = new VerifySuppliers();
                    listItem.SupplierName = item.SupplierOrganizationName;
                    listItem.SupplierId = item.SupplierId;
                    listItem.OtherReferrer = string.Join("; ", item.SupplierReferrers.Where(u => u.LandingReferrer != true).Select(u => u.BuyerOrganizationName).ToList());
                    //foreach (var otherReferrerName in refferes)
                    //{
                    //    item.OtherReferres += otherReferrerName + "; ";
                    //}

                    listItem.LandingPageReferrer = item.SupplierReferrers.FirstOrDefault(u => u.LandingReferrer == true) != null ?
                        item.SupplierReferrers.FirstOrDefault(u => u.LandingReferrer == true).BuyerOrganizationName : string.Empty;

                    if (item.SupplierStatus == (short)CompanyStatus.Submitted)
                    {
                        listItem.Details = CommonMethods.Description(VerifySupplierReportStatus.Awaiting);
                    }
                    else if (item.SupplierStatus == (short)CompanyStatus.RegistrationVerified)
                    {
                        listItem.Details = CommonMethods.Description(VerifySupplierReportStatus.Verifed);
                        listItem.DetailsDate = (item.DetailsVerifiedDate != null && item.DetailsVerifiedDate.HasValue) ? item.DetailsVerifiedDate.Value : item.ProfileVerifiedDate;
                        //if (item.SupplierProfileAnswers != null && item.SupplierProfileAnswers.Count > 0)
                        //{
                        //    listItem.Profile = CommonMethods.Description(VerifySupplierReportStatus.InProgess);
                        //}
                        //else
                        //{
                        //    listItem.Profile = CommonMethods.Description(VerifySupplierReportStatus.Awaiting);
                        //}
                    }
                    if (item.SupplierStatus == (short)CompanyStatus.ProfileVerified)
                    {
                        listItem.Details = CommonMethods.Description(VerifySupplierReportStatus.Verifed);
                        listItem.DetailsDate = (item.DetailsVerifiedDate != null && item.DetailsVerifiedDate.HasValue) ? item.DetailsVerifiedDate.Value : item.ProfileVerifiedDate;
                        listItem.Profile = CommonMethods.Description(VerifySupplierReportStatus.Verifed);
                        listItem.ProfileDate = item.ProfileVerifiedDate;
                        //if (item.IsSanctionVerificationStarted)
                        //{
                        //    listItem.Sanction = CommonMethods.Description(VerifySupplierReportStatus.InProgess);
                        //}
                        //else
                        //{
                        //    listItem.Sanction = CommonMethods.Description(VerifySupplierReportStatus.Awaiting);
                        //}
                    }
                    else if (item.SupplierStatus == (short)CompanyStatus.SanctionVerified)
                    {
                        listItem.Details = CommonMethods.Description(VerifySupplierReportStatus.Verifed);
                        listItem.DetailsDate = (item.DetailsVerifiedDate != null && item.DetailsVerifiedDate.HasValue) ? item.DetailsVerifiedDate.Value : item.ProfileVerifiedDate;
                        listItem.Profile = CommonMethods.Description(VerifySupplierReportStatus.Verifed);
                        listItem.ProfileDate = item.ProfileVerifiedDate;
                        listItem.Sanction = CommonMethods.Description(VerifySupplierReportStatus.Verifed);
                        listItem.SanctionDate = item.ProfileVerifiedDate;
                    }
                    //if (item.SupplierProducts.Any(u => u.ProductId == (short)Pillar.FinanceInsuranceTax && u.Status >= (short)SupplierProductStatus.PaymentDone))
                    //{
                    //    if (!item.SupplierProducts.Any(u => u.ProductId == (short)Pillar.FinanceInsuranceTax && u.EvaluationStartedDate.HasValue))
                    //    {
                    //        listItem.FIT = CommonMethods.Description(VerifySupplierReportStatus.Awaiting);
                    //    }
                    //    else if (item.SupplierProducts.Any(u => u.ProductId == (short)Pillar.FinanceInsuranceTax && u.EvaluationStartedDate.HasValue && u.PublishedDate.HasValue))
                    //    {
                    //        listItem.FIT = CommonMethods.Description(VerifySupplierReportStatus.Verifed);
                    //    }
                    //    if (item.SupplierProducts.Any(u => u.ProductId == (short)Pillar.FinanceInsuranceTax && u.EvaluationStartedDate.HasValue && !u.PublishedDate.HasValue))
                    //    {
                    //        listItem.FIT = CommonMethods.Description(VerifySupplierReportStatus.InProgess);
                    //    }
                    //    listItem.FITDate = item.SupplierProducts.FirstOrDefault(u => u.ProductId == (short)Pillar.FinanceInsuranceTax).PublishedDate;
                    //}

                    //if (item.SupplierProducts.Any(u => u.ProductId == (short)Pillar.HealthSafety && u.Status >= (short)SupplierProductStatus.PaymentDone))
                    //{
                    //    if (!item.SupplierProducts.Any(u => u.ProductId == (short)Pillar.HealthSafety && u.EvaluationStartedDate.HasValue))
                    //    {
                    //        listItem.HS = CommonMethods.Description(VerifySupplierReportStatus.Awaiting);
                    //    }
                    //    else if (item.SupplierProducts.Any(u => u.ProductId == (short)Pillar.HealthSafety && u.EvaluationStartedDate.HasValue && u.PublishedDate.HasValue))
                    //    {
                    //        listItem.HS = CommonMethods.Description(VerifySupplierReportStatus.Verifed);
                    //    }
                    //    if (item.SupplierProducts.Any(u => u.ProductId == (short)Pillar.HealthSafety && u.EvaluationStartedDate.HasValue && !u.PublishedDate.HasValue))
                    //    {
                    //        listItem.HS = CommonMethods.Description(VerifySupplierReportStatus.InProgess);
                    //    }
                    //    listItem.HSDate = item.SupplierProducts.FirstOrDefault(u => u.ProductId == (short)Pillar.HealthSafety).PublishedDate;
                    //}

                    //if (item.SupplierProducts.Any(u => u.ProductId == (short)Pillar.DataSecurity && u.Status >= (short)SupplierProductStatus.PaymentDone))
                    //{
                    //    if (!item.SupplierProducts.Any(u => u.ProductId == (short)Pillar.DataSecurity && u.EvaluationStartedDate.HasValue))
                    //    {
                    //        listItem.DS = CommonMethods.Description(VerifySupplierReportStatus.Awaiting);
                    //    }
                    //    else if (item.SupplierProducts.Any(u => u.ProductId == (short)Pillar.DataSecurity && u.EvaluationStartedDate.HasValue && u.PublishedDate.HasValue))
                    //    {
                    //        listItem.DS = CommonMethods.Description(VerifySupplierReportStatus.Verifed);
                    //    }
                    //    if (item.SupplierProducts.Any(u => u.ProductId == (short)Pillar.DataSecurity && u.EvaluationStartedDate.HasValue && !u.PublishedDate.HasValue))
                    //    {
                    //        listItem.DS = CommonMethods.Description(VerifySupplierReportStatus.InProgess);
                    //    }
                    //    listItem.DSDate = item.SupplierProducts.FirstOrDefault(u => u.ProductId == (short)Pillar.DataSecurity).PublishedDate;
                    //}

                    verifyList.Add(listItem);
                }

                string exclude = string.Empty;
                var file = CommonMethods.CreateDownloadExcel(verifyList, "", exclude, "Suppliers Report", "Supplier Information To Verify/Audit", filtersList);
                return File(file.GetBuffer(), "application/vnd.ms-excel", "SuppliersReport.xls");
            }
            catch (Exception ex)
            {
                Logger.Error("AdminController : SupplierInfoDownload() : Caught an error" + ex);
                throw;
            }
        }
    }
}