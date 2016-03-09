using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class MasterDataMap : EntityTypeConfiguration<MasterData>
    {
        public MasterDataMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(120);

            // Table & Column Mappings
            this.ToTable("MasterData");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RefMasterDataType).HasColumnName("RefMasterDataType");
            this.Property(t => t.Value).HasColumnName("Value");
            this.Property(t => t.OrderId).HasColumnName("OrderId");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.RefLocaleId).HasColumnName("RefLocaleId");

            // Relationships
            this.HasOptional(t => t.Locale)
                .WithMany(t => t.MasterDatas)
                .HasForeignKey(d => d.RefLocaleId);
            this.HasRequired(t => t.MasterDataType)
                .WithMany(t => t.MasterDatas)
                .HasForeignKey(d => d.RefMasterDataType);

        }
    }
}
