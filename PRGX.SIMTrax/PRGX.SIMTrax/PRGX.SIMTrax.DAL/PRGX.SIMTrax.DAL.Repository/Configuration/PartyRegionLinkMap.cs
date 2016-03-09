using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class PartyRegionLinkMap : EntityTypeConfiguration<PartyRegionLink>
    {
        public PartyRegionLinkMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.LinkType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("PartyRegionLink");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.LinkType).HasColumnName("LinkType");
            this.Property(t => t.RefParty).HasColumnName("RefParty");
            this.Property(t => t.RefRegion).HasColumnName("RefRegion");

            // Relationships
            this.HasRequired(t => t.Party)
                .WithMany(t => t.PartyRegionLinks)
                .HasForeignKey(d => d.RefParty);
            this.HasRequired(t => t.Region)
                .WithMany(t => t.PartyRegionLinks)
                .HasForeignKey(d => d.RefRegion);

        }
    }
}
