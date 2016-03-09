using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class PartyPartyLinkMap : EntityTypeConfiguration<PartyPartyLink>
    {
        public PartyPartyLinkMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.PartyPartyLinkType)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.PartyPartyLinkSubType)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.RowVersion)
                .IsFixedLength()
                .HasMaxLength(8)
                .IsRowVersion();

            // Table & Column Mappings
            this.ToTable("PartyPartyLink");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PartyPartyLinkType).HasColumnName("PartyPartyLinkType");
            this.Property(t => t.PartyPartyLinkSubType).HasColumnName("PartyPartyLinkSubType");
            this.Property(t => t.RefParty).HasColumnName("RefParty");
            this.Property(t => t.RefLinkedParty).HasColumnName("RefLinkedParty");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.RefCreatedBy).HasColumnName("RefCreatedBy");
            this.Property(t => t.RefLastUpdatedBy).HasColumnName("RefLastUpdatedBy");
            this.Property(t => t.RowVersion).HasColumnName("RowVersion");

            // Relationships
            this.HasRequired(t => t.Party)
                .WithMany(t => t.PartyPartyLinks)
                .HasForeignKey(d => d.RefLinkedParty);
            this.HasRequired(t => t.Party1)
                .WithMany(t => t.PartyPartyLinks1)
                .HasForeignKey(d => d.RefParty);
        }
    }
}
