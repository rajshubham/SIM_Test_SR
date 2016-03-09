using PRGX.SIMTrax.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class CampaignInvitationMap : EntityTypeConfiguration<CampaignInvitation>
    {
        public CampaignInvitationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.SupplierCompanyName)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.EmailAddress)
                .HasMaxLength(300);

            this.Property(t => t.RegistrationCode)
                .HasMaxLength(100);

            this.Property(t => t.FirstName)
                .HasMaxLength(80);

            this.Property(t => t.LastName)
                .HasMaxLength(80);

            this.Property(t => t.JobTitle)
                .HasMaxLength(50);

            this.Property(t => t.AddressLine1)
                .HasMaxLength(256);

            this.Property(t => t.AddressLine2)
                .HasMaxLength(256);

            this.Property(t => t.City)
                .HasMaxLength(128);

            this.Property(t => t.State)
                .HasMaxLength(128);

            this.Property(t => t.ZipCode)
                .HasMaxLength(15);

            this.Property(t => t.Telephone)
                .HasMaxLength(20);

            this.Property(t => t.Identifier1)
                .HasMaxLength(50);

            this.Property(t => t.IdentifierType1)
                .HasMaxLength(50);

            this.Property(t => t.Identifier2)
                .HasMaxLength(50);

            this.Property(t => t.IdentifierType2)
                .HasMaxLength(50);

            this.Property(t => t.Identifier3)
                .HasMaxLength(50);

            this.Property(t => t.IdentifierType3)
                .HasMaxLength(50);

            this.Property(t => t.UltimateParent)
                .HasMaxLength(255);

            this.Property(t => t.InvalidComments)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("CampaignInvitation");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RefCampaign).HasColumnName("RefCampaign");
            this.Property(t => t.SupplierCompanyName).HasColumnName("SupplierCompanyName");
            this.Property(t => t.EmailAddress).HasColumnName("EmailAddress");
            this.Property(t => t.RegistrationCode).HasColumnName("RegistrationCode");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.JobTitle).HasColumnName("JobTitle");
            this.Property(t => t.AddressLine1).HasColumnName("AddressLine1");
            this.Property(t => t.AddressLine2).HasColumnName("AddressLine2");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.RefCountry).HasColumnName("RefCountry");
            this.Property(t => t.ZipCode).HasColumnName("ZipCode");
            this.Property(t => t.Telephone).HasColumnName("Telephone");
            this.Property(t => t.Identifier1).HasColumnName("Identifier1");
            this.Property(t => t.IdentifierType1).HasColumnName("IdentifierType1");
            this.Property(t => t.Identifier2).HasColumnName("Identifier2");
            this.Property(t => t.IdentifierType2).HasColumnName("IdentifierType2");
            this.Property(t => t.Identifier3).HasColumnName("Identifier3");
            this.Property(t => t.IdentifierType3).HasColumnName("IdentifierType3");
            this.Property(t => t.IsSubsidary).HasColumnName("IsSubsidary");
            this.Property(t => t.UltimateParent).HasColumnName("UltimateParent");
            this.Property(t => t.IsValid).HasColumnName("IsValid");
            this.Property(t => t.InvalidComments).HasColumnName("InvalidComments");
            this.Property(t => t.IsRegistered).HasColumnName("IsRegistered");
            this.Property(t => t.RefCreatedBy).HasColumnName("RefCreatedBy");
            this.Property(t => t.RefLastUpdatedBy).HasColumnName("RefLastUpdatedBy");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.RefSupplier).HasColumnName("RefSupplier");

            // Relationships
            this.HasRequired(t => t.BuyerCampaign)
                .WithMany(t => t.CampaignInvitations)
                .HasForeignKey(d => d.RefCampaign);
            this.HasOptional(t => t.Supplier)
                .WithMany(t => t.CampaignInvitations)
                .HasForeignKey(d => d.RefSupplier);
            this.HasOptional(t => t.Region)
                .WithMany(t => t.CampaignInvitations)
                .HasForeignKey(d => d.RefCountry);

        }
    }
}
