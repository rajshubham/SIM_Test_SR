using PRGX.SIMTrax.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository.Abstract
{
    public interface IIndustryCodeOrganizationLinksRepository : IGenericRepository<IndustryCodeOrganizationLink>
    {
         bool AddUpdateIndustryCodeOrganisationLinks(List<IndustryCodeOrganizationLink> industryCodeOrganisationLinks,long organisationIs);
    }
}
