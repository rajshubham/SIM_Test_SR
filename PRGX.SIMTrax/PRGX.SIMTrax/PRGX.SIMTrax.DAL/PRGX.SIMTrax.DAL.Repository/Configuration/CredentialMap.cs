using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class CredentialMap : EntityTypeConfiguration<Credential>
    {
        public CredentialMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.LoginId)
                .IsRequired()
                .HasMaxLength(300);

            this.Property(t => t.Password)
                .HasMaxLength(100);

            this.Property(t => t.RowVersion)
                .IsFixedLength()
                .HasMaxLength(8)
                .IsRowVersion();

            // Table & Column Mappings
            this.ToTable("Credential");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.LoginId).HasColumnName("LoginId");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.IsLocked).HasColumnName("IsLocked");
            this.Property(t => t.LoginAttemptCount).HasColumnName("LoginAttemptCount");
            this.Property(t => t.LockedTime).HasColumnName("LockedTime");
            this.Property(t => t.LastLoginDate).HasColumnName("LastLoginDate");
            this.Property(t => t.RefUser).HasColumnName("RefUser");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.RefCreatedBy).HasColumnName("RefCreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.RefLastUpdatedBy).HasColumnName("RefLastUpdatedBy");
            this.Property(t => t.RowVersion).HasColumnName("RowVersion");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.Credentials)
                .HasForeignKey(d => d.RefUser);

        }
    }
}
