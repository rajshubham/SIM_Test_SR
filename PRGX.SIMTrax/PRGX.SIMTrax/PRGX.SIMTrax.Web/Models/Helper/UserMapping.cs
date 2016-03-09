using PRGX.SIMTrax.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PRGX.SIMTrax.Web.Models.Helper
{
    public static class UserMapping
    {
        public static User Mapping(this AuditorUser auditorUserModel)
        {
            return new User()
            {
                ConfirmPassword = auditorUserModel.ConfirmPassword,
                Email = auditorUserModel.Email,
                FirstName = auditorUserModel.FirstName,
                IsActive = auditorUserModel.IsActive,
                LastName = auditorUserModel.LastName,
                OrganizationPartyId = auditorUserModel.OrganizationPartyId,
                Password = auditorUserModel.Password,
                UserType = auditorUserModel.UserType,
                UserId = auditorUserModel.Id
            };
        }
    }
}