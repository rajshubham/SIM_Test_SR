using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.ViewModel
{
    public  class EmailTemplate
    {

        public long Id { get; set; }
        public string Mnemonic { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }
        public Nullable<long> RefLocale { get; set; }
    }
}
