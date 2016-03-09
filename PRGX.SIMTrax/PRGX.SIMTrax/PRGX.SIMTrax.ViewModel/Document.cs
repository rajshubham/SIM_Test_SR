using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.ViewModel
{
    public class Document
    {

        public long Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public Nullable<long> ContentLength { get; set; }
        public string ContentType { get; set; }
    }
}
