using PRGX.SIMTrax.DAL.Abstract;
using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Abstract;
using PRGX.SIMTrax.DAL.Repository.Context;
using PRGX.SIMTrax.DAL.Repository.Helper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL
{
    public class CampaignUow : GenericUow, ICampaignUow, IDisposable
    {
        public CampaignUow(IRepositoryProvider repositoryProvider)
        {
            CreateDbContext();

            repositoryProvider.DbContext = DbContext;
            RepositoryProvider = repositoryProvider;
        }

        public CampaignUow()
        {
            CreateDbContext();

            RepositoryFactories factory = new RepositoryFactories();
            RepositoryProvider repoProvider = new RepositoryProvider(factory);
            repoProvider.DbContext = DbContext;
            RepositoryProvider = repoProvider;
        }
        
        public IBuyerRepository Buyers { get { return GetRepo<IBuyerRepository>(); } }
        
        public IGenericRepository<Party> Parties { get { return GetStandardRepo<Party>(); } }

        public ICampaignInvitationRepository CampaignInvitations { get { return GetRepo<ICampaignInvitationRepository>(); } }

        public IBuyerCampaignRepository BuyerCampaigns { get { return GetRepo<IBuyerCampaignRepository>(); } }

        public IGenericRepository<Document> Documents { get { return GetStandardRepo<Document>(); } }

        public IGenericRepository<CampaignMessage> CampaignMessages { get { return GetStandardRepo<CampaignMessage>(); } }

        public IGenericRepository<EmailTemplate> EmailTemplates { get { return GetStandardRepo<EmailTemplate>(); } }

        public IGenericRepository<SupplierReferrer> SupplierReferrers { get { return GetStandardRepo<SupplierReferrer>(); } }

        public IGenericRepository<PartyPartyLink> PartyPartyLinks { get { return GetStandardRepo<PartyPartyLink>(); } }

        /// <summary>
        /// Save pending changes to the database
        /// </summary>
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
            DbContext = new CampaignContext();

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


        private CampaignContext DbContext { get; set; }

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
