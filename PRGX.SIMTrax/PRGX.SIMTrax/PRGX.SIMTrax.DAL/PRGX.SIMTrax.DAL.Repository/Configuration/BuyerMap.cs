using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class BuyerMap : EntityTypeConfiguration<Buyer>
    {
        public BuyerMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.RowVersion)
                .IsFixedLength()
                .HasMaxLength(8)
                .IsRowVersion();

            // Table & Column Mappings
            this.ToTable("Buyer");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.MaxCampaignSupplierCount).HasColumnName("MaxCampaignSupplierCount");
            this.Property(t => t.ActivationDate).HasColumnName("ActivationDate");
            this.Property(t => t.FirstSignInDate).HasColumnName("FirstSignInDate");
            this.Property(t => t.HasPaid).HasColumnName("HasPaid");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.RefCreatedBy).HasColumnName("RefCreatedBy");
            this.Property(t => t.RefLastUpdatedBy).HasColumnName("RefLastUpdatedBy");
            this.Property(t => t.RowVersion).HasColumnName("RowVersion");

            // Relationships
            this.HasRequired(t => t.Organization)
                .WithOptional(t => t.Buyer);

        }
    }
}
