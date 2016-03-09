using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Abstract;

namespace PRGX.SIMTrax.DAL.Abstract
{
    public interface IUserUow
    {
        void SaveChanges();

        IUserRepository Users { get; }

        IPartyRepository Parties { get; }

        IGenericRepository<TermsOfUse> TermsOfUses { get; }

        IGenericRepository<PartyPartyLink> PartyPartyLinks { get; }

        IGenericRepository<AcceptedTermsOfUse> AcceptedTermsOfUses { get; }

        ICredentialRepository Credentials { get; }

        IGenericRepository<ContactMethod> ContactMethods { get; }

        IGenericRepository<ContactPerson> ContactPersons { get; }

        IGenericRepository<Person> People { get; }

        IGenericRepository<PartyContactMethodLink> PartyContactMethodLinks { get; }

        IGenericRepository<Email> Emails { get; }

        IGenericRepository<Phone> Phones { get; }

        IGenericRepository<TemporaryPasswordUrl> TemporaryPasswordUrls { get; }

        void Dispose();

        void Rollback();

        void Commit();

        void BeginTransaction();
    }
}
