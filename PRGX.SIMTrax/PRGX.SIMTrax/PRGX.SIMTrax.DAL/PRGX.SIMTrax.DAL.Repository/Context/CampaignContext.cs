using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Configuration;
using System.Data.Entity;


namespace PRGX.SIMTrax.DAL.Repository.Context
{
    public partial class CampaignContext : GenericContext
    {
        static CampaignContext()
        {
            Database.SetInitializer<CampaignContext>(null);
        }

        //TODO: read it from Util.Constants
        private static string ConnectionString
        {
            get
            {
                //TODO: remove hardcoded value
                return "SIMTraxContext";
                //return Domain.Util.Configuration.ConnectionString;
            }
        }

        public CampaignContext()
            : base(nameOrConnectionString: ConnectionString)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<CampaignInvitation> CampaignInvitations { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Party> Parties { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<DiscountVoucher> DiscountVouchers { get; set; }
        public DbSet<BuyerCampaign> BuyerCampaigns { get; set; }
        public DbSet<CampaignMessage> CampaignMessage { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Credential> Credentials { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<SupplierReferrer> SupplierReferrers { get; set; }
        public DbSet<PartyPartyLink> PartyPartyLinks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new PersonMap());
            modelBuilder.Configurations.Add(new CampaignInvitationMap());
            modelBuilder.Configurations.Add(new PartyMap());
            modelBuilder.Configurations.Add(new OrganizationMap());
            modelBuilder.Configurations.Add(new BuyerMap());
            modelBuilder.Configurations.Add(new DiscountVoucherMap());
            modelBuilder.Configurations.Add(new BuyerCampaignMap());
            modelBuilder.Configurations.Add(new CampaignMessageMap());
            modelBuilder.Configurations.Add(new EmailTemplateMap());
            modelBuilder.Configurations.Add(new DocumentMap());
            modelBuilder.Configurations.Add(new CredentialMap());
            modelBuilder.Configurations.Add(new RegionMap());
            modelBuilder.Configurations.Add(new SupplierReferrerMap());
            modelBuilder.Configurations.Add(new PartyPartyLinkMap());
        }

        public override int SaveChanges()
        {
            ApplySaveRule();
            return base.SaveChanges();
        }
    }
}
