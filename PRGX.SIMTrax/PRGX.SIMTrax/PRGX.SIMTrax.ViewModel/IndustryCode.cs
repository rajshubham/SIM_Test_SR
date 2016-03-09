using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.ViewModel
{
    public class IndustryCode
    {



        public long Id { get; set; }
        public string SectorName { get; set; }
        public Nullable<long> RefParentId { get; set; }
        public string CodeNumber { get; set; }
        public string CodeDescription { get; set; }
        public long RefIndustryCodeSet { get; set; }
        public List<IndustryCode> ChildCodes { get; set; }
    }
   }
