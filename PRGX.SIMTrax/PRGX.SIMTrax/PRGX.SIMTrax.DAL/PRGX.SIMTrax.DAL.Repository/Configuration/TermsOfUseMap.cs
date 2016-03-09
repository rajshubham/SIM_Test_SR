using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class TermsOfUseMap : EntityTypeConfiguration<TermsOfUse>
    {
        public TermsOfUseMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Version)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TermsOfUse");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.HTMLText).HasColumnName("HTMLText");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.RefCreatedBy).HasColumnName("RefCreatedBy");
        }
    }
}
