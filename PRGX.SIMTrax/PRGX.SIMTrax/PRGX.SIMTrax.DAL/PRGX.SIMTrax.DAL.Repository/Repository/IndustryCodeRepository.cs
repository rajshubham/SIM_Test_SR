using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Abstract;
using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PRGX.SIMTrax.DAL.Repository.Repository
{
    public class IndustryCodeRepository : GenericRepository<IndustryCode>, IIndustryCodeRepository
    {
        public IndustryCodeRepository(DbContext context) : base(context) { }

        public List<IndustryCode> GetIndustryCodes(string regionIdentifier, int? ParentId, bool AllSICCodes)
        {
            try
            {
                Logger.Info("IndustryCodeRepository : GetIndustryCodes() : Enter the method");

                List<IndustryCode> codeList = null;
                var query = this.All().AsQueryable();
                this.DbContext.Configuration.LazyLoadingEnabled = true;
                codeList = query.Where(v => v.IndustryCodeSet.IndustryCodeSetRegionLinks.Any(x => x.Region.Name.ToLower() == regionIdentifier.ToLower()
                           && x.Region.RegionType.Trim() == Constants.COUNTRY_NAME)).ToList();
                this.DbContext.Configuration.LazyLoadingEnabled = false;
         
                Logger.Info("IndustryCodeRepository : GetIndustryCodes() : Exit the method");
                return codeList;
            }
            catch (Exception ex)
            {
                Logger.Error("IndustryCodeRepository : GetIndustryCodes() : Caught an exception" + ex);
                throw;
            }
        }


    }
}
