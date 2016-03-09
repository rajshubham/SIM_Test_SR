using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class BankAccountMap : EntityTypeConfiguration<BankAccount>
    {
        public BankAccountMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.AccountName)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(200);

            this.Property(t => t.AccountNumber)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(200);

            this.Property(t => t.SwiftCode)
                .IsFixedLength()
                .HasMaxLength(200);

            this.Property(t => t.IBAN)
                .IsFixedLength()
                .HasMaxLength(200);

            this.Property(t => t.BankName)
                .IsFixedLength()
                .HasMaxLength(200);

            this.Property(t => t.PreferredMode)
                .IsFixedLength()
                .HasMaxLength(200);

            this.Property(t => t.BranchIdentifier)
                .IsFixedLength()
                .HasMaxLength(200);

            this.Property(t => t.RowVersion)
                .IsFixedLength()
                .HasMaxLength(8)
                .IsRowVersion();

            // Table & Column Mappings
            this.ToTable("BankAccount");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RefLegalEntity).HasColumnName("RefLegalEntity");
            this.Property(t => t.AccountName).HasColumnName("AccountName");
            this.Property(t => t.AccountNumber).HasColumnName("AccountNumber");
            this.Property(t => t.SwiftCode).HasColumnName("SwiftCode");
            this.Property(t => t.IBAN).HasColumnName("IBAN");
            this.Property(t => t.BankName).HasColumnName("BankName");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.RefCountryId).HasColumnName("RefCountryId");
            this.Property(t => t.PreferredMode).HasColumnName("PreferredMode");
            this.Property(t => t.BranchIdentifier).HasColumnName("BranchIdentifier");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.RefCreatedBy).HasColumnName("RefCreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.RefLastUpdatedBy).HasColumnName("RefLastUpdatedBy");
            this.Property(t => t.RowVersion).HasColumnName("RowVersion");

            this.HasRequired(t => t.Region)
                .WithMany(t => t.BankAccounts)
                .HasForeignKey(d => d.RefCountryId);
            this.HasRequired(t => t.LegalEntity)
                .WithMany(t => t.BankAccounts)
                .HasForeignKey(d => d.RefLegalEntity);

        }
    }
}
