using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Abstract;

namespace PRGX.SIMTrax.DAL.Abstract
{
    public interface ISupplierUow
    {
        void SaveChanges();
        
        IPartyRepository Parties { get; }

        ISupplierRepository Suppliers { get; }

        IOrganizationRepository Organizations { get; }

        IPartyIdentifierRepository PartyIdentifers { get; }

        IGenericRepository<ContactMethod> ContactMethods { get; }
        IGenericRepository<Document> Documents { get; }

        IAddressRepository Addresses { get; }
        IContactPersonRepository ContactPersons { get; }
        IBankAccountRepository BankAccounts { get; }

        IGenericRepository<Person> Persons { get; }
        IGenericRepository<Email> Emails { get; }
        IGenericRepository<Phone> Phones { get; }
        IInviteeRepository Invitees { get; }
        IGenericRepository<LegalEntity> LegalEntites { get; }
        IGenericRepository<BuyerSupplierReference> BuyerSupplierReferences { get; }
        IIndustryCodeOrganizationLinksRepository IndustryCodeOrganizationLinks { get; }
        IGenericRepository<LegalEntityProfile> LegalEntityProfiles { get; }
        IPartyRegionLinkRepository PartyRegionLinks { get; }
        IGenericRepository<PartyPartyLink> PartyPartyLinks { get; }
        IGenericRepository<PartyContactMethodLink> PartyContactMethodLinks { get; }
        IGenericRepository<CampaignInvitation> CampaignInvitations { get; }

        void Dispose();

        void Rollback();

        void Commit();

        void BeginTransaction();
    }
}
