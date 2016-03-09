using PRGX.SIMTrax.Domain.Util;
using PRGX.SIMTrax.ServiceFacade;
using PRGX.SIMTrax.ServiceFacade.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace PRGX.SIMTrax.Web.Filters
{
    public class PreCampaignRouteConstraint : IRouteConstraint
    {
        private readonly ICampaignServiceFacade _campaignServiceFacade;

        public PreCampaignRouteConstraint()
        {
            _campaignServiceFacade = new CampaignServiceFacade();
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (values[parameterName] != null && !values[parameterName].ToString().Contains("/"))
            {
                var permalink = values[parameterName].ToString();
                return _campaignServiceFacade.GetCampaignUrlSpecificForBuyer(permalink, CampaignType.PreRegistered);
            }
            else
            {
                return false;
            }
        }
    }

    public class CampaignRouteConstraint : IRouteConstraint
    {
        private readonly ICampaignServiceFacade _campaignServiceFacade;

        public CampaignRouteConstraint()
        {
            _campaignServiceFacade = new CampaignServiceFacade();
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (values[parameterName] != null && !values[parameterName].ToString().Contains("/"))
            {
                var permalink = values[parameterName].ToString();
                return _campaignServiceFacade.GetCampaignUrlSpecificForBuyer(permalink, CampaignType.NotRegistered);
            }
            else
            {
                return false;
            }
        }
    }
}