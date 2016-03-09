using PRGX.SIMTrax.DAL.Entity;
using System.Collections.Generic;

namespace PRGX.SIMTrax.DAL.Repository.Abstract
{
    public interface IIndustryCodeRepository : IGenericRepository<IndustryCode>
    {
        List<IndustryCode> GetIndustryCodes(string regionIdentifier,int? ParentId, bool AllSICCodes);
    }
}
