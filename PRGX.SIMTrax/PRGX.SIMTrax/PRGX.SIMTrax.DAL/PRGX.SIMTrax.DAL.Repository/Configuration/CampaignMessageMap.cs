using PRGX.SIMTrax.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class CampaignMessageMap : EntityTypeConfiguration<CampaignMessage>
    {
        public CampaignMessageMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.WelcomeMessage)
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("CampaignMessage");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.WelcomeMessage).HasColumnName("WelcomeMessage");
            this.Property(t => t.RefCampaign).HasColumnName("RefCampaign");
            this.Property(t => t.RefLocale).HasColumnName("RefLocale");

            // Relationships
            this.HasRequired(t => t.BuyerCampaign)
                .WithMany(t => t.CampaignMessages)
                .HasForeignKey(d => d.RefCampaign);
            this.HasOptional(t => t.Locale)
                .WithMany(t => t.CampaignMessages)
                .HasForeignKey(d => d.RefLocale);

        }
    }
}
