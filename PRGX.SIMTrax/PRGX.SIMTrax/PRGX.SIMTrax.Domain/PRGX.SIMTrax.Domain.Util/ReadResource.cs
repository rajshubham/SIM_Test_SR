using System;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace PRGX.SIMTrax.Domain.Util
{

    public class ReadResource
    {
        #region Variable Declaration
        static System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");
        static ResourceManager resourceManagerEmailConstant = new ResourceManager(Constants.RESOURCE_EMAIL_CONSTANTS, Assembly.Load(@"PRGX.SIMTrax.Domain.Resource"));
        static ResourceManager globalizationConstants = new ResourceManager(Constants.RESOURCE_INTERNATIONALISATION, Assembly.Load(@"PRGX.SIMTrax.Domain.Resource"));
        static ResourceManager resourceManagerSimTraxConstants = new ResourceManager(Constants.RESOURCE_SIMTRAX_CONSTANTS, Assembly.Load(@"PRGX.SIMTrax.Domain.Resource"));

        #endregion

        public static string GetResource(string sMsgCode, ResourceType resourceType)
        {
            string resourceValue = string.Empty;
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = ci;
                System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
                switch (resourceType)
                {
                    case ResourceType.Email:
                        resourceValue = resourceManagerEmailConstant.GetString(sMsgCode, ci);
                        break;
                    case ResourceType.Constants:
                        resourceValue = resourceManagerSimTraxConstants.GetString(sMsgCode, ci);
                        break;
                    default:
                        resourceValue = sMsgCode;
                        break;

                }
            }
            catch
            {
                //TODO :: use default message or no message;
                resourceValue = "";
            }
            return resourceValue;
        }
        public static string GetResourceForGlobalization(string sMsgCode, CultureInfo cir)
        {
            string resourceValue = string.Empty;
            try
            {

                resourceValue = globalizationConstants.GetString(sMsgCode, cir);

            }
            catch
            {
                //TODO :: use default message or no message;
                resourceValue = "";
            }
            return resourceValue;
        }

    }
}
