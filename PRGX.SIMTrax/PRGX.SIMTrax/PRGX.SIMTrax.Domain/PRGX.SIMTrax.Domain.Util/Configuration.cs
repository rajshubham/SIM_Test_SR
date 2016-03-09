using System;
using System.Web.Configuration;

namespace PRGX.SIMTrax.Domain.Util
{
    /// <summary>
    /// TODO :: Contain all configuration(Appsetting) variable
    /// TODO :: All Appsetting configuration must be access from this class
    /// </summary>
    public class Configuration
    {
        public static string PageSize
        {
            get { return WebConfigurationManager.AppSettings.Get("PageSize"); }
        }
        public static string Environment
        {
            get { return WebConfigurationManager.AppSettings.Get("Environment"); }
        }

        public static string ErrorMailReceiver1
        {
            get { return WebConfigurationManager.AppSettings.Get("ErrorMailReceiver1").ToString() != null ? WebConfigurationManager.AppSettings.Get("ErrorMailReceiver1").ToString() : "mahak@raremile.com"; }
        }

        public static string ErrorMailReceiver2
        {
            get { return WebConfigurationManager.AppSettings.Get("ErrorMailReceiver2"); }
        }

        public static string ErrorMailReceiver3
        {
            get { return WebConfigurationManager.AppSettings.Get("ErrorMailReceiver3"); }
        }

        public static string ErrorMailReceiver4
        {
            get { return WebConfigurationManager.AppSettings.Get("ErrorMailReceiver4"); }
        }

        public static string ErrorMailReceiver5
        {
            get { return WebConfigurationManager.AppSettings.Get("ErrorMailReceiver5"); }
        }

        public static string ErrorMailReceiver6
        {
            get { return WebConfigurationManager.AppSettings.Get("ErrorMailReceiver6"); }
        }

        public static string Excel97ConnectionString
        {
            get { return WebConfigurationManager.AppSettings.Get("Excel97ConnectionString"); }
        }

        public static string Excel07ConnectionString
        {
            get { return WebConfigurationManager.AppSettings.Get("Excel07ConnectionString"); }
        }

        public static string REGION_IDENTIFIER
        {
            get { return WebConfigurationManager.AppSettings.Get("REGION_IDENTIFIER"); }
        }

        public static string AccountLockCount
        {
            get { return WebConfigurationManager.AppSettings.Get("AccountLockCount"); }
        }

        public static string AccountLockTime
        {
            get { return WebConfigurationManager.AppSettings.Get("AccountLockTime"); }
        }

        public static string DocumentFileUploadPath
        {
            get { return WebConfigurationManager.AppSettings.Get("DocumentFileUploadPath") != null ? Convert.ToString((WebConfigurationManager.AppSettings.Get("DocumentFileUploadPath"))) : @"~\Documents"; }
        }

        public static string BuyerRequestCount
        {
            get { return WebConfigurationManager.AppSettings.Get("BuyerRequestCount") != null ? (WebConfigurationManager.AppSettings.Get("BuyerRequestCount")) : "50"; }
        }

        public static int TemparoryUrlExpiration
        {
            get { return WebConfigurationManager.AppSettings.Get("TemparoryUrlExpiration") != null ? Convert.ToInt32((WebConfigurationManager.AppSettings.Get("TemparoryUrlExpiration"))) : 24; }
        }


        //public static string ConnectionString
        //{
        //    get { return WebConfigurationManager.AppSettings.Get("ConnectionString") != null ? ((WebConfigurationManager.AppSettings.Get("ConnectionString"))) : @"Data Source=MAHAKJAIN-RM\SQLEXPRESS;Initial Catalog=SIMTrax_Latest;User Id=sa;Password=rare123;MultipleActiveResultSets=True"; }
        //}
        public static string ConnectionString
        {
            get { return WebConfigurationManager.AppSettings.Get("ConnectionString") != null ? ((WebConfigurationManager.AppSettings.Get("ConnectionString"))) : @"Data Source=SHUBHAMR_RM\SQLEXPRESS_2014;Initial Catalog=SIMTrax_Latest;User Id=sa;Password=rare123;MultipleActiveResultSets=True"; }
        }

    }
}
