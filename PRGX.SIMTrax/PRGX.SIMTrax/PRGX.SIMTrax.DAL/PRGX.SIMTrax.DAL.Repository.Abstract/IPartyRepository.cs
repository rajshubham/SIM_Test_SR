using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.Domain.Model;
using System.Collections.Generic;

namespace PRGX.SIMTrax.DAL.Repository.Abstract
{
    public interface IPartyRepository : IGenericRepository<Party>
    {
        bool IsOrganisationExists(string organisationName);
        
        Party  GetCompanyDetailsByPartyId(long organizationPartyId);

        Party GetCapabilityDetailsByPartyId(long organizationPartyId);

        Party GetMarketingDetailsByPartyId(long organizationPartyId);
        List<long> GetIndustryCodesByOrganisationPartyId(long sellerPartyId);

        Party GetSellerOrganizationDetailsByPartyId(long organizationPartyId);
        Profile SellerProfileDetails(long sellerPartyId, long buyerPartyId,long buyerUserPartyId);

    }
}
