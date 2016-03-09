using PRGX.SIMTrax.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class BuyerCampaignMap : EntityTypeConfiguration<BuyerCampaign>
    {
        public BuyerCampaignMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.CampaignName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.CampaignUrl)
                .HasMaxLength(256);

            this.Property(t => t.DataSource)
                .HasMaxLength(256);

            this.Property(t => t.Notes)
                .HasMaxLength(1000);

            this.Property(t => t.PreRegisteredFilePath)
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("BuyerCampaign");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CampaignName).HasColumnName("CampaignName");
            this.Property(t => t.CampaignStartDate).HasColumnName("CampaignStartDate");
            this.Property(t => t.CampaignEndDate).HasColumnName("CampaignEndDate");
            this.Property(t => t.SupplierCount).HasColumnName("SupplierCount");
            this.Property(t => t.CampaignUrl).HasColumnName("CampaignUrl");
            this.Property(t => t.CampaignType).HasColumnName("CampaignType");
            this.Property(t => t.CampaignStatus).HasColumnName("CampaignStatus");
            this.Property(t => t.TemplateType).HasColumnName("TemplateType");
            this.Property(t => t.DataSource).HasColumnName("DataSource");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.RefEmailTemplate).HasColumnName("RefEmailTemplate");
            this.Property(t => t.IsDownloaded).HasColumnName("IsDownloaded");
            this.Property(t => t.PreRegisteredFilePath).HasColumnName("PreRegisteredFilePath");
            this.Property(t => t.RefAuditorId).HasColumnName("RefAuditorId");
            this.Property(t => t.RefCampaignLogo).HasColumnName("RefCampaignLogo");
            this.Property(t => t.RefBuyer).HasColumnName("RefBuyer");
            this.Property(t => t.RefDiscountVoucher).HasColumnName("RefDiscountVoucher");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.RefCreatedBy).HasColumnName("RefCreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.RefLastUpdatedBy).HasColumnName("RefLastUpdatedBy");

            // Relationships
            this.HasOptional(t => t.Buyer)
                .WithMany(t => t.BuyerCampaigns)
                .HasForeignKey(d => d.RefBuyer);
            this.HasOptional(t => t.DiscountVoucher)
                .WithMany(t => t.BuyerCampaigns)
                .HasForeignKey(d => d.RefDiscountVoucher);
            this.HasOptional(t => t.Document)
                .WithMany(t => t.BuyerCampaigns)
                .HasForeignKey(d => d.RefCampaignLogo);
            this.HasOptional(t => t.User)
                .WithMany(t => t.BuyerCampaigns)
                .HasForeignKey(d => d.RefAuditorId);
            this.HasOptional(t => t.EmailTemplate)
                .WithMany(t => t.BuyerCampaigns)
                .HasForeignKey(d => d.RefEmailTemplate);
        }
    }
}
