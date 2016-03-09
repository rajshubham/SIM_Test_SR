using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.Domain.Model
{
    public class OrganizationDetail
    {
        public string OrganizationName { get; set; }
        public long OrganizationId { get; set; }
        public long RefSellerId { get; set; }
        public long RefPartyId { get; set; }
        public long RefLegalEntityId { get; set; }
        public short Status { get; set; }
    }
}
