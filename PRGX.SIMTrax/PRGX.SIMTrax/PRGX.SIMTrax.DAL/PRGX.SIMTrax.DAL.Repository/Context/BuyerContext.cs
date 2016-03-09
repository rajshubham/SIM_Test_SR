using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Entity.Registration;
using PRGX.SIMTrax.DAL.Repository.Configuration;
using PRGX.SIMTrax.DAL.Repository.Configuration.Registration;
using System.Data.Entity;


namespace PRGX.SIMTrax.DAL.Repository.Context
{
    public partial class BuyerContext : GenericContext
    {
        static BuyerContext()
        {
            Database.SetInitializer<BuyerContext>(null);
        }

        //TODO: read it from Util.Constants
        private static string ConnectionString { get {
                //TODO: remove hardcoded value
                return "SIMTraxContext";
                //return Domain.Util.Configuration.ConnectionString;
            } }

        public BuyerContext()
            : base(nameOrConnectionString: ConnectionString)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Party> Parties { get; set; }
        public DbSet<LegalEntity> LegalEntities { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<Credential> Credentials { get; set; }
        public DbSet<PartyPartyLink> PartyPartyLinks { get; set; }
        public DbSet<ContactMethod> ContactMethods { get; set; }
        public DbSet<ContactPerson> ContactPersons { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRoleLink> UserRoleLinks { get; set; }
        public DbSet<AcceptedTermsOfUse> AcceptedTermsOfUses { get; set; }
        public DbSet<PartyContactMethodLink> PartyContactMethodLinks { get; set; }
        public DbSet<DiscountVoucher> DiscountVouchers { get; set; }
        public DbSet<BuyerCampaign> BuyerCampaigns { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AcceptedTermsOfUseMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new PersonMap());
            modelBuilder.Configurations.Add(new CredentialMap());
            modelBuilder.Configurations.Add(new PartyMap());
            modelBuilder.Configurations.Add(new OrganizationMap());
            modelBuilder.Configurations.Add(new BuyerMap());
            modelBuilder.Configurations.Add(new PartyPartyLinkMap());
            modelBuilder.Configurations.Add(new LegalEntityMap());
            modelBuilder.Configurations.Add(new ContactMethodMap());
            modelBuilder.Configurations.Add(new ContactPersonMap());
            modelBuilder.Configurations.Add(new EmailMap());
            modelBuilder.Configurations.Add(new PhoneMap());
            modelBuilder.Configurations.Add(new AddressMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new PermissionMap());
            modelBuilder.Configurations.Add(new RolePermissionLinkMap());
            modelBuilder.Configurations.Add(new UserRoleLinkMap());
            modelBuilder.Configurations.Add(new PartyContactMethodLinkMap());
            modelBuilder.Configurations.Add(new DiscountVoucherMap());
            modelBuilder.Configurations.Add(new BuyerCampaignMap());
        }


        public override int SaveChanges()
        {
            ApplySaveRule();
            return base.SaveChanges();
        }
    }
}
