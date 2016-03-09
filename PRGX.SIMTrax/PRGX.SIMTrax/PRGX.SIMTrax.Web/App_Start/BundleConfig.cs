using System.Web.Optimization;

namespace PRGX.SIMTrax.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/Content/js").Include(
                   "~/Content/Scripts/jquery-{version}.js",
                   "~/Content/Bootstrap/js/bootstrap.min.js",
                   "~/Content/Scripts/jquery.unobtrusive*",
                   "~/Content/Scripts/jquery.validate*",
                   "~/Content/Plugins/Toastr/toastr.js",
                   "~/Content/Scripts/Pagination.js",
                   "~/Content/Plugins/Tipso/tipso.min.js",
                   "~/Content/Scripts/jquery-ui-1.9.2.custom.min.js",
                   "~/Content/Plugins/FooTable/footable.js",
                  "~/Content/Scripts/Common.js"));

            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                "~/Content/Scripts/Common.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/AES").Include(
               "~/Content/Scripts/AES.js"));


            bundles.Add(new ScriptBundle("~/bundles/Profile").Include(
           "~/Content/Scripts/Supplier/Profile.js"));

            bundles.Add(new StyleBundle("~/bundles/ProfileCss").Include(
                "~/Content/Css/Supplier/Profile.css"));

            bundles.Add(new ScriptBundle("~/bundles/SupplierRegister").Include(
                "~/Content/Scripts/Supplier/Register.js"));

            bundles.Add(new StyleBundle("~/bundles/TipsoCss").Include(
                "~/Content/Plugins/Tipso/tipso.min.css",
                "~/Content/Plugins/Tipso/animate.css"
              ));

            bundles.Add(new ScriptBundle("~/bundles/Tipso").Include(
                "~/Content/Plugins/Tipso/tipso.min.js"
              ));

            bundles.Add(new ScriptBundle("~/bundles/multiselects").Include(
                "~/Content/Bootstrap/js/bootstrap-multiselect.js"
             ));

            bundles.Add(new StyleBundle("~/bundles/multiselectCss").Include(
                "~/Content/Bootstrap/css/bootstrap-multiselect.css"
             ));

            bundles.Add(new ScriptBundle("~/bundles/Charts").Include(
                "~/Content/Plugins/Charts/DoNut.js",
                "~/Content/Plugins/Charts/excanvas.min.js",
                "~/Content/Plugins/Charts/jquery.jqplot.js",
                "~/Content/Plugins/Charts/jqplot.donutRenderer.js"
           ));

            bundles.Add(new ScriptBundle("~/bundles/EditGeneralInformationAndContacts").Include(
                "~/Content/Scripts/Supplier/GeneralInformationAndContacts.js"));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/Content/Bootstrap/css/bootstrap.css",
                "~/Content/Bootstrap/css/bootstrap-theme.min.css",
                "~/Content/Plugins/Toastr/toastr.css",
                "~/Content/font-awesome.css",
                                "~/Content/Css/common.css",
                "~/Content/Css/TableGrid.css",
                "~/Content/Css/Pagination.css",
                "~/Content/Plugins/Tipso/tipso.min.css",
                "~/Content/jquery-ui-1.9.2.custom.min.css",
                "~/Content/Plugins/FooTable/footable.core.min.css"));

            bundles.Add(new StyleBundle("~/Content/Themes/base/css").Include(
              "~/Content/Themes/base/jquery.ui.core.css",
              "~/Content/Themes/base/jquery.ui.autocomplete.css",
              "~/Content/Themes/base/jquery.ui.theme.css"));

            bundles.Add(new StyleBundle("~/bundles/ChartsCss").Include(
                "~/Content/Plugins/Charts/DoNut.css",
                "~/Content/Plugins/style.css",
                "~/Content/Plugins/Charts/jquery.jqplot.min.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/custom-intellisense").Include(
                "~/Content/Plugins/CustomIntellisense/custom-intellisense.js"));

            bundles.Add(new ScriptBundle("~/bundles/multi-custom-intellisense").Include(
                "~/Content/Plugins/CustomIntellisense/multi-custom-intellisense.js"));

            bundles.Add(new StyleBundle("~/bundles/intellisenseCss").Include(
                "~/Content/Css/intellisense.css"
             ));

            bundles.Add(new ScriptBundle("~/bundles/respond").Include(
                "~/Content/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/Login").Include(
                "~/Content/Scripts/Account/Login.js"));

            bundles.Add(new ScriptBundle("~/bundles/ResetPassword").Include(
                "~/Content/Scripts/Account/ResetPassword.js"));

            bundles.Add(new ScriptBundle("~/bundles/BuyerRegister").Include(
                "~/Content/Scripts/Buyer/Register.js"));

            bundles.Add(new ScriptBundle("~/bundles/BuyerOrganisation").Include(
                "~/Content/Scripts/Admin/BuyerOrganisation.js"));

            bundles.Add(new ScriptBundle("~/bundles/SupplierHome").Include(
                "~/Content/Scripts/Supplier/Home.js"));

            bundles.Add(new ScriptBundle("~/bundles/AdminHome").Include(
                "~/Content/Scripts/Admin/Home.js"));

            bundles.Add(new ScriptBundle("~/bundles/DefineAccessTypes").Include(
               "~/Content/Scripts/Admin/DefineAccessTypes.js"));

            bundles.Add(new ScriptBundle("~/bundles/SupplierSearch").Include(
                "~/Content/Scripts/Buyer/SupplierSearch.js"));

            bundles.Add(new ScriptBundle("~/bundles/ManageUsers").Include(
                "~/Content/Scripts/Admin/ManageUsers.js"));

            bundles.Add(new ScriptBundle("~/bundles/BuyerDashboard").Include(
                "~/Content/Scripts/Admin/BuyerDashBoard.js" ));

            bundles.Add(new ScriptBundle("~/bundles/SupplierOrganisations").Include(
                "~/Content/Scripts/Admin/SupplierOrganisations.js"));

            bundles.Add(new ScriptBundle("~/bundles/SupplierDashboard").Include(
               "~/Content/Scripts/Admin/SupplierDashboard.js"));

            bundles.Add(new ScriptBundle("~/bundles/VerifyCampaign").Include(
                "~/Content/Scripts/Campaign/VerifyCampaign.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
