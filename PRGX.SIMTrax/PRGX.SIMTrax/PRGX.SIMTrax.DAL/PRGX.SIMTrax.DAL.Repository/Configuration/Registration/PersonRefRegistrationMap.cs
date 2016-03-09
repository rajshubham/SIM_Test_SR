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
    public class PersonRefRegistrationMap : EntityTypeConfiguration<PersonRefRegistration>
    {
        public PersonRefRegistrationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(80);

            this.Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.JobTitle)
                .HasMaxLength(50);

            this.Property(t => t.PersonType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.RowVersion)
                .IsFixedLength()
                .HasMaxLength(8)
                .IsRowVersion();

            // Table & Column Mappings
            this.ToTable("Person");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.JobTitle).HasColumnName("JobTitle");
            this.Property(t => t.PersonType).HasColumnName("PersonType");
            this.Property(t => t.RefParty).HasColumnName("RefParty");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.RefCreatedBy).HasColumnName("RefCreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.RefLastUpdatedBy).HasColumnName("RefLastUpdatedBy");
            this.Property(t => t.RowVersion).HasColumnName("RowVersion");

            // Relationships
            this.HasRequired(t => t.Party)
                .WithMany(t => t.PeopleRefRegistration)
                .HasForeignKey(d => d.RefParty);
        }
    }
}
