using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.Domain.Model;

namespace PRGX.SIMTrax.DAL.Repository.Abstract
{
    public interface IOrganizationRepository : IGenericRepository<Organization>
    {
        bool UpdateOrganizationDetails(Organization organization, long partyId);

        bool UpdateOrganizationStatus(long partyId, short organizationStatus, long userPartyId);

        OrganizationDetail GetOrganizationDetail(long organizationPartyId);
    }
}
