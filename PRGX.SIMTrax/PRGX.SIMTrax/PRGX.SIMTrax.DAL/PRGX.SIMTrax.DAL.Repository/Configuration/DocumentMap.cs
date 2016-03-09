using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{ 
    public class DocumentMap : EntityTypeConfiguration<Document>
    {
        public DocumentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.FileName)
                .HasMaxLength(100);

            this.Property(t => t.FilePath)
                .HasMaxLength(1000);

            this.Property(t => t.ContentType)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("Document");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.FileName).HasColumnName("FileName");
            this.Property(t => t.FilePath).HasColumnName("FilePath");
            this.Property(t => t.ContentLength).HasColumnName("ContentLength");
            this.Property(t => t.ContentType).HasColumnName("ContentType");
        }
    }
}
