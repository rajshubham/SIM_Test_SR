using System;
using PRGX.SIMTrax.DAL.Entity;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class SupplierReferrerMap : EntityTypeConfiguration<SupplierReferrer>
    {
        public SupplierReferrerMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("SupplierReferrer");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RefCampaign).HasColumnName("RefCampaign");
            this.Property(t => t.RefSupplier).HasColumnName("RefSupplier");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.RefCreatedBy).HasColumnName("RefCreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.RefLastUpdatedBy).HasColumnName("RefLastUpdatedBy");
            this.Property(t => t.LandingReferrer).HasColumnName("LandingReferrer");

            // Relationships
            this.HasRequired(t => t.BuyerCampaign)
                .WithMany(t => t.SupplierReferrers)
                .HasForeignKey(d => d.RefCampaign);
            this.HasRequired(t => t.Supplier)
                .WithMany(t => t.SupplierReferrers)
                .HasForeignKey(d => d.RefSupplier);

        }
    }
}
