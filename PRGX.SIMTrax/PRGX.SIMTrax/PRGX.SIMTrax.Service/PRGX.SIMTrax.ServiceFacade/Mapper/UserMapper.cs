using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.Domain.Util;
using PRGX.SIMTrax.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.ServiceFacade.Mapper
{
    public static class UserMapper
    {
        public static ViewModel.User ToUserViewModel(this UserDetails userPM)
        {
            return new ViewModel.User()
            {
                Email = userPM.LoginId,
                FirstName = userPM.FirstName,
                JobTitle = userPM.PrimaryContactJobTitle,
                LastName = userPM.LastName,
                LoginId = userPM.LoginId,
                PrimaryContactPartyId = userPM.PrimaryContactPartyId,
                PrimaryEmail = userPM.PrimaryContactEmail,
                PrimaryFirstName = userPM.PrimaryFirstName,
                PrimaryLastName = userPM.PrimaryLastName,
                Telephone = userPM.PrimaryContactTelephone,
                UserId = userPM.UserId,
                OrganizationPartyId = userPM.RefOrganisationPartyId,
                UserType = (UserType)userPM.UserType,
                IsAdminUser = ((UserType)userPM.UserType == UserType.AdminAuditor || (UserType)userPM.UserType == UserType.AdminBuyer || (UserType)userPM.UserType == UserType.AdminSupplier) ? true : false,
                IsActive = userPM.IsActive
            };
        }

        public static Domain.Model.TemporaryPasswordUrl ToDomainModel(this DAL.Entity.TemporaryPasswordUrl temporaryPassowrdUrlPM)
        {
            return new Domain.Model.TemporaryPasswordUrl()
            {
                TemporaryPasswordUrlId = temporaryPassowrdUrlPM.Id,
                Token = temporaryPassowrdUrlPM.Token,
                CreatedDate = temporaryPassowrdUrlPM.CreatedOn,
                ModifiedDate = temporaryPassowrdUrlPM.LastUpdatedOn,
                PasswordURL = temporaryPassowrdUrlPM.PasswordURL,
                UserId = temporaryPassowrdUrlPM.RefUser
            };
        }

        public static DAL.Entity.TemporaryPasswordUrl ToEntityModel(this Domain.Model.TemporaryPasswordUrl temporaryPassowrdUrl)
        {
            return new DAL.Entity.TemporaryPasswordUrl()
            {
                Id = temporaryPassowrdUrl.TemporaryPasswordUrlId,
                Token = temporaryPassowrdUrl.Token,
                CreatedOn = temporaryPassowrdUrl.CreatedDate,
                LastUpdatedOn = temporaryPassowrdUrl.ModifiedDate,
                PasswordURL = temporaryPassowrdUrl.PasswordURL,
                RefUser = temporaryPassowrdUrl.UserId
            };
        }

        public static Party ToUserEntityModel(this ViewModel.User userModel, long auditorId)
        {
            var credentials = new List<Credential>();
            var userList = new DAL.Entity.User();
            var peopleList = new Person();

            credentials.Add(new Credential()
            {
                IsLocked = false,
                LoginId = userModel.Email.Trim(),
                Password = CommonMethods.EncryptMD5Password(userModel.Email.ToLower() + userModel.Password.Trim()),
                RefCreatedBy = auditorId
            });
            userList = new DAL.Entity.User()
            {
                UserType = (long)userModel.UserType,
                NeedsPasswordChange = false,
                Credentials = credentials,
                RefCreatedBy = auditorId
            };
            peopleList = new Person()
            {
                FirstName = userModel.FirstName.Trim(),
                LastName = userModel.LastName.Trim(),
                JobTitle = !string.IsNullOrWhiteSpace(userModel.JobTitle) ? userModel.JobTitle.Trim() : string.Empty,
                PersonType = Constants.PERSON_TYPE_USER,
                User = userList,
                RefCreatedBy = auditorId
            };
            var party = new Party()
            {
                PartyName = string.Concat(userModel.FirstName.Trim(), " ", userModel.LastName.Trim()),
                PartyType = Constants.PARTY_TYPE_PERSON,
                IsActive = true,
                Person = peopleList,
                ProjectSource = (long)ProjectSource.SIM,
                RefCreatedBy = auditorId
            };
            return party;
        }
    }
}
