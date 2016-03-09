using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.Domain.Util;
using PRGX.SIMTrax.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.ServiceFacade.Mapper
{
    public static class CampaignInvitationMapper
    {
        public static List<CampaignInvitation> MappingToDBModel(this List<CampaignPreRegSupplier> campaignPreRegSupplierDMList, long auditorId)
        {
            List<CampaignInvitation> campaignPreRegSupplierPMList = new List<CampaignInvitation>();
            foreach (var campaignPreRegSupplierDM in campaignPreRegSupplierDMList)
            {
                CampaignInvitation campaignPreRegSupplierPM = campaignPreRegSupplierDM.MappingToDBModel(auditorId);
                campaignPreRegSupplierPMList.Add(campaignPreRegSupplierPM);
            }
            return campaignPreRegSupplierPMList;
        }

        public static List<CampaignPreRegSupplier> MappingToDomainModel(this List<CampaignInvitation> campaignPreRegSupplierPMList)
        {
            List<CampaignPreRegSupplier> campaignPreRegSupplierDMList = new List<CampaignPreRegSupplier>();
            foreach (var campaignPreRegSupplierPM in campaignPreRegSupplierPMList)
            {
                CampaignPreRegSupplier campaignPreRegSupplierDM = campaignPreRegSupplierPM.MappingToDomainModel();
                campaignPreRegSupplierDMList.Add(campaignPreRegSupplierDM);
            }
            return campaignPreRegSupplierDMList;
        }

        public static CampaignInvitation MappingToDBModel(this CampaignPreRegSupplier campaignPreRegSupplier, long auditorId)
        {
            var campaignPreRegSupplierPM = new CampaignInvitation();
            campaignPreRegSupplierPM.Id = campaignPreRegSupplier.PreRegSupplierId;
            campaignPreRegSupplierPM.RefCampaign = campaignPreRegSupplier.CampaignId;
            campaignPreRegSupplierPM.SupplierCompanyName = campaignPreRegSupplier.SupplierName;
            campaignPreRegSupplierPM.EmailAddress = campaignPreRegSupplier.LoginId;
            campaignPreRegSupplierPM.FirstName = campaignPreRegSupplier.FirstName;
            campaignPreRegSupplierPM.LastName = campaignPreRegSupplier.LastName;
            campaignPreRegSupplierPM.JobTitle = campaignPreRegSupplier.JobTitle;
            campaignPreRegSupplierPM.AddressLine1 = campaignPreRegSupplier.AddressLine1;
            campaignPreRegSupplierPM.AddressLine2 = campaignPreRegSupplier.AddressLine2;
            campaignPreRegSupplierPM.City = campaignPreRegSupplier.City;
            campaignPreRegSupplierPM.State = campaignPreRegSupplier.State;
            campaignPreRegSupplierPM.RefCountry = campaignPreRegSupplier.Country;
            campaignPreRegSupplierPM.ZipCode = campaignPreRegSupplier.ZipCode;
            campaignPreRegSupplierPM.Telephone = campaignPreRegSupplier.Telephone;
            campaignPreRegSupplierPM.Identifier1 = campaignPreRegSupplier.RegistrationNumber;
            campaignPreRegSupplierPM.IdentifierType1 = Constants.IDENTIFIER_TYPE_REGISTRATION_NUMBER;
            campaignPreRegSupplierPM.Identifier2 = campaignPreRegSupplier.VatNumber;
            campaignPreRegSupplierPM.IdentifierType2 = Constants.IDENTIFIER_TYPE_VAT_NUMBER;
            campaignPreRegSupplierPM.Identifier3 = campaignPreRegSupplier.DunsNumber;
            campaignPreRegSupplierPM.IdentifierType3 = Constants.IDENTIFIER_TYPE_DUNS_NUMBER;
            campaignPreRegSupplierPM.IsSubsidary = campaignPreRegSupplier.IsSubsidary;
            campaignPreRegSupplierPM.UltimateParent = campaignPreRegSupplier.UltimateParent;
            campaignPreRegSupplierPM.IsValid = campaignPreRegSupplier.IsValid;
            campaignPreRegSupplierPM.InvalidComments = campaignPreRegSupplier.InvalidSupplierComments;
            campaignPreRegSupplierPM.IsRegistered = campaignPreRegSupplier.IsRegistered;
            campaignPreRegSupplierPM.RegistrationCode = campaignPreRegSupplier.Password;
            campaignPreRegSupplierPM.RefCreatedBy = auditorId;
            campaignPreRegSupplierPM.RefLastUpdatedBy = auditorId;
            campaignPreRegSupplierPM.CreatedOn = DateTime.UtcNow;
            campaignPreRegSupplierPM.LastUpdatedOn = DateTime.UtcNow;
            //campaignPreRegSupplierPM.IsDSMapped = campaignPreRegSupplier.IsDSMappedToBuyer;
            //campaignPreRegSupplierPM.IsFITMapped = campaignPreRegSupplier.IsFITMappedToBuyer;
            //campaignPreRegSupplierPM.IsHSMapped = campaignPreRegSupplier.IsHSMappedToBuyer;
            return campaignPreRegSupplierPM;
        }

        public static CampaignPreRegSupplier MappingToDomainModel(this CampaignInvitation campaignPreRegSupplierPM)
        {
            CampaignPreRegSupplier campaignPreRegSupplierDM = new CampaignPreRegSupplier();
            campaignPreRegSupplierDM.PreRegSupplierId = campaignPreRegSupplierPM.Id;
            campaignPreRegSupplierDM.CampaignId = campaignPreRegSupplierPM.RefCampaign;
            campaignPreRegSupplierDM.SupplierName = campaignPreRegSupplierPM.SupplierCompanyName;
            campaignPreRegSupplierDM.LoginId = campaignPreRegSupplierPM.EmailAddress;
            campaignPreRegSupplierDM.FirstName = campaignPreRegSupplierPM.FirstName;
            campaignPreRegSupplierDM.LastName = campaignPreRegSupplierPM.LastName;
            campaignPreRegSupplierDM.JobTitle = campaignPreRegSupplierPM.JobTitle;
            campaignPreRegSupplierDM.AddressLine1 = campaignPreRegSupplierPM.AddressLine1;
            campaignPreRegSupplierDM.AddressLine2 = campaignPreRegSupplierPM.AddressLine2;
            campaignPreRegSupplierDM.City = campaignPreRegSupplierPM.City;
            campaignPreRegSupplierDM.State = campaignPreRegSupplierPM.State;
            campaignPreRegSupplierDM.Country = Convert.ToInt32(campaignPreRegSupplierPM.RefCountry);
            campaignPreRegSupplierDM.CountryName = campaignPreRegSupplierPM.Region.Name;
            campaignPreRegSupplierDM.ZipCode = campaignPreRegSupplierPM.ZipCode;
            campaignPreRegSupplierDM.Telephone = campaignPreRegSupplierPM.Telephone;
            campaignPreRegSupplierDM.RegistrationNumber = campaignPreRegSupplierPM.Identifier1;
            campaignPreRegSupplierDM.VatNumber = campaignPreRegSupplierPM.Identifier2;
            campaignPreRegSupplierDM.DunsNumber = campaignPreRegSupplierPM.Identifier3;
            campaignPreRegSupplierDM.IsSubsidary = campaignPreRegSupplierPM.IsSubsidary.HasValue ? campaignPreRegSupplierPM.IsSubsidary.Value : false;
            campaignPreRegSupplierDM.UltimateParent = !string.IsNullOrWhiteSpace(campaignPreRegSupplierPM.UltimateParent) ? campaignPreRegSupplierPM.UltimateParent : string.Empty;
            campaignPreRegSupplierDM.IsValid = campaignPreRegSupplierPM.IsValid.HasValue ? campaignPreRegSupplierPM.IsValid.Value : false;
            campaignPreRegSupplierDM.InvalidSupplierComments = campaignPreRegSupplierPM.InvalidComments;
            campaignPreRegSupplierDM.IsRegistered = campaignPreRegSupplierPM.IsRegistered.HasValue ? campaignPreRegSupplierPM.IsRegistered.Value : false;
            campaignPreRegSupplierDM.Password = campaignPreRegSupplierPM.RegistrationCode;
            //campaignPreRegSupplierDM.IsDSMappedToBuyer = campaignPreRegSupplierPM.IsDSMapped;
            //campaignPreRegSupplierDM.IsFITMappedToBuyer = campaignPreRegSupplierPM.IsFITMapped;
            //campaignPreRegSupplierDM.IsHSMappedToBuyer = campaignPreRegSupplierPM.IsHSMapped
            campaignPreRegSupplierDM.CampaignURL = !string.IsNullOrWhiteSpace(campaignPreRegSupplierPM.BuyerCampaign.CampaignUrl) ? campaignPreRegSupplierPM.BuyerCampaign.CampaignUrl : string.Empty;
            campaignPreRegSupplierDM.BuyerOrganization = campaignPreRegSupplierPM.BuyerCampaign.Buyer != null ? campaignPreRegSupplierPM.BuyerCampaign.Buyer.Organization.Party.PartyName : string.Empty;
            campaignPreRegSupplierDM.EmailContent = campaignPreRegSupplierPM.BuyerCampaign.EmailTemplate != null ? campaignPreRegSupplierPM.BuyerCampaign.EmailTemplate.Content : string.Empty;
            return campaignPreRegSupplierDM;
        }

        public static SellerRegister MappingPreRegToRegisterModel(this CampaignInvitation campaignPreRegSupplier)
        {
            var supplierRegisterModel = new SellerRegister();
            supplierRegisterModel.OrganisationName = campaignPreRegSupplier.SupplierCompanyName;
            supplierRegisterModel.Email = campaignPreRegSupplier.EmailAddress;
            supplierRegisterModel.FirstName = campaignPreRegSupplier.FirstName;
            supplierRegisterModel.LastName = campaignPreRegSupplier.LastName;
            supplierRegisterModel.JobTitle = campaignPreRegSupplier.JobTitle;
            supplierRegisterModel.FirstAddressLine1 = campaignPreRegSupplier.AddressLine1;
            supplierRegisterModel.FirstAddressLine2 = campaignPreRegSupplier.AddressLine2;
            supplierRegisterModel.FirstAddressCity = campaignPreRegSupplier.City;
            supplierRegisterModel.FirstAddressState = campaignPreRegSupplier.State;
            supplierRegisterModel.FirstAddressCountry = campaignPreRegSupplier.RefCountry.ToString();
            supplierRegisterModel.FirstAddressPostalCode = campaignPreRegSupplier.ZipCode;

            if(campaignPreRegSupplier.IdentifierType1.Trim().Equals(Constants.IDENTIFIER_TYPE_REGISTRATION_NUMBER))
                supplierRegisterModel.CompanyRegistrationNumber = campaignPreRegSupplier.Identifier1;
            else if (campaignPreRegSupplier.IdentifierType2.Trim().Equals(Constants.IDENTIFIER_TYPE_REGISTRATION_NUMBER))
                supplierRegisterModel.CompanyRegistrationNumber = campaignPreRegSupplier.Identifier2;
            else if (campaignPreRegSupplier.IdentifierType3.Trim().Equals(Constants.IDENTIFIER_TYPE_REGISTRATION_NUMBER))
                supplierRegisterModel.CompanyRegistrationNumber = campaignPreRegSupplier.Identifier3;

            if (campaignPreRegSupplier.IdentifierType1.Trim().Equals(Constants.IDENTIFIER_TYPE_VAT_NUMBER))
                supplierRegisterModel.VATNumber = campaignPreRegSupplier.Identifier1;
            else if (campaignPreRegSupplier.IdentifierType2.Trim().Equals(Constants.IDENTIFIER_TYPE_VAT_NUMBER))
                supplierRegisterModel.VATNumber = campaignPreRegSupplier.Identifier2;
            else if (campaignPreRegSupplier.IdentifierType3.Trim().Equals(Constants.IDENTIFIER_TYPE_VAT_NUMBER))
                supplierRegisterModel.VATNumber = campaignPreRegSupplier.Identifier3;

            if (campaignPreRegSupplier.IdentifierType1.Trim().Equals(Constants.IDENTIFIER_TYPE_DUNS_NUMBER))
                supplierRegisterModel.DUNSNumber = campaignPreRegSupplier.Identifier1;
            else if (campaignPreRegSupplier.IdentifierType2.Trim().Equals(Constants.IDENTIFIER_TYPE_DUNS_NUMBER))
                supplierRegisterModel.DUNSNumber = campaignPreRegSupplier.Identifier2;
            else if (campaignPreRegSupplier.IdentifierType3.Trim().Equals(Constants.IDENTIFIER_TYPE_DUNS_NUMBER))
                supplierRegisterModel.DUNSNumber = campaignPreRegSupplier.Identifier3;

            supplierRegisterModel.DummyOrganisationName = campaignPreRegSupplier.SupplierCompanyName;
            supplierRegisterModel.HaveDuns = string.IsNullOrEmpty(supplierRegisterModel.DUNSNumber) ? false : true;
            supplierRegisterModel.IsVAT = string.IsNullOrEmpty(supplierRegisterModel.VATNumber) ? false : true; 

            supplierRegisterModel.IsPreRegistered = true;
            supplierRegisterModel.UserPartyId = campaignPreRegSupplier.Id;
            supplierRegisterModel.IsSubsidaryStatus = campaignPreRegSupplier.IsSubsidary.HasValue ? campaignPreRegSupplier.IsSubsidary.Value : false;
            supplierRegisterModel.UltimateParent = campaignPreRegSupplier.UltimateParent;
            supplierRegisterModel.ContactDetails = new List<ViewModel.ContactDetails>();
            supplierRegisterModel.ContactDetails.Add(new ViewModel.ContactDetails()
            {
                ContactType = ContactType.Primary,
                Email = campaignPreRegSupplier.EmailAddress,
                FirstName = campaignPreRegSupplier.FirstName,
                LastName = campaignPreRegSupplier.LastName,
                JobTitle = campaignPreRegSupplier.JobTitle,
                Telephone = campaignPreRegSupplier.Telephone
            });
            return supplierRegisterModel;
        }
    }
}
