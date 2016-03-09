using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.Domain.Model
{
    public class SupplierCountBasedOnStage
    {
        public string Stage { set; get; }
        public long DetailsScore { set; get; }
        public long ProfileScore { set; get; }
        public long SanctionScore { set; get; }
        //public long FITScore { set; get; }
        //public long HSScore { set; get; }
        //public long DSScore { set; get; }
    }
}
