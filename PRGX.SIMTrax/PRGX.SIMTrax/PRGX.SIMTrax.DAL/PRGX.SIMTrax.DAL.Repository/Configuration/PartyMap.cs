using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class PartyMap : EntityTypeConfiguration<Party>
    {
        public PartyMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.PartyName)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.PartyType)
                .IsFixedLength()
                .HasMaxLength(50);

            this.Property(t => t.RowVersion)
                .IsFixedLength()
                .HasMaxLength(8)
                .IsRowVersion();

            // Table & Column Mappings
            this.ToTable("Party");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PartyName).HasColumnName("PartyName");
            this.Property(t => t.PartyType).HasColumnName("PartyType");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.ProjectSource).HasColumnName("ProjectSource");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.RefCreatedBy).HasColumnName("RefCreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.RefLastUpdatedBy).HasColumnName("RefLastUpdatedBy");
            this.Property(t => t.RowVersion).HasColumnName("RowVersion");
            
        }
    }
}
