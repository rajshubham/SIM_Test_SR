using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class LocaleMap : EntityTypeConfiguration<Locale>
    {
        public LocaleMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ISOCode)
                .IsFixedLength()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Locale");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ISOCode).HasColumnName("ISOCode");
        }
    }
}
