using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.Domain.Model
{
    public class ComplianceChart
    {
        public int VerifiedCount { set; get; }
        public int NotVerifiedCount { set; get; }
        public int MissingCount { set; get; }
        public double VerifiedPercentage { set; get; }
        public double NotVerifiedPercentage { set; get; }
        public double MissingPercentage { set; get; }
        public Nullable<DateTime> VerifiedDate { set; get; }
        public Nullable<DateTime> SubmittedDate { get; set; }
        public int Status { set; get; }
        public string StatusValue { set; get; }
        public bool IsDataPresent { set; get; }
        public int CategoryId { set; get; }
        public string FormattedVerified
        {
            get
            {
                if (VerifiedDate != null)
                    return ((DateTime)VerifiedDate).ToString("dd-MM-yyyy");
                else
                {
                    return DateTime.UtcNow.ToString("dd-MM-yyyy");
                }
            }
        }
    }
}
