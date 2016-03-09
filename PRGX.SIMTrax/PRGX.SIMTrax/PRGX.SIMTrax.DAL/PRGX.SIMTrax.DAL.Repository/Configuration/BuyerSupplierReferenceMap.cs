using PRGX.SIMTrax.DAL.Entity;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class BuyerSupplierReferenceMap : EntityTypeConfiguration<BuyerSupplierReference>
    {
        public BuyerSupplierReferenceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("BuyerSupplierReference");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RefInvitee).HasColumnName("RefInvitee");
            this.Property(t => t.RefReferredTo).HasColumnName("RefReferredTo");

            // Relationships
            this.HasRequired(t => t.Buyer)
                .WithMany(t => t.BuyerSupplierReferences)
                .HasForeignKey(d => d.RefReferredTo);
            this.HasRequired(t => t.Invitee)
                .WithMany(t => t.BuyerSupplierReferences)
                .HasForeignKey(d => d.RefInvitee);


        }
    }
}
