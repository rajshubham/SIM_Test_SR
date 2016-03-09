using System.Web.Mvc;
using System.Web.Routing;

namespace PRGX.SIMTrax.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "PrivacyPolicy",
                url: "privacy-policy",
                defaults: new { controller = "Account", action = "PrivacyPolicy" }
            );

            routes.MapRoute(
                name: "TermsOfUse",
                url: "terms-of-use",
                defaults: new { controller = "Account", action = "TermsOfUse" }
            );

            routes.MapRoute(
                name: "ContactUs",
                url: "contact-us",
                defaults: new { controller = "Account", action = "ContactUs" }
            );

            routes.MapRoute(
                name: "FrequentlyAskedQuestions",
                url: "FAQ",
                defaults: new { controller = "Account", action = "FrequentlyAskedQuestions" }
            );

            routes.MapRoute(
                name: "SupplierHome",
                url: "supplier-home",
                defaults: new { controller = "Supplier", action = "Home" }
            );

            routes.MapRoute(
                name: "EditGeneralInformationAndContacts",
                url: "supplier/questionnaire/general-info",
                defaults: new { controller = "Supplier", action = "GeneralInformationAndContacts" }
            );

            routes.MapRoute(
                name: "ResetPassword",
                url: "change-password",
                defaults: new { controller = "Account", action = "ResetPassword" }
            );

            routes.MapRoute(
                name: "Profile",
                url: "Profile/{sellerPartyId}",
                defaults: new { controller = "Common", action = "SupplierProfile", sellerPartyId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CampaignVerify",
                url: "Campaign/Verify/{campaignId}",
                defaults: new { controller = "Campaign", action = "Verify", campaignId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Login",
                url: "Login",
                defaults: new { controller = "Account", action = "Login" }
             );

            routes.MapRoute(
                name: "PreCampaignLogin",
                url: "{permalink}",
                defaults: new { controller = "Campaign", action = "Login", permalink = UrlParameter.Optional },
                constraints: new { permalink = new Filters.PreCampaignRouteConstraint() }
            );

            routes.MapRoute(
                name: "CampaignLogin",
                url: "{permalink}",
                defaults: new { controller = "Campaign", action = "LandingPage", permalink = UrlParameter.Optional },
                constraints: new { permalink = new Filters.CampaignRouteConstraint() }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Account", action = "Login" }
            );
        }
    }
}
