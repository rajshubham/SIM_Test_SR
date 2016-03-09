using PRGX.SIMTrax.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository.Abstract
{
    public interface ICampaignInvitationRepository : IGenericRepository<CampaignInvitation>
    {
        bool InsertListOfPreRegSupplier(List<CampaignInvitation> campaignPreRegSupplierList);

        bool UpdateListOfPreRegSupplier(List<CampaignInvitation> campaignPreRegSupplierList);

        List<CampaignInvitation> GetPreRegSupplierInCampaign(int campaignId, string filterCriteria, out int total, int pageIndex, int size);

        List<CampaignInvitation> GetPreRegSupplierListwithPasswordString(long campaignId);

        CampaignInvitation GetCampaignInvitationRecord(long id);
    }
}
