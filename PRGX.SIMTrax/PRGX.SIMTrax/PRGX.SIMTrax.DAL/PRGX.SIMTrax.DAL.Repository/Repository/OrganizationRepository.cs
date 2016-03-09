using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Abstract;
using PRGX.SIMTrax.DAL.Repository.Context;
using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.Domain.Util;
using System;
using System.Data.Entity;
using System.Linq;

namespace PRGX.SIMTrax.DAL.Repository.Repository
{
    public class OrganizationRepository : GenericRepository<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(DbContext context) : base(context) { }

        public bool UpdateOrganizationDetails(Organization organization, long partyId)
        {
            try
            {
                Logger.Info("OrganizationRepository : UpdateOrganizationDetails() : Enter the method");
                var result = false;
                var organizationPM = this.All().Include("IndustryCodeOrganizationLinks").Where(x => x.Id == partyId).FirstOrDefault();

                if (null != organizationPM)
                {
                    organizationPM.BusinessSectorDescription = organization.BusinessSectorDescription;
                    organizationPM.BusinessSectorId = organization.BusinessSectorId;
                    organizationPM.EmployeeSize = organization.EmployeeSize;
                    organizationPM.Status = (short)organization.Status;
                    organizationPM.TurnOverSize = organization.TurnOverSize;
                    organizationPM.RefLastUpdatedBy = organization.RefLastUpdatedBy;

                    Update(organizationPM);
                    result = true;
                }
                Logger.Info("OrganizationRepository : UpdateOrganizationDetails() : Exit the method");
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("OrganizationRepository : UpdateOrganizationDetails() : Caught an error" + ex);
                throw;
            }
        }

        public bool UpdateOrganizationStatus(long partyId, short organizationStatus, long userPartyId)
        {
            try
            {
                Logger.Info("OrganizationRepository : UpdateOrganizationStatus() : Enter the method");
                var result = false;
                var organizationPM = this.All().Where(x => x.Id == partyId).FirstOrDefault();

                if (null != organizationPM)
                {
                    organizationPM.RefLastUpdatedBy = userPartyId;
                    if (organizationStatus == (Int16)CompanyStatus.Submitted)
                        organizationPM.RegistrationSubmittedOn = DateTime.UtcNow;
                    organizationPM.Status = organizationStatus;

                    Update(organizationPM);
                    result = true;
                }
                Logger.Info("OrganizationRepository : UpdateOrganizationDetails() : Exit the method");
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("OrganizationRepository : UpdateOrganizationDetails() : Caught an error" + ex);
                throw;
            }
        }

        public OrganizationDetail GetOrganizationDetail(long organizationPartyId)
        {
            try
            {
                Logger.Info("OrganizationRepository : GetOrganizationDetail() : Enter the method");

                OrganizationDetail organizationDetails = new OrganizationDetail();
                using (var ctx = new SellerContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = true;
                    var organizationPM = ctx.Organizations.Where(p => p.Id == organizationPartyId).FirstOrDefault();
                    if (null != organizationPM)
                    {
                        organizationDetails.OrganizationId = organizationPM.Id;
                        organizationDetails.OrganizationName = organizationPM.Party.PartyName;
                        organizationDetails.RefLegalEntityId = organizationPM.RefLegalEntity.HasValue ? organizationPM.RefLegalEntity.Value : 0;
                        organizationDetails.RefPartyId = organizationPM.Id;
                        organizationDetails.RefSellerId = organizationPM.Supplier != null ? organizationPM.Supplier.Id : 0;
                        organizationDetails.Status = organizationPM.Status.HasValue ? organizationPM.Status.Value : (short)0;
                    }
                }
                Logger.Info("OrganizationRepository : GetOrganizationDetail() : Exit the method");
                return organizationDetails;
            }
            catch (Exception ex)
            {
                Logger.Error("OrganizationRepository : GetOrganizationDetail() : Caught an exception" + ex);
                throw;
            }
        }
    }
}
