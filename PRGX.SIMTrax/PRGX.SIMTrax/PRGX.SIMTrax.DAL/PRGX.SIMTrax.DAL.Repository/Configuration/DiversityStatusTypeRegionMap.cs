using PRGX.SIMTrax.DAL.Entity;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class DiversityStatusTypeRegionMap : EntityTypeConfiguration<DiversityStatusTypeRegion>
    {
        public DiversityStatusTypeRegionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("DiversityStatusTypeRegion");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RefDiversityStatusType).HasColumnName("RefDiversityStatusType");
            this.Property(t => t.RefApplicableRegion).HasColumnName("RefApplicableRegion");
            this.Property(t => t.IsApplicableInSubRegion).HasColumnName("IsApplicableInSubRegion");

            // Relationships
            this.HasRequired(t => t.DiversityStatusType)
                .WithMany(t => t.DiversityStatusTypeRegions)
                .HasForeignKey(d => d.RefDiversityStatusType);
            this.HasRequired(t => t.Region)
                .WithMany(t => t.DiversityStatusTypeRegions)
                .HasForeignKey(d => d.RefApplicableRegion);

        }
    }
}
