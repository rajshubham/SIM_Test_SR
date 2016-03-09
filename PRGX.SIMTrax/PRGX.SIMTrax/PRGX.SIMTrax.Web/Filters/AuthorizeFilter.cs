using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PRGX.SIMTrax.Web.Filters
{
    public class AuthorizeFilter :ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                if (HttpContext.Current.Session != null && HttpContext.Current.Session[Constants.SESSION_USER] == null)
                {
                  //  var singleSignOn = HttpContext.Current.Request.Cookies["SIMTraxSSO"];
                    //Logger.Info("Global.asax : singleSignOn :" + singleSignOn);
                    //if (singleSignOn != null)
                    //{
                    //    Logger.Info("Global.asax : cookie Exist :");
                    //    var _userService = new UserService();
                    //    var _companyService = new CompanyService();
                    //    var id = new Guid(FormsAuthentication.Decrypt(singleSignOn.Value).Name);
                    //    var loginInfo = _userService.GetLoginInfoFromCommonDB(id);
                    //    if (loginInfo != null && loginInfo.Id != Guid.Empty)
                    //    {
                    //        var userDetails = _userService.GetUserDetailsByEmail(loginInfo.LoginId);
                    //        if (userDetails != null)
                    //        {
                    //            Logger.Info("Global.asax : userDetails :" + userDetails);
                    //            HttpContext.Current.Session[Constants.SessionUser] = userDetails;
                    //            HttpContext.Current.Session[Constants.UserCompanyDetails] = _companyService.GetCompanyDetails((long)userDetails.CompanyId);
                    //            HttpContext.Current.Session[Constants.SESSION_USER_TYPE] = userDetails.UserType;
                    //            HttpContext.Current.Session[Constants.SESSION_COMPANY_ID] = userDetails.CompanyId;
                    //            //TODO :: After login make an entry into db.
                    //            //If already exist then make it logoff = false.
                    //            var login = new CommonLogin()
                    //            {
                    //                Id = loginInfo.Id,
                    //                LoginId = userDetails.LoginId,
                    //                SIMTraxLogOff = false,
                    //                CIPSLogOff = true
                    //            };
                    //            _userService.AddLoginUserInCommonDB(login);
                    //        }
                    //    }
                    //}
                }

                if (filterContext.Controller.GetType() == typeof(Controllers.AccountController))
                    return;
               
                if (filterContext.ActionDescriptor.ActionName == "Register")
                    return;
                
                if (filterContext.ActionDescriptor.ActionName == "AddOrUpdateSellerOrganisationDetails")
                    return;

                if (filterContext.ActionDescriptor.ActionName == "AddSellerContactDetails")
                    return;

                if (filterContext.ActionDescriptor.ActionName == "LandingPage")
                    return;

                if (filterContext.ActionDescriptor.ActionName == "Login" && filterContext.Controller.GetType() == typeof(Controllers.CampaignController))
                    return;

                if (filterContext.ActionDescriptor.ActionName == "Header")
                    return;
                
                if (filterContext.ActionDescriptor.ActionName == "Footer")
                    return;

                HttpContext ctx = HttpContext.Current;

                // If the browser session or authentication session has expired...
                if (ctx.Session[Constants.SESSION_USER_TYPE] == null)
                {
                    //if (filterContext.HttpContext.Request.IsAuthenticated)
                    //{
                    //    ctx.Cache[Constants.SESSION_EXPIRED] = ReadResource.GetDisplayMessage(Constants.ERROR + Constants.SESSION_EXPIRED);
                    //}

                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        //// For AJAX requests, we're overriding the returned JSON result with a simple string,
                        //// indicating to the calling JavaScript code that a redirect should be performed.
                        var resultString = new { logoutAction = "Account/Logout" };
                        var result = new JsonResult();
                        result.Data = resultString;
                        result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                        filterContext.Result = result;
                    }
                    else
                    {
                        //filterContext.Result = new RedirectResult("/Account/Logout");
                        // For round-trip posts
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Account" }, { "Action", "Logout" } });
                    }
                }
                else
                {
                    var userType = Convert.ToInt64(ctx.Session[Constants.SESSION_USER_TYPE]);
                    switch (userType)
                    {
                        case (Int16)UserType.Buyer:
                        case (Int16)UserType.AdminBuyer:
                            //if (filterContext.ActionDescriptor.ActionName == "GetSupplierInformation")
                            //    return;
                            if (filterContext.Controller.GetType() == typeof(Controllers.BuyerController)
                                ||  filterContext.Controller.GetType() == typeof(Controllers.HomeController)
                                || filterContext.Controller.GetType() == typeof(Controllers.CommonController))
                                return;
                            break;
                        case (Int16)UserType.Supplier:
                        case (Int16)UserType.AdminSupplier:
                            if (filterContext.Controller.GetType() == typeof(Controllers.SupplierController)
                                || filterContext.Controller.GetType() == typeof(Controllers.HomeController)
                                 || filterContext.Controller.GetType() == typeof(Controllers.CommonController))
                            {
                                if (filterContext.ActionDescriptor.ActionName == "Home")
                                {
                                    if (ctx.Session[Constants.SESSION_ORGANIZATION] != null)
                                    {
                                        var organisation = (PRGX.SIMTrax.Domain.Model.OrganizationDetail)ctx.Session[Constants.SESSION_ORGANIZATION];
                                        if (organisation.Status >= (int)(CompanyStatus.Submitted))
                                            return;
                                    }
                                }
                                if (filterContext.ActionDescriptor.ActionName == "users" && userType == (Int16)UserType.Supplier)
                                {
                                    break;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            break;
                        case (Int16)UserType.AdminAuditor:
                        case (Int16)UserType.Auditor:
                            if ((filterContext.Controller.GetType() == typeof(Controllers.AdminController))
                                || (filterContext.Controller.GetType() == typeof(Controllers.HomeController))
                                || filterContext.Controller.GetType() == typeof(Controllers.CampaignController)
                                || filterContext.Controller.GetType() == typeof(Controllers.CommonController))
                                   return;
                            break;
                    }
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        var errorMsg = "You are not authorized to view this page.";
                        bool displayDescription = true;
                        var resultString = new { errorUrl = "Common/Error", errorMessage = errorMsg, errorDisplay = displayDescription };
                        var result = new JsonResult();
                        result.Data = resultString;
                        result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                        filterContext.Result = result;
                    }
                    else
                    {
                        //filterContext.Result = new RedirectResult("/Account/Logout");
                        // For round-trip posts
                        var errorMsg = "You are not authorized to view this page.";
                        bool displayDescription = true;
                        RouteData routeData = new RouteData();
                        routeData.Values.Add("controller", "Common");
                        routeData.Values.Add("action", "Error");
                        routeData.Values.Add("message", errorMsg);
                        routeData.Values.Add("displayDescription", displayDescription);
                        //filterContext.RequestContext = new RequestContext(new HttpContextWrapper(HttpContext.Current), routeData);
                        IController controller = new Controllers.CommonController();
                        controller.Execute(new RequestContext(new HttpContextWrapper(HttpContext.Current), routeData));
                        HttpContext.Current.Response.End();
                    }
                }
                base.OnActionExecuting(filterContext);
            }
            catch (Exception ex)
            {
                Logger.Error("AuthorizeFilter : OnActionExecuting() : Caught an error " + ex);
                throw ex;
            }
        }

    }
}