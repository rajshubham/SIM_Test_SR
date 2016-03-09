using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.Domain.Model
{
    public class BuyerSupplierSearchFilter
    {
        public BuyerSupplierSearchFilter()
        {
            this.ComplianceFilters = new List<BuyerSupplierComplianceSearchFilter>();
        }
        public List<Int64> SupplierStatus { set; get; }
        public List<Int64> SupplierType { set; get; }
        public List<Int64> Sector { set; get; }
        public List<Int64> EmployeeSize { set; get; }
        public List<Int64> TurnOver { set; get; }
        public List<Int64> TypeOfCompany { set; get; }
        public int PageNo { set; get; }
        public int PageSize { set; get; }
        public string SortParameter { set; get; }
        public int SortDirection { set; get; }
        public string SupplierName { set; get; }
        public List<BuyerSupplierComplianceSearchFilter> ComplianceFilters { set; get; }
        public string SectorText { set; get; }
        public string EmployeeSizeText { set; get; }
        public string TurnOverText { set; get; }
        public string TypeOfCompanyText { set; get; }

        public bool FromBuyerHome { get; set; }
    }
    public class BuyerSupplierComplianceSearchFilter
    {
        public int Pillar { set; get; }
        public List<Int64> RequiredStatus { set; get; }
        public List<Int64> SharedStatus { set; get; }
        public List<Int64> Status { set; get; }
    }
}
