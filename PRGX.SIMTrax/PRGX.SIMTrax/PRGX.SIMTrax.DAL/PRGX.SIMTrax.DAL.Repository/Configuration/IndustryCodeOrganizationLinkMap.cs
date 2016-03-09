using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class IndustryCodeOrganizationLinkMap : EntityTypeConfiguration<IndustryCodeOrganizationLink>
    {
        public IndustryCodeOrganizationLinkMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Table & Column Mappings
            this.ToTable("IndustryCodeOrganizationLink");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RefOrganization).HasColumnName("RefOrganization");
            this.Property(t => t.RefIndustryCode).HasColumnName("RefIndustryCode");

            // Relationships
            this.HasRequired(t => t.IndustryCode)
                .WithMany(t => t.IndustryCodeOrganizationLinks)
                .HasForeignKey(d => d.RefIndustryCode);
            this.HasRequired(t => t.Organization)
                .WithMany(t => t.IndustryCodeOrganizationLinks)
                .HasForeignKey(d => d.RefOrganization);

        }
    }
}
