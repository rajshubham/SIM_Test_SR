using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Abstract
{
    public interface ICampaignUow
    {
        void SaveChanges();

        IBuyerRepository Buyers { get; }

        IGenericRepository<Party> Parties { get; }

        ICampaignInvitationRepository CampaignInvitations { get; }

        IBuyerCampaignRepository BuyerCampaigns { get; }

        IGenericRepository<Document> Documents { get; }

        IGenericRepository<CampaignMessage> CampaignMessages { get; }

        IGenericRepository<EmailTemplate> EmailTemplates { get; }

        IGenericRepository<SupplierReferrer> SupplierReferrers { get; }

        IGenericRepository<PartyPartyLink> PartyPartyLinks { get; }

        void Dispose();

        void Rollback();

        void Commit();

        void BeginTransaction();
    }
}
