using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Abstract;
using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository.Repository
{
    public class CredentialRepository : GenericRepository<Credential>, ICredentialRepository
    {
        public CredentialRepository(DbContext context) : base(context) { }

        public bool UpdateUserLastLoginDate(long userId, string loginId)
        {
            try
            {
                Logger.Info("CredentialRepository : UpdateUserLastLoginDate() : Enter the method");
                var result = false;
                var credentialPM = this.All().FirstOrDefault(v => v.LoginId == loginId && v.RefUser == userId);
                if (credentialPM != null)
                {
                    credentialPM.LastLoginDate = DateTime.UtcNow;
                    credentialPM.RefLastUpdatedBy = userId;
                    Update(credentialPM);
                    result = true;
                }
                Logger.Info("CredentialRepository : UpdateUserLastLoginDate() : Exit the method");
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("CredentialRepository : UpdateUserLastLoginDate() : Caught an exception" + ex);
                throw;
            }
        }

        public long GetUserIdFromCredentials(string email, string password, int lockCount, int timeSpanLimit, out bool isLocked)
        {
            try
            {
                Logger.Info("CredentialRepository : GetUserIdFromCredentials() : Enter into Method");
                isLocked = false;
                long userId = 0;
                if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password))
                {
                    var credentialPM = this.All().FirstOrDefault(u => u.LoginId == email.ToLower().Trim());
                    if (credentialPM != null)
                    {
                        if (credentialPM.Password.Trim().Equals(password.Trim()))
                        {
                            if (credentialPM.IsLocked)
                            {
                                var lockedTime = credentialPM.LockedTime;
                                if (lockedTime != null)
                                {
                                    TimeSpan span = DateTime.UtcNow - (DateTime)lockedTime;
                                    double totalMinutes = span.TotalMinutes;
                                    if (totalMinutes <= timeSpanLimit)
                                    {
                                        isLocked = true;
                                    }
                                    else
                                    {
                                        credentialPM.LockedTime = null;
                                        credentialPM.IsLocked = false;
                                        credentialPM.LoginAttemptCount = 0;
                                        credentialPM.RefLastUpdatedBy = userId;

                                        // let's update the db
                                        Update(credentialPM);
                                    }
                                }
                            }
                            else
                            {
                                credentialPM.LockedTime = null;
                                credentialPM.IsLocked = false;
                                credentialPM.LoginAttemptCount = 0;
                                credentialPM.RefLastUpdatedBy = userId;

                                // let's update the db
                                Update(credentialPM);
                            }

                            // get the validated userId
                            userId = credentialPM.RefUser;
                        }
                        else
                        {
                            var count = (credentialPM.LoginAttemptCount != null) ? credentialPM.LoginAttemptCount.Value : 0;

                            credentialPM.LoginAttemptCount = (count == lockCount) ? count : count + 1;
                            credentialPM.IsLocked = (credentialPM.LoginAttemptCount == lockCount) ? true : false;

                            if (credentialPM.LockedTime == null && credentialPM.IsLocked == true)
                            {
                                credentialPM.LockedTime = DateTime.UtcNow;
                            }

                            isLocked = credentialPM.IsLocked;
                            credentialPM.RefLastUpdatedBy = userId;

                            // let's update the db
                            Update(credentialPM);
                        }
                    }
                }
                Logger.Info("CredentialRepository : GetUserIdFromCredentials() : Exit from Method");
                return userId;
            }
            catch (Exception ex)
            {
                Logger.Error("CredentialRepository : GetUserIdFromCredentials() : Caught an Error " + ex);
                throw;
            }
        }

        public void UpdatePassword(string loginId, string encryptedPassword, long userId)
        {
            try
            {
                Logger.Info("CredentialRepository : UpdatePassword() : Enter the method");

                var credentialPM = this.All().FirstOrDefault(v => v.LoginId == loginId && v.RefUser == userId);
                if (credentialPM != null)
                {
                    credentialPM.Password = encryptedPassword;
                    credentialPM.RefLastUpdatedBy = userId;
                    Update(credentialPM);
                }
                Logger.Info("CredentialRepository : UpdatePassword() : Exit the method");
            }
            catch (Exception ex)
            {
                Logger.Error("CredentialRepository : UpdatePassword() : Caught an exception" + ex);
                throw;
            }
        }
    }
}
