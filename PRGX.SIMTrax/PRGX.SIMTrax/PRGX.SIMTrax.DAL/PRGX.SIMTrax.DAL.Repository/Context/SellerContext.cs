using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Configuration;
using System.Data.Entity;


namespace PRGX.SIMTrax.DAL.Repository.Context
{
    public partial class SellerContext : GenericContext
    {
        static SellerContext()
        {
            Database.SetInitializer<SellerContext>(null);
        }

        //TODO: read it from Util.Constants
        private static string ConnectionString { get {
                //TODO: remove hardcoded value
                return "SIMTraxContext";
                //return Domain.Util.Configuration.ConnectionString;
            } }

        public SellerContext()
            : base(nameOrConnectionString: ConnectionString)
        {
        }

        public DbSet<Party> Parties { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<PartyIdentifier> PartyIdentifiers { get; set; }
        //public DbSet<PartyIdentifierDocument> PartyIdentifierDocuments { get; set; }
        public DbSet<PartyIdentifierType> PartyIdentifierTypes { set; get; }
        public DbSet<Region> Regions { set; get; }
        public DbSet<PartyRegionLink> PartyRegionLinks { set; get; }
        public DbSet<IndustryCode> IndustryCodes { set; get; }
        public DbSet<IndustryCodeSet> IndustryCodeSet { set; get; }
        public DbSet<IndustryCodeOrganizationLink> IndustryCodeOrganizationLinks { set; get; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ContactPerson> ContactPersons { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<ContactMethod> ContactMethods { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Invitee> Invitees { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<LegalEntity> LegalEntities { get; set; }
        public DbSet<LegalEntityProfile> LegalEntityProfiles { get; set; }
        public DbSet<PartyPartyLink> PartyPartyLinks { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<BuyerSupplierReference> BuyerSupplierReferences { get; set; }
        public DbSet<PartyContactMethodLink> PartyContactMethodLinks { get; set; }
        public DbSet<MasterData> MasterDatas { get; set; }
        public DbSet<MasterDataType> MasterDataTypes { get; set; }
        public DbSet<BuyerCampaign> BuyerCampaigns { get; set; }
        public DbSet<CampaignInvitation> CampaignInvitations { get; set; }
        public DbSet<SupplierReferrer> SupplierReferrers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new PartyMap());
            modelBuilder.Configurations.Add(new OrganizationMap());
            modelBuilder.Configurations.Add(new SupplierMap());
            modelBuilder.Configurations.Add(new PartyIdentifierMap());
            //modelBuilder.Configurations.Add(new PartyIdentifierDocumentMap());
            modelBuilder.Configurations.Add(new PartyIdentifierTypeMap());
            modelBuilder.Configurations.Add(new RegionMap());
            modelBuilder.Configurations.Add(new PartyRegionLinkMap());
            modelBuilder.Configurations.Add(new IndustryCodeMap());
            modelBuilder.Configurations.Add(new IndustryCodeSetMap());
            modelBuilder.Configurations.Add(new IndustryCodeOrganizationLinkMap());
            modelBuilder.Configurations.Add(new AddressMap());
            modelBuilder.Configurations.Add(new ContactPersonMap());
            modelBuilder.Configurations.Add(new PersonMap());
            modelBuilder.Configurations.Add(new EmailMap());
            modelBuilder.Configurations.Add(new PhoneMap());
            modelBuilder.Configurations.Add(new ContactMethodMap());
            modelBuilder.Configurations.Add(new DocumentMap());
            modelBuilder.Configurations.Add(new InviteeMap());
            modelBuilder.Configurations.Add(new BankAccountMap());
            modelBuilder.Configurations.Add(new LegalEntityMap());
            modelBuilder.Configurations.Add(new BuyerMap());
            modelBuilder.Configurations.Add(new BuyerSupplierReferenceMap());
            modelBuilder.Configurations.Add(new LegalEntityProfileMap());
            modelBuilder.Configurations.Add(new PartyPartyLinkMap());
            modelBuilder.Configurations.Add(new PartyContactMethodLinkMap());
            modelBuilder.Configurations.Add(new MasterDataTypeMap());
            modelBuilder.Configurations.Add(new MasterDataMap());
            modelBuilder.Configurations.Add(new BuyerCampaignMap());
            modelBuilder.Configurations.Add(new CampaignInvitationMap());
            modelBuilder.Configurations.Add(new SupplierReferrerMap());
        }

        public override int SaveChanges()
        {
            ApplySaveRule();

            return base.SaveChanges();
        }
    }
}
