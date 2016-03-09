using PRGX.SIMTrax.DAL.Entity;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class LegalEntityProfileMap : EntityTypeConfiguration<LegalEntityProfile>
    {
        public LegalEntityProfileMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ProfileType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("LegalEntityProfile");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ProfileType).HasColumnName("ProfileType");
            this.Property(t => t.RefPartyId).HasColumnName("RefPartyId");
            this.Property(t => t.RefContactMethod).HasColumnName("RefContactMethod");
            this.Property(t => t.RefBank).HasColumnName("RefBank");

            // Relationships
            this.HasOptional(t => t.BankAccount)
                .WithMany(t => t.LegalEntityProfiles)
                .HasForeignKey(d => d.RefBank);
            this.HasOptional(t => t.ContactMethod)
                .WithMany(t => t.LegalEntityProfiles)
                .HasForeignKey(d => d.RefContactMethod);
            this.HasRequired(t => t.Party)
                .WithMany(t => t.LegalEntityProfiles)
                .HasForeignKey(d => d.RefPartyId);

        }
    }
}
