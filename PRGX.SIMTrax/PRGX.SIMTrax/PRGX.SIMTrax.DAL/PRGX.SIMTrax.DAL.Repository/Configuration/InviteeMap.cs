using PRGX.SIMTrax.DAL.Entity;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class InviteeMap : EntityTypeConfiguration<Invitee>
    {
        public InviteeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ClientName)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(100);

            this.Property(t => t.ContactName)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(100);

            this.Property(t => t.JobTitle)
                .IsFixedLength()
                .HasMaxLength(200);

            this.Property(t => t.Email)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(200);

            this.Property(t => t.PhoneNumber)
                .IsFixedLength()
                .HasMaxLength(100);

            this.Property(t => t.MailingAddress)
                .IsFixedLength()
                .HasMaxLength(300);

            this.Property(t => t.Fax)
                .IsFixedLength()
                .HasMaxLength(100);

            this.Property(t => t.ClientRole)
                .IsFixedLength()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("Invitee");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ClientName).HasColumnName("ClientName");
            this.Property(t => t.ContactName).HasColumnName("ContactName");
            this.Property(t => t.RefSupplier).HasColumnName("RefSupplier");
            this.Property(t => t.JobTitle).HasColumnName("JobTitle");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            this.Property(t => t.MailingAddress).HasColumnName("MailingAddress");
            this.Property(t => t.Fax).HasColumnName("Fax");
            this.Property(t => t.CanWeContact).HasColumnName("CanWeContact");
            this.Property(t => t.ClientRole).HasColumnName("ClientRole");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.RefCreatedBy).HasColumnName("RefCreatedBy");
            this.Property(t => t.RefLastUpdatedBy).HasColumnName("RefLastUpdatedBy");
            this.Property(t => t.RefReferee).HasColumnName("RefReferee");

            // Relationships
            this.HasOptional(t => t.Supplier)
                .WithMany(t => t.Invitees)
                .HasForeignKey(d => d.RefSupplier);
            this.HasRequired(t => t.Supplier1)
    .WithMany(t => t.Invitees1)
    .HasForeignKey(d => d.RefReferee);

        }
    }
}
