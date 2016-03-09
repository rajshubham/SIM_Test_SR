using PRGX.SIMTrax.DAL.Entity;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class DiversityStatusTypeMap : EntityTypeConfiguration<DiversityStatusType>
    {
        public DiversityStatusTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("DiversityStatusType");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
        }
    }
}
