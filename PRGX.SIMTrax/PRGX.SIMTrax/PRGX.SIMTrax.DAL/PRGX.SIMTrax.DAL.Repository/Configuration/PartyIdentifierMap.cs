using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class PartyIdentifierMap : EntityTypeConfiguration<PartyIdentifier>
    {
        public PartyIdentifierMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.IdentifierNumber)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.RowVersion)
                .IsFixedLength()
                .HasMaxLength(8)
                .IsRowVersion();

            // Table & Column Mappings
            this.ToTable("PartyIdentifier");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RefPartyIdentifierType).HasColumnName("RefPartyIdentifierType");
            this.Property(t => t.IdentifierNumber).HasColumnName("IdentifierNumber");
            this.Property(t => t.RefParty).HasColumnName("RefParty");
            this.Property(t => t.RefRegion).HasColumnName("RefRegion");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.RefCreatedBy).HasColumnName("RefCreatedBy");
            this.Property(t => t.RefLastUpdatedBy).HasColumnName("RefLastUpdatedBy");
            this.Property(t => t.RowVersion).HasColumnName("RowVersion");

            // Relationships
            this.HasRequired(t => t.Party)
                .WithMany(t => t.PartyIdentifiers)
                .HasForeignKey(d => d.RefParty);
            this.HasRequired(t => t.PartyIdentifierType)
                .WithMany(t => t.PartyIdentifiers)
                .HasForeignKey(d => d.RefPartyIdentifierType);
            this.HasRequired(t => t.Region)
                .WithMany(t => t.PartyIdentifiers)
                .HasForeignKey(d => d.RefRegion);

        }
    }
}
