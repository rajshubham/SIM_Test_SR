using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prgxResource = PRGX.SIMTrax.Domain.Resource.ApplicationLanguage.PRGXResource;

namespace PRGX.SIMTrax.Domain.Model
{
    public  class SupplierDetails
    {
        public long BuyerId { get; set; }
        [Display(Name = Constants.SUPPLIER_ID, ResourceType = typeof(prgxResource))]
        public long CompanyId { get; set; }
        [Display(Name = Constants.COMPANY_NAME, ResourceType = typeof(prgxResource))]
        public string CompanyName { get; set; }
        public bool IsFavourite { get; set; }
        public bool IsTradingWith { get; set; }
        public Nullable<int> StatusId { get; set; }
        [Display(Name = Constants.REGISTRATION_STATUS, ResourceType = typeof(prgxResource))]
        public string Status
        {
            get; set;
        }
        public Nullable<DateTime> CreatedDate { get; set; }
        public int SectionSharedCount { get; set; }
        public long RequestSupplierAnswerId { get; set; }

        public bool IsDiscussionStarted{get;set;}
        public List<int> AprrovedTypes { get; set; }

        public string DUNSnumber { get; set; }
        [Display(Name = Constants.STARTED_DATE, ResourceType = typeof(prgxResource))]
        public string StartedDate { get; set; }
        [Display(Name = Constants.REGISTRATION_DATE, ResourceType = typeof(prgxResource))]
        public string ProfileCreatedDate { get; set; }
        public bool IsNotRegisteredSupplier { get; set; }
        public string RegistrationStatus { get { if (IsNotRegisteredSupplier) return "Registered"; else return "Not Registered"; } }
        [Display(Name = Constants.IS_FAVOURITE, ResourceType = typeof(prgxResource))]
        public string IsFavouriteText { get { if (IsFavourite) return "Yes"; else return "No"; } }
        [Display(Name = Constants.IS_TRADING_WITH, ResourceType = typeof(prgxResource))]
        public string IsTradingWithText { get { if (IsTradingWith) return "Yes"; else return "No"; } }
        public string AnswersShared { get { if (SectionSharedCount == 4) return "All"; else if (SectionSharedCount < 4 && SectionSharedCount > 0) return "Some"; else return "None"; } }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public string Registrationnumber { get; set; }
        public string VATnumber { get; set; }

    }
}
