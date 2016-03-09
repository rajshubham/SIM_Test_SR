using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Abstract
{
    public interface IMasterDataUow
    {
        void SaveChanges();

        void Dispose();

        void Rollback();

        void Commit();

        void BeginTransaction();

        IMasterDataRepository MasterDataValues { get; }

        IIndustryCodeRepository IndustryCodeValues { get; }

        IGenericRepository<Region> Regions { get; }
    }
}
