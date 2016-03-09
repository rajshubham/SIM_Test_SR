using PRGX.SIMTrax.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class PotentialRoleMap : EntityTypeConfiguration<PotentialRole>
    {
        public PotentialRoleMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("PotentialRole");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RefExistingRole).HasColumnName("RefExistingRole");
            this.Property(t => t.RefPotentialRole).HasColumnName("RefPotentialRole");

            // Relationships
            this.HasRequired(t => t.Role)
                .WithMany(t => t.PotentialRoles)
                .HasForeignKey(d => d.RefExistingRole);
            this.HasRequired(t => t.Role1)
                .WithMany(t => t.PotentialRoles1)
                .HasForeignKey(d => d.RefPotentialRole);

        }
    }
}
