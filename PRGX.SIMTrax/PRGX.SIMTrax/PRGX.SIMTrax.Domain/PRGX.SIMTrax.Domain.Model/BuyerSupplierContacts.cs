using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.Domain.Model
{
    public class BuyerSupplierContacts
    {
        public long ContactId { set; get; }
        public long BuyerId { set; get; }
        public long BuyerPartyId { set; get; }
        public long ContactPartyId { set; get; }
        public string BuyerName { set; get; }
        public string RoleValue { set; get; }

        public int Role { get
            {
                if (RoleValue != null)
                    return Convert.ToInt32(RoleValue);
                else
                    return 0; 
            }
        }

        public bool IsAssigned { set; get; }
    }
}
