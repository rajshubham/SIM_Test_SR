using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository.Abstract
{
    public interface IAddressRepository : IGenericRepository<Address>
    {
        void AddUpdateAddress(Address partyAddress, long contactMethodId);
        List<Address> GetAddressDetailsByPartyId(long PartyId);
        Address GetAddressDetailsByContactMethodId(long contactMethodID);
        List<BuyerSupplierAddressList> BuyerSupplierAddressList(int pageNo, string sortParameter, int sortDirection, string buyerName, long sellerPartyId, long addressId, out int totalRecords);

    }
}
