using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.Domain.Util;
using PRGX.SIMTrax.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.ServiceFacade.Mapper
{
    public static class CampaignMapper
    {
        public static Campaign ToViewModel(this BuyerCampaign campaignPM)
        {
            var campaign = new Campaign();
            campaign.CampaignId = campaignPM.Id;
            campaign.BuyerId = campaignPM.RefBuyer;
            campaign.CampaignName = campaignPM.CampaignName;
            campaign.CampaignType = (CampaignType)campaignPM.CampaignType;
            campaign.CampaignUrl = campaignPM.CampaignUrl;
            campaign.EmailContent = campaignPM.EmailTemplate != null ? campaignPM.EmailTemplate.Content : string.Empty;
            campaign.EmailTemplateId = campaignPM.RefEmailTemplate;
            campaign.CampaignStatus = (CampaignStatus)campaignPM.CampaignStatus;
            campaign.Notes = campaignPM.Notes;
            campaign.SupplierCount = campaignPM.SupplierCount.HasValue ? campaignPM.SupplierCount.Value : 0;
            campaign.VoucherId = campaignPM.RefDiscountVoucher;
            campaign.WelcomeMessage = campaignPM.CampaignMessages.FirstOrDefault() != null ? campaignPM.CampaignMessages.FirstOrDefault().WelcomeMessage : string.Empty;
            if (campaignPM.CampaignMessages.FirstOrDefault() != null)
            {
                campaign.CampaignMessageId = campaignPM.CampaignMessages.FirstOrDefault().Id;
            }
            campaign.MasterVendor = campaignPM.Buyer != null ? campaignPM.Buyer.MaxCampaignSupplierCount : 0;
            campaign.AuditorId = campaignPM.RefAuditorId;
            campaign.TemplateType = (CampaignLandingTemplate)campaignPM.TemplateType;
            campaign.DataSource = campaignPM.DataSource;
            if (campaignPM.RefCampaignLogo.HasValue)
            {
                campaign.CampaignLogoFileName = campaignPM.Document.FileName;
                campaign.CampaignLogoFilePath =  campaignPM.Document.FilePath;
                campaign.CampaignLogoDocumentId = campaignPM.Document.Id;
            }
            campaign.BuyerOrganisation = campaignPM.Buyer != null ? campaignPM.Buyer.Organization.Party.PartyName : string.Empty;
            return campaign;
        }

        public static BuyerCampaign ToEntityModel(this Campaign campaignModel, ViewModel.Document documentLogo, long auditorUserId)
        {
            var campaignMessages = new List<CampaignMessage>();
            if (!string.IsNullOrWhiteSpace(campaignModel.WelcomeMessage))
            {
                campaignMessages.Add(new CampaignMessage() {
                    WelcomeMessage = campaignModel.WelcomeMessage
                });
            }
            var campaignPM = new BuyerCampaign()
            {
                CampaignName = campaignModel.CampaignName,
                CampaignStatus = (long)campaignModel.CampaignStatus,
                CampaignType = (long)campaignModel.CampaignType,
                CampaignUrl = campaignModel.CampaignUrl,
                DataSource = campaignModel.DataSource,
                Notes = campaignModel.Notes,
                RefAuditorId = campaignModel.AuditorId,
                RefBuyer = campaignModel.BuyerId,
                TemplateType = campaignModel.TemplateType.HasValue ? (long?)campaignModel.TemplateType.Value : null,
                SupplierCount = campaignModel.SupplierCount,
                CampaignMessages = campaignMessages,
                RefDiscountVoucher = campaignModel.VoucherId,
                RefCreatedBy = auditorUserId,
                RefLastUpdatedBy = auditorUserId,
            };
            if (!string.IsNullOrWhiteSpace(documentLogo.FileName))
            {
                campaignPM.Document = new DAL.Entity.Document()
                {
                    ContentLength = documentLogo.ContentLength,
                    ContentType = documentLogo.ContentType,
                    FileName = documentLogo.FileName,
                    FilePath = documentLogo.FilePath,
                };
            }
            var emailTemplate = new DAL.Entity.EmailTemplate();
            if (!string.IsNullOrWhiteSpace(campaignModel.EmailContent))
            {
                campaignPM.EmailTemplate = new DAL.Entity.EmailTemplate()
                {
                    Mnemonic = Constants.CAMPAIGN_BUYER_CONTENT,
                    Content = campaignModel.EmailContent,
                    Subject = string.Empty,
                };
            }
            return campaignPM;
        }

        public static List<Campaign> ToBuyerDashboardViewModel(this List<BuyerCampaign> campaignListPM)
        {
            return campaignListPM.Select(x => new Campaign() {
                CampaignId = x.Id,
                CampaignName = x.CampaignName,
                AuditorId = x.RefAuditorId.HasValue ? x.RefAuditorId.Value : 0,
                BuyerId = x.RefBuyer.HasValue ? x.RefBuyer.Value : 0,
                CampaignStatus = (CampaignStatus)x.CampaignStatus,
                CampaignType = (CampaignType)x.CampaignType
            }).ToList();
        }

        public static List<Campaign> ToAuditorHomeViewModel(this List<BuyerCampaign> campaignListPM)
        {
            return campaignListPM.Select(x => new Campaign()
            {
                CampaignId = x.Id,
                CampaignName = x.CampaignName,
                AuditorId = x.RefAuditorId.HasValue ? x.RefAuditorId.Value : 0,
                BuyerId = x.RefBuyer.HasValue ? x.RefBuyer.Value : 0,
                CampaignStatus = (CampaignStatus)x.CampaignStatus,
                CampaignType = (CampaignType)x.CampaignType,
                BuyerOrganisation = x.Buyer.Organization.Party.PartyName,
                SupplierCount = x.SupplierCount.HasValue ? x.SupplierCount.Value : 0,
                AssignedToAuditor = x.User != null ? x.User.Person.Party.PartyName : string.Empty,
                CreatedDate = x.CreatedOn,
                MasterVendor = x.Buyer != null ? (x.Buyer.MaxCampaignSupplierCount.HasValue ? x.Buyer.MaxCampaignSupplierCount.Value : 0) : 0,
                IsDownloaded = x.IsDownloaded.HasValue ? x.IsDownloaded.Value : false
            }).ToList();
        }
    }
}
