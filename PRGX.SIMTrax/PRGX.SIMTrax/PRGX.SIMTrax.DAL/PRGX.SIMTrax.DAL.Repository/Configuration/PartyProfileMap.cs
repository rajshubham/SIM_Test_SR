using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Entity.Models.Mapping
{
    public class PartyProfileMap : EntityTypeConfiguration<PartyProfile>
    {
        public PartyProfileMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Type)
                .HasMaxLength(100);

            this.Property(t => t.Explanation)
                .HasMaxLength(256);

            this.Property(t => t.RowVersion)
                .IsFixedLength()
                .HasMaxLength(8)
                .IsRowVersion();

            // Table & Column Mappings
            this.ToTable("PartyProfile");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RefSubjectId).HasColumnName("RefSubjectId");
            this.Property(t => t.RefAccessorId).HasColumnName("RefAccessorId");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Explanation).HasColumnName("Explanation");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.RefCreatedBy).HasColumnName("RefCreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.RefLastUpdatedBy).HasColumnName("RefLastUpdatedBy");
            this.Property(t => t.RowVersion).HasColumnName("RowVersion");

            // Relationships
            this.HasRequired(t => t.Party)
                .WithMany(t => t.PartyProfiles)
                .HasForeignKey(d => d.RefAccessorId);
            this.HasRequired(t => t.Party1)
                .WithMany(t => t.PartyProfiles1)
                .HasForeignKey(d => d.RefSubjectId);

        }
    }
}
