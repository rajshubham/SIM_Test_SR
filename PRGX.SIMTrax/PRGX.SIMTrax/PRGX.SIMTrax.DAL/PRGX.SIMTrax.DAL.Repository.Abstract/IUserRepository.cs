using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.Domain.Model;
using System.Collections.Generic;

namespace PRGX.SIMTrax.DAL.Repository.Abstract
{
    public interface IUserRepository : IGenericRepository<User>
    {
        bool IsEmailExists(string email);

        UserDetails GetUserDetailsByOrganisationPartyId(long organisationPartyId);

        UserDetails GetUserDetailByUserId(long userId);

        List<User> GetPartyUser(long organizationPartyId);

        List<UserAccount> GetAllUsers(string loginId, string userName, int userType, string status, short source, out int total, int pageIndex, int pageSize, int sortDirection);

        List<string> GetUserName(string userName);

        List<UserAccount> GetUserDetailsForDashboard(long buyerPartyId, out int totalUsers, int pageNumber, int sortDirection);
    }
}
