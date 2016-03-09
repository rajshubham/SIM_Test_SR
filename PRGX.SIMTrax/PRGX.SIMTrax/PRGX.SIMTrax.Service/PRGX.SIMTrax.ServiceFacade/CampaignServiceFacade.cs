using PRGX.SIMTrax.DAL;
using PRGX.SIMTrax.DAL.Abstract;
using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.Domain.Util;
using PRGX.SIMTrax.ServiceFacade.Abstract;
using PRGX.SIMTrax.ServiceFacade.Mapper;
using PRGX.SIMTrax.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.ServiceFacade
{
    public class CampaignServiceFacade : ICampaignServiceFacade
    {
        private readonly ICampaignUow _campaignUow;

        public CampaignServiceFacade()
        {
            _campaignUow = new CampaignUow();
        }
        
        public Campaign GetCampaignInfo(long campaignId)
        {
            try
            {
                Logger.Info("CampaignServiceFacade : GetCampaignInfo() : Enter the method");

                var campaign = new Campaign();
                var buyerCampaign = _campaignUow.BuyerCampaigns.GetCampaignInfo(campaignId);

                if (null != buyerCampaign)
                {
                    campaign = buyerCampaign.ToViewModel();
                }

                Logger.Info("CampaignServiceFacade : GetCampaignInfo() : Exit the method");
                return campaign;
            }
            catch (Exception ex)
            {
                Logger.Error("CampaignServiceFacade : GetCampaignInfo() : Caught an exception" + ex);
                throw;
            }
        }

        public bool AddOrUpdateBuyerCampaign(Campaign campaignModel, ViewModel.Document documentLogo, long auditorId)
        {
            Logger.Info("CampaignServiceFacade : AddOrUpdateBuyerCampaign() : Enter the method");
            ICampaignUow _campaignUow = null;
            var result = false;
            try
            {
                _campaignUow = new CampaignUow();
                _campaignUow.BeginTransaction();

                if (campaignModel.BuyerId.HasValue)
                {
                    var buyer = _campaignUow.Buyers.GetById(campaignModel.BuyerId.Value);
                    if (null != buyer)
                    {
                        buyer.MaxCampaignSupplierCount = campaignModel.MasterVendor;
                        _campaignUow.Buyers.Update(buyer);
                        _campaignUow.SaveChanges();
                    }
                }

                if (campaignModel.CampaignId == 0)
                {
                    var buyerCampaign = campaignModel.ToEntityModel(documentLogo, auditorId);
                    _campaignUow.BuyerCampaigns.Add(buyerCampaign);
                    _campaignUow.SaveChanges();
                }
                else
                {
                    var buyerCampaignPM = _campaignUow.BuyerCampaigns.GetById(campaignModel.CampaignId);
                    if (campaignModel.CampaignLogoDocumentId.HasValue && !string.IsNullOrWhiteSpace(campaignModel.CampaignLogoFileName))
                    {
                        var campaignLogoDocument = _campaignUow.Documents.GetById(campaignModel.CampaignLogoDocumentId.Value);
                        if (null != campaignLogoDocument)
                        {
                            campaignLogoDocument.ContentLength = documentLogo.ContentLength;
                            campaignLogoDocument.ContentType = documentLogo.ContentType;
                            campaignLogoDocument.FileName = documentLogo.FileName;
                            campaignLogoDocument.FilePath = documentLogo.FilePath;
                            _campaignUow.Documents.Update(campaignLogoDocument);
                            _campaignUow.SaveChanges();
                        }
                    }
                    else
                    {
                        var campaignLogoDocument = new DAL.Entity.Document() {
                            ContentLength = documentLogo.ContentLength,
                            ContentType = documentLogo.ContentType,
                            FileName = documentLogo.FileName,
                            FilePath = documentLogo.FilePath
                        };
                        _campaignUow.Documents.Add(campaignLogoDocument);
                        _campaignUow.SaveChanges();

                        buyerCampaignPM.RefCampaignLogo = campaignLogoDocument.Id;
                    }

                    if (!string.IsNullOrWhiteSpace(campaignModel.WelcomeMessage))
                    {
                        if (campaignModel.CampaignMessageId.HasValue)
                        {
                            var campaignMessage = _campaignUow.CampaignMessages.GetById(campaignModel.CampaignMessageId.Value);
                            campaignMessage.WelcomeMessage = campaignModel.WelcomeMessage;
                            _campaignUow.CampaignMessages.Update(campaignMessage);
                            _campaignUow.SaveChanges();
                        }
                        else
                        {
                            var campaignMessage = new CampaignMessage();
                            campaignMessage.WelcomeMessage = campaignModel.WelcomeMessage;
                            campaignMessage.RefCampaign = campaignModel.CampaignId;
                            _campaignUow.CampaignMessages.Add(campaignMessage);
                            _campaignUow.SaveChanges();
                        }
                    }
                    else if (campaignModel.CampaignMessageId.HasValue && string.IsNullOrWhiteSpace(campaignModel.WelcomeMessage))
                    {
                        var campaignMessage = _campaignUow.CampaignMessages.GetById(campaignModel.CampaignMessageId.Value);
                        _campaignUow.CampaignMessages.Delete(campaignMessage);
                        _campaignUow.SaveChanges();
                    }

                    if (!string.IsNullOrWhiteSpace(campaignModel.EmailContent))
                    {
                        if (campaignModel.EmailTemplateId.HasValue)
                        {
                            var campaignEmailTemplate = _campaignUow.EmailTemplates.GetById(campaignModel.EmailTemplateId.Value);
                            campaignEmailTemplate.Content = campaignModel.EmailContent;
                            _campaignUow.EmailTemplates.Update(campaignEmailTemplate);
                            _campaignUow.SaveChanges();
                        }
                        else
                        {
                            var campaignEmailTemplate = new DAL.Entity.EmailTemplate();
                            campaignEmailTemplate.Mnemonic = Constants.CAMPAIGN_BUYER_CONTENT;
                            campaignEmailTemplate.Content = campaignModel.EmailContent;
                            campaignEmailTemplate.Subject = string.Empty;
                            _campaignUow.EmailTemplates.Add(campaignEmailTemplate);
                            _campaignUow.SaveChanges();

                            buyerCampaignPM.RefEmailTemplate = campaignEmailTemplate.Id;
                        }
                    }
                    else if (campaignModel.EmailTemplateId.HasValue && string.IsNullOrWhiteSpace(campaignModel.EmailContent))
                    {
                        var campaignEmailTemplate = _campaignUow.EmailTemplates.GetById(campaignModel.EmailTemplateId.Value);
                        _campaignUow.EmailTemplates.Delete(campaignEmailTemplate);
                        _campaignUow.SaveChanges();
                    }

                    buyerCampaignPM.CampaignName = campaignModel.CampaignName;
                    buyerCampaignPM.CampaignType = (long)campaignModel.CampaignType;
                    buyerCampaignPM.CampaignUrl = campaignModel.CampaignUrl;
                    buyerCampaignPM.DataSource = campaignModel.DataSource;
                    buyerCampaignPM.Notes = campaignModel.Notes;
                    buyerCampaignPM.TemplateType = campaignModel.TemplateType.HasValue ? (long?)campaignModel.TemplateType.Value : null;
                    buyerCampaignPM.SupplierCount = campaignModel.SupplierCount;
                    buyerCampaignPM.RefDiscountVoucher = campaignModel.VoucherId;
                    buyerCampaignPM.RefLastUpdatedBy = auditorId;
                    _campaignUow.BuyerCampaigns.Update(buyerCampaignPM);
                    _campaignUow.SaveChanges();
                }

                _campaignUow.Commit();
                result = true;

                Logger.Info("CampaignServiceFacade : AddOrUpdateBuyerCampaign() : Exit the method");
                return result;
            }
            catch (Exception ex)
            {
                if (_campaignUow != null)
                    _campaignUow.Rollback();
                Logger.Error("CampaignServiceFacade : AddOrUpdateBuyerCampaign() : Caught an exception" + ex);
                throw;
            }
        }

        public List<Campaign> GetBuyerCampaignDetailsForDashboard(out int totalCampaigns, long partyId, int pageNumber, int sortDirection)
        {
            try
            {
                Logger.Info("CampaignServiceFacade : GetBuyerCampaignDetailsForDashboard() : Enter the method");
                Logger.Info("CampaignServiceFacade : GetBuyerCampaignDetailsForDashboard() : Exit the method");

                return _campaignUow.Buyers.GetBuyerCampaignDetailsForDashboard(out totalCampaigns, partyId, pageNumber, sortDirection).ToBuyerDashboardViewModel();
            }
            catch (Exception ex)
            {
                Logger.Error("CampaignServiceFacade : GetBuyerCampaignDetailsForDashboard() : Caught an exception" + ex);
                throw;
            }
        }

        public bool AssignCampaignToAuditor(long auditorId, int campaignId, long loggedInAuditor)
        {
            ICampaignUow _campaignUow = null;
            try
            {
                Logger.Info("CampaignServiceFacade : AssignCampaignToAuditor() : Entering the method.");

                _campaignUow = new CampaignUow();
                var result = false;
                _campaignUow.BeginTransaction();

                var campaign = _campaignUow.BuyerCampaigns.GetById(campaignId);
                if (null != campaign)
                {
                    campaign.RefAuditorId = auditorId;
                    campaign.CampaignStatus = (long)CampaignStatus.Assigned;
                    campaign.RefLastUpdatedBy = loggedInAuditor;
                    _campaignUow.BuyerCampaigns.Update(campaign);
                    _campaignUow.SaveChanges();
                    _campaignUow.Commit();
                    result = true;
                }
                Logger.Info("CampaignServiceFacade : AssignCampaignToAuditor() : Exiting the method.");
                return result;
            }
            catch (Exception ex)
            {
                if (null != _campaignUow)
                    _campaignUow.Rollback();
                Logger.Error("CampaignServiceFacade : AssignCampaignToAuditor() : Caught an exception" + ex);
                throw ex;
            }
        }

        public List<Campaign> GetAssignedCampaigns(long auditorId, int index, out int total)
        {
            try
            {
                Logger.Info("CampaignServiceFacade : GetAssignedCampaigns() : Enter the method");
                Logger.Info("CampaignServiceFacade : GetAssignedCampaigns() : Exit the method");

                return _campaignUow.BuyerCampaigns.GetAssignedCampaigns(auditorId, index, out total).ToAuditorHomeViewModel();
            }
            catch (Exception ex)
            {
                Logger.Error("CampaignServiceFacade : GetAssignedCampaigns() : Caught an exception" + ex);
                throw;
            }
        }

        public List<Campaign> GetCampaignsAwaitingAction(out int total, int PageNo, long auditorId)
        {
            try
            {
                Logger.Info("CampaignServiceFacade : GetCampaignsAwaitingAction() : Enter the method");
                Logger.Info("CampaignServiceFacade : GetCampaignsAwaitingAction() : Exit the method");

                return _campaignUow.BuyerCampaigns.GetCampaignsAwaitingAction(out total, PageNo, auditorId).ToAuditorHomeViewModel();
            }
            catch (Exception ex)
            {
                Logger.Error("CampaignServiceFacade : GetCampaignsAwaitingAction() : Caught an exception" + ex);
                throw;
            }
        }

        public bool UpdateCampaignPreRegFilePath(long campaignId, string filePath)
        {
            ICampaignUow _campaignUow = null;
            try
            {
                Logger.Info("CampaignServiceFacade : UpdateCampaignPreRegFilePath() : Entering the method.");
                var result = false;
                _campaignUow = new CampaignUow();

                _campaignUow.BeginTransaction();
                var buyerCampaign = _campaignUow.BuyerCampaigns.GetById(campaignId);
                if (null != buyerCampaign)
                {
                    buyerCampaign.PreRegisteredFilePath = filePath;
                    _campaignUow.BuyerCampaigns.Update(buyerCampaign);
                    _campaignUow.SaveChanges();
                }
                _campaignUow.Commit();
                result = true;

                Logger.Info("CampaignServiceFacade : UpdateCampaignPreRegFilePath() : Exiting the method.");
                return result;
            }
            catch (Exception ex)
            {
                if (null != _campaignUow)
                    _campaignUow.Rollback();
                Logger.Error("CampaignServiceFacade : UpdateCampaignPreRegFilePath() : Caught an exception" + ex);
                throw ex;
            }
        }

        public bool InsertListOfPreRegSuppliers(List<CampaignPreRegSupplier> campaignPreRegSupplierDomain, long auditorId)
        {
            ICampaignUow _campaignUow = null;
            try
            {
                Logger.Info("CampaignServiceFacade : InsertListOfPreRegSuppliers() : Entering the method.");
                _campaignUow = new CampaignUow();
                var result = false;
                _campaignUow.BeginTransaction();

                result = _campaignUow.CampaignInvitations.InsertListOfPreRegSupplier(campaignPreRegSupplierDomain.MappingToDBModel(auditorId));
                _campaignUow.SaveChanges();

                _campaignUow.Commit();

                Logger.Info("CampaignServiceFacade : InsertListOfPreRegSuppliers() : Exiting the method.");
                return result;
            }
            catch (Exception ex)
            {
                if (null != _campaignUow)
                    _campaignUow.Rollback();
                Logger.Error("CampaignServiceFacade : UpdateListOfPreRegSupplier() : Caught an exception" + ex);
                throw ex;
            }
        }

        public List<CampaignPreRegSupplier> GetPreRegSupplierInCampaign(int campaignId, string filterCriteria, out int total, int pageIndex, int size)
        {
            try
            {
                Logger.Info("CampaignServiceFacade : GetPreRegSupplierInCampaign() : Entering the method.");
                Logger.Info("CampaignServiceFacade : GetPreRegSupplierInCampaign() : Exiting the method.");
                return _campaignUow.CampaignInvitations.GetPreRegSupplierInCampaign(campaignId, filterCriteria, out total, pageIndex, size).MappingToDomainModel();
            }
            catch (Exception ex)
            {
                Logger.Error("CampaignServiceFacade : GetPreRegSupplierInCampaign() : Caught an exception" + ex);
                throw ex;
            }
        }

        public bool UpdateListOfPreRegSupplier(List<CampaignPreRegSupplier> campaignPreRegSupplierList, long auditorId)
        {
            ICampaignUow _campaignUow = null;
            Logger.Info("CampaignServiceFacade : UpdateListOfPreRegSupplier() : Entering the method");
            try
            {
                Logger.Info("CampaignServiceFacade : UpdateListOfPreRegSupplier() : Exiting the method");
                var result = false;
                _campaignUow = new CampaignUow();
                _campaignUow.BeginTransaction();

                result = _campaignUow.CampaignInvitations.UpdateListOfPreRegSupplier(campaignPreRegSupplierList.MappingToDBModel(auditorId));
                _campaignUow.SaveChanges();

                _campaignUow.Commit();

                return result;
            }
            catch (Exception ex)
            {
                if (null != _campaignUow)
                    _campaignUow.Rollback();
                Logger.Error("CampaignServiceFacade : UpdateListOfPreRegSupplier() : Caught an exception" + ex);
                throw ex;
            }
        }

        public bool DeletePreRegSupplier(long preRegSupplierId)
        {
            ICampaignUow _campaignUow = null;
            Logger.Info("CampaignServiceFacade : DeletePreRegSupplier() : Entering the method");
            try
            {
                Logger.Info("CampaignServiceFacade : DeletePreRegSupplier() : Exiting the method");
                var result = false;
                _campaignUow = new CampaignUow();
                _campaignUow.BeginTransaction();

                var campaignInvitation = _campaignUow.CampaignInvitations.GetById(preRegSupplierId);
                if (null != campaignInvitation)
                {
                    _campaignUow.CampaignInvitations.Delete(campaignInvitation);
                    _campaignUow.SaveChanges();
                }
                _campaignUow.Commit();

                result = true;
                return result;
            }
            catch (Exception ex)
            {
                if (null != _campaignUow)
                    _campaignUow.Rollback();
                Logger.Error("CampaignServiceFacade : DeletePreRegSupplier() : Caught an exception" + ex);
                throw ex;
            }
        }

        public bool DeleteInvalidPreRegSupplierBasedOnCampaign(int campaignId)
        {
            ICampaignUow _campaignUow = null;
            Logger.Info("CampaignServiceFacade : DeleteInvalidPreRegSupplierBasedOnCampaign() : Entering the method");
            var result = false;
            try
            {
                _campaignUow = new CampaignUow();
                _campaignUow.BeginTransaction();

                var inValidPreRegSuppliers = _campaignUow.CampaignInvitations.GetAll().Where(item => item.RefCampaign == campaignId && item.IsValid == false).ToList();
                _campaignUow.CampaignInvitations.DeleteRange(inValidPreRegSuppliers);
                _campaignUow.SaveChanges();
                _campaignUow.Commit();
                result = true;
            }
            catch (Exception ex)
            {
                if (null != _campaignUow)
                    _campaignUow.Rollback();
                Logger.Error("CampaignServiceFacade : DeleteInvalidPreRegSupplierBasedOnCampaign() : Caught an exception" + ex);
                throw ex;
            }
            Logger.Info("CampaignServiceFacade : DeleteInvalidPreRegSupplierBasedOnCampaign() : Exiting the method");
            return result;
        }

        public bool UpdateCampaignStatus(long campaignId, short campaignStatus)
        {
            ICampaignUow _campaignUow = null;
            try
            {
                Logger.Info("CampaignServiceFacade : UpdateCampaignStatus() : Entering the method.");
                var result = false;
                _campaignUow = new CampaignUow();
                _campaignUow.BeginTransaction();

                var campaign = _campaignUow.BuyerCampaigns.GetById(campaignId);
                if (null != campaign)
                {
                    campaign.CampaignStatus = campaignStatus;
                    _campaignUow.BuyerCampaigns.Update(campaign);
                    _campaignUow.SaveChanges();
                    _campaignUow.Commit();
                    result = true;
                }
                Logger.Info("CampaignServiceFacade : UpdateCampaignStatus() : Exiting the method.");
                return result;
            }
            catch (Exception ex)
            {
                if (null != _campaignUow)
                    _campaignUow.Rollback();
                Logger.Error("CampaignServiceFacade : UpdateCampaignStatus() : Caught an exception" + ex);
                throw ex;
            }
        }

        public bool UpdateCampaignSupplierCount(int campaignId)
        {
            ICampaignUow _campaignUow = null;
            Logger.Info("CampaignServiceFacade : UpdateCampaignSupplierCount() : Entering the method");
            var result = false;
            try
            {
                _campaignUow = new CampaignUow();
                _campaignUow.BeginTransaction();

                var uploadedSupplierCount = _campaignUow.CampaignInvitations.GetAll().Count(elem => elem.RefCampaign == campaignId);

                var campaign = _campaignUow.BuyerCampaigns.GetById(campaignId);
                if (campaign != null)
                {
                    campaign.SupplierCount = uploadedSupplierCount;
                    _campaignUow.BuyerCampaigns.Update(campaign);
                    _campaignUow.SaveChanges();
                    _campaignUow.Commit();

                    result = true;
                }
            }
            catch (Exception ex)
            {
                if (null != _campaignUow)
                    _campaignUow.Rollback();
                Logger.Error("CampaignServiceFacade : UpdateCampaignSupplierCount() : Caught an exception" + ex);
                throw ex;
            }
            Logger.Info("CampaignServiceFacade : UpdateCampaignSupplierCount() : Exiting the method");
            return result;
        }

        public List<Campaign> GetSubmittedOrApprovedCampaigns(short campaignStatus, int index, out int total)
        {
            try
            {
                Logger.Info("CampaignServiceFacade : GetSubmittedOrApprovedCampaigns() : Enter the method");
                Logger.Info("CampaignServiceFacade : GetSubmittedOrApprovedCampaigns() : Exit the method");

                return _campaignUow.BuyerCampaigns.GetSubmittedOrApprovedCampaigns(campaignStatus, index, out total).ToAuditorHomeViewModel();
            }
            catch (Exception ex)
            {
                Logger.Error("CampaignServiceFacade : GetSubmittedOrApprovedCampaigns() : Caught an exception" + ex);
                throw;
            }
        }

        public bool CompareSupplierCountAndMasterVendor(int campaignId)
        {
            Logger.Info("CampaignServiceFacade : CompareSupplierCountAndMasterVendor() : Entering the method");
            var result = false;
            try
            {
                var campaign = _campaignUow.BuyerCampaigns.GetById(campaignId);
                if (null != campaign && campaign.RefBuyer.HasValue)
                {
                    var masterVendor = 0;
                    var supplierCount = _campaignUow.BuyerCampaigns.GetAll().Where(elem => elem.RefBuyer == campaign.RefBuyer.Value).Sum(elem => elem.SupplierCount);
                    var buyer = _campaignUow.Buyers.GetById(campaign.RefBuyer.Value);
                    if (buyer != null)
                    {
                        masterVendor = buyer.MaxCampaignSupplierCount.HasValue ? buyer.MaxCampaignSupplierCount.Value : 0;
                    }
                    result = supplierCount <= masterVendor ? true : false;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("CampaignServiceFacade : CompareSupplierCountAndMasterVendor() : Caught an exception" + ex);
                throw;
            }
            Logger.Info("CampaignServiceFacade : CompareSupplierCountAndMasterVendor() : Exiting the method");
            return result;
        }

        public List<CampaignReleaseSupplier> GetPreRegSupplierListwithPasswordString(long campaignId)
        {
            Logger.Info("CampaignServiceFacade : GetPreRegSupplierListwithPasswordString() : Entering the method");
            var supplierWithPasswordList = new List<CampaignReleaseSupplier>();
            Random random = new Random();
            ICampaignUow _campaignUow = null;
            try
            {
                _campaignUow = new CampaignUow();
                _campaignUow.BeginTransaction();

                var supplierList = _campaignUow.CampaignInvitations.GetPreRegSupplierListwithPasswordString(campaignId);
                foreach (var supplier in supplierList)
                {
                    var supplierWithPassword = new CampaignReleaseSupplier();
                    supplierWithPassword.BuyerOrganisation = supplier.BuyerCampaign.Buyer.Organization.Party.PartyName;
                    supplierWithPassword.SupplierName = supplier.SupplierCompanyName;
                    supplierWithPassword.LoginId = supplier.EmailAddress;
                    var url = CommonMethods.GetBaseUrl();
                    supplierWithPassword.CampaignUrl = url + supplier.BuyerCampaign.CampaignUrl;
                    var domainSupplier = !string.IsNullOrWhiteSpace(supplier.EmailAddress) && supplier.EmailAddress.Contains('@') ? supplier.EmailAddress.Substring(supplier.EmailAddress.IndexOf('@')) : string.Empty;
                    var passwordString = CommonMethods.GetUniqueKey(random);
                    supplierWithPassword.RandomPasswordString = passwordString;
                    var encryptedRandomPassword = CommonMethods.EncryptMD5Password(domainSupplier.ToLower() + passwordString.Trim());

                    supplier.RegistrationCode = encryptedRandomPassword;
                    _campaignUow.CampaignInvitations.Update(supplier);
                    _campaignUow.SaveChanges();

                    supplierWithPasswordList.Add(supplierWithPassword);
                }
                var campaign = _campaignUow.BuyerCampaigns.GetById(campaignId);
                if(null != campaign)
                {
                    campaign.IsDownloaded = true;
                    _campaignUow.BuyerCampaigns.Update(campaign);
                    _campaignUow.SaveChanges();
                }
                _campaignUow.Commit();
            }
            catch (Exception ex)
            {
                if (null != _campaignUow)
                    _campaignUow.Rollback();
                Logger.Error("CampaignServiceFacade : GetPreRegSupplierListWithPasswordString() : Caught an exception" + ex);
                throw;
            }
            Logger.Info("CampaignServiceFacade : GetPreRegSupplierListwithPasswordString() : Exiting the method");
            return supplierWithPasswordList;
        }

        public bool UpdatePreRegSupplierPassword(long preRegSupplierId, string password)
        {
            Logger.Info("CampaignServiceFacade : UpdatePreRegSupplierPassword() : Entering the method");
            bool result = false;
            ICampaignUow _campaignUow = null;
            try
            {
                _campaignUow = new CampaignUow();
                _campaignUow.BeginTransaction();

                var supplier = _campaignUow.CampaignInvitations.GetAll().FirstOrDefault(elem => elem.Id == preRegSupplierId);
                if (supplier != null)
                {
                    supplier.RegistrationCode = password;
                    _campaignUow.CampaignInvitations.Update(supplier);
                    _campaignUow.SaveChanges();
                    _campaignUow.Commit();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                if (null != _campaignUow)
                    _campaignUow.Rollback();
                Logger.Error("CampaignServiceFacade : UpdatePreRegSupplierPassword() : Caught an Error" + ex);
                throw;
            }
            Logger.Info("CampaignServiceFacade : UpdatePreRegSupplierPassword() : Exiting the method");
            return result;
        }

        public bool UpdateCampaignDownloadStatus(long campaignId, bool isDownloaded)
        {
            Logger.Info("CampaignServiceFacade : UpdateCampaignDownloadStatus() : Entering the method");
            var result = false;
            ICampaignUow _campaignUow = null;
            try
            {
                _campaignUow = new CampaignUow();
                _campaignUow.BeginTransaction();
                
                var campaign = _campaignUow.BuyerCampaigns.GetById(campaignId);
                if (null != campaign)
                {
                    campaign.IsDownloaded = isDownloaded;
                    _campaignUow.BuyerCampaigns.Update(campaign);
                    _campaignUow.SaveChanges();
                }
                _campaignUow.Commit();
                result = true;
            }
            catch (Exception ex)
            {
                if (null != _campaignUow)
                    _campaignUow.Rollback();
                Logger.Error("CampaignServiceFacade : UpdateCampaignDownloadStatus() : Caught an exception" + ex);
                throw;
            }
            Logger.Info("CampaignServiceFacade : UpdateCampaignDownloadStatus() : Exiting the method");
            return result;
        }

        public bool GetCampaignUrlSpecificForBuyer(string link, CampaignType campaignType)
        {
            Logger.Info("CampaignServiceFacade : GetCampaignUrlSpecificForBuyer() : Entering the method");
            var result = false;
            int released = (int)CampaignStatus.Release;
            try
            {
                var campaign = _campaignUow.BuyerCampaigns.GetAll().Where(elem => elem.CampaignUrl == link).FirstOrDefault();

                if (campaign != null && (short)campaignType == campaign.CampaignType)
                {
                    if (campaignType == CampaignType.PreRegistered && campaign.CampaignStatus == released)
                    {
                        result = true;
                    }
                    else if (campaignType == CampaignType.NotRegistered)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("CampaignServiceFacade : GetCampaignUrlSpecificForBuyer() : Caught an exception" + ex);
                throw ex;
            }
            Logger.Info("CampaignServiceFacade : GetCampaignUrlSpecificForBuyer() : Exiting the method");
            return result;
        }

        public Campaign GetCampaignInfoBasedOnCampaignURL(string link)
        {
            Logger.Info("CampaignServiceFacade : GetCampaignInfoBasedOnCampaignURL() : Entering the method");
            var campaign = new Campaign();
            try
            {
                var campaignPM = _campaignUow.BuyerCampaigns.GetCampaignInfo(0, link);
                if(null != campaignPM)
                {
                    campaign = campaignPM.ToViewModel();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("CampaignServiceFacade : GetCampaignInfoBasedOnCampaignURL() : Caught an exception" + ex);
                throw ex;
            }
            Logger.Info("CampaignServiceFacade : GetCampaignInfoBasedOnCampaignURL() : Exiting the method");
            return campaign;
        }

        public long ValidatePreRegSupplierCode(string loginId, string encryptedPassword, out bool isRegistered)
        {
            Logger.Info("CampaignServiceFacade : ValidatePreRegSupplierCode() : Entering the method");
            long preRegId = 0;
            try
            {
                var preRegSupplier = _campaignUow.CampaignInvitations.GetAll().FirstOrDefault(elem => elem.EmailAddress.Trim().ToLower().Equals(loginId.Trim().ToLower()) && elem.RegistrationCode == encryptedPassword);
                if (null != preRegSupplier)
                {
                    preRegId = preRegSupplier.Id;
                    isRegistered = preRegSupplier.IsRegistered.HasValue ? preRegSupplier.IsRegistered.Value : false;
                }
                else
                {
                    isRegistered = false;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("CampaignServiceFacade : ValidatePreRegSupplierCode() : Caught an exception" + ex);
                throw ex;
            }
            Logger.Info("CampaignServiceFacade : ValidatePreRegSupplierCode() : Exiting the method");
            return preRegId;
        }

        public SellerRegister GetPreRegSupplierDetails(long id)
        {
            Logger.Info("CampaignServiceFacade : GetPreRegSupplierDetails() : Entering the method");
            try
            {
                Logger.Info("CampaignServiceFacade : GetPreRegSupplierDetails() : Exiting the method");

                var supplierRegister = new SellerRegister();
                var preRegSupplier = _campaignUow.CampaignInvitations.GetById(id);
                if(null != preRegSupplier)
                {
                    supplierRegister = preRegSupplier.MappingPreRegToRegisterModel();
                }
                return supplierRegister;
            }
            catch (Exception ex)
            {
                Logger.Error("CampaignServiceFacade : GetPreRegSupplierDetails() : Caught an exception" + ex);
                throw;
            }
        }

        public CampaignPreRegSupplier GetPublicDataRecord(long id)
        {
            Logger.Info("CampaignServiceFacade : GetPublicDataRecord() : Entering the method");
            try
            {
                Logger.Info("CampaignServiceFacade : GetPublicDataRecord() : Exiting the method");

                var supplierRegister = new CampaignPreRegSupplier();
                supplierRegister = _campaignUow.CampaignInvitations.GetCampaignInvitationRecord(id).MappingToDomainModel();

                return supplierRegister;
            }
            catch (Exception ex)
            {
                Logger.Error("CampaignServiceFacade : GetPreRegSupplierDetails() : Caught an exception" + ex);
                throw;
            }
        }

        public bool AddSupplierReferrer(long campaignId, long supplierId, bool landingReferrer)
        {
            Logger.Info("CampaignServiceFacade : AddSupplierReferrer() : Entering the method");
            ICampaignUow _campaignUow = null;
            try
            {
                _campaignUow = new CampaignUow();
                _campaignUow.BeginTransaction();
                var result = false;

                var supplierReferrer = _campaignUow.SupplierReferrers.GetAll().FirstOrDefault(x => x.RefCampaign == campaignId && x.RefSupplier == supplierId && x.LandingReferrer == landingReferrer);
                if (null == supplierReferrer)
                {
                    var referrer = new DAL.Entity.SupplierReferrer() {
                        RefCampaign = campaignId,
                        RefSupplier = supplierId,
                        LandingReferrer = landingReferrer,
                        RefCreatedBy = supplierId
                    };
                    _campaignUow.SupplierReferrers.Add(referrer);
                    _campaignUow.SaveChanges();
                    _campaignUow.Commit();
                }
                result = true;
                Logger.Info("CampaignServiceFacade : AddSupplierReferrer() : Exiting the method");
                return result;
            }
            catch (Exception ex)
            {
                if (null != _campaignUow)
                    _campaignUow.Rollback();
                Logger.Error("CampaignServiceFacade : AddSupplierReferrer() : Caught an exception" + ex);
                throw ex;
            }
        }

        public bool SetPreRegSupplierToRegistered(long id, long supplierId)
        {
            Logger.Info("CampaignServiceFacade : SetPreRegSupplierToRegistered() : Entering the method");
            var result = false;
            ICampaignUow _campaignUow = null;
            try
            {
                _campaignUow = new CampaignUow();
                _campaignUow.BeginTransaction();

                var preRegSupplier = _campaignUow.CampaignInvitations.GetAll().FirstOrDefault(elem => elem.Id == id);
                if (preRegSupplier != null)
                {
                    preRegSupplier.IsRegistered = true;
                    preRegSupplier.RefSupplier = supplierId;
                    _campaignUow.CampaignInvitations.Update(preRegSupplier);
                    _campaignUow.SaveChanges();
                    _campaignUow.Commit();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("CampaignServiceFacade : SetPreRegSupplierToRegistered() : Caught an exception" + ex);
                throw ex;
            }
            Logger.Info("CampaignServiceFacade : SetPreRegSupplierToRegistered() : Exiting the method");
            return result;
        }

        public List<Domain.Model.SupplierReferrer> GetSupplierReferrerBuyerCampaignDetails(int pageNo, string buyerName, string campaignName, long supplierId, out int total)
        {
            Logger.Info("CampaignServiceFacade : GetSupplierReferrerBuyerCampaignDetails() : Entering the method");
            try
            {
                Logger.Info("CampaignServiceFacade : GetSupplierReferrerBuyerCampaignDetails() : Exiting the method");

                var supplierReferrer = new List<Domain.Model.SupplierReferrer>();
                supplierReferrer = _campaignUow.BuyerCampaigns.GetSupplierReferrerBuyerCampaignDetails(pageNo, buyerName, campaignName, supplierId, out total);

                return supplierReferrer;
            }
            catch (Exception ex)
            {
                Logger.Error("CampaignServiceFacade : GetSupplierReferrerBuyerCampaignDetails() : Caught an exception" + ex);
                throw;
            }
        }

        public bool UpdateSupplierReferrerDetails(long supplierId, string[] assignReferrerCampaign, string[] removeReferrerCampaign, string[] primaryReferrerCampaign, long auditorId)
        {
            ICampaignUow _campaignUow = null; 
            try
            {
                Logger.Info("CampaignServiceFacade : UpdateSupplierReferrerDetails() : Enter the method");

                var result = false;
                _campaignUow = new CampaignUow();
                _campaignUow.BeginTransaction();

                if (null != assignReferrerCampaign)
                {
                    foreach (var campaign in assignReferrerCampaign)
                    {
                        var campaignId = Convert.ToInt64(campaign);
                        var referrer = _campaignUow.SupplierReferrers.GetAll().FirstOrDefault(s => s.RefCampaign == campaignId && s.RefSupplier == supplierId);
                        if (referrer == null)
                        {
                            referrer = new DAL.Entity.SupplierReferrer()
                            {
                                RefSupplier = supplierId,
                                RefCampaign = campaignId,
                                LandingReferrer = false,
                                RefCreatedBy = auditorId,
                                RefLastUpdatedBy = auditorId
                            };
                            _campaignUow.SupplierReferrers.Add(referrer);
                            _campaignUow.SaveChanges();
                        }
                        else
                        {
                            referrer.RefSupplier = supplierId;
                            referrer.RefCampaign = campaignId;
                            referrer.LandingReferrer = false;
                            referrer.RefLastUpdatedBy = auditorId;
                            _campaignUow.SupplierReferrers.Update(referrer);
                            _campaignUow.SaveChanges();
                        }
                        var buyerCampaign = _campaignUow.BuyerCampaigns.GetById(campaignId);
                        if (buyerCampaign.RefBuyer.HasValue)
                        {
                            var partyPartyLinkTrading = _campaignUow.PartyPartyLinks.GetAll().FirstOrDefault(v => v.RefParty == buyerCampaign.RefBuyer.Value && v.RefLinkedParty == supplierId && v.PartyPartyLinkType.Trim() == Constants.BUYER_TRADING_SUPPLIER);
                            if (null == partyPartyLinkTrading)
                            {
                                var partyPartyLink = new PartyPartyLink();
                                partyPartyLink.RefParty = supplierId;
                                partyPartyLink.RefLinkedParty = buyerCampaign.RefBuyer.Value;
                                partyPartyLink.PartyPartyLinkType = Constants.BUYER_TRADING_SUPPLIER;
                                partyPartyLink.RefCreatedBy = auditorId;
                                partyPartyLink.RefLastUpdatedBy = auditorId;
                                _campaignUow.PartyPartyLinks.Add(partyPartyLink);
                                _campaignUow.SaveChanges();
                            }
                        }
                    }
                }

                if (null != primaryReferrerCampaign)
                {
                    foreach (var campaign in primaryReferrerCampaign)
                    {
                        var campaignId = Convert.ToInt64(campaign);
                        var landingreferrer = _campaignUow.SupplierReferrers.GetAll().FirstOrDefault(u => u.RefSupplier == supplierId && u.LandingReferrer == true);
                        if (landingreferrer != null)
                        {
                            landingreferrer.LandingReferrer = false;
                            landingreferrer.RefLastUpdatedBy = auditorId;
                            _campaignUow.SupplierReferrers.Update(landingreferrer);
                            _campaignUow.SaveChanges();
                        }
                        var supplier = _campaignUow.SupplierReferrers.GetAll().FirstOrDefault(u => u.RefSupplier == supplierId && u.RefCampaign == campaignId);
                        if (supplier != null)
                        {
                            supplier.LandingReferrer = true;
                            supplier.RefLastUpdatedBy = auditorId;
                            _campaignUow.SupplierReferrers.Update(supplier);
                            _campaignUow.SaveChanges();
                        }
                        else
                        {
                            var referrer = new DAL.Entity.SupplierReferrer()
                            {
                                RefSupplier = supplierId,
                                RefCampaign = campaignId,
                                LandingReferrer = true,
                                RefCreatedBy = auditorId,
                                RefLastUpdatedBy = auditorId
                            };
                            _campaignUow.SupplierReferrers.Add(referrer);
                            _campaignUow.SaveChanges();
                        }
                        var buyerCampaign = _campaignUow.BuyerCampaigns.GetById(campaignId);
                        if (buyerCampaign.RefBuyer.HasValue)
                        {
                            var partyPartyLinkTrading = _campaignUow.PartyPartyLinks.GetAll().FirstOrDefault(v => v.RefParty == buyerCampaign.RefBuyer.Value && v.RefLinkedParty == supplierId && v.PartyPartyLinkType.Trim() == Constants.BUYER_TRADING_SUPPLIER);
                            if (null == partyPartyLinkTrading)
                            {
                                var partyPartyLink = new PartyPartyLink();
                                partyPartyLink.RefParty = supplierId;
                                partyPartyLink.RefLinkedParty = buyerCampaign.RefBuyer.Value;
                                partyPartyLink.PartyPartyLinkType = Constants.BUYER_TRADING_SUPPLIER;
                                partyPartyLink.RefCreatedBy = auditorId;
                                partyPartyLink.RefLastUpdatedBy = auditorId;
                                _campaignUow.PartyPartyLinks.Add(partyPartyLink);
                                _campaignUow.SaveChanges();
                            }
                        }
                    }
                }

                if (null != removeReferrerCampaign)
                {
                    foreach (var campaign in removeReferrerCampaign)
                    {
                        var campaignId = Convert.ToInt64(campaign);
                        var supplier = _campaignUow.SupplierReferrers.GetAll().FirstOrDefault(u => u.RefSupplier == supplierId && u.RefCampaign == campaignId);
                        if (supplier != null)
                        {
                            _campaignUow.SupplierReferrers.Delete(supplier);
                            _campaignUow.SaveChanges();
                        }
                    }
                }
                _campaignUow.Commit();

                result = true;
                Logger.Info("CampaignServiceFacade : UpdateSupplierReferrerDetails() : Exit from method");
                return result;
            }
            catch (Exception ex)
            {
                if (null != _campaignUow)
                    _campaignUow.Rollback();
                Logger.Error("CampaignServiceFacade : UpdateSupplierReferrerDetails() : Caught an exception " + ex);
                throw ex;
            }
        }
    }
}
