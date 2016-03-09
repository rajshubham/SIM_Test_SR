using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository.Context
{
    public partial class EmailContext : GenericContext
    {
        static EmailContext()
        {
            Database.SetInitializer<EmailContext>(null);
        }

        //TODO: read it from Util.Constants
        private static string ConnectionString { get {
                //TODO: remove hardcoded value
                return "SIMTraxContext";
                //return Domain.Util.Configuration.ConnectionString;
            }
        }

        public EmailContext()
            : base(nameOrConnectionString: ConnectionString)
        {
        }

        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<EmailAudit> EmailAudits { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new EmailTemplateMap());
            modelBuilder.Configurations.Add(new EmailAuditMap());
            ;




        }

        public override int SaveChanges()
        {
            ApplySaveRule();

            return base.SaveChanges();
        }
    }
}
