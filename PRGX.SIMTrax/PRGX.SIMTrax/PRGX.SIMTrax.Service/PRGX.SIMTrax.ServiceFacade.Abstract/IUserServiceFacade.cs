using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.ViewModel;
using System.Collections.Generic;

namespace PRGX.SIMTrax.ServiceFacade.Abstract
{
    public interface IUserServiceFacade
    {
        bool AddUser(SellerRegister registerModel, out long sellerPartyId, out long userPartyId);

        bool IsEmailExists(string email);

        UserDetails GetUserDetailsByOrganisationPartyId(long organisationPartyId);

        bool HasUserAcceptedLatestTermsOfUse(long userId);

        bool UpdateUserLastLoginDate(long userId, string loginId);

        long GetUserIdFromCredentials(string email, string password, int lockCount, int timeSpanLimit, out bool isLocked);

        UserDetails GetUserDetailByUserId(long userId);

        long GetLatestTermsOfUseId();

        bool UpdateAcceptedTermsOfUse(long userId, long latestTermsOfUseId);

        bool UpdatePassword(string loginId, string encryptedPassword, long userId, bool needPasswordChange);

        bool UpdatePasswordBasedOnOrganizationPartyId(long organizationPartyId, string encryptedPassword, bool needPasswordChange);

        List<UserAccount> GetAllUsers(string loginId, string userName, int userType, string status, short source, out int total, int pageIndex, int pageSize, int sortDirection);

        bool DeleteUser(long userId);

        User GetUserViewModelByUserId(long userId);

        bool UpdateUserProfile(User userDetails, long auditorId);

        long ValidateUserLoginId(string loginId);

        TemporaryPasswordUrl GetTemporaryPasswordUrl(long userId);

        bool AddorUpdateTemporaryUrl(TemporaryPasswordUrl tempPasswordUrl);

        long IsTemporaryUrlValid(string token, out string email);

        List<string> GetUserName(string userName);

        List<string> GetUserEmail(string email);

        List<UserAccount> GetUserDetailsForDashboard(long buyerPartyId, out int totalUsers, int pageNumber, int sortDirection);

        long AddNewUser(User userModel, long auditorId);

        long UpdateUser(User userModel, long auditorId);
    }
}
