using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace PRGX.SIMTrax.Web.Controllers
{
    public class BaseController : Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            var culture = new CultureInfo("en-GB");
            //Checking whether he selected any language from Dropdown
            if (Session[Constants.USER_PREFERENCE] != null)
            {
                culture = new CultureInfo(Session[Constants.USER_PREFERENCE].ToString());
            }
            else
            {
                culture = ResolveCulture();
                Session[Constants.USER_PREFERENCE] = ResolveCulture();
            }
            Session[Constants.USER_PREFERENCE_CULTURE] = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
        // changing culture
        public ActionResult ChangeCulture(string ddlCulture)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(ddlCulture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(ddlCulture);
            //Setting which Language he selected from dropdown
            Session[Constants.USER_PREFERENCE] = ddlCulture;
            return RedirectToAction("Register", "Buyer");
        }

        public static CultureInfo ResolveCulture()
        {
            List<string> ExitingLanguages = new List<string>();
            string[] languages = System.Web.HttpContext.Current.Request.UserLanguages;

            if (languages == null || languages.Length == 0)
                return null;

            try
            {
                string language = "en-GB";
                foreach (var item in languages)
                {
                    var lang = item.Contains(";") ? item.Split(';')[0].ToLowerInvariant().Trim() : item.ToLowerInvariant().Trim();
                    if (ExitingLanguages.Contains(lang))
                    {
                        language = lang;
                        break;
                    }
                }
                return CultureInfo.CreateSpecificCulture(language);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
    }
}