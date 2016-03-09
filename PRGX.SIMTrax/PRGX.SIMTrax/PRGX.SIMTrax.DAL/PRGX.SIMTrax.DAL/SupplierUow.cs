using PRGX.SIMTrax.DAL.Abstract;
using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Abstract;
using PRGX.SIMTrax.DAL.Repository.Context;
using PRGX.SIMTrax.DAL.Repository.Helper;
using System;
using System.Data.Entity;

namespace PRGX.SIMTrax.DAL
{
    public class SupplierUow : GenericUow, ISupplierUow, IDisposable
    {
        public SupplierUow(IRepositoryProvider repositoryProvider)
        {
            CreateDbContext();

            repositoryProvider.DbContext = DbContext;
            RepositoryProvider = repositoryProvider;
        }

        public SupplierUow()
        {
            CreateDbContext();

            RepositoryFactories factory = new RepositoryFactories();
            RepositoryProvider repoProvider = new RepositoryProvider(factory);
            repoProvider.DbContext = DbContext;
            RepositoryProvider = repoProvider;
        }

        public IPartyRepository Parties { get { return GetRepo<IPartyRepository>(); } }
        
        public ISupplierRepository Suppliers { get { return GetRepo<ISupplierRepository>(); } }

        public IOrganizationRepository Organizations { get { return GetRepo<IOrganizationRepository>(); } }
        
        public IPartyIdentifierRepository PartyIdentifers { get { return GetRepo<IPartyIdentifierRepository>(); } }

        public IGenericRepository<ContactMethod> ContactMethods { get { return GetStandardRepo<ContactMethod>(); } }

        public IGenericRepository<Document> Documents { get { return GetStandardRepo<Document>(); } }

        public IInviteeRepository Invitees { get { return GetRepo<IInviteeRepository>(); } }

        public IIndustryCodeOrganizationLinksRepository IndustryCodeOrganizationLinks { get { return GetRepo<IIndustryCodeOrganizationLinksRepository>(); } }

        public IPartyRegionLinkRepository PartyRegionLinks { get { return GetRepo<IPartyRegionLinkRepository>(); } }

        public IAddressRepository Addresses { get { return GetRepo<IAddressRepository>(); } }
        public IContactPersonRepository ContactPersons { get { return GetRepo<IContactPersonRepository>(); } }
        public IBankAccountRepository BankAccounts { get { return GetRepo<IBankAccountRepository>(); } }
        public IGenericRepository<Person> Persons { get { return GetStandardRepo<Person>(); } }
        public IGenericRepository<Email> Emails { get { return GetStandardRepo<Email>(); } }
        public IGenericRepository<Phone> Phones { get { return GetStandardRepo<Phone>(); } }
        public IGenericRepository<LegalEntity> LegalEntites { get { return GetStandardRepo<LegalEntity>(); } }
        public IGenericRepository<LegalEntityProfile> LegalEntityProfiles { get { return GetStandardRepo<LegalEntityProfile>(); } }
        public IGenericRepository<PartyPartyLink> PartyPartyLinks { get { return GetStandardRepo<PartyPartyLink>(); } }
        public IGenericRepository<PartyContactMethodLink> PartyContactMethodLinks { get { return GetStandardRepo<PartyContactMethodLink>(); } }
        public IGenericRepository<BuyerSupplierReference> BuyerSupplierReferences { get { return GetStandardRepo<BuyerSupplierReference>(); } }
        public IGenericRepository<CampaignInvitation> CampaignInvitations { get { return GetStandardRepo<CampaignInvitation>(); } }

        private DbContextTransaction _dbContextTransaction;

        /// <summary>
        /// Save pending changes to the database
        /// </summary>
        public void BeginTransaction()
        {
            _dbContextTransaction = DbContext.Database.BeginTransaction();
        }

        public void Commit()
        {
            if (null != _dbContextTransaction)
                _dbContextTransaction.Commit();

            this.Dispose();
        }

        public void Rollback()
        {
            if (null != _dbContextTransaction)
                _dbContextTransaction.Rollback();

            this.Dispose();

        }

        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }

        protected void CreateDbContext()
        {
            DbContext = new SellerContext();

            // Do NOT enable proxied entities, else serialization fails
            DbContext.Configuration.ProxyCreationEnabled = false;

            // Load navigation properties explicitly (avoid serialization trouble)
            DbContext.Configuration.LazyLoadingEnabled = false;

            // Because Web API will perform validation, we don't need/want EF to do so
            DbContext.Configuration.ValidateOnSaveEnabled = false;

            //DbContext.Configuration.AutoDetectChangesEnabled = false;
            // We won't use this performance tweak because we don't need 
            // the extra performance and, when autodetect is false,
            // we'd have to be careful. We're not being that careful.
        }

        private SellerContext DbContext { get; set; }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DbContext != null)
                {
                    DbContext.Dispose();
                }
            }
        }

        #endregion
    }
}
