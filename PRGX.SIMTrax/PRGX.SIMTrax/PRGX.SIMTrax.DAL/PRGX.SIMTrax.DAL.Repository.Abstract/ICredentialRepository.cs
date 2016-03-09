using PRGX.SIMTrax.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository.Abstract
{
    public interface ICredentialRepository : IGenericRepository<Credential>
    {
        bool UpdateUserLastLoginDate(long userId, string loginId);

        long GetUserIdFromCredentials(string email, string password, int lockCount, int timeSpanLimit, out bool isLocked);

        void UpdatePassword(string loginId, string encryptedPassword, long userId);
    }
}
