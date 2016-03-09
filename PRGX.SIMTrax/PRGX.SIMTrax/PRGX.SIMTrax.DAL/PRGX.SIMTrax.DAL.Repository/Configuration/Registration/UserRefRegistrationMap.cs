using PRGX.SIMTrax.DAL.Entity.Registration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository.Configuration.Registration
{
    public class UserRefRegistrationMap : EntityTypeConfiguration<UserRefRegistration>
    {
        public UserRefRegistrationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.RowVersion)
                .IsFixedLength()
                .HasMaxLength(8)
                .IsRowVersion();

            // Table & Column Mappings
            this.ToTable("User");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.NeedsPasswordChange).HasColumnName("NeedsPasswordChange");
            this.Property(t => t.UserType).HasColumnName("UserType");
            this.Property(t => t.RefPerson).HasColumnName("RefPerson");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.RefCreatedBy).HasColumnName("RefCreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.RefLastUpdatedBy).HasColumnName("RefLastUpdatedBy");
            this.Property(t => t.RowVersion).HasColumnName("RowVersion");

            // Relationships
            this.HasRequired(t => t.Person)
                .WithMany(t => t.UserRefRegistrations)
                .HasForeignKey(d => d.RefPerson);
        }
    }
}
