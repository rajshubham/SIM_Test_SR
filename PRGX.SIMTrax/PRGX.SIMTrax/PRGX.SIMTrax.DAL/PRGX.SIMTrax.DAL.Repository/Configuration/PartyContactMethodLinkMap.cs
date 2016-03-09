using PRGX.SIMTrax.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository.Configuration
{
    public class PartyContactMethodLinkMap : EntityTypeConfiguration<PartyContactMethodLink>
    {
        public PartyContactMethodLinkMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("PartyContactMethodLink");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RefParty).HasColumnName("RefParty");
            this.Property(t => t.RefContactMethod).HasColumnName("RefContactMethod");

            // Relationships
            this.HasRequired(t => t.ContactMethod)
                .WithMany(t => t.PartyContactMethodLinks)
                .HasForeignKey(d => d.RefContactMethod);
            this.HasRequired(t => t.Party)
                .WithMany(t => t.PartyContactMethodLinks)
                .HasForeignKey(d => d.RefParty);

        }
    }
}
