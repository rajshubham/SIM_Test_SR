using PRGX.SIMTrax.DAL.Entity;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class DiversityStatusRegionMap : EntityTypeConfiguration<DiversityStatusRegion>
    {
        public DiversityStatusRegionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("DiversityStatusRegion");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RefDiversityStatus).HasColumnName("RefDiversityStatus");
            this.Property(t => t.RefRegion).HasColumnName("RefRegion");

            // Relationships
            this.HasRequired(t => t.DiversityStatu)
                .WithMany(t => t.DiversityStatusRegions)
                .HasForeignKey(d => d.RefDiversityStatus);
            this.HasRequired(t => t.Region)
                .WithMany(t => t.DiversityStatusRegions)
                .HasForeignKey(d => d.RefRegion);

        }
    }
}
