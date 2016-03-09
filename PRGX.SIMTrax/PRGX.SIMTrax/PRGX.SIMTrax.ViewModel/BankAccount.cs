using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.ViewModel
{
    public class BankAccount
    {
        public long Id { get; set; }
        public long RefLegalEntity { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string SwiftCode { get; set; }
        public string IBAN { get; set; }
        public string BankName { get; set; }
        public string Address { get; set; }
        public long RefCountryId { get; set; }
        public string PreferredMode { get; set; }
        public string BranchIdentifier { get; set; }
        public string CountryName { get; set; }
        public long organisationId { get; set; }
        public int AssignedCount { get; set; }
    }
}

