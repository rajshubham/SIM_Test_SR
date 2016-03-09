using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class AddressMap : EntityTypeConfiguration<Address>
    {
        public AddressMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Line1)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.Line2)
                .HasMaxLength(256);

            this.Property(t => t.Line3)
                .HasMaxLength(256);

            this.Property(t => t.Zone)
                .HasMaxLength(100);

            this.Property(t => t.City)
                .HasMaxLength(128);

            this.Property(t => t.State)
                .HasMaxLength(128);

            this.Property(t => t.ZipCode)
                .HasMaxLength(15);

            this.Property(t => t.RowVersion)
                .IsFixedLength()
                .HasMaxLength(8)
                .IsRowVersion();

            // Table & Column Mappings
            this.ToTable("Address");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Line1).HasColumnName("Line1");
            this.Property(t => t.Line2).HasColumnName("Line2");
            this.Property(t => t.Line3).HasColumnName("Line3");
            this.Property(t => t.Zone).HasColumnName("Zone");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.RefCountryId).HasColumnName("RefCountryId");
            this.Property(t => t.ZipCode).HasColumnName("ZipCode");
            this.Property(t => t.AddressType).HasColumnName("AddressType");
            this.Property(t => t.RefContactMethod).HasColumnName("RefContactMethod");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.RefCreatedBy).HasColumnName("RefCreatedBy");
            this.Property(t => t.RefLastUpdatedBy).HasColumnName("RefLastUpdatedBy");
            this.Property(t => t.RowVersion).HasColumnName("RowVersion");

            // Relationships
            this.HasRequired(t => t.ContactMethod)
                .WithMany(t => t.Addresses)
                .HasForeignKey(d => d.RefContactMethod);
            this.HasOptional(t => t.Region)
                .WithMany(t => t.Addresses)
                .HasForeignKey(d => d.RefCountryId);

        }
    }
}
