using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class AcceptedTermsOfUseMap : EntityTypeConfiguration<AcceptedTermsOfUse>
    {
        public AcceptedTermsOfUseMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Table & Column Mappings
            this.ToTable("AcceptedTermsOfUse");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RefTermsOfUse).HasColumnName("RefTermsOfUse");
            this.Property(t => t.RefAcceptingUser).HasColumnName("RefAcceptingUser");
            this.Property(t => t.AcceptedDate).HasColumnName("AcceptedDate");

            // Relationships
            this.HasRequired(t => t.TermsOfUse)
                .WithMany(t => t.AcceptedTermsOfUses)
                .HasForeignKey(d => d.RefTermsOfUse);
            this.HasRequired(t => t.User)
                .WithMany(t => t.AcceptedTermsOfUses)
                .HasForeignKey(d => d.RefAcceptingUser);

        }
    }
}
