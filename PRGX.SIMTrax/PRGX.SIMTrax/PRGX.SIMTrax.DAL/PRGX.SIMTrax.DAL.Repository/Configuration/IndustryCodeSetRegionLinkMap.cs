using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class IndustryCodeSetRegionLinkMap : EntityTypeConfiguration<IndustryCodeSetRegionLink>
    {
        public IndustryCodeSetRegionLinkMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("IndustryCodeSetRegionLink");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RefIndustryCodeSet).HasColumnName("RefIndustryCodeSet");
            this.Property(t => t.RefRegion).HasColumnName("RefRegion");

            // Relationships
            this.HasRequired(t => t.IndustryCodeSet)
                .WithMany(t => t.IndustryCodeSetRegionLinks)
                .HasForeignKey(d => d.RefIndustryCodeSet);
            this.HasRequired(t => t.Region)
                .WithMany(t => t.IndustryCodeSetRegionLinks)
                .HasForeignKey(d => d.RefRegion);

        }
    }
}
