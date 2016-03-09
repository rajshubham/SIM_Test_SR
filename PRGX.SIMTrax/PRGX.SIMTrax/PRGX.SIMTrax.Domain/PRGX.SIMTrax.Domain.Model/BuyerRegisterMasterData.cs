using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.Domain.Model
{
    public class BuyerRegisterMasterData
    {
        public BuyerRegisterMasterData()
        {
            CountryList = new List<ItemList>();
            EmployeesNumberList = new List<ItemList>();
            TurnOverList = new List<ItemList>();
            BusinessSectorList = new List<ItemList>();
        }
        
        public List<ItemList> CountryList { get; set; }
        public List<ItemList> EmployeesNumberList { get; set; }
        public List<ItemList> TurnOverList { get; set; }
        public List<ItemList> BusinessSectorList { get; set; }

        //public long RefRegionId { get; set; }
    }
}
