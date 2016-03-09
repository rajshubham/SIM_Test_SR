using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.ViewModel
{
    public class EmailAudit
    {
        public long Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public Nullable<long> SentBy { get; set; }
        public Nullable<bool> IsEmailSent { get; set; }
        public Nullable<System.DateTime> SentDate { get; set; }
    }
}
