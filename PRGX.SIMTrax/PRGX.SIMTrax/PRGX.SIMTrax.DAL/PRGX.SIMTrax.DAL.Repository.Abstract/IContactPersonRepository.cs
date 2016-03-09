using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository.Abstract
{
    public interface IContactPersonRepository : IGenericRepository<ContactPerson>
    {
        List<ContactPerson> GetContactDetailsByPartyId(long sellerPartyId);
        ContactPerson GetContactByRoleAndPartyId(long sellerPartyId, int contactType);
        List<BuyerSupplierContacts> BuyerSupplierContactList(int pageNo, string sortParameter, int sortDirection, string buyerName, long sellerId, long contactPartyId, out int totalRecords);
    }
}
