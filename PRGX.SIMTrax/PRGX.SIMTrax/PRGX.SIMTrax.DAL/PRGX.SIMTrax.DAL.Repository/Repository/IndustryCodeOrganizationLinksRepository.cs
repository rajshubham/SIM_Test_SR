using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Abstract;
using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository.Repository
{
   public  class IndustryCodeOrganizationLinksRepository: GenericRepository<IndustryCodeOrganizationLink>, IIndustryCodeOrganizationLinksRepository
    {

        public IndustryCodeOrganizationLinksRepository(DbContext context) : base(context) { }

        public bool AddUpdateIndustryCodeOrganisationLinks(List<IndustryCodeOrganizationLink> industryCodeOrganisationLinks, long organisationId)
        {
            try
            {
                var result = true;
                Logger.Info("IndustryCodeOrganizationLinksRepository : AddUpdateIndustryCodeOrganisationLinks() : Enter the method");

                var existingIndustryCodeOrganizationLinks = this.All().Where(v => v.RefOrganization == organisationId).ToList();

                var newIndustryCodeIds = industryCodeOrganisationLinks.Select(u => u.RefIndustryCode).ToList();
                var deleteIndustryCodes = existingIndustryCodeOrganizationLinks.Where(u => !newIndustryCodeIds.Contains(u.RefIndustryCode)).ToList();

                var existingIndustryCodeIds = existingIndustryCodeOrganizationLinks.Select(u => u.RefIndustryCode).ToList();
                var newIndustryCodes = industryCodeOrganisationLinks.Where(u => !existingIndustryCodeIds.Contains(u.RefIndustryCode)).ToList();

                foreach (var industryCode in deleteIndustryCodes)
                {
                    Delete(industryCode);
                }
                foreach (var industryCode in newIndustryCodes)
                {
                    Add(industryCode);
                }
                Logger.Info("IndustryCodeOrganizationLinksRepository : AddUpdateIndustryCodeOrganisationLinks() : Exit the method");

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("IndustryCodeOrganizationLinksRepository : AddUpdateIndustryCodeOrganisationLinks() : Caught an exception" + ex);
                throw;
            }

        }
    }
}
