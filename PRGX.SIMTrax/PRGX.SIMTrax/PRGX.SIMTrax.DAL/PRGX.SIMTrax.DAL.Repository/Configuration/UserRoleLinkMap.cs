using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class UserRoleLinkMap : EntityTypeConfiguration<UserRoleLink>
    {
        public UserRoleLinkMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Table & Column Mappings
            this.ToTable("UserRoleLink");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RefUser).HasColumnName("RefUser");
            this.Property(t => t.RefRole).HasColumnName("RefRole");

            // Relationships
            this.HasRequired(t => t.Role)
                .WithMany(t => t.UserRoleLinks)
                .HasForeignKey(d => d.RefRole);
            this.HasRequired(t => t.User)
                .WithMany(t => t.UserRoleLinks)
                .HasForeignKey(d => d.RefUser);

        }
    }
}
