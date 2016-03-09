using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Configuration;
using System.Data.Entity;

namespace PRGX.SIMTrax.DAL.Repository.Context
{
    public partial class RoleContext : GenericContext
    {
        static RoleContext()
        {
            Database.SetInitializer<EmailContext>(null);
        }

        //TODO: read it from Util.Constants
        private static string ConnectionString { get {
                //TODO: remove hardcoded value
                return "SIMTraxContext";
                //return Domain.Util.Configuration.ConnectionString;
            } }

        public RoleContext()
            : base(nameOrConnectionString: ConnectionString)
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermissionLink> RolePermissionLinks { get; set; }
        public DbSet<UserRoleLink> UserRoleLinks { get; set; }
        public DbSet<PotentialRole> PotentialRoles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Party> Parties { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new PermissionMap());
            modelBuilder.Configurations.Add(new RolePermissionLinkMap());
            modelBuilder.Configurations.Add(new UserRoleLinkMap());
            modelBuilder.Configurations.Add(new PotentialRoleMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new PersonMap());
            modelBuilder.Configurations.Add(new PartyMap());
        }

        public override int SaveChanges()
        {
            ApplySaveRule();

            return base.SaveChanges();
        }
    }
}
