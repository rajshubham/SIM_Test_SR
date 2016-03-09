using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class TemporaryPasswordUrlMap : EntityTypeConfiguration<TemporaryPasswordUrl>
    {
        public TemporaryPasswordUrlMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.PasswordURL)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1000);

            this.Property(t => t.Token)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("TemporaryPasswordUrl");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PasswordURL).HasColumnName("PasswordURL");
            this.Property(t => t.RefUser).HasColumnName("RefUser");
            this.Property(t => t.Token).HasColumnName("Token");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.TemporaryPasswordUrls)
                .HasForeignKey(d => d.RefUser);
        }
    }
}
