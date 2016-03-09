using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Abstract
{
    public interface IBuyerUow
    {
        void SaveChanges();

        IBuyerRepository Buyers { get; }

        IUserRepository Users { get; }

        IOrganizationRepository Organizations { get; }

        IPartyRepository Parties { get; }

        IGenericRepository<PartyPartyLink> PartyPartyLinks { get; }

        IGenericRepository<DiscountVoucher> DiscountVouchers { get; }

        IGenericRepository<ContactMethod> ContactMethods { get; }

        IGenericRepository<Person> Persons { get; }

        IGenericRepository<Email> Emails { get; }

        IGenericRepository<Phone> Phones { get; }

        IContactPersonRepository ContactPersons { get; }

        IAddressRepository Addresses { get; }

        IGenericRepository<UserRoleLink> UserRoleLinks { get; }

        IGenericRepository<AcceptedTermsOfUse> AcceptedTermsOfUses { get; }

        IGenericRepository<PartyContactMethodLink> PartyContactMethodLinks { get; }

        ICredentialRepository Credentials { get; }

        void Dispose();

        void Rollback();

        void Commit();

        void BeginTransaction();
    }
}
