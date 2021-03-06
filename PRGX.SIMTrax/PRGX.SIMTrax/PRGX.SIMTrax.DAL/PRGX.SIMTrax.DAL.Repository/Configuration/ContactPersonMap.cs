using PRGX.SIMTrax.DAL.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class ContactPersonMap : EntityTypeConfiguration<ContactPerson>
    {
        public ContactPersonMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.JobTitle)
                .HasMaxLength(50);
            this.Property(t => t.Id)
          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);


            this.Property(t => t.RowVersion)
                .IsFixedLength()
                .HasMaxLength(8)
                .IsRowVersion();

            // Table & Column Mappings
            this.ToTable("ContactPerson");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.JobTitle).HasColumnName("JobTitle");
            this.Property(t => t.ContactType).HasColumnName("ContactType");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.RefCreatedBy).HasColumnName("RefCreatedBy");
            this.Property(t => t.RefLastUpdatedBy).HasColumnName("RefLastUpdatedBy");
            this.Property(t => t.RowVersion).HasColumnName("RowVersion");

            // Relationships
            this.HasRequired(t => t.Person)
                .WithOptional(t => t.ContactPerson);



        }
    }
}
