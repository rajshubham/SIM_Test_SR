using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class EmailTemplateMap : EntityTypeConfiguration<EmailTemplate>
    {
        public EmailTemplateMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Mnemonic)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(50);

            this.Property(t => t.Content)
                .IsRequired();

            this.Property(t => t.Subject)
                .IsRequired()
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("EmailTemplate");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Mnemonic).HasColumnName("Mnemonic");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.RefLocale).HasColumnName("RefLocale");

            // Relationships
            this.HasOptional(t => t.Locale)
                .WithMany(t => t.EmailTemplates)
                .HasForeignKey(d => d.RefLocale);

        }
    }
}
