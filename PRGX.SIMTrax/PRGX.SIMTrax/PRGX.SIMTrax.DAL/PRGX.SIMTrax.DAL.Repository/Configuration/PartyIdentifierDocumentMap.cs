using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class PartyIdentifierDocumentMap : EntityTypeConfiguration<PartyIdentifierDocument>
    {
        public PartyIdentifierDocumentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.RowVersion)
                .IsFixedLength()
                .HasMaxLength(8)
                .IsRowVersion();

            // Table & Column Mappings
            this.ToTable("PartyIdentifierDocument");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RefPartyIdentifier).HasColumnName("RefPartyIdentifier");
            this.Property(t => t.RefDocument).HasColumnName("RefDocument");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.RefCreatedBy).HasColumnName("RefCreatedBy");
            this.Property(t => t.RefLastUpdatedBy).HasColumnName("RefLastUpdatedBy");
            this.Property(t => t.RowVersion).HasColumnName("RowVersion");

            // Relationships
            this.HasRequired(t => t.Document)
                .WithMany(t => t.PartyIdentifierDocuments)
                .HasForeignKey(d => d.RefDocument);
            this.HasRequired(t => t.PartyIdentifier)
                .WithMany(t => t.PartyIdentifierDocuments)
                .HasForeignKey(d => d.RefPartyIdentifier);

        }
    }
}
