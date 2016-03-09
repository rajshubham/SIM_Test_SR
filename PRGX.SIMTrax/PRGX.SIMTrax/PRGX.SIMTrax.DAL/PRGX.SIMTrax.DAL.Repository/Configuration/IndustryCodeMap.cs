using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class IndustryCodeMap : EntityTypeConfiguration<IndustryCode>
    {
        public IndustryCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.SectorName)
                .HasMaxLength(250);

            this.Property(t => t.CodeNumber)
                .HasMaxLength(20);

            this.Property(t => t.CodeDescription)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("IndustryCode");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SectorName).HasColumnName("SectorName");
            this.Property(t => t.RefParentId).HasColumnName("RefParentId");
            this.Property(t => t.CodeNumber).HasColumnName("CodeNumber");
            this.Property(t => t.CodeDescription).HasColumnName("CodeDescription");
            this.Property(t => t.RefIndustryCodeSet).HasColumnName("RefIndustryCodeSet");

            // Relationships
            this.HasOptional(t => t.IndustryCode2)
                .WithMany(t => t.IndustryCode1)
                .HasForeignKey(d => d.RefParentId);
            this.HasRequired(t => t.IndustryCodeSet)
                .WithMany(t => t.IndustryCodes)
                .HasForeignKey(d => d.RefIndustryCodeSet);

        }
    }
}
