using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.Domain.Model
{
    public class DownloadProfile
    {
        public DownloadProfile()
        {
            this.Charts = new List<ChartImages>();
        }
        public string CompanyName { set; get; }
        public long ComanyPartyId { set; get; }
        public List<ChartImages> Charts { set; get; }
        public ProfileGeneralInformation GeneralInformation { set; get; }
    }

    public class ChartImages
    {
        public byte[] Logo { get; set; }
        public string ChartBaseString { get; set; }
        public int Category { set; get; }
        public string DivHtml { set; get; }
        public bool IsPublished { set; get; }
    }

    public class ProfileGeneralInformation
    {
        public long SupplierPartyId { get; set; }
        public string SIMId { get; set; }
        public string BusinessSector { get; set; }
        public string NoOfEmployess { get; set; }
        public string TurnOver { get; set; }
        public string CustomerSector { get; set; }
        public string ServiceIn { get; set; }
        public string Subsidiaries { get; set; }
        public string TradingName { get; set; }
        public string FacebookAccount { get; set; }
        public string TwitterAccount { get; set; }
        public string LinkeldinAccount { get; set; }
        public string WebsiteLink { get; set; }
        public string MaxContract { get; set; }
        public string MinContract { get; set; }
        public string CompanyName { get; set; }
        public string LogoBaseString { get; set; }
        public string BusinessDescription { get; set; }
    }
}
