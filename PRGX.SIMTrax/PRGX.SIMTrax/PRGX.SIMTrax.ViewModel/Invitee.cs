using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.ViewModel
{
    public class Invitee
    {

        public long Id { get; set; }
        public string ClientName { get; set; }
        public string ContactName { get; set; }
        public long RefReferee { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string MailingAddress { get; set; }
        public string Fax { get; set; }
        public Nullable<bool> CanWeContact { get; set; }
        public string ClientRole { get; set; }
        public int AssignedCount { set; get; }

    }
}
