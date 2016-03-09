using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{ 
    public class SupplierMap : EntityTypeConfiguration<Supplier>
    {
        public SupplierMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.WebsiteLink)
                .HasMaxLength(300);

            this.Property(t => t.BusinessDescription)
                .HasMaxLength(1200);

            this.Property(t => t.TradingName)
                .HasMaxLength(256);

            this.Property(t => t.EstablishedYear)
                .HasMaxLength(50);

            this.Property(t => t.FacebookAccount)
                .HasMaxLength(300);

            this.Property(t => t.TwitterAccount)
                .HasMaxLength(300);

            this.Property(t => t.LinkedInAccount)
                .HasMaxLength(300);

            this.Property(t => t.MaxContractValue)
                .HasMaxLength(50);

            this.Property(t => t.MinContractValue)
                .HasMaxLength(50);

            this.Property(t => t.UltimateParent)
                .HasMaxLength(200);

            this.Property(t => t.RowVersion)
                .IsFixedLength()
                .HasMaxLength(8)
                .IsRowVersion();

            // Table & Column Mappings
            this.ToTable("Supplier");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.TypeOfSeller).HasColumnName("TypeOfSeller");
            this.Property(t => t.WebsiteLink).HasColumnName("WebsiteLink");
            this.Property(t => t.BusinessDescription).HasColumnName("BusinessDescription");
            this.Property(t => t.TradingName).HasColumnName("TradingName");
            this.Property(t => t.RegisteredCountry).HasColumnName("RegisteredCountry");
            this.Property(t => t.EstablishedYear).HasColumnName("EstablishedYear");
            this.Property(t => t.FacebookAccount).HasColumnName("FacebookAccount");
            this.Property(t => t.TwitterAccount).HasColumnName("TwitterAccount");
            this.Property(t => t.LinkedInAccount).HasColumnName("LinkedInAccount");
            this.Property(t => t.MaxContractValue).HasColumnName("MaxContractValue");
            this.Property(t => t.MinContractValue).HasColumnName("MinContractValue");
            this.Property(t => t.IsSubsidary).HasColumnName("IsSubsidary");
            this.Property(t => t.UltimateParent).HasColumnName("UltimateParent");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.RefCreatedBy).HasColumnName("RefCreatedBy");
            this.Property(t => t.RefLastUpdatedBy).HasColumnName("RefLastUpdatedBy");
            this.Property(t => t.RowVersion).HasColumnName("RowVersion");

            // Relationships
            this.HasRequired(t => t.Organization)
                .WithOptional(t => t.Supplier);
        }
    }
}
