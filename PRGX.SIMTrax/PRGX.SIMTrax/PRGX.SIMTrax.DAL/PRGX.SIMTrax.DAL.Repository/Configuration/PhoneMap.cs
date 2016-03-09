using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class PhoneMap : EntityTypeConfiguration<Phone>
    {
        public PhoneMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.PhoneNumber)
                .IsRequired()
                .HasMaxLength(256);
            

            // Table & Column Mappings
            this.ToTable("Phone");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.RefContactMethod).HasColumnName("RefContactMethod");

            // Relationships
            this.HasRequired(t => t.ContactMethod)
                .WithMany(t => t.Phones)
                .HasForeignKey(d => d.RefContactMethod);

        }
    }
}
