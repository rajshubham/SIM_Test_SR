using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.Domain.Model
{
    
    public class SellerRegisterMasterData
    {
        public SellerRegisterMasterData()
        {
            CountryList = new List<ItemList>();
            EmployeesNumberList = new List<ItemList>();
            TurnOverList = new List<ItemList>();
            BusinessSectorList = new List<ItemList>();
            GeographicSalesList = new List<ItemList>();
            GeographicServiceList = new List<ItemList>();
            CompanyTypeList = new List<ItemList>();
            IdentifierTypeList = new List<ItemList>();
    }
    public List<ItemList> CompanyTypeList { get; set; }

        public List<ItemList> CountryList { get; set; }
        public List<ItemList> EmployeesNumberList { get; set; }
        public List<ItemList> TurnOverList { get; set; }
        public List<ItemList> BusinessSectorList { get; set; }
        public List<ItemList> GeographicSalesList { get; set; }
        public List<ItemList> GeographicServiceList { get; set; }

        public long RefRegionId { get; set; }
        public long TermsOfUseId { get; set; }
        public List<ItemList>  IdentifierTypeList { get; set; }

    }
}
