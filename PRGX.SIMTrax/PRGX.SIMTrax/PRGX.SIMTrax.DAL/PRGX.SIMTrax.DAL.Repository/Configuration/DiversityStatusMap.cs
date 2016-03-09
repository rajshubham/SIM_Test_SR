using PRGX.SIMTrax.DAL.Entity;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class DiversityStatusMap : EntityTypeConfiguration<DiversityStatus>
    {
        public DiversityStatusMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("DiversityStatus");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RefSupplier).HasColumnName("RefSupplier");
            this.Property(t => t.RefDiversityStatusType).HasColumnName("RefDiversityStatusType");

            // Relationships
            this.HasRequired(t => t.DiversityStatusType)
                .WithMany(t => t.DiversityStatus)
                .HasForeignKey(d => d.RefDiversityStatusType);
            this.HasRequired(t => t.Supplier)
                .WithMany(t => t.DiversityStatus)
                .HasForeignKey(d => d.RefSupplier);

        }
    }
}
