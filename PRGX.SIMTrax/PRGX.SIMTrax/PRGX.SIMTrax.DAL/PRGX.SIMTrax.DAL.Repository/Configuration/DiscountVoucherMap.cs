using PRGX.SIMTrax.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class DiscountVoucherMap : EntityTypeConfiguration<DiscountVoucher>
    {
        public DiscountVoucherMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.PromotionalCode)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("DiscountVoucher");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PromotionalCode).HasColumnName("PromotionalCode");
            this.Property(t => t.DiscountPercent).HasColumnName("DiscountPercent");
            this.Property(t => t.PromotionStartDate).HasColumnName("PromotionStartDate");
            this.Property(t => t.PromotionEndDate).HasColumnName("PromotionEndDate");
            this.Property(t => t.RefBuyer).HasColumnName("RefBuyer");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.RefCreatedBy).HasColumnName("RefCreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.RefLastUpdatedBy).HasColumnName("RefLastUpdatedBy");

            // Relationships
            this.HasOptional(t => t.Buyer)
                .WithMany(t => t.DiscountVouchers)
                .HasForeignKey(d => d.RefBuyer);

        }
    }
}
