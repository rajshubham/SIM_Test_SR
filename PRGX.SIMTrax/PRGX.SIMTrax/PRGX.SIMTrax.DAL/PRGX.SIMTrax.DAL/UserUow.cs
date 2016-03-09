using PRGX.SIMTrax.DAL.Abstract;
using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Abstract;
using PRGX.SIMTrax.DAL.Repository.Context;
using PRGX.SIMTrax.DAL.Repository.Helper;
using System;
using System.Data.Entity;

namespace PRGX.SIMTrax.DAL
{

    public class UserUow : GenericUow, IUserUow, IDisposable
    {
        public UserUow(IRepositoryProvider repositoryProvider)
        {
            CreateDbContext();

            repositoryProvider.DbContext = DbContext;
            RepositoryProvider = repositoryProvider;
        }

        public UserUow()
        {
            CreateDbContext();

            RepositoryFactories factory = new RepositoryFactories();
            RepositoryProvider repoProvider = new RepositoryProvider(factory);
            repoProvider.DbContext = DbContext;
            RepositoryProvider = repoProvider;
        }

        public IUserRepository Users { get { return GetRepo<IUserRepository>(); } }

        public IPartyRepository Parties { get { return GetRepo<IPartyRepository>(); } }

        public IGenericRepository<PartyPartyLink> PartyPartyLinks { get { return GetStandardRepo<PartyPartyLink>(); } }

        public IGenericRepository<TermsOfUse> TermsOfUses { get { return GetStandardRepo<TermsOfUse>(); } }

        public IGenericRepository<AcceptedTermsOfUse> AcceptedTermsOfUses { get { return GetStandardRepo<AcceptedTermsOfUse>(); } }

        public ICredentialRepository Credentials { get { return GetRepo<ICredentialRepository>(); } }

        public IGenericRepository<ContactPerson> ContactPersons { get { return GetStandardRepo<ContactPerson>(); } }

        public IGenericRepository<ContactMethod> ContactMethods { get { return GetStandardRepo<ContactMethod>(); } }

        public IGenericRepository<Person> People { get { return GetStandardRepo<Person>(); } }

        public IGenericRepository<PartyContactMethodLink> PartyContactMethodLinks { get { return GetStandardRepo<PartyContactMethodLink>(); } }

        public IGenericRepository<Email> Emails { get { return GetStandardRepo<Email>(); } }

        public IGenericRepository<Phone> Phones { get { return GetStandardRepo<Phone>(); } }

        public IGenericRepository<TemporaryPasswordUrl> TemporaryPasswordUrls { get { return GetStandardRepo<TemporaryPasswordUrl>(); } }

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
            DbContext = new UserContext();

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

        private UserContext DbContext { get; set; }

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