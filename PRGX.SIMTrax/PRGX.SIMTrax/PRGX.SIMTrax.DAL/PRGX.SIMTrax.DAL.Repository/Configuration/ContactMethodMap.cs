using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class ContactMethodMap : EntityTypeConfiguration<ContactMethod>
    {
        public ContactMethodMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ContactMethodType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ContactMethod");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ContactMethodType).HasColumnName("ContactMethodType");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.RefCreatedBy).HasColumnName("RefCreatedBy");
            this.Property(t => t.RefLastUpdatedBy).HasColumnName("RefLastUpdatedBy");
        }
    }
}
