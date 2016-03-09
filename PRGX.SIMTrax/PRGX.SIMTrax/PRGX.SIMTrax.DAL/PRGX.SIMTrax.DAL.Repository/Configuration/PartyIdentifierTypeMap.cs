using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class PartyIdentifierTypeMap : EntityTypeConfiguration<PartyIdentifierType>
    {
        public PartyIdentifierTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.IdentifierType)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("PartyIdentifierType");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.IdentifierType).HasColumnName("IdentifierType");
            this.Property(t => t.RefRegion).HasColumnName("RefRegion");

            // Relationships
            this.HasRequired(t => t.Region)
                .WithMany(t => t.PartyIdentifierTypes)
                .HasForeignKey(d => d.RefRegion);

        }
    }
}
