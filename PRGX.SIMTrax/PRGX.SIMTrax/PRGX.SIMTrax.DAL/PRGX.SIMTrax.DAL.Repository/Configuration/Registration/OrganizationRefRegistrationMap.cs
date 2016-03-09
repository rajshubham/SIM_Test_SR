using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using PRGX.SIMTrax.DAL.Entity.Registration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration.Registration
{
    public class OrganizationRefRegistrationMap : EntityTypeConfiguration<OrganizationRefRegistration>
    {
        public OrganizationRefRegistrationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.OrganizationType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("Organization");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.OrganizationType).HasColumnName("OrganizationType");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.RefCreatedBy).HasColumnName("RefCreatedBy");
            this.Property(t => t.RefLastUpdatedBy).HasColumnName("RefLastUpdatedBy");
            this.Property(t => t.RefParty).HasColumnName("RefParty");
            this.Property(t => t.RefLegalEntity).HasColumnName("RefLegalEntity");

            // Relationships
            this.HasRequired(t => t.Party)
                .WithMany(t => t.OrganizationRefRegistrations)
                .HasForeignKey(d => d.RefParty);
        }
    }
}
