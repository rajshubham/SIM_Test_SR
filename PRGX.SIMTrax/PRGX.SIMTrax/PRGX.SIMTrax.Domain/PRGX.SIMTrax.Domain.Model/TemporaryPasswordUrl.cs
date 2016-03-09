using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.Domain.Model
{
    public class TemporaryPasswordUrl
    {
        public long TemporaryPasswordUrlId { get; set; }
        public string PasswordURL { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime? ModifiedDate { get; set; }

        public long UserId { get; set; }
        public string Token { get; set; }
    }
}
