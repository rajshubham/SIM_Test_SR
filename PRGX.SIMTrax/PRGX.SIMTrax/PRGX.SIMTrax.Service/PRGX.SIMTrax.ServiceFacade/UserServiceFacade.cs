using PRGX.SIMTrax.DAL;
using PRGX.SIMTrax.DAL.Abstract;
using PRGX.SIMTrax.ServiceFacade.Abstract;
using System.Linq;
using System;
using PRGX.SIMTrax.Domain.Util;
using PRGX.SIMTrax.ServiceFacade.Mapper;
using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.ViewModel;
using PRGX.SIMTrax.Domain.Model;
using System.Collections.Generic;

namespace PRGX.SIMTrax.ServiceFacade
{
    public class UserServiceFacade : IUserServiceFacade
    {
        private readonly IUserUow _userUow;

        public UserServiceFacade()
        {
            _userUow = new UserUow();
        }

        public bool AddUser(SellerRegister registerModel, out long sellerPartyId, out long userPartyId)
        {
            IUserUow userUow = null;
            try
            {
                Logger.Info("UserServiceFacade : AddUser() : Entering the method");

                userUow = new UserUow();

                var partyUser = registerModel.ToPartyModelUser();
                var partySeller = registerModel.ToPartyModelSeller();

                //Let's start transaction
                userUow.BeginTransaction();

                // add party user
                userUow.Parties.Add(partyUser);
                userUow.SaveChanges();

                userUow.Parties.Add(partySeller);
                userUow.SaveChanges();

                var partyPartyLink = new PartyPartyLink
                {
                    RefParty = partyUser.Id,
                    RefLinkedParty = partySeller.Id,
                    PartyPartyLinkType = Constants.PRIMARY_ORGANIZATION,
                    RefCreatedBy = partyUser.Id
                };

                userUow.PartyPartyLinks.Add(partyPartyLink);
                userUow.SaveChanges();

                // all fine, let's commit
                userUow.Commit();

                sellerPartyId = partySeller.Id;
                userPartyId = partyUser.Id;
                Logger.Info("UserServiceFacade : AddUser() : Exiting the method");
                return true;
            }
            catch (Exception ex)
            {
                if (userUow != null)
                    userUow.Rollback();
                Logger.Error("UserServiceFacade : AddUser() : Caught an exception" + ex);
                throw;
            }
        }

        public bool IsEmailExists(string email)
        {
            try
            {
                Logger.Info("UserServiceFacade : IsEmailExists() : Enter the method");
                var result = false;
                result = _userUow.Users.IsEmailExists(email);
                Logger.Info("UserServiceFacade : IsEmailExists() : Exit the method");
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("UserServiceFacade : IsEmailExists() : Caught an exception" + ex);
                throw;
            }
        }

        public UserDetails GetUserDetailsByOrganisationPartyId(long organisationPartyId)
        {
            try
            {
                Logger.Info("UserServiceFacade: GetUserDetailsByOrganisationPartyId() : Enter the method");
                UserDetails userDetails = null;
                userDetails = _userUow.Users.GetUserDetailsByOrganisationPartyId(organisationPartyId);
                Logger.Info("UserServiceFacade: GetUserDetailsByOrganisationPartyId() : Exit the method");
                return userDetails;
            }
            catch (Exception ex)
            {
                Logger.Error("UserServiceFacade : GetUserDetailsByOrganisationPartyId() : Caught an exception" + ex);
                throw;
            }
        }

        public bool HasUserAcceptedLatestTermsOfUse(long userId)
        {
            try
            {
                Logger.Info("UserServiceFacade : HasUserAcceptedLatestTermsOfUse() : Enter the method");
                var result = false;
                var acceptedTerms = _userUow.AcceptedTermsOfUses.GetAll().FirstOrDefault(u => u.RefAcceptingUser == userId);
                if (null != acceptedTerms)
                {
                    var latestTermsId = _userUow.TermsOfUses.GetAll().OrderByDescending(t => t.CreatedOn).First().Id;
                    if (latestTermsId == acceptedTerms.RefTermsOfUse)
                        result = true;
                }
                Logger.Info("UserServiceFacade : HasUserAcceptedLatestTermsOfUse() : Exit the method");
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("UserServiceFacade : HasUserAcceptedLatestTermsOfUse() : Caught an error" + ex);
                throw;
            }
        }

        public bool UpdateUserLastLoginDate(long userId, string loginId)
        {
            IUserUow _userUow = null;
            try
            {
                Logger.Info("UserServiceFacade : UpdateUserLastLoginDate() : Enter the method");
                _userUow = new UserUow();
                _userUow.BeginTransaction();
                var result = _userUow.Credentials.UpdateUserLastLoginDate(userId, loginId);
                _userUow.SaveChanges();
                _userUow.Commit();
                Logger.Info("UserServiceFacade : UpdateUserLastLoginDate() : Exit the method");
                return result;
            }
            catch (Exception ex)
            {
                if (_userUow != null)
                    _userUow.Rollback();
                Logger.Error("UserServiceFacade : UpdateUserLastLoginDate() : Caught an error" + ex);
                throw;
            }
        }

        public long GetUserIdFromCredentials(string email, string password, int lockCount, int timeSpanLimit, out bool isLocked)
        {
            IUserUow _userUow = null;
            try
            {
                Logger.Info("UserServiceFacade : GetUserIdFromCredentials() : Enter the method");
                _userUow = new UserUow();

                _userUow.BeginTransaction();
                var result = _userUow.Credentials.GetUserIdFromCredentials(email, password, lockCount, timeSpanLimit, out isLocked);
                _userUow.SaveChanges();

                _userUow.Commit();
                Logger.Info("UserServiceFacade : GetUserIdFromCredentials() : Exit the method");
                return result;
            }
            catch (Exception ex)
            {
                if (_userUow != null)
                    _userUow.Rollback();
                Logger.Error("UserServiceFacade : GetUserIdFromCredentials() : Caught an error" + ex);
                throw;
            }
        }

        public UserDetails GetUserDetailByUserId(long userId)
        {
            try
            {
                Logger.Info("UserServiceFacade: GetUserDetailByUserId() : Enter the method");
                UserDetails userDetails = null;
                userDetails = _userUow.Users.GetUserDetailByUserId(userId);
                Logger.Info("UserServiceFacade: GetUserDetailByUserId() : Exit the method");
                return userDetails;
            }
            catch (Exception ex)
            {
                Logger.Error("UserServiceFacade : GetUserDetailByUserId() : Caught an exception" + ex);
                throw;
            }
        }

        public long GetLatestTermsOfUseId()
        {
            try
            {
                Logger.Info("UserServiceFacade : GetLatestTermsOfUseId() : Enter the method");

                var termsOfUseId = _userUow.TermsOfUses.GetAll().OrderByDescending(t => t.CreatedOn).First().Id;

                Logger.Info("UserServiceFacade : GetLatestTermsOfUseId() : Exit the method");
                return termsOfUseId;
            }
            catch (Exception ex)
            {
                Logger.Error("UserServiceFacade : GetLatestTermsOfUseId() : Caught an error" + ex);
                throw;
            }
        }

        public bool UpdateAcceptedTermsOfUse(long userId, long latestTermsOfUseId)
        {
            IUserUow _userUow = null;
            try
            {
                Logger.Info("UserServiceFacade : UpdateAcceptedTermsOfUse() : Enter the method");
                var result = false;
                _userUow = new UserUow();
                _userUow.BeginTransaction();
                var acceptedTermsOfUse = _userUow.AcceptedTermsOfUses.GetAll().FirstOrDefault(v => v.RefAcceptingUser == userId);
                if (null != acceptedTermsOfUse)
                {
                    acceptedTermsOfUse.RefTermsOfUse = latestTermsOfUseId;
                    _userUow.AcceptedTermsOfUses.Update(acceptedTermsOfUse);
                }
                else
                {
                    acceptedTermsOfUse = new AcceptedTermsOfUse();
                    acceptedTermsOfUse.RefAcceptingUser = userId;
                    acceptedTermsOfUse.RefTermsOfUse = latestTermsOfUseId;
                    acceptedTermsOfUse.AcceptedDate = DateTime.UtcNow;
                    _userUow.AcceptedTermsOfUses.Add(acceptedTermsOfUse);
                }
                _userUow.SaveChanges();
                _userUow.Commit();
                result = true;
                Logger.Info("UserServiceFacade : UpdateAcceptedTermsOfUse() : Exit the method");
                return result;
            }
            catch (Exception ex)
            {
                if (_userUow != null)
                    _userUow.Rollback();
                Logger.Error("UserServiceFacade : UpdateAcceptedTermsOfUse() : Caught an error" + ex);
                throw;
            }
        }

        public bool UpdatePassword(string loginId, string encryptedPassword, long userId, bool needPasswordChange)
        {
            IUserUow _userUow = null;
            try
            {
                Logger.Info("UserServiceFacade : UpdatePassword() : Enter the method");
                _userUow = new UserUow();
                var result = false;
                _userUow.BeginTransaction();
                _userUow.Credentials.UpdatePassword(loginId, encryptedPassword, userId);
                _userUow.SaveChanges();

                var userPM = _userUow.Users.GetAll().FirstOrDefault(u => u.Id == userId);
                if (null != userPM)
                {
                    userPM.NeedsPasswordChange = needPasswordChange;
                    _userUow.Users.Update(userPM);
                    _userUow.SaveChanges();
                    result = true;
                }

                _userUow.Commit();
                Logger.Info("UserServiceFacade : UpdatePassword() : Exit the method");
                return result;
            }
            catch (Exception ex)
            {
                if (_userUow != null)
                    _userUow.Rollback();
                Logger.Error("UserServiceFacade : UpdatePassword() : Caught an error" + ex);
                throw;
            }
        }

        public bool UpdatePasswordBasedOnOrganizationPartyId(long organizationPartyId, string randomPassword, bool needPasswordChange)
        {
            IUserUow _userUow = null;
            try
            {
                Logger.Info("UserServiceFacade : UpdatePassword() : Enter the method");
                var result = false;
                _userUow = new UserUow();
                var user = _userUow.Users.GetPartyUser(organizationPartyId).FirstOrDefault();

                if (null != user)
                {
                    _userUow.BeginTransaction();
                    user.NeedsPasswordChange = needPasswordChange;
                    _userUow.Users.Update(user);
                    _userUow.SaveChanges();

                    var credentials = _userUow.Credentials.GetAll().Where(c => c.RefUser == user.Id).FirstOrDefault();
                    if (null != credentials)
                    {
                        credentials.Password = CommonMethods.EncryptMD5Password(credentials.LoginId.ToLower() + randomPassword.Trim()); ;
                        _userUow.Credentials.Update(credentials);
                        _userUow.SaveChanges();
                    }

                    _userUow.Commit();
                    result = true;
                }
                Logger.Info("UserServiceFacade : UpdatePassword() : Exit the method");
                return result;
            }
            catch (Exception ex)
            {
                if (_userUow != null)
                    _userUow.Rollback();
                Logger.Error("UserServiceFacade : UpdatePassword() : Caught an error" + ex);
                throw;
            }
        }

        public List<UserAccount> GetAllUsers(string loginId, string userName, int userType, string status, short source, out int total, int pageIndex, int pageSize, int sortDirection)
        {
            Logger.Info("UserServiceFacade : GetAllUsers() : Entering the method");
            var users = new List<UserAccount>();
            try
            {
                users = _userUow.Users.GetAllUsers(loginId, userName, userType, status, source, out total, pageIndex, pageSize, sortDirection);
            }
            catch (Exception ex)
            {
                Logger.Error("UserServiceFacade : GetAllUsers() : Caught an exception" + ex);
                throw ex;
            }
            Logger.Info("UserServiceFacade : GetAllUsers() : Exiting the method");
            return users;
        }

        public bool DeleteUser(long userId)
        {
            IUserUow _userUow = null;
            try
            {
                Logger.Info("UserServiceFacade : DeleteUser() : Enter the method");
                _userUow = new UserUow();
                var result = false;
                _userUow.BeginTransaction();

                var credentials = _userUow.Credentials.GetAll().FirstOrDefault(c => c.RefUser == userId);
                if (null != credentials)
                {
                    _userUow.Credentials.Delete(credentials);
                    _userUow.SaveChanges();
                }

                var acceptedTermsOfUse = _userUow.AcceptedTermsOfUses.GetAll().Where(c => c.RefAcceptingUser == userId).ToList();
                foreach (var terms in acceptedTermsOfUse)
                {
                    _userUow.AcceptedTermsOfUses.Delete(terms);
                    _userUow.SaveChanges();
                }

                var userPM = _userUow.Users.GetAll().FirstOrDefault(u => u.Id == userId);
                if (null != userPM)
                {
                    _userUow.Users.Delete(userPM);
                    _userUow.SaveChanges();
                }

                var personPM = _userUow.People.GetAll().FirstOrDefault(u => u.Id == userId);
                if (null != personPM)
                {
                    _userUow.People.Delete(personPM);
                    _userUow.SaveChanges();
                }

                var partyPM = _userUow.Parties.GetAll().FirstOrDefault(u => u.Id == userId);
                if (null != partyPM)
                {
                    _userUow.Parties.Delete(partyPM);
                    _userUow.SaveChanges();
                }

                _userUow.Commit();
                result = true;
                Logger.Info("UserServiceFacade : DeleteUser() : Exit the method");
                return result;
            }
            catch (Exception ex)
            {
                if (_userUow != null)
                    _userUow.Rollback();
                Logger.Error("UserServiceFacade : DeleteUser() : Caught an error" + ex);
                throw;
            }
        }

        public ViewModel.User GetUserViewModelByUserId(long userId)
        {
            try
            {
                Logger.Info("UserServiceFacade : GetUserViewModelByUserId() : Enter the method");

                var userDetails = _userUow.Users.GetUserDetailByUserId(userId).ToUserViewModel();

                Logger.Info("UserServiceFacade : GetUserViewModelByUserId() : Exit the method");
                return userDetails;
            }
            catch (Exception ex)
            {
                Logger.Error("UserServiceFacade : GetUserViewModelByUserId() : Caught an error" + ex);
                throw;
            }
        }

        public bool UpdateUserProfile(ViewModel.User userDetails, long auditorId)
        {
            IUserUow _userUow = null;
            try
            {
                Logger.Info("UserServiceFacade : UpdateUserProfile() : Enter the method");
                _userUow = new UserUow();
                var result = false;
                _userUow.BeginTransaction();

                var userPersonPM = _userUow.People.GetAll().FirstOrDefault(u => u.Id == userDetails.PrimaryContactPartyId);
                if (null != userPersonPM)
                {
                    userPersonPM.FirstName = userDetails.PrimaryFirstName;
                    userPersonPM.LastName = userDetails.PrimaryLastName;
                    userPersonPM.JobTitle = userDetails.JobTitle;
                    userPersonPM.RefLastUpdatedBy = auditorId;
                    _userUow.People.Update(userPersonPM);
                    _userUow.SaveChanges();
                }

                var primaryContactPersonPM = _userUow.ContactPersons.GetAll().FirstOrDefault(u => u.Id == userDetails.PrimaryContactPartyId);
                if (null != primaryContactPersonPM)
                {
                    primaryContactPersonPM.RefLastUpdatedBy = auditorId;
                    primaryContactPersonPM.JobTitle = userDetails.JobTitle;
                    _userUow.ContactPersons.Update(primaryContactPersonPM);
                    _userUow.SaveChanges();
                }

                var primaryContactPartyPM = _userUow.Parties.GetAll().FirstOrDefault(u => u.Id == primaryContactPersonPM.Id);
                if (null != primaryContactPartyPM)
                {
                    primaryContactPartyPM.PartyName = string.Concat(userDetails.PrimaryFirstName,' ', userDetails.PrimaryLastName);
                    primaryContactPartyPM.RefLastUpdatedBy = auditorId;
                    _userUow.Parties.Update(primaryContactPartyPM);
                    _userUow.SaveChanges();
                }

                var primaryContactMethod = _userUow.PartyContactMethodLinks.GetAll().Where(x => x.RefParty == userDetails.PrimaryContactPartyId).Select(x => x.RefContactMethod).ToList();
                if (primaryContactMethod.Count > 0)
                {
                    var primaryContactMethodEmail = _userUow.ContactMethods.GetAll().FirstOrDefault(x => x.ContactMethodType.Trim().Equals(Constants.CONTACT_METHOD_EMAIL) && primaryContactMethod.Contains(x.Id));
                    if (null != primaryContactMethodEmail)
                    {
                        primaryContactMethodEmail.RefLastUpdatedBy = auditorId;
                        _userUow.ContactMethods.Update(primaryContactMethodEmail);
                        _userUow.SaveChanges();
                    }

                    var primaryContactMethodPhone = _userUow.ContactMethods.GetAll().FirstOrDefault(x => x.ContactMethodType.Trim().Equals(Constants.CONTACT_METHOD_PHONE) && primaryContactMethod.Contains(x.Id));
                    if (null != primaryContactMethodPhone)
                    {
                        primaryContactMethodPhone.RefLastUpdatedBy = auditorId;
                        _userUow.ContactMethods.Update(primaryContactMethodPhone);
                        _userUow.SaveChanges();
                    }

                    var primaryContactEmail = _userUow.Emails.GetAll().FirstOrDefault(c => primaryContactMethod.Contains(c.RefContactMethod));
                    if(null != primaryContactEmail)
                    {
                        primaryContactEmail.EmailAddress = userDetails.PrimaryEmail;
                        _userUow.Emails.Update(primaryContactEmail);
                        _userUow.SaveChanges();
                    }

                    var primaryContactTel = _userUow.Phones.GetAll().FirstOrDefault(c => primaryContactMethod.Contains(c.RefContactMethod) && c.Type.Trim().Equals(Constants.PHONE_TYPE_TELEPHONE));
                    if (null != primaryContactTel)
                    {
                        primaryContactTel.PhoneNumber = userDetails.Telephone;
                        _userUow.Phones.Update(primaryContactTel);
                        _userUow.SaveChanges();
                    }
                }

                _userUow.Commit();
                result = true;
                Logger.Info("UserServiceFacade : UpdateUserProfile() : Exit the method");
                return result;
            }
            catch (Exception ex)
            {
                if (_userUow != null)
                    _userUow.Rollback();
                Logger.Error("UserServiceFacade : UpdateUserProfile() : Caught an error" + ex);
                throw;
            }
        }

        public long ValidateUserLoginId(string loginId)
        {
            try
            {
                Logger.Info("UserServiceFacade : ValidateUserLoginId() : Enter the method");

                long userId = 0;
                var credentials = _userUow.Credentials.GetAll().FirstOrDefault(c => c.LoginId == loginId && !string.IsNullOrWhiteSpace(c.Password));
                if (null != credentials)
                    userId = credentials.RefUser;

                Logger.Info("UserServiceFacade : ValidateUserLoginId() : Exit the method");
                return userId;
            }
            catch (Exception ex)
            {
                Logger.Error("UserServiceFacade : ValidateUserLoginId() : Caught an error" + ex);
                throw;
            }
        }

        public Domain.Model.TemporaryPasswordUrl GetTemporaryPasswordUrl(long userId)
        {
            IUserUow _userUow = null;
            try
            {
                Logger.Info("UserServiceFacade : GetTemporaryPasswordUrl() : Enter into method");
                Domain.Model.TemporaryPasswordUrl urlModel = new Domain.Model.TemporaryPasswordUrl();
                _userUow = new UserUow();
                _userUow.BeginTransaction();

                var tempModel = _userUow.TemporaryPasswordUrls.GetAll().FirstOrDefault(u => u.RefUser == userId);

                if (tempModel != null)
                {
                    TimeSpan duration = DateTime.UtcNow - tempModel.CreatedOn;
                    if (duration.TotalHours <= Configuration.TemparoryUrlExpiration)
                    {
                        urlModel = tempModel.ToDomainModel();
                    }
                    else
                    {
                        _userUow.TemporaryPasswordUrls.Delete(tempModel);
                        _userUow.SaveChanges();
                    }
                    _userUow.Commit();

                }
                Logger.Info("UserServiceFacade : GetTemporaryPasswordUrl() : Exit from method");
                return urlModel;
            }
            catch (Exception ex)
            {
                if (_userUow != null)
                    _userUow.Rollback();
                Logger.Error("UserServiceFacade : GetTemporaryPasswordUrl() : Caught an exception " + ex);
                throw ex;
            }
        }

        public bool AddorUpdateTemporaryUrl(Domain.Model.TemporaryPasswordUrl tempPasswordUrl)
        {
            IUserUow _userUow = null;
            Logger.Info("UserServiceFacade : AddorUpdateTemporaryUrl() : Enter into method");
            try
            {
                var result = false;

                _userUow = new UserUow();
                _userUow.BeginTransaction();
                
                if (tempPasswordUrl.TemporaryPasswordUrlId > 0)
                {
                    var existing = _userUow.TemporaryPasswordUrls.GetAll().FirstOrDefault(u => u.Id == tempPasswordUrl.TemporaryPasswordUrlId);
                    if (null != existing)
                    {
                        existing.LastUpdatedOn = tempPasswordUrl.ModifiedDate;
                        _userUow.TemporaryPasswordUrls.Update(existing);
                        _userUow.SaveChanges();
                    }
                }
                else
                {
                    _userUow.TemporaryPasswordUrls.Add(tempPasswordUrl.ToEntityModel());
                    _userUow.SaveChanges();
                }
                _userUow.Commit();
                result = true;
                Logger.Info("UserServiceFacade : AddorUpdateTemporaryUrl() : Exit from method");
                return result;
            }
            catch (Exception ex)
            {
                if (_userUow != null)
                    _userUow.Rollback();
                Logger.Error("UserServiceFacade : AddorUpdateTemporaryUrl() : Caught an exception " + ex);
                throw ex;
            }
        }

        public long IsTemporaryUrlValid(string token, out string email)
        {
            IUserUow _userUow = null;
            Logger.Info("UserServiceFacade : IsTemporaryUrlValid() : Enter into method");
            try
            {
                _userUow = new UserUow();
                _userUow.BeginTransaction();

                long userId = 0;
                email = string.Empty;
                var existing = _userUow.TemporaryPasswordUrls.GetAll().FirstOrDefault(u => u.Token.Trim() == token.Trim());
                if (existing != null)
                {
                    TimeSpan duration = DateTime.UtcNow - existing.CreatedOn;
                    if (duration.TotalHours <= Configuration.TemparoryUrlExpiration)
                    {
                        userId = existing.RefUser;
                        email = _userUow.Credentials.GetAll().FirstOrDefault(c => c.RefUser == userId) != null ? _userUow.Credentials.GetAll().FirstOrDefault(c => c.RefUser == userId).LoginId : string.Empty;
                    }
                    else
                    {
                        _userUow.TemporaryPasswordUrls.Delete(existing);
                        _userUow.SaveChanges();
                    }
                    _userUow.Commit();
                }
                Logger.Info("UserServiceFacade : IsTemporaryUrlValid() : Exit from method");
                return userId;
            }
            catch (Exception ex)
            {
                if (_userUow != null)
                        _userUow.Rollback();
                    Logger.Error("UserServiceFacade : IsTemporaryUrlValid() : Caught an exception " + ex);
                throw ex;
            }
        }

        public List<string> GetUserName(string userName)
        {
            List<string> userList = null;
            Logger.Info("UserServiceFacade : GetUserName() : Enter into method");
            try
            {
                userList =  _userUow.Users.GetUserName(userName);
            }
            catch (Exception ex)
            {
                Logger.Error("UserServiceFacade : GetUserName() : Caught an exception " + ex);
                throw ex;
            }
            Logger.Info("UserServiceFacade : GetUserName() : Exit from method");
            return userList;
        }

        public List<string> GetUserEmail(string email)
        {
            List<string> userEmailList = null;
            Logger.Info("UserServiceFacade : GetUserEmail() : Enter into method");
            try
            {
                userEmailList = _userUow.Credentials.All().Where(u => u.LoginId.Contains(email)).Select(ele => ele.LoginId).Distinct().ToList();
            }
            catch (Exception ex)
            {
                Logger.Error("UserServiceFacade : GetUserEmail() : Caught an exception " + ex);
                throw ex;
            }
            Logger.Info("UserServiceFacade : GetUserEmail() : Exit from method");
            return userEmailList;
        }

        public List<UserAccount> GetUserDetailsForDashboard(long buyerPartyId, out int totalUsers, int pageNumber, int sortDirection)
        {
            try
            {
                Logger.Info("BuyerServiceFacade : GetUserDetailsForDashboard() : Enter the method");

                Logger.Info("BuyerServiceFacade : GetUserDetailsForDashboard() : Exit the method");
                return _userUow.Users.GetUserDetailsForDashboard(buyerPartyId, out totalUsers, pageNumber, sortDirection);
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerServiceFacade : GetUserDetailsForDashboard() : Caught an exception" + ex);
                throw;
            }
        }

        public long AddNewUser(ViewModel.User userModel, long auditorId)
        {
            IUserUow userUow = null;
            try
            {
                Logger.Info("UserServiceFacade : AddNewUser() : Entering the method");

                userUow = new UserUow();

                var partyUser = userModel.ToUserEntityModel(auditorId);

                //Let's start transaction
                userUow.BeginTransaction();

                // add party user
                userUow.Parties.Add(partyUser);
                userUow.SaveChanges();
                
                var partyPartyLink = new PartyPartyLink
                {
                    RefParty = partyUser.Id,
                    RefLinkedParty = userModel.OrganizationPartyId,
                    PartyPartyLinkType = Constants.PRIMARY_ORGANIZATION,
                    RefCreatedBy = auditorId,
                    RefLastUpdatedBy = auditorId
                };

                userUow.PartyPartyLinks.Add(partyPartyLink);
                userUow.SaveChanges();

                // all fine, let's commit
                userUow.Commit();
                Logger.Info("UserServiceFacade : AddNewUser() : Exiting the method");
                return partyUser.Id;
            }
            catch (Exception ex)
            {
                if (userUow != null)
                    userUow.Rollback();
                Logger.Error("UserServiceFacade : AddNewUser() : Caught an exception" + ex);
                throw;
            }
        }

        public long UpdateUser(ViewModel.User userModel, long auditorId)
        {
            IUserUow userUow = null;
            try
            {
                Logger.Info("UserServiceFacade : UpdateUser() : Entering the method");

                userUow = new UserUow();
                
                userUow.BeginTransaction();

                var userPartyPM = _userUow.Parties.GetAll().FirstOrDefault(u => u.Id == userModel.UserId);
                if (null != userPartyPM)
                {
                    userPartyPM.PartyName = string.Concat(userModel.FirstName.Trim(), " ", userModel.LastName.Trim());
                    _userUow.Parties.Update(userPartyPM);
                    _userUow.SaveChanges();
                }

                var userPersonPM = _userUow.People.GetAll().FirstOrDefault(u => u.Id == userModel.UserId);
                if (null != userPersonPM)
                {
                    userPersonPM.FirstName = userModel.FirstName;
                    userPersonPM.LastName = userModel.LastName;
                    userPersonPM.JobTitle = userModel.JobTitle;
                    userPersonPM.RefLastUpdatedBy = auditorId;
                    _userUow.People.Update(userPersonPM);
                    _userUow.SaveChanges();
                }

                var userPM = _userUow.Users.GetAll().FirstOrDefault(u => u.Id == userModel.UserId);
                if (null != userPM)
                {
                    userPM.UserType = (long)userModel.UserType;
                    _userUow.Users.Update(userPM);
                    _userUow.SaveChanges();
                }

                // all fine, let's commit
                userUow.Commit();
                Logger.Info("UserServiceFacade : UpdateUser() : Exiting the method");
                return userModel.UserId;
            }
            catch (Exception ex)
            {
                if (userUow != null)
                    userUow.Rollback();
                Logger.Error("UserServiceFacade : UpdateUser() : Caught an exception" + ex);
                throw;
            }
        }
    }
}
