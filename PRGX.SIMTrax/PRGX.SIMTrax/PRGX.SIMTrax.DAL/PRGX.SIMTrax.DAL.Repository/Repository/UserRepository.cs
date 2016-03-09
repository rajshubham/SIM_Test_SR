using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Abstract;
using PRGX.SIMTrax.DAL.Repository.Context;
using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PRGX.SIMTrax.DAL.Repository.Repository
{
    public class UserRepository :  GenericRepository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context) { }

        /// <summary>
        /// To Check whether ERmail Already Exists during Registration
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public  bool IsEmailExists(string email)
        {
            try
            {
                Logger.Info("UserRepository : IsEmailExists() : Enter the method");
                var result = false;
                var user = this.All().AsQueryable().Include("Credentials").FirstOrDefault(v => v.Credentials.Any(x => x.LoginId.ToLower() == email.ToLower()));
                if (user != null)
                {
                    result = true;
                }
                Logger.Info("UserRepository : IsEmailExists() : Exit the method");
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("UserRepository : IsEmailExists() : Caught an exception" + ex);
                throw;
            }
        }

        public UserDetails GetUserDetailsByOrganisationPartyId(long organisationPartyId)
        {
            try
            {
                Logger.Info("UserRepository : GetUserDetailsByOrganisationPartyId() : Enter the method");

                UserDetails userDetails = null;
                using (var ctx = new UserContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = true;
                    var details = ctx.Users.FirstOrDefault(v => v.Person.Party.PartyPartyLinks1.Any(x => x.RefLinkedParty == organisationPartyId));
                    if (details != null)
                    {
                        userDetails = new UserDetails();
                        userDetails.UserId = details.Id;
                        userDetails.LoginId = (details.Credentials.FirstOrDefault() != null) ? details.Credentials.FirstOrDefault().LoginId : string.Empty;
                        userDetails.FirstName = details.Person.FirstName;
                        userDetails.LastName = details.Person.LastName;
                        Party OrganisationParty = (details.Person.Party.PartyPartyLinks1.FirstOrDefault(x => x.RefLinkedParty == organisationPartyId && x.PartyPartyLinkType.Trim() == Constants.PRIMARY_ORGANIZATION) != null) ?
                            details.Person.Party.PartyPartyLinks1.FirstOrDefault(x => x.RefLinkedParty == organisationPartyId && x.PartyPartyLinkType.Trim() == Constants.PRIMARY_ORGANIZATION).Party : null;
                        userDetails.OrganisationName = (OrganisationParty != null) ? OrganisationParty.PartyName : string.Empty;
                        if (null != OrganisationParty)
                        {
                            var contactMethodParties = OrganisationParty.PartyPartyLinks.Where(x => x.PartyPartyLinkType.Trim().Equals(Constants.CONTACT_ORGANIZATION) && x.RefLinkedParty == OrganisationParty.Id).Select(p => p.Party1.Id).ToList();

                            var primaryContact = ctx.ContactPersons.FirstOrDefault(x => contactMethodParties.Contains(x.Person.Id) && x.ContactType == (Int16)ContactType.Primary);
                            if (null != primaryContact)
                            {
                                var contactMethodPhone = ctx.PartyContactMethodLinks.Any(x => x.RefParty == primaryContact.Person.Party.Id && x.ContactMethod.ContactMethodType == Constants.CONTACT_METHOD_PHONE) ? ctx.PartyContactMethodLinks.FirstOrDefault(x => x.RefParty == primaryContact.Person.Party.Id && x.ContactMethod.ContactMethodType == Constants.CONTACT_METHOD_PHONE).RefContactMethod : 0;
                                var primaryTelephoneNumber = ctx.Phones.FirstOrDefault(x => x.Type == Constants.PHONE_TYPE_TELEPHONE && x.RefContactMethod == contactMethodPhone);
                                userDetails.PrimaryContactTelephone = (primaryTelephoneNumber != null) ? primaryTelephoneNumber.PhoneNumber : string.Empty;
                                userDetails.PrimaryContactJobTitle = primaryContact.JobTitle;
                            }
                        }
                        userDetails.Password = (details.Credentials.FirstOrDefault() != null) ? details.Credentials.FirstOrDefault().Password : string.Empty;
                        userDetails.NeedPasswordChange = details.NeedsPasswordChange;
                        userDetails.UserType = details.UserType;
                        userDetails.RefOrganisationPartyId = OrganisationParty != null ? OrganisationParty.Id : 0;
                        userDetails.RefPersonPartyId = details.Person.Id;
                    }
                }
                Logger.Info("UserRepository : GetUserDetailsByOrganisationPartyId() : Exit the method");
                return userDetails;
            }
            catch (Exception ex)
            {
                Logger.Error("UserRepository : GetUserDetailsByOrganisationPartyId() : Caught an exception" + ex);
                throw;
            }
        }

        public UserDetails GetUserDetailByUserId (long userId)
        {
            try
            {
                Logger.Info("UserRepository : GetUserDetailByUserId() : Enter the method");
                UserDetails userDetails = null;
                using (var ctx = new UserContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = true;
                    var details = ctx.Users.FirstOrDefault(v => v.Id == userId);
                    if (details != null)
                    {
                        userDetails = new UserDetails();
                        userDetails.UserId = details.Id;
                        userDetails.LoginId = (details.Credentials.FirstOrDefault() != null) ? details.Credentials.FirstOrDefault().LoginId : string.Empty;
                        userDetails.FirstName = details.Person.FirstName;
                        userDetails.LastName = details.Person.LastName;
                        userDetails.JobTitle = details.Person.JobTitle;

                        Party OrganisationParty = details.Person.Party.PartyPartyLinks1.FirstOrDefault(x => x.PartyPartyLinkType.Trim() == Constants.PRIMARY_ORGANIZATION && x.RefParty == details.Person.Id) != null
                            ? details.Person.Party.PartyPartyLinks1.FirstOrDefault(x => x.PartyPartyLinkType.Trim() == Constants.PRIMARY_ORGANIZATION && x.RefParty == details.Person.Id).Party : null;
                        userDetails.OrganisationName = (OrganisationParty != null) ? OrganisationParty.PartyName : string.Empty;
                        if (null != OrganisationParty)
                        {
                            var contactMethodParties = OrganisationParty.PartyPartyLinks.Where(x => x.PartyPartyLinkType.Trim().Equals(Constants.CONTACT_ORGANIZATION) && x.RefLinkedParty == OrganisationParty.Id).Select(p => p.Party1.Id).ToList();

                            var primaryContact = ctx.ContactPersons.FirstOrDefault(x => contactMethodParties.Contains(x.Person.Id) && x.ContactType == (Int16)ContactType.Primary);
                            
                            if (null != primaryContact)
                            {
                                var contactMethodPhone = ctx.PartyContactMethodLinks.Any(x => x.RefParty == primaryContact.Person.Party.Id && x.ContactMethod.ContactMethodType == Constants.CONTACT_METHOD_PHONE) ? ctx.PartyContactMethodLinks.FirstOrDefault(x => x.RefParty == primaryContact.Person.Party.Id && x.ContactMethod.ContactMethodType == Constants.CONTACT_METHOD_PHONE).RefContactMethod : 0;
                                var primaryTelephoneNumber = ctx.Phones.FirstOrDefault(x => x.Type == Constants.PHONE_TYPE_TELEPHONE && x.RefContactMethod == contactMethodPhone);
                                var contactMethodEmail = ctx.PartyContactMethodLinks.Any(x => x.RefParty == primaryContact.Person.Party.Id && x.ContactMethod.ContactMethodType == Constants.CONTACT_METHOD_EMAIL) ? ctx.PartyContactMethodLinks.FirstOrDefault(x => x.RefParty == primaryContact.Person.Party.Id && x.ContactMethod.ContactMethodType == Constants.CONTACT_METHOD_EMAIL).RefContactMethod : 0;
                                var primaryEmail = ctx.Emails.FirstOrDefault(x => x.RefContactMethod == contactMethodEmail);

                                userDetails.PrimaryFirstName = primaryContact.Person.FirstName;
                                userDetails.PrimaryLastName = primaryContact.Person.LastName;
                                userDetails.PrimaryContactEmail = primaryEmail != null ? primaryEmail.EmailAddress : string.Empty;
                                userDetails.PrimaryContactTelephone = (primaryTelephoneNumber != null) ? primaryTelephoneNumber.PhoneNumber : string.Empty;
                                userDetails.PrimaryContactJobTitle = primaryContact.JobTitle;
                                userDetails.PrimaryContactPartyId = primaryContact.Person.Party.Id;
                            }
                        }
                        userDetails.Password = (details.Credentials.FirstOrDefault() != null) ? details.Credentials.FirstOrDefault().Password : string.Empty;
                        userDetails.NeedPasswordChange = details.NeedsPasswordChange;
                        userDetails.UserType = details.UserType;
                        userDetails.RefOrganisationPartyId = OrganisationParty != null ? OrganisationParty.Id : 0;
                        userDetails.RefPersonPartyId = details.Person.Id;
                        userDetails.IsActive = details.Person.Party.IsActive.HasValue ? details.Person.Party.IsActive.Value : false;
                    }
                }
                Logger.Info("UserRepository : GetUserDetailByUserId() : Exit the method");
                return userDetails;
            }
            catch (Exception ex)
            {
                Logger.Error("UserRepository : GetUserDetailByUserId() : Caught an exception" + ex);
                throw;
            }
        }

        public List<User> GetPartyUser(long organizationPartyId)
        {
            try
            {
                Logger.Info("UserRepository : GetPartyUser() : Enter the method");
                var users = new List<User>();
                using (var ctx = new UserContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = true;
                    var userPartyId = ctx.PartyPartyLinks.Where(p => p.RefLinkedParty == organizationPartyId && p.PartyPartyLinkType.Trim().Equals(Constants.PRIMARY_ORGANIZATION)).Select(p => p.RefParty).ToList();
                    users = ctx.Users.Where(u => userPartyId.Contains(u.Person.Party.Id)).ToList();
                }
                Logger.Info("UserRepository : GetPartyUser() : Exit the method");
                return users;
            }
            catch (Exception ex)
            {
                Logger.Error("UserRepository : GetPartyUser() : Caught an exception" + ex);
                throw;
            }
        }

        public List<UserAccount> GetAllUsers(string loginId, string userName, int userType, string status, short source, out int total, int pageIndex, int pageSize, int sortDirection)
        {
            Logger.Info("UserRepository : GetAllUsers() : Entering the method");
            var userAccounts = new List<UserAccount>();
            var users = new List<User>();
            try
            {
                using (var ctx = new UserContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = true;

                    var query = ctx.Users.AsQueryable();

                    if (userType != (short)UserType.None)
                    {
                        query = query.Where(u => u.UserType == userType);
                    }
                    if (!String.IsNullOrWhiteSpace(userName))
                    {
                        query = query.Where(u => u.Person.Party.PartyName.ToLower().Contains(userName.ToLower()));
                    }
                    if (!String.IsNullOrWhiteSpace(loginId))
                    {
                        query = query.Where(u => u.Credentials.Any(c => c.LoginId.ToLower().Contains(loginId.ToLower())));
                    }
                    if (status == "Active")
                    {
                        query = query.Where(u => u.Person.Party.IsActive == true);
                    }
                    if (status == "Disabled")
                    {
                        query = query.Where(u => u.Person.Party.IsActive == false);
                    }
                    if (source == Convert.ToInt16(ProjectSource.SIM))
                    {
                        query = query.Where(u => u.Person.Party.ProjectSource == (short)ProjectSource.SIM);
                    }
                    if (source == Convert.ToInt16(ProjectSource.CIPS))
                    {
                        query = query.Where(u => u.Person.Party.ProjectSource == (short)ProjectSource.CIPS);
                    }
                    total = query.Count();
                    int skipCount = Convert.ToInt32(pageIndex - 1) * pageSize;
                    switch (sortDirection)
                    {
                        case 1:
                            users = query.OrderBy(elem => elem.Person.Party.PartyName).Skip(skipCount).Take(pageSize).ToList();
                            break;
                        case 2:
                            users = query.OrderByDescending(elem => elem.Person.Party.PartyName).Skip(skipCount).Take(pageSize).ToList();
                            break;
                        case 3:
                            users = query.OrderByDescending(elem => elem.CreatedOn).Skip(skipCount).Take(pageSize).ToList();
                            break;
                    }
                    foreach (var userPM in users)
                    {
                        var userAccount = new UserAccount();
                        userAccount.UserId = userPM.Id;
                        userAccount.UserName = userPM.Person.Party.PartyName;
                        userAccount.LoginId = userPM.Credentials.FirstOrDefault() != null ? userPM.Credentials.FirstOrDefault().LoginId : string.Empty;
                        userAccount.UserType = CommonMethods.Description((UserType)userPM.UserType);
                        userAccount.OriginalTermsVersion = userPM.AcceptedTermsOfUses.FirstOrDefault() != null ? userPM.AcceptedTermsOfUses.OrderBy(d => d.AcceptedDate).FirstOrDefault().TermsOfUse.Version : string.Empty;
                        userAccount.LatestTermsVersion = userPM.AcceptedTermsOfUses.FirstOrDefault() != null ? userPM.AcceptedTermsOfUses.OrderByDescending(d => d.AcceptedDate).FirstOrDefault().TermsOfUse.Version : string.Empty;

                        if (userPM.AcceptedTermsOfUses.FirstOrDefault() != null)
                            userAccount.TermsAcceptedDate = userPM.AcceptedTermsOfUses.OrderByDescending(d => d.AcceptedDate).FirstOrDefault().AcceptedDate;

                        userAccount.LastLogin = userPM.Credentials.FirstOrDefault() != null ? userPM.Credentials.FirstOrDefault().LastLoginDate : null;
                        userAccount.ProjectSource = CommonMethods.Description((ProjectSource)userPM.Person.Party.ProjectSource);
                        userAccount.ActiveStatus = userPM.Person.Party.IsActive.HasValue ? (userPM.Person.Party.IsActive.Value ? "Active" : "Disabled") : "Disabled";
                        userAccounts.Add(userAccount);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("UserRepository : GetAllUsers() : Caught an exception" + ex);
                throw ex;
            }
            Logger.Info("UserRepository : GetAllUsers() : Exiting the method");
            return userAccounts;
        }

        public List<string> GetUserName(string userName)
        {
            Logger.Info("UserRepository : GetUserName() : Entering the method");
            List<string> userList = null;
            try
            {
                using (var ctx = new UserContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = true;

                    userList = ctx.Users.Where(u => u.Person.Party.PartyName.ToLower().Contains(userName.ToLower())).Select(u => u.Person.Party.PartyName).Distinct().ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("UserRepository : GetUserName() : Caught an exception" + ex);
                throw ex;
            }
            Logger.Info("UserRepository : GetUserName() : Exiting the method");
            return userList;
        }

        public List<UserAccount> GetUserDetailsForDashboard(long buyerPartyId, out int totalUsers, int pageNumber, int sortDirection)
        {
            try
            {
                Logger.Info("UserRepository : GetUserDetailsForDashboard() : Enter the method");
                var users = new List<UserAccount>();
                int pageSize = 5;
                int skipcount = (pageNumber - 1) * pageSize;
                using (var ctx = new UserContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = true;
                    var userParties = new List<Party>();
                    var query = ctx.PartyPartyLinks.Where(p => p.RefLinkedParty == buyerPartyId && p.PartyPartyLinkType.Trim().Equals(Constants.PRIMARY_ORGANIZATION)).Select(v => v.Party1).AsQueryable();
                    totalUsers = query.Count();
                    switch (sortDirection)
                    {
                        case 1:
                            userParties = query.OrderBy(u => u.PartyName).Skip(skipcount).Take(pageSize).ToList();
                            break;
                        case 2:
                            userParties = query.OrderByDescending(u => u.PartyName).Skip(skipcount).Take(pageSize).ToList();
                            break;
                        case 3:
                            userParties = query.OrderBy(u => u.CreatedOn).Skip(skipcount).Take(pageSize).ToList();
                            break;
                    }
                    foreach (var userPM in userParties)
                    {
                        users.Add(new UserAccount()
                        {
                            UserId = userPM.Person.Party.Id,
                            UserName = userPM.PartyName,
                            UserType = CommonMethods.Description((UserType)userPM.Person.User.UserType),
                            ActiveStatus = userPM.IsActive.HasValue ? (userPM.IsActive.Value ? "Active" : "Disabled") : "Disabled",
                            LoginId = userPM.Person.User.Credentials.FirstOrDefault() != null ? userPM.Person.User.Credentials.FirstOrDefault().LoginId : string.Empty
                        });
                    }
                }
                Logger.Info("UserRepository : GetUserDetailsForDashboard() : Exit the method");
                return users;
            }
            catch (Exception ex)
            {
                Logger.Error("UserRepository : GetUserDetailsForDashboard() : Caught an exception" + ex);
                throw;
            }
        }
    }
}

