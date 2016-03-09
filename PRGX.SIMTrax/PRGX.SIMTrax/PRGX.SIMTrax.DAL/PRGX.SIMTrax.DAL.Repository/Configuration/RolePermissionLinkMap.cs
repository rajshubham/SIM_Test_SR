using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class RolePermissionLinkMap : EntityTypeConfiguration<RolePermissionLink>
    {
        public RolePermissionLinkMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Table & Column Mappings
            this.ToTable("RolePermissionLink");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RefPermission).HasColumnName("RefPermission");
            this.Property(t => t.RefRole).HasColumnName("RefRole");

            // Relationships
            this.HasRequired(t => t.Permission)
                .WithMany(t => t.RolePermissionLinks)
                .HasForeignKey(d => d.RefPermission);
            this.HasRequired(t => t.Role)
                .WithMany(t => t.RolePermissionLinks)
                .HasForeignKey(d => d.RefRole);

        }
    }
}
