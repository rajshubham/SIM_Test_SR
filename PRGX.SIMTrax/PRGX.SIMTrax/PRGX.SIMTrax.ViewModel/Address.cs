using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.ViewModel
{
    public class Address
    {
        public long Id { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Zone { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public Nullable<long> RefCountryId { get; set; }
        public string ZipCode { get; set; }
        public short AddressType { get; set; }
        public string AddressTypeValue { get; set; }
        public string CountryName { get; set; }
        public long RefContactMethod { get; set; }
        public long SellerPartyId { get; set; }
    }
}
