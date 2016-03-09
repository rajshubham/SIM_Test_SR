using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.Domain.Model
{
    public class ProfileSummary
    {
        public long CompanyDetailsScore { set; get; }
        public long ContactDetailsScore { set; get; }
        public long AddressDetailsScore { set; get; }
        public long MarketingDetailsScore { set; get; }
        public long CapabilityDetailsScore { set; get; }
        public long TotalScore { set; get; }
    }
}
