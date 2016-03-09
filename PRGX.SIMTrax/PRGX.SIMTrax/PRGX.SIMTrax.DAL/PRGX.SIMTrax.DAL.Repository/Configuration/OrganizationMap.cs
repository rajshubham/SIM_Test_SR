using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class OrganizationMap : EntityTypeConfiguration<Organization>
    {
        public OrganizationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.OrganizationType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(50);

            this.Property(t => t.RowVersion)
                .IsFixedLength()
                .HasMaxLength(8)
                .IsRowVersion();

            // Table & Column Mappings
            this.ToTable("Organization");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.OrganizationType).HasColumnName("OrganizationType");
            this.Property(t => t.EmployeeSize).HasColumnName("EmployeeSize");
            this.Property(t => t.TurnOverSize).HasColumnName("TurnOverSize");
            this.Property(t => t.RefLogoDocument).HasColumnName("RefLogoDocument");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.BusinessSectorId).HasColumnName("BusinessSectorId");
            this.Property(t => t.BusinessSectorDescription).HasColumnName("BusinessSectorDescription");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.RefCreatedBy).HasColumnName("RefCreatedBy");
            this.Property(t => t.RefLastUpdatedBy).HasColumnName("RefLastUpdatedBy");
            this.Property(t => t.RegistrationSubmittedOn).HasColumnName("RegistrationSubmittedOn");
            this.Property(t => t.RegistrationVerifiedOn).HasColumnName("RegistrationVerifiedOn");
            this.Property(t => t.ProfileVerifiedOn).HasColumnName("ProfileVerifiedOn");
            this.Property(t => t.RefProfileVerifiedBy).HasColumnName("RefProfileVerifiedBy");
            this.Property(t => t.PublishedDate).HasColumnName("PublishedDate");
            this.Property(t => t.CSIVerificationStatus).HasColumnName("CSIVerificationStatus");
            this.Property(t => t.RowVersion).HasColumnName("RowVersion");
            this.Property(t => t.RefLegalEntity).HasColumnName("RefLegalEntity");

            // Relationships
            this.HasOptional(t => t.Document)
                .WithMany(t => t.Organizations)
                .HasForeignKey(d => d.RefLogoDocument);
            this.HasOptional(t => t.LegalEntity)
                .WithMany(t => t.Organizations)
                .HasForeignKey(d => d.RefLegalEntity);
            this.HasRequired(t => t.Party)
                .WithOptional(t => t.Organization);
            this.HasOptional(t => t.Party1)
                .WithMany(t => t.Organizations1)
                .HasForeignKey(d => d.RefProfileVerifiedBy);

        }
    }
}
