using PRGX.SIMTrax.DAL.Entity.AuditModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class CampaignInvitation : AuditableEntity<long>
    {
        //public long Id { get; set; }
        public long RefCampaign { get; set; }
        public string SupplierCompanyName { get; set; }
        public string EmailAddress { get; set; }
        public string RegistrationCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public Nullable<long> RefCountry { get; set; }
        public string ZipCode { get; set; }
        public string Telephone { get; set; }
        public string Identifier1 { get; set; }
        public string IdentifierType1 { get; set; }
        public string Identifier2 { get; set; }
        public string IdentifierType2 { get; set; }
        public string Identifier3 { get; set; }
        public string IdentifierType3 { get; set; }
        public Nullable<bool> IsSubsidary { get; set; }
        public string UltimateParent { get; set; }
        public Nullable<bool> IsValid { get; set; }
        public string InvalidComments { get; set; }
        public Nullable<bool> IsRegistered { get; set; }
        //public long RefCreatedBy { get; set; }
        //public Nullable<long> RefLastUpdatedBy { get; set; }
        //public System.DateTime CreatedOn { get; set; }
        //public Nullable<System.DateTime> LastUpdatedOn { get; set; }
        public Nullable<long> RefSupplier { get; set; }
        public virtual BuyerCampaign BuyerCampaign { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual Region Region { get; set; }
    }
}
