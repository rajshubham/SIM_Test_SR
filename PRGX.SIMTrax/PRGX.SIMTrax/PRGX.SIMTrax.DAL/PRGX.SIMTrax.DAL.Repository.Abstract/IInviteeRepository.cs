using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository.Abstract
{
    public interface IInviteeRepository : IGenericRepository<Invitee>
    {
        List<Invitee> GetInviteeDetailsBySellerId(long sellerId);
        List<BuyerSupplierReferenceList> BuyerSupplierReferenceList(int pageNo, string sortParameter, int sortDirection, string buyerName, long sellerId, long referenceId, out int totalRecords);
    }
}
