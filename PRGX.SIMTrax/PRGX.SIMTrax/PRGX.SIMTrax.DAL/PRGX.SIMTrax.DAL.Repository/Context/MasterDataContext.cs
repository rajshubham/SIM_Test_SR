using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Configuration;
using System.Data.Entity;

namespace PRGX.SIMTrax.DAL.Repository.Context
{
    public partial class MasterDataContext : GenericContext
    {
        static MasterDataContext()
        {
            Database.SetInitializer<MasterDataContext>(null);
        }

        //TODO: read it from Util.Constants
        private static string ConnectionString { get {
                //TODO: remove hardcoded value
                return "SIMTraxContext";
                //return Domain.Util.Configuration.ConnectionString;
            }
        }

        public MasterDataContext()
            : base(nameOrConnectionString: ConnectionString)
        {
        }

        public DbSet<MasterDataType> MasterDataTypes { get; set; }
        public DbSet<MasterData> MasterDatas { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<IndustryCode> IndustryCodes { get; set; }
        public DbSet<IndustryCodeSet> IndustryCodeSets { get; set; }
        public DbSet<IndustryCodeSetRegionLink> IndustryCodeSetRegionLinks { get; set; }
        public DbSet<PartyIdentifierType> PartyIdentifierTypes { get; set; }
        public DbSet<TermsOfUse> TermsOfUses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new MasterDataTypeMap());
            modelBuilder.Configurations.Add(new MasterDataMap());
            modelBuilder.Configurations.Add(new RegionMap());
            modelBuilder.Configurations.Add(new IndustryCodeMap());
            modelBuilder.Configurations.Add(new IndustryCodeSetRegionLinkMap());
            modelBuilder.Configurations.Add(new IndustryCodeSetMap());
            modelBuilder.Configurations.Add(new PartyIdentifierTypeMap());
            modelBuilder.Configurations.Add(new TermsOfUseMap());
        }

        public override int SaveChanges()
        {
            ApplySaveRule();

            return base.SaveChanges();
        }
    }
}
