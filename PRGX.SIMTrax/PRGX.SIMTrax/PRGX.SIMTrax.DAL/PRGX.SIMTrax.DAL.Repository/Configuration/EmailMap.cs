using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class EmailMap : EntityTypeConfiguration<Email>
    {
        public EmailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.EmailAddress)
                .IsRequired()
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("Email");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.EmailAddress).HasColumnName("EmailAddress");
            this.Property(t => t.RefContactMethod).HasColumnName("RefContactMethod");

            // Relationships
            this.HasRequired(t => t.ContactMethod)
                .WithMany(t => t.Emails)
                .HasForeignKey(d => d.RefContactMethod);

        }
    }
}
