using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository.Abstract
{
    public interface IBankAccountRepository : IGenericRepository<BankAccount>
    {
        List<BankAccount> GetBankDetailsByOrganisationId(long organisationId);
        List<BuyerSupplierBankAccount> BuyerSupplierBankList(int pageNo, string sortParameter, int sortDirection, string buyerName, long organisationId, long bankId, out int totalRecords);
    }
}
