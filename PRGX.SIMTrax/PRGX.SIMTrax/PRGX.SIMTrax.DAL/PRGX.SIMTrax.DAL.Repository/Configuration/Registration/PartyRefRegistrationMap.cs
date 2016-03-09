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
    public class PartyRefRegistrationMap : EntityTypeConfiguration<PartyRefRegistration>
    {
        public PartyRefRegistrationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.PartyName)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.PartyType)
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.RowVersion)
                .IsFixedLength()
                .HasMaxLength(8)
                .IsRowVersion();

            // Table & Column Mappings
            this.ToTable("Party");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PartyName).HasColumnName("PartyName");
            this.Property(t => t.PartyType).HasColumnName("PartyType");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.RefCreatedBy).HasColumnName("RefCreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.RefLastUpdatedBy).HasColumnName("RefLastUpdatedBy");
            this.Property(t => t.RowVersion).HasColumnName("RowVersion");
        }
    }
}
