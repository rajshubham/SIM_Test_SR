using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.Domain.Model
{
    public class BuyerSupplierReferenceList
    {
        public long ReferenceId { set; get; }
        public long BuyerId { set; get; }
        public string BuyerName { set; get; }
        public bool IsAssigned { set; get; }
    }
}
