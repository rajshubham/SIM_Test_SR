using PRGX.SIMTrax.Domain.Util;
using PRGX.SIMTrax.Web.Filters;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PRGX.SIMTrax.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Logger.XmlConfigure();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalFilters.Filters.Add(new AuthorizeFilter());
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Application["ContactEmail"] = Constants.CONTACT_EMAIL;
            Application["ContactNo"] = Constants.CONTACT_NUMBER;
            Logger.Info(" Global.aspx : Application_Start : Enter into method");
            Logger.Info(" Global.aspx : Application_Start : Exit from method");

        }
    }
}
