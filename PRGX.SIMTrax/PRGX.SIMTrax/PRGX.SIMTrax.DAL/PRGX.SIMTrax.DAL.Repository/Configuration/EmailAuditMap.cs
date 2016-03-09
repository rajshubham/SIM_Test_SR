using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class EmailAuditMap : EntityTypeConfiguration<EmailAudit>
    {
        public EmailAuditMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.From)
                .HasMaxLength(100);

            this.Property(t => t.To)
                .HasMaxLength(100);

            this.Property(t => t.cc)
                .HasMaxLength(100);

            this.Property(t => t.Bcc)
                .HasMaxLength(100);

            this.Property(t => t.Subject)
                .HasMaxLength(700);

            // Table & Column Mappings
            this.ToTable("EmailAudit");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.From).HasColumnName("From");
            this.Property(t => t.To).HasColumnName("To");
            this.Property(t => t.cc).HasColumnName("cc");
            this.Property(t => t.Bcc).HasColumnName("Bcc");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.Body).HasColumnName("Body");
            this.Property(t => t.SentBy).HasColumnName("SentBy");
            this.Property(t => t.IsEmailSent).HasColumnName("IsEmailSent");
            this.Property(t => t.SentDate).HasColumnName("SentDate");

            // Relationships
            this.HasOptional(t => t.Party)
                .WithMany(t => t.EmailAudits)
                .HasForeignKey(d => d.SentBy);

        }
    }
}
