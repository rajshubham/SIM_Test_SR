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
    public static class BuyerRegisterMapper
    {
        public static Party ToPartyModelUser(this BuyerRegister registerModel)
        {
            var credentials = new List<Credential>();
            var user = new DAL.Entity.User();
            var people =new  Person();

            credentials.Add(new Credential()
            {
                IsLocked = false,
                LoginId = registerModel.BuyerEmail.Trim(),
                Password = string.Empty,
                RefCreatedBy = 1
            });
            user = new DAL.Entity.User()
            {
                UserType = (long)UserType.AdminBuyer,
                NeedsPasswordChange = true,
                Credentials = credentials,
                RefCreatedBy = 1
            };
            people = new Person()
            {
                FirstName = registerModel.BuyerFirstName.Trim(),
                LastName = registerModel.BuyerLastName.Trim(),
                JobTitle = !string.IsNullOrWhiteSpace(registerModel.BuyerJobTitle) ? registerModel.BuyerJobTitle.Trim() : string.Empty,
                PersonType = Constants.PERSON_TYPE_USER,
                User = user,
                RefCreatedBy = 1
            };

            var party = new Party()
            {
                PartyName = string.Concat(registerModel.BuyerFirstName.Trim(), " ", registerModel.BuyerLastName.Trim()),
                PartyType = Constants.PARTY_TYPE_PERSON,
                IsActive = true,
                Person = people,
                ProjectSource = (long)ProjectSource.SIM
            };
            return party;
        }

        public static Party ToPartyModelBuyer(this BuyerRegister registerModel)
        {
            var buyer = new Buyer();
            var organization = new Organization();
            var address = new List<DAL.Entity.Address>();
            var contactPerson = new DAL.Entity.ContactPerson();
            var contactMethodLink = new List<PartyContactMethodLink>();
            var legalEntity = new LegalEntity()
            {
                IsActive = true,
                PartyName = registerModel.BuyerOrganisationName.Trim(),
                RefCreatedBy = 1
            };
            buyer = new Buyer()
            {
                RefCreatedBy = 1,
            };
            organization = new Organization()
            {
                Buyer = buyer,
                LegalEntity = legalEntity,
                RefCreatedBy = 1,
                OrganizationType = Constants.ORGANIZATION_TYPE_BUYER,
                BusinessSectorDescription = registerModel.BusinessSectorDescription,
                BusinessSectorId = registerModel.BuyerSector,
                EmployeeSize = registerModel.BuyerNumberOfEmployees,
                RegistrationSubmittedOn = DateTime.UtcNow,
                Status = (short)CompanyStatus.Submitted,
                TurnOverSize = registerModel.BuyerTurnOver
            };
            address.Add(new DAL.Entity.Address()
            {
                Line1 = registerModel.BuyerFirstAddressLine1,
                Line2 = registerModel.BuyerFirstAddressLine2,
                City = registerModel.BuyerFirstAddressCity,
                State = registerModel.BuyerFirstAddressState,
                RefCountryId = Convert.ToInt64(registerModel.BuyerFirstAddressCountry),
                AddressType = (short)AddressType.Primary,
                ZipCode = registerModel.BuyerFirstAddressPostalCode
            });
            var contactMethod = new ContactMethod
            {
                Addresses = address,
                ContactMethodType = Constants.CONTACT_METHOD_ADDRESS,
                RefCreatedBy = registerModel.BuyerPartyId,
                RefLastUpdatedBy = registerModel.BuyerPartyId
            };
            contactMethodLink.Add(new PartyContactMethodLink
            {
                ContactMethod = contactMethod,
                RefParty = registerModel.BuyerPartyId
            });
            return new Party
            {
                PartyName = registerModel.BuyerOrganisationName.Trim(),
                PartyType = Constants.PARTY_TYPE_ORGANIZATION,
                IsActive = true,
                RefCreatedBy = 1,
                Organization = organization,
                PartyContactMethodLinks = contactMethodLink,
                ProjectSource = (long)ProjectSource.SIM
            };
        }

        public static Party ToContactMethodPersonPartyModel(this BuyerRegister registerModel, long refPartyId)
        {
            var contactPerson = new DAL.Entity.ContactPerson();
            contactPerson = new DAL.Entity.ContactPerson
            {
                ContactType = (short)ContactType.Primary,
                JobTitle = registerModel.BuyerJobTitle,
                RefCreatedBy = refPartyId,
                RefLastUpdatedBy = refPartyId
            };
            var person = new Person();
            person = new Person
            {
                FirstName = registerModel.BuyerFirstName,
                LastName = registerModel.BuyerLastName,
                JobTitle = registerModel.BuyerJobTitle,
                RefCreatedBy = refPartyId,
                RefLastUpdatedBy = refPartyId,
                PersonType = Constants.PERSON_TYPE_CONTACT_PERSON,
                ContactPerson = contactPerson
            };
            return new Party()
            {
                PartyName = string.Concat(registerModel.BuyerFirstName.Trim(), " ", registerModel.BuyerLastName.Trim()),
                PartyType = Constants.PARTY_TYPE_PERSON,
                IsActive = true,
                Person = person,
                ProjectSource = (long)ProjectSource.SIM,
                RefCreatedBy = refPartyId,
                RefLastUpdatedBy = refPartyId
            };
        }

        public static BuyerRegister ToBuyerRegisterModel(this Buyer buyer)
        {
            var buyerLoginId = string.Empty;
            var buyerFirstName = string.Empty;
            var buyerLastName = string.Empty;
            var buyerJobTitle = string.Empty;
            var buyerUser = buyer.Organization.Party.PartyPartyLinks.Where(p => p.PartyPartyLinkType.Trim().Equals(Constants.PRIMARY_ORGANIZATION)).FirstOrDefault();
            if (buyerUser != null)
            {
                buyerLoginId = buyerUser.Party1.Person.User.Credentials.FirstOrDefault().LoginId;
                buyerFirstName = buyerUser.Party1.Person.FirstName;
                buyerLastName = buyerUser.Party1.Person.LastName;
                buyerJobTitle = buyerUser.Party1.Person.JobTitle;
            }

            var buyerAddress = buyer.Organization.Party.PartyContactMethodLinks.Where(c => c.ContactMethod.ContactMethodType.Trim().Equals(Constants.CONTACT_METHOD_ADDRESS)).FirstOrDefault() != null ? buyer.Organization.Party.PartyContactMethodLinks.Where(c => c.ContactMethod.ContactMethodType.Trim().Equals(Constants.CONTACT_METHOD_ADDRESS)).FirstOrDefault().ContactMethod.Addresses.FirstOrDefault() : null;

            var contactMethodParties = buyer.Organization.Party.PartyPartyLinks.Where(x => x.PartyPartyLinkType.Trim().Equals(Constants.CONTACT_ORGANIZATION) && x.RefLinkedParty == buyer.Organization.Id).ToList();

            var primaryContact = contactMethodParties.FirstOrDefault(x => x.Party1.Person.ContactPerson.ContactType == (Int16)ContactType.Primary);

            var primaryTelephoneNumber = primaryContact.Party1.PartyContactMethodLinks.FirstOrDefault(x => x.ContactMethod.ContactMethodType.Trim().Equals(Constants.CONTACT_METHOD_PHONE)) != null ? primaryContact.Party1.PartyContactMethodLinks.FirstOrDefault(x => x.ContactMethod.ContactMethodType.Trim().Equals(Constants.CONTACT_METHOD_PHONE)).ContactMethod : null;

            var buyerTelephone = primaryTelephoneNumber != null ? primaryTelephoneNumber.Phones.FirstOrDefault() : null;

            return new BuyerRegister
            {
                BusinessSectorDescription = buyer.Organization.BusinessSectorDescription,
                BuyerEmail = buyerLoginId,
                BuyerFirstAddressCity = buyerAddress != null ? buyerAddress.City : string.Empty,
                BuyerFirstAddressCountry = buyerAddress != null ? buyerAddress.RefCountryId.Value.ToString() : string.Empty,
                BuyerFirstAddressLine1 = buyerAddress != null ? buyerAddress.Line1 : string.Empty,
                BuyerFirstAddressLine2 = buyerAddress != null ? buyerAddress.Line2 : string.Empty,
                BuyerFirstAddressPostalCode = buyerAddress != null ? buyerAddress.ZipCode : string.Empty,
                BuyerFirstAddressState = buyerAddress != null ? buyerAddress.State : string.Empty,
                BuyerFirstName = buyerFirstName,
                BuyerLastName = buyerLastName,
                BuyerJobTitle = buyerJobTitle,
                BuyerNumberOfEmployees = buyer.Organization.EmployeeSize.HasValue ? buyer.Organization.EmployeeSize.Value : 0,
                BuyerOrganisationName = buyer.Organization.Party.PartyName,
                BuyerPartyId = buyer.Organization.Party.Id,
                BuyerSector = buyer.Organization.BusinessSectorId.HasValue ? buyer.Organization.BusinessSectorId.Value : 0,
                BuyerTelephone = buyerTelephone != null ? buyerTelephone.PhoneNumber : string.Empty,
                BuyerTurnOver = buyer.Organization.TurnOverSize.HasValue ? buyer.Organization.TurnOverSize.Value : 0,
                ContactPersonId = primaryContact != null ? (primaryContact.Party1.Person != null ? primaryContact.Party1.Person.Id : 0) : 0,
                UserPartyId = buyerUser.Party1.Id
            };
        }
    }
}
