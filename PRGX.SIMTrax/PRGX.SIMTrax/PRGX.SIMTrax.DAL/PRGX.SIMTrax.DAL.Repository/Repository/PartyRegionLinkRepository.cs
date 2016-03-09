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
    public class PartyRegionLinkRepository : GenericRepository<PartyRegionLink>, IPartyRegionLinkRepository
    {
        public PartyRegionLinkRepository(DbContext context) : base(context) { }

        public bool AddUpdatePartyRegions(List<PartyRegionLink> partyRegionLinkList, long partyId)
        {
            try
            {
                Logger.Info("PartyRegionLinkRepository : AddUpdatePartyRegions() : Enter the method");
                var result = false;

                var partyRegionLinkPM = this.All().Where(x => x.RefParty == partyId).ToList();

                var salesPartyRegion = partyRegionLinkPM.Where(x => x.LinkType.Trim() == Constants.PARTY_REGION_SALES_REGION).ToList();
                var servicePartyRegion = partyRegionLinkPM.Where(x => x.LinkType.Trim() == Constants.PARTY_REGION_SERVICE_REGION).ToList();

                var newSalesIds = partyRegionLinkList.Where(x => x.LinkType.Trim() == Constants.PARTY_REGION_SALES_REGION).Select(u => u.RefRegion).ToList();
                var deleteSales = salesPartyRegion.Where(u => !newSalesIds.Contains(u.RefRegion)).ToList();
                var existingSalesIds = salesPartyRegion.Select(u => u.RefRegion).ToList();
                var newSales = partyRegionLinkList.Where(x => x.LinkType.Trim() == Constants.PARTY_REGION_SALES_REGION && !existingSalesIds.Contains(x.RefRegion)).ToList();
                foreach (var saleRegion in deleteSales)
                {
                    Delete(saleRegion);
                }
                foreach (var saleRegion in newSales)
                {
                    Add(saleRegion);
                }

                var newServicesIds = partyRegionLinkList.Where(x => x.LinkType.Trim() == Constants.PARTY_REGION_SERVICE_REGION).Select(u => u.RefRegion).ToList();
                var deleteServices = servicePartyRegion.Where(u => !newServicesIds.Contains(u.RefRegion)).ToList();
                var existingServicesIds = servicePartyRegion.Select(u => u.RefRegion).ToList();
                var newServices = partyRegionLinkList.Where(x => x.LinkType.Trim() == Constants.PARTY_REGION_SERVICE_REGION && !existingServicesIds.Contains(x.RefRegion)).ToList();
                foreach (var serviceRegion in deleteServices)
                {
                    Delete(serviceRegion);
                }
                foreach (var serviceRegion in newServices)
                {
                    Add(serviceRegion);
                }

                result = true;
                Logger.Info("PartyRegionLinkRepository : AddUpdatePartyRegions() : Exit the method");
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyRegionLinkRepository : AddUpdatePartyRegions() : Caught an exception" + ex);
                throw;
            }
        }
    }
}
