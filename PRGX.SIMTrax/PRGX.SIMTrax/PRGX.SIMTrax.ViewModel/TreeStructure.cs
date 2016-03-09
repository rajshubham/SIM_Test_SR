using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.ViewModel
{
   
    public class TreeStructure
    {
        public Attributes attr { get; set; }
        public string data { get; set; }
        public string state { get; set; }
        public List<TreeStructure> children { get; set; }
    }
    public class Attributes
    {
        public string id { get; set; }
        public string rel { get; set; }
        public string mdata { get; set; }
        public bool IsChecked { get; set; }
    }
}
