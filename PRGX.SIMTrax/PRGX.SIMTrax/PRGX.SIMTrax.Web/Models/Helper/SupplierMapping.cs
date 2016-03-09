using PRGX.SIMTrax.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PRGX.SIMTrax.Web.Models.Helper
{
    public static class SupplierMapping
    {
        public static ProfileGeneralInformation Mapping(this Profile profileModel)
        {
            var company = new ProfileGeneralInformation();
            company.SupplierPartyId = profileModel.SellerPartyId;
            company.SIMId = "SIM-" + profileModel.SellerPartyId;
            company.CompanyName = profileModel.CompanyName;
            company.NoOfEmployess = profileModel.CompanySize;
            company.TurnOver = profileModel.TurnOver;
            company.WebsiteLink = profileModel.WebsiteLink;
            company.TradingName = profileModel.TradingName;
            company.BusinessSector= profileModel.BusinessSector;
            company.CustomerSector = profileModel.CustomerSectors;
            company.ServiceIn = profileModel.CompanyService;
            company.Subsidiaries = profileModel.CompanySubsidiaries;
            company.TwitterAccount = profileModel.TwitterAccount;
            company.LinkeldinAccount = profileModel.LinkeldInAccount;
            company.FacebookAccount = profileModel.FacebookAccount;
            company.MaxContract = profileModel.CurrencyCodeHtml + " " + profileModel.MaxContractValue;
            company.MinContract = profileModel.CurrencyCodeHtml + " " + profileModel.MinContractValue;
            company.BusinessDescription = profileModel.BusinessDescription;
                return company;
        }
    }
}