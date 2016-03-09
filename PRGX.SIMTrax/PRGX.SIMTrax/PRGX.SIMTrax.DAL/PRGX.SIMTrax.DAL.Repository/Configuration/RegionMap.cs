using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class RegionMap : EntityTypeConfiguration<Region>
    {
        public RegionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.RegionType)
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("Region");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.RegionType).HasColumnName("RegionType");
            this.Property(t => t.RefParentRegion).HasColumnName("RefParentRegion");

            // Relationships
            this.HasRequired(t => t.Region2)
                .WithMany(t => t.Region1)
                .HasForeignKey(d => d.RefParentRegion);

        }
    }
}
