using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class Organization : AuditableEntityWithoutId
    {
        public Organization()
        {
            this.IndustryCodeOrganizationLinks = new List<IndustryCodeOrganizationLink>();
        }

        [Key, ForeignKey("Party")]
        public long Id { get; set; }
        public string OrganizationType { get; set; }
        public Nullable<long> EmployeeSize { get; set; }
        public Nullable<long> TurnOverSize { get; set; }
        public Nullable<long> RefLogoDocument { get; set; }
        public Nullable<short> Status { get; set; }
        public Nullable<long> BusinessSectorId { get; set; }
        public string BusinessSectorDescription { get; set; }
        //public System.DateTime CreatedOn { get; set; }
        //public Nullable<System.DateTime> LastUpdatedOn { get; set; }
        //public long RefCreatedBy { get; set; }
        //public Nullable<long> RefLastUpdatedBy { get; set; }
        public Nullable<System.DateTime> RegistrationSubmittedOn { get; set; }
        public Nullable<System.DateTime> RegistrationVerifiedOn { get; set; }
        public Nullable<System.DateTime> ProfileVerifiedOn { get; set; }
        public Nullable<long> RefProfileVerifiedBy { get; set; }
        public Nullable<System.DateTime> PublishedDate { get; set; }
        public Nullable<bool> CSIVerificationStatus { get; set; }
        public byte[] RowVersion { get; set; }
        public Nullable<long> RefLegalEntity { get; set; }
        public virtual Buyer Buyer { get; set; }
        public virtual Document Document { get; set; }
        public virtual ICollection<IndustryCodeOrganizationLink> IndustryCodeOrganizationLinks { get; set; }
        public virtual LegalEntity LegalEntity { get; set; }
        public virtual Party Party { get; set; }
        public virtual Party Party1 { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}
