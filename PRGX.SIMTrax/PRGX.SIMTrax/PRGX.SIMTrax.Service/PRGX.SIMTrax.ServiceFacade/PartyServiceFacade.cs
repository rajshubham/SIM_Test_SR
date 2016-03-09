using PRGX.SIMTrax.DAL;
using PRGX.SIMTrax.DAL.Abstract;
using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.Domain.Util;
using PRGX.SIMTrax.ServiceFacade.Abstract;
using PRGX.SIMTrax.ServiceFacade.Mapper;
using PRGX.SIMTrax.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PRGX.SIMTrax.ServiceFacade
{
    public class PartyServiceFacade : IPartyServiceFacade
    {
        private readonly ISupplierUow _sellerUow;

        public PartyServiceFacade()
        {
            _sellerUow = new SupplierUow();
        }

        public bool IsOrganisationExists(string organisationName)
        {
            try
            {
                Logger.Info("PartyServiceFacade : IsOrganisationExists() : Enter the method");
                var result = false;
                result = _sellerUow.Parties.IsOrganisationExists(organisationName);
                Logger.Info("PartyServiceFacade : IsOrganisationExists() : Exit the method");
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : IsOrganisationExists() : Caught an exception" + ex);
                throw;
            }
        }

        public bool UpdateSellerPartyDetails(SellerRegister registerModel)
        {
            ISupplierUow sellerUow = null;
            try
            {
                Logger.Info("PartyServiceFacade : UpdateSellerPartyDetails() : Enter the method");

                sellerUow = new SupplierUow();
                sellerUow.BeginTransaction();

                sellerUow.Suppliers.UpdateSellerDetails(registerModel.ToSellerModel(), registerModel.SellerPartyId);
                sellerUow.SaveChanges();

                var orgId = sellerUow.Organizations.GetAll().Where(x => x.Id == registerModel.SellerPartyId).FirstOrDefault().Id;
                sellerUow.Organizations.UpdateOrganizationDetails(registerModel.ToOrganizationModel(orgId), registerModel.SellerPartyId);
                sellerUow.SaveChanges();

                sellerUow.IndustryCodeOrganizationLinks.AddUpdateIndustryCodeOrganisationLinks(registerModel.MapToIndustryCodeOrganizationLinks(orgId), orgId);
                sellerUow.SaveChanges();

                sellerUow.PartyIdentifers.AddUpdateRange(registerModel.ToPartyIdentifierModel());
                sellerUow.SaveChanges();

                sellerUow.PartyRegionLinks.AddUpdatePartyRegions(registerModel.ToPartyRegionLinkModel(), registerModel.SellerPartyId);
                sellerUow.SaveChanges();

                var partyContactMethodLink = sellerUow.PartyContactMethodLinks.GetAll().Where(x => x.RefParty == registerModel.SellerPartyId).Select(x => x.RefContactMethod).ToList();
                var addressContactMethod = sellerUow.ContactMethods.GetAll().Where(x => x.ContactMethodType.Trim().Equals(Constants.CONTACT_METHOD_ADDRESS) && partyContactMethodLink.Contains(x.Id)).Select(x => x.Id).ToList();
                var addressList = registerModel.ToAddressModel();
                foreach (var address in addressList)
                {
                    var addressPM = sellerUow.Addresses.GetAll().FirstOrDefault(x => x.AddressType == address.AddressType && addressContactMethod.Contains(x.RefContactMethod));
                    if (null == addressPM)
                    {
                        var contactMethodLink = new List<PartyContactMethodLink>();
                        contactMethodLink.Add(new PartyContactMethodLink
                        {
                            RefParty = registerModel.SellerPartyId
                        });
                        var newAddressContactMethod = new ContactMethod
                        {
                            ContactMethodType = Constants.CONTACT_METHOD_ADDRESS,
                            PartyContactMethodLinks = contactMethodLink,
                            RefCreatedBy = registerModel.UserPartyId,
                            RefLastUpdatedBy = registerModel.UserPartyId
                        };
                        sellerUow.ContactMethods.Add(newAddressContactMethod);
                        sellerUow.SaveChanges();

                        address.RefContactMethod = newAddressContactMethod.Id;
                        sellerUow.Addresses.AddUpdateAddress(address, newAddressContactMethod.Id);
                        sellerUow.SaveChanges();
                    }
                    else
                    {
                        sellerUow.Addresses.AddUpdateAddress(address, addressPM.RefContactMethod);
                        sellerUow.SaveChanges();
                    }
                }
                sellerUow.Commit();
                Logger.Info("PartyServiceFacade : UpdateSellerPartyDetails() : Exit the method");
                return true;
            }
            catch (Exception ex)
            {
                if (sellerUow != null)
                    sellerUow.Rollback();
                Logger.Error("PartyServiceFacade : UpdateSellerPartyDetails() : Caught an error" + ex);
                throw;
            }
        }

        public bool UpdatePartyContactDetails(SellerRegister registerModel)
        {
            ISupplierUow sellerUow = null;
            try
            {
                Logger.Info("PartyServiceFacade : UpdatePartyContactDetails() : Enter the method");

                sellerUow = new SupplierUow();
                sellerUow.BeginTransaction();

                sellerUow.Organizations.UpdateOrganizationStatus(registerModel.SellerPartyId, (short)registerModel.Status, registerModel.UserPartyId);
                sellerUow.SaveChanges();

                var contactPersonPartyList = registerModel.ToContactPersonPartyModel();

                foreach (var contactPersonParty in contactPersonPartyList)
                {
                    sellerUow.Parties.Add(contactPersonParty);
                    sellerUow.SaveChanges();
                    var partyPartyLink = new PartyPartyLink
                    {
                        RefParty = contactPersonParty.Id,
                        RefLinkedParty = registerModel.SellerPartyId,
                        PartyPartyLinkType = Constants.CONTACT_ORGANIZATION,
                        RefCreatedBy = registerModel.UserPartyId,
                        RefLastUpdatedBy = registerModel.UserPartyId
                    };

                    sellerUow.PartyPartyLinks.Add(partyPartyLink);
                    sellerUow.SaveChanges();

                    var partyContactMethodLink = sellerUow.PartyContactMethodLinks.GetAll().Where(x => x.RefParty == contactPersonParty.Id).Select(x => x.RefContactMethod).ToList();
                    var contact = registerModel.ContactDetails.FirstOrDefault(c => (short)c.ContactType == contactPersonParty.Person.ContactPerson.ContactType);
                    if (null != contact)
                    {
                        if (!string.IsNullOrWhiteSpace(contact.Email))
                        {
                            var contactMethodEmailLink = new List<PartyContactMethodLink>();
                            contactMethodEmailLink.Add(new PartyContactMethodLink
                            {
                                RefParty = contactPersonParty.Id
                            });
                            var contactMethod = new ContactMethod
                            {
                                ContactMethodType = Constants.CONTACT_METHOD_EMAIL,
                                PartyContactMethodLinks = contactMethodEmailLink,
                                RefCreatedBy = registerModel.UserPartyId,
                                RefLastUpdatedBy = registerModel.UserPartyId
                            };
                            sellerUow.ContactMethods.Add(contactMethod);
                            sellerUow.SaveChanges();

                            sellerUow.Emails.Add(new DAL.Entity.Email
                            {
                                EmailAddress = contact.Email,
                                RefContactMethod = contactMethod.Id
                            });
                            sellerUow.SaveChanges();
                        }
                        if (!string.IsNullOrWhiteSpace(contact.Telephone))
                        {
                            var contactMethodPhoneLink = new List<PartyContactMethodLink>();
                            contactMethodPhoneLink.Add(new PartyContactMethodLink
                            {
                                RefParty = contactPersonParty.Id
                            });
                            var contactMethod = new ContactMethod
                            {
                                ContactMethodType = Constants.CONTACT_METHOD_PHONE,
                                PartyContactMethodLinks = contactMethodPhoneLink,
                                RefCreatedBy = registerModel.UserPartyId,
                                RefLastUpdatedBy = registerModel.UserPartyId
                            };
                            sellerUow.ContactMethods.Add(contactMethod);
                            sellerUow.SaveChanges();

                            sellerUow.Phones.Add(new Phone
                            {
                                PhoneNumber = contact.Telephone,
                                RefContactMethod = contactMethod.Id,
                                Type = Constants.PHONE_TYPE_TELEPHONE
                            });
                            sellerUow.SaveChanges();
                        }
                    }
                }

                sellerUow.Commit();
                Logger.Info("PartyServiceFacade : UpdatePartyContactDetails() : Exit the method");
                return true;
            }
            catch (Exception ex)
            {
                if (sellerUow != null)
                    sellerUow.Rollback();
                Logger.Error("PartyServiceFacade : UpdatePartyContactDetails() : Caught an error" + ex);
                throw;
            }
        }

        public OrganizationDetail GetOrganizationDetail(long organizationPartyId)
        {
            try
            {
                Logger.Info("UserServiceFacade: GetOrganizationDetail() : Enter the method : [organizationPartyId: " + organizationPartyId + " ]");

                OrganizationDetail organizationDetails = null;
                organizationDetails = _sellerUow.Organizations.GetOrganizationDetail(organizationPartyId);

                Logger.Info("UserServiceFacade: GetOrganizationDetail() : Exit the method: [organizationPartyId: " + organizationPartyId + " ]");
                return organizationDetails;
            }
            catch (Exception ex)
            {
                Logger.Error("UserServiceFacade : GetOrganizationDetail() : Caught an exception" + ex);
                throw;
            }
        }

        public SellerRegister GetSellerOrganizationDetailsByPartyId(long organizationPartyId)
        {
            try
            {
                Logger.Info("PartyServiceFacade: GetSellerOrganizationDetailsByPartyId() : Enter the method");
                SellerRegister companyDetails = null;
                var sellerPartyDetails = _sellerUow.Parties.GetSellerOrganizationDetailsByPartyId(organizationPartyId);
                companyDetails = sellerPartyDetails.MappingToSellerRegisterModel();
                Logger.Info("PartyServiceFacade: GetSellerOrganizationDetailsByPartyId() : Exit the method");
                return companyDetails;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : GetSellerOrganizationDetailsByPartyId() : Caught an exception" + ex);
                throw;
            }
        }

        #region Edit Profile
        public SellerRegister GetCompanyDetailsByPartyId(long organizationPartyId)
        {
            try
            {
                Logger.Info("PartyServiceFacade: GetCompanyDetailsByPartyId() : Enter the method");
                SellerRegister companyDetails = null;
                var sellerPartyDetails = _sellerUow.Parties.GetCompanyDetailsByPartyId(organizationPartyId);
                companyDetails = sellerPartyDetails.MappingToCompanyDetails();
                Logger.Info("PartyServiceFacade: GetCompanyDetailsByPartyId() : Exit the method");
                return companyDetails;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : GetCompanyDetailsByPartyId() : Caught an exception" + ex);
                throw;
            }
        }

        public bool SaveCompanyDetails(SellerRegister model)
        {
            ISupplierUow sellerUow = null;
            try
            {
                Logger.Info("PartyServiceFacade: SaveCompanyDetails() : Enter the method");
                bool result = false;

                sellerUow = new SupplierUow();
                sellerUow.BeginTransaction();

                var seller = sellerUow.Suppliers.All().FirstOrDefault(v => v.Organization.Id == model.SellerPartyId);
                if (seller != null)
                {
                    seller.TradingName = model.Trading;
                    seller.TypeOfSeller = model.TypeOfCompany;
                    seller.IsSubsidary = model.IsSubsidaryStatus;
                    seller.UltimateParent = model.UltimateParent;
                    seller.EstablishedYear = model.CompanyYear;
                    sellerUow.SaveChanges();
                }

                sellerUow.PartyIdentifers.AddUpdateRange(model.ToPartyIdentifierModel());
                sellerUow.SaveChanges();
                sellerUow.Commit();

                Logger.Info("PartyServiceFacade: SaveCompanyDetails() : Exit the method");
                result = true;
                return result;
            }
            catch (Exception ex)
            {
                if (sellerUow != null)
                    sellerUow.Rollback();
                Logger.Error("PartyServiceFacade : SaveCompanyDetails() : Caught an exception" + ex);
                throw;
            }
        }

        public SellerRegister GetCapabilityDetailsByPartyId(long organizationPartyId)
        {
            try
            {
                Logger.Info("PartyServiceFacade: GetCapabilityDetailsByPartyId() : Enter the method");
                SellerRegister companyDetails = null;
                var sellerPartyDetails = _sellerUow.Parties.GetCapabilityDetailsByPartyId(organizationPartyId);
                companyDetails = sellerPartyDetails.MappingToCapabilityDetails();
                Logger.Info("PartyServiceFacade: GetCapabilityDetailsByPartyId() : Exit the method");
                return companyDetails;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : GetCapabilityDetailsByPartyId() : Caught an exception" + ex);
                throw;
            }
        }
        public bool SaveCapabilityDetails(SellerRegister model)
        {
            ISupplierUow sellerUow = null;
            try
            {
                Logger.Info("PartyServiceFacade: SaveCapabilityDetails() : Enter the method");
                bool result = false;
                sellerUow = new SupplierUow();
                sellerUow.BeginTransaction();

                var organisation = sellerUow.Organizations.All().FirstOrDefault(v => v.Id == model.SellerPartyId);
                var seller = sellerUow.Suppliers.All().FirstOrDefault(v => v.Organization.Id == model.SellerPartyId);
                if (organisation != null)
                {
                    organisation.EmployeeSize = model.NumberOfEmployees;
                    organisation.TurnOverSize = model.TurnOver;
                    organisation.BusinessSectorId = model.sector;
                    organisation.BusinessSectorDescription = model.BusinessSectorDescription;
                    sellerUow.SaveChanges();

                    var IndustryCodeOrganizationLinks = model.MapToIndustryCodeOrganizationLinks(organisation.Id);
                    sellerUow.IndustryCodeOrganizationLinks.AddUpdateIndustryCodeOrganisationLinks(IndustryCodeOrganizationLinks, organisation.Id);
                    sellerUow.SaveChanges();
                }
                if (seller != null)
                {
                    seller.MaxContractValue = model.MaxContractValue;
                    seller.MinContractValue = model.MinContractValue;
                    sellerUow.SaveChanges();
                }
                sellerUow.PartyRegionLinks.AddUpdatePartyRegions(model.ToPartyRegionLinkModel(), model.SellerPartyId);
                sellerUow.SaveChanges();
                Logger.Info("PartyServiceFacade: SaveCapabilityDetails() : Exit the method");
                sellerUow.Commit();
                result = true;
                return result;
            }
            catch (Exception ex)
            {
                if (sellerUow != null)
                    sellerUow.Rollback();
                Logger.Error("PartyServiceFacade : SaveCapabilityDetails() : Caught an exception" + ex);
                throw;
            }
        }

        public bool SaveMarketingDetails(SellerRegister model)
        {
            ISupplierUow sellerUow = null;
            try
            {
                Logger.Info("PartyServiceFacade: SaveMarketingDetails() : Enter the method");
                bool result = false;
                sellerUow = new SupplierUow();
                sellerUow.BeginTransaction();
                var seller = sellerUow.Suppliers.All().FirstOrDefault(v => v.Organization.Id == model.SellerPartyId);
                if (seller != null)
                {
                    seller.FacebookAccount = model.OrganisationFacebookaccount;
                    seller.TwitterAccount = model.OrganisationTwitteraccount;
                    seller.LinkedInAccount = model.OrganisationLinkedInaccount;
                    seller.WebsiteLink = model.WebsiteLink;
                    seller.BusinessDescription = model.BusinessDescription;
                    
                    sellerUow.SaveChanges();
                }
                Logger.Info("PartyServiceFacade: SaveMarketingDetails() : Exit the method");
                sellerUow.Commit();
                result = true;
                return result;
            }
            catch (Exception ex)
            {
                if (sellerUow != null)
                    sellerUow.Rollback();
                Logger.Error("PartyServiceFacade : SaveMarketingDetails() : Caught an exception" + ex);
                throw;
            }
        }

        public List<ViewModel.Address> GetAddressDetailsByPartyId(long partyId)
        {
            try
            {
                Logger.Info("PartyServiceFacade: GetAddressDetailsByPartyId() : Enter the method");
                var addressList = new List<ViewModel.Address>();
                var addressListPM = _sellerUow.Addresses.GetAddressDetailsByPartyId(partyId);
                addressList = addressListPM.ToViewModel();
                Logger.Info("PartyServiceFacade: GetAddressDetailsByPartyId() : Exit the method");
                return addressList;
            }
            catch (Exception ex)
            {

                Logger.Error("PartyServiceFacade : GetAddressDetailsByPartyId() : Caught an exception" + ex);
                throw;
            }
        }

        public long AddOrUpdateAddressList(ViewModel.Address address, long sellerPartyId,long sellerUserPartyId)
        {
            ISupplierUow sellerUow = null;
            try
            {
                sellerUow = new SupplierUow();
                sellerUow.BeginTransaction();
                Logger.Info("PartyServiceFacade: AddOrUpdateAddressList() : Enter the method");
                long contactMethoId = 0;
                if (address.Id > 0)
                {
                    var addressPM = sellerUow.Addresses.All().FirstOrDefault(v => v.Id == address.Id);
                    if (addressPM != null)
                    {
                        addressPM.Line1 = address.Line1;
                        addressPM.Line2 = address.Line2;
                        addressPM.City = address.City;
                        addressPM.State = address.State;
                        addressPM.ZipCode = address.ZipCode;
                        addressPM.RefCountryId = address.RefCountryId;
                        addressPM.AddressType = address.AddressType;
                        addressPM.RefLastUpdatedBy = sellerUserPartyId;
                        sellerUow.Addresses.Update(addressPM);
                        sellerUow.SaveChanges();
                        contactMethoId = addressPM.RefContactMethod;
                    }
                }
                else
                {

                    var addressPM = address.ToDBModel();
                    var contactMethod = new ContactMethod()
                    {
                        ContactMethodType = Constants.CONTACT_METHOD_ADDRESS,
                        RefCreatedBy = sellerUserPartyId,
                        RefLastUpdatedBy = sellerUserPartyId
                    };
                    sellerUow.ContactMethods.Add(contactMethod);
                    sellerUow.SaveChanges();
                    var partyContactMethodLink = new PartyContactMethodLink()
                    {
                        RefParty = sellerPartyId,
                        RefContactMethod = contactMethod.Id
                    };
                    sellerUow.PartyContactMethodLinks.Add(partyContactMethodLink);
                    sellerUow.SaveChanges();
                    addressPM.RefContactMethod = contactMethod.Id;
                    addressPM.RefLastUpdatedBy = sellerUserPartyId;
                    addressPM.RefCreatedBy = sellerUserPartyId;
                    sellerUow.Addresses.Add(addressPM);
                    sellerUow.SaveChanges();
                    contactMethoId = contactMethod.Id;
                }
                sellerUow.Commit();
                Logger.Info("PartyServiceFacade: AddOrUpdateAddressList() : Exit the method");
                return contactMethoId;
            }
            catch (Exception ex)
            {
                if (sellerUow != null)
                    sellerUow.Rollback();
                Logger.Error("PartyServiceFacade : AddOrUpdateAddressList() : Caught an exception" + ex);
                throw;
            }


        }
        public bool DeleteAddressById(long addressId)
        {
            ISupplierUow sellerUow = null;

            try
            {
                Logger.Info("PartyServiceFacade: DeleteAddressById() : Enter the method");
                var result = false;
                sellerUow = new SupplierUow();
                var address = sellerUow.Addresses.All().FirstOrDefault(v => v.Id == addressId);
                if (address != null)
                {
                    sellerUow.BeginTransaction();
                    var contactMethod = sellerUow.ContactMethods.All().FirstOrDefault(v => v.Id == address.RefContactMethod);
                    if (address.AddressType == (Int16)AddressType.Remittance && contactMethod != null)
                    {
                        //ToDo:We have to delte mapped buyer supplier address
                        var legalEntites = sellerUow.LegalEntityProfiles.All().Where(v => v.RefContactMethod == contactMethod.Id && v.ProfileType == Constants.PROFILE_TYPE_ADDRESS).ToList();
                        foreach (var item in legalEntites)
                        {
                            sellerUow.LegalEntityProfiles.Delete(item);
                            sellerUow.SaveChanges();
                        }
                    }

                    var partyContactMethodLinks = sellerUow.PartyContactMethodLinks.All().Where(v => v.ContactMethod.Id == contactMethod.Id).ToList();
                    foreach (var item in partyContactMethodLinks)
                    {
                        sellerUow.PartyContactMethodLinks.Delete(item);
                        sellerUow.SaveChanges();
                    }

                    sellerUow.Addresses.Delete(address);
                    sellerUow.SaveChanges();
                    sellerUow.ContactMethods.Delete(contactMethod);
                    sellerUow.SaveChanges();
                    sellerUow.Commit();
                    result = true;
                }
                Logger.Info("PartyServiceFacade: DeleteAddressById() : Exit the method");
                return result;
            }
            catch (Exception ex)
            {
                if (sellerUow != null)
                    sellerUow.Rollback();
                Logger.Error("PartyServiceFacade : DeleteAddressById() : Caught an exception" + ex);

                throw;
            }
        }
        public SellerRegister GetMarketingDetailsByPartyId(long organizationPartyId)
        {
            try
            {
                Logger.Info("PartyServiceFacade: GetCapabilityDetailsByPartyId() : Enter the method");
                SellerRegister companyDetails = null;
                var sellerPartyDetails = _sellerUow.Parties.GetMarketingDetailsByPartyId(organizationPartyId);
                companyDetails = sellerPartyDetails.MappingToMarketingDetails();
                Logger.Info("PartyServiceFacade: GetCapabilityDetailsByPartyId() : Exit the method");
                return companyDetails;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : GetCapabilityDetailsByPartyId() : Caught an exception" + ex);
                throw;
            }
        }
        public bool AddOrUpdateSellerLogoDetails(ViewModel.Document document, long sellerPartyId)
        {
            ISupplierUow sellerUow = null;
            try
            {
                Logger.Info("PartyServiceFacade: AddOrUpdateSellerLogoDetails() : Enter the method");
                bool result = false;
                sellerUow = new SupplierUow();
                var organisaton = sellerUow.Organizations.All().FirstOrDefault(v => v.Id == sellerPartyId);
                sellerUow.BeginTransaction();
                if (organisaton != null && organisaton.RefLogoDocument != null)
                {
                    var documentPM = sellerUow.Documents.All().FirstOrDefault(v => v.Id == organisaton.RefLogoDocument);
                    if (documentPM != null)
                    {
                        documentPM.FileName = document.FileName;
                        documentPM.FilePath = document.FilePath;
                        documentPM.ContentType = document.ContentType;
                        documentPM.ContentLength = document.ContentLength;
                    }
                    sellerUow.Documents.Update(documentPM);
                    sellerUow.SaveChanges();
                    organisaton.RefLogoDocument = documentPM.Id;
                    sellerUow.SaveChanges();
                    result = true;
                }
                else
                {
                    var documentPM = new DAL.Entity.Document();
                    documentPM.FileName = document.FileName;
                    documentPM.FilePath = document.FilePath;
                    documentPM.ContentType = document.ContentType;
                    documentPM.ContentLength = document.ContentLength;
                    sellerUow.Documents.Add(documentPM);
                    sellerUow.SaveChanges();
                    organisaton.RefLogoDocument = documentPM.Id;
                    sellerUow.SaveChanges();
                    result = true;
                }
                sellerUow.Commit();
                Logger.Info("PartyServiceFacade: AddOrUpdateSellerLogoDetails() : Exit the method");
                return result;
            }
            catch (Exception ex)
            {
                if (sellerUow != null)
                    sellerUow.Rollback();

                Logger.Error("PartyServiceFacade : GetCapabilityDetailsByPartyId() : Caught an exception" + ex); Logger.Info("PartyServiceFacade: GetCapabilityDetailsByPartyId() : Enter the method");
                throw;
            }
        }
        public List<long> GetIndustryCodesByOrganisationPartyId(long sellerPartyId)
        {
            try
            {
                Logger.Info("PartyServiceFacade: GetIndustryCodesByOrganisationPartyId() : Enter the method");
                List<long> industryCodes = null;
                industryCodes = _sellerUow.Parties.GetIndustryCodesByOrganisationPartyId(sellerPartyId);
                Logger.Info("PartyServiceFacade: GetIndustryCodesByOrganisationPartyId() : Exit the method");
                return industryCodes;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : GetIndustryCodesByOrganisationPartyId() : Caught an exception" + ex);
                throw;
            }
        }
        public List<ViewModel.ContactPerson> GetContactDetailsByPartyId(long sellerPartyId)
        {
            try
            {
                Logger.Info("PartyServiceFacade: GetContactDetailsByPartyId() : Enter the method");
                List<ViewModel.ContactPerson> contacts = null;
                var contactPersonsPM = _sellerUow.ContactPersons.GetContactDetailsByPartyId(sellerPartyId);
                contacts = contactPersonsPM.ToViewModel();
                Logger.Info("PartyServiceFacade: GetContactDetailsByPartyId() : Exit the method");
                return contacts;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : GetContactDetailsByPartyId() : Caught an exception" + ex);
                throw;
            }
        }
        public ViewModel.Address GetAddressDetailsByContactMethodId(long contactMethodId)
        {
            try
            {
                Logger.Info("PartyServiceFacade: GetAddressDetailsByContactMethodId() : Enter the method");
                ViewModel.Address address = null;
                var addressPM = _sellerUow.Addresses.GetAddressDetailsByContactMethodId(contactMethodId);
                address = addressPM.ToViewModel();
                Logger.Info("PartyServiceFacade: GetAddressDetailsByContactMethodId() : Exit the method");
                return address;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : GetContactDetailsByPartyId() : Caught an exception" + ex);
                throw;
            }
        }
        public ViewModel.ContactPerson GetContactByRoleAndPartyId(long sellerPartyId, int contactType)
        {
            try
            {
                Logger.Info("PartyServiceFacade: GetContactByRoleAndPartyId() : Enter the method");
                ViewModel.ContactPerson contact = null;
                var contactPM = _sellerUow.ContactPersons.GetContactByRoleAndPartyId(sellerPartyId, contactType);
                if (contactPM != null)
                    contact = contactPM.ToViewModel();
                Logger.Info("PartyServiceFacade: GetContactByRoleAndPartyId() : Exit the method");
                return contact;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : GetContactDetailsByPartyId() : Caught an exception" + ex);
                throw;
            }
        }

        public bool AddOrUpdateContactsList(ViewModel.ContactPerson contact, long sellerPartyId,long sellerUserPartyId)
        {
            ISupplierUow sellerUow = null;
            Logger.Info("PartyServiceFacade: AddOrUpdateContactsList() : Enter the method");
            try
            {
                var result = false;
                sellerUow = new SupplierUow();
                sellerUow.BeginTransaction();
                if (contact.ContactPartyId > 0)
                {
                    var contactMethods = sellerUow.ContactMethods.All().Where(x => x.PartyContactMethodLinks.Any(v => v.RefParty == contact.ContactPartyId)).ToList();
                    
                    if (contact.RefAddressContactMethod > 0)
                    {
                        var contactAddressMethodLink = sellerUow.PartyContactMethodLinks.All().Where(v => v.RefParty == contact.ContactPartyId && v.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS).ToList();
                        foreach (var item in contactAddressMethodLink)
                        {
                            sellerUow.PartyContactMethodLinks.Delete(item);
                            sellerUow.SaveChanges();
                        }
                        var partyContactMethodLink = new PartyContactMethodLink()
                        {
                            RefParty = contact.ContactPartyId,
                            RefContactMethod = (long)contact.RefAddressContactMethod
                        };
                        sellerUow.PartyContactMethodLinks.Add(partyContactMethodLink);
                        sellerUow.SaveChanges();
                    }
                    var emailContactMethod = contactMethods.FirstOrDefault(v => v.ContactMethodType.Trim() == Constants.CONTACT_METHOD_EMAIL);
                    if (emailContactMethod != null)
                    {
                        var email = sellerUow.Emails.All().FirstOrDefault(v => v.RefContactMethod == emailContactMethod.Id);
                        if (email != null)
                        {
                            email.EmailAddress = contact.Email;
                            sellerUow.Emails.Update(email);
                        }
                    }
                    var phoneContactMethod = contactMethods.Where(v => v.ContactMethodType.Trim() == Constants.CONTACT_METHOD_PHONE).Select(c => c.Id).ToList();
                    if (phoneContactMethod.Count > 0)
                    {
                        var phone = sellerUow.Phones.All().FirstOrDefault(v => phoneContactMethod.Contains(v.RefContactMethod) && v.Type.Trim() == Constants.PHONE_TYPE_TELEPHONE);
                        if (phone != null)
                        {
                            phone.PhoneNumber = contact.Telephone;
                            phone.Type = Constants.PHONE_TYPE_TELEPHONE;
                            sellerUow.Phones.Update(phone);
                            sellerUow.SaveChanges();
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(contact.Telephone))
                            {
                                var contactMethod = new ContactMethod
                                {
                                    ContactMethodType = Constants.CONTACT_METHOD_PHONE,
                                    RefCreatedBy = sellerUserPartyId,
                                    RefLastUpdatedBy = sellerUserPartyId
                                };
                                sellerUow.ContactMethods.Add(contactMethod);
                                sellerUow.SaveChanges();
                                var partyContactMethodLink = new PartyContactMethodLink()
                                {
                                    RefParty = contact.ContactPartyId,
                                    RefContactMethod = contactMethod.Id
                                };
                                sellerUow.PartyContactMethodLinks.Add(partyContactMethodLink);
                                sellerUow.SaveChanges();
                                sellerUow.Phones.Add(new DAL.Entity.Phone
                                {
                                    PhoneNumber = contact.Telephone,
                                    Type = Constants.PHONE_TYPE_TELEPHONE,
                                    RefContactMethod = contactMethod.Id
                                });
                                sellerUow.SaveChanges();
                            }
                        }
                        var fax = sellerUow.Phones.All().FirstOrDefault(v => phoneContactMethod.Contains(v.RefContactMethod) && v.Type.Trim() == Constants.PHONE_TYPE_FAX);
                        if (fax != null)
                        {
                            if (!string.IsNullOrWhiteSpace(contact.Fax))
                            {
                                fax.PhoneNumber = contact.Fax;
                                fax.Type = Constants.PHONE_TYPE_FAX;
                                sellerUow.Phones.Update(fax);
                                sellerUow.SaveChanges();
                            }
                            else
                            {
                                var contactMethod = sellerUow.ContactMethods.All().FirstOrDefault(v => v.Id == fax.RefContactMethod);
                                var partyContactMethodLink = sellerUow.PartyContactMethodLinks.All().FirstOrDefault(v => v.RefContactMethod == contactMethod.Id && v.RefParty == contact.ContactPartyId);
                                sellerUow.Phones.Delete(fax);
                                sellerUow.SaveChanges();
                                if(partyContactMethodLink != null)
                                {
                                    sellerUow.PartyContactMethodLinks.Delete(partyContactMethodLink);
                                    sellerUow.SaveChanges();
                                }
                                if(contactMethod != null)
                                {

                                    sellerUow.ContactMethods.Delete(contactMethod);
                                    sellerUow.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(contact.Fax))
                            {
                                var contactMethod = new ContactMethod
                                {
                                    ContactMethodType = Constants.CONTACT_METHOD_PHONE,
                                    RefCreatedBy = sellerUserPartyId,
                                    RefLastUpdatedBy = sellerUserPartyId
                                };
                                sellerUow.ContactMethods.Add(contactMethod);
                                sellerUow.SaveChanges();
                                var partyContactMethodLink = new PartyContactMethodLink()
                                {
                                    RefParty = contact.ContactPartyId,
                                    RefContactMethod = contactMethod.Id
                                };
                                sellerUow.PartyContactMethodLinks.Add(partyContactMethodLink);
                                sellerUow.SaveChanges();
                                sellerUow.Phones.Add(new DAL.Entity.Phone
                                {
                                    PhoneNumber = contact.Fax,
                                    Type = Constants.PHONE_TYPE_FAX,
                                    RefContactMethod = contactMethod.Id
                                });
                                sellerUow.SaveChanges();
                            }
                        }
                    }
                    var person = sellerUow.Persons.All().FirstOrDefault(v => v.Id == contact.ContactPartyId);
                    if (person != null)
                    {
                        person.FirstName = contact.FirstName;
                        person.LastName = contact.LastName;
                        person.RefLastUpdatedBy = sellerUserPartyId;
                        sellerUow.Persons.Update(person);
                        sellerUow.SaveChanges();
                    }
                    var existingList = sellerUow.ContactPersons.All().Where(v => v.Person.Party.PartyPartyLinks1.Any(c => c.RefLinkedParty == sellerPartyId) && v.ContactType == contact.ContactType).ToList();
                    foreach (var item in existingList)
                    {
                        item.ContactType = null;
                        sellerUow.ContactPersons.Update(item);
                        sellerUow.SaveChanges();
                    }
                    var contactperson = sellerUow.ContactPersons.All().FirstOrDefault(v => v.Id == contact.Id);

                    if (contactperson != null)
                    {
                        contactperson.JobTitle = contact.JobTitle;
                        contactperson.ContactType = contact.ContactType;
                        contactperson.RefLastUpdatedBy = sellerUserPartyId;
                        sellerUow.ContactPersons.Update(contactperson);
                        sellerUow.SaveChanges();
                    }
                }
                else
                {
                    var contactPersonParty = contact.ToContactPersonPartyModel(sellerUserPartyId);
                    sellerUow.Parties.Add(contactPersonParty);
                    sellerUow.SaveChanges();
                    var partyPartyLink = new PartyPartyLink
                    {
                        RefParty = contactPersonParty.Id,
                        RefLinkedParty = sellerPartyId,
                        PartyPartyLinkType = Constants.CONTACT_ORGANIZATION,
                        RefCreatedBy = sellerUserPartyId,
                        RefLastUpdatedBy =sellerUserPartyId
                    };

                    sellerUow.PartyPartyLinks.Add(partyPartyLink);
                    sellerUow.SaveChanges();
                    if (contact.RefAddressContactMethod > 0)
                    {
                        var partyContactMethodLink = new PartyContactMethodLink()
                        {
                            RefParty = contactPersonParty.Id,
                            RefContactMethod = (long)contact.RefAddressContactMethod
                        };
                        sellerUow.PartyContactMethodLinks.Add(partyContactMethodLink);
                        sellerUow.SaveChanges();
                    }
                    if (!string.IsNullOrWhiteSpace(contact.Email))
                    {
                        var contactMethod = new ContactMethod
                        {
                            ContactMethodType = Constants.CONTACT_METHOD_EMAIL,
                            RefCreatedBy = sellerUserPartyId,
                            RefLastUpdatedBy = sellerUserPartyId
                        };
                        sellerUow.ContactMethods.Add(contactMethod);
                        sellerUow.SaveChanges();
                        var partyContactMethodLink = new PartyContactMethodLink()
                        {
                            RefParty = contactPersonParty.Id,
                            RefContactMethod = contactMethod.Id
                        };
                        sellerUow.PartyContactMethodLinks.Add(partyContactMethodLink);
                        sellerUow.SaveChanges();

                        sellerUow.Emails.Add(new DAL.Entity.Email
                        {
                            EmailAddress = contact.Email,
                            RefContactMethod = contactMethod.Id
                        });
                        sellerUow.SaveChanges();
                    }
                    if (!string.IsNullOrWhiteSpace(contact.Telephone))
                    {
                        var contactMethod = new ContactMethod
                        {
                            ContactMethodType = Constants.CONTACT_METHOD_PHONE,
                            RefCreatedBy = sellerUserPartyId,
                            RefLastUpdatedBy = sellerUserPartyId
                        };
                        sellerUow.ContactMethods.Add(contactMethod);
                        sellerUow.SaveChanges();
                        var partyContactMethodLink = new PartyContactMethodLink()
                        {
                            RefParty = contactPersonParty.Id,
                            RefContactMethod = contactMethod.Id
                        };
                        sellerUow.PartyContactMethodLinks.Add(partyContactMethodLink);
                        sellerUow.SaveChanges();
                        sellerUow.Phones.Add(new DAL.Entity.Phone
                        {
                            PhoneNumber = contact.Telephone,
                            Type = Constants.PHONE_TYPE_TELEPHONE,
                            RefContactMethod = contactMethod.Id
                        });
                        sellerUow.SaveChanges();
                    }
                    if (!string.IsNullOrWhiteSpace(contact.Fax))
                    {
                        var contactMethod = new ContactMethod
                        {
                            ContactMethodType = Constants.CONTACT_METHOD_PHONE,
                            RefCreatedBy = sellerUserPartyId,
                            RefLastUpdatedBy = sellerUserPartyId
                        };
                        sellerUow.ContactMethods.Add(contactMethod);
                        sellerUow.SaveChanges();
                        var partyContactMethodLink = new PartyContactMethodLink()
                        {
                            RefParty = contactPersonParty.Id,
                            RefContactMethod = contactMethod.Id
                        };
                        sellerUow.PartyContactMethodLinks.Add(partyContactMethodLink);
                        sellerUow.SaveChanges();
                        sellerUow.Phones.Add(new DAL.Entity.Phone
                        {
                            PhoneNumber = contact.Fax,
                            Type = Constants.PHONE_TYPE_FAX,
                            RefContactMethod = contactMethod.Id
                        });
                        sellerUow.SaveChanges();
                    }
                }
                sellerUow.Commit();
                result = true;
                Logger.Info("PartyServiceFacade: AddOrUpdateContactsList() : Exit the method");
                return result;
            }
            catch (Exception ex)
            {

                if (sellerUow != null)
                    sellerUow.Rollback();
                Logger.Error("PartyServiceFacade : AddOrUpdateContactsList() : Caught an exception" + ex);

                throw;
            }
        }


        public int BuyersAssignedToContact(long contactPartyId)
        {
            int count = 0;
            try
            {
                count = _sellerUow.PartyPartyLinks.All().Count(v => v.RefParty == contactPartyId && v.PartyPartyLinkType == Constants.CONTACT_BUYER);
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : BuyersAssignedToContact() : Caught an exception" + ex);
                throw;
            }
            return count;
        }

        public bool DeleteContactById(long contactPartyId, long sellerPartyId)
        {
            ISupplierUow sellerUow = null;

            try
            {
                Logger.Info("PartyServiceFacade: DeleteContactById() : Enter the method");
                var result = false;
                sellerUow = new SupplierUow();
                var contact = sellerUow.ContactPersons.All().FirstOrDefault(v => v.Person.Party.Id == contactPartyId);
                if (contact != null)
                {
                    sellerUow.BeginTransaction();

                    //ToDo:We have to delte mapped buyer supplier contacts
                    sellerUow.ContactPersons.Delete(contact);
                    sellerUow.SaveChanges();
                    var partyPartyLink = sellerUow.PartyPartyLinks.All().FirstOrDefault(v => v.RefParty == contactPartyId && v.PartyPartyLinkType == Constants.CONTACT_ORGANIZATION && v.RefLinkedParty == sellerPartyId);
                    if (partyPartyLink != null)
                    {
                        sellerUow.PartyPartyLinks.Delete(partyPartyLink);
                        sellerUow.SaveChanges();
                    }
                    var buyerContactPartyLinks = sellerUow.PartyPartyLinks.All().Where(v => v.RefParty == contactPartyId && v.PartyPartyLinkType == Constants.CONTACT_BUYER).ToList();

                    foreach (var item in buyerContactPartyLinks)
                    {
                        sellerUow.PartyPartyLinks.Delete(item);
                        sellerUow.SaveChanges();
                    }
                    var email = sellerUow.Emails.All().FirstOrDefault(v => v.ContactMethod.PartyContactMethodLinks.Any(c => c.RefParty == contactPartyId) && v.ContactMethod.ContactMethodType == Constants.CONTACT_METHOD_EMAIL);
                    if (email != null)
                    {
                        sellerUow.Emails.Delete(email);
                        sellerUow.SaveChanges();
                    }
                    var phones = sellerUow.Phones.All().Where(v => v.ContactMethod.PartyContactMethodLinks.Any(c => c.RefParty == contactPartyId) && v.ContactMethod.ContactMethodType == Constants.CONTACT_METHOD_PHONE).ToList();
                    foreach (var item in phones)
                    {
                        sellerUow.Phones.Delete(item);
                        sellerUow.SaveChanges();
                    }
                    var person = sellerUow.Persons.All().FirstOrDefault(v => v.Id == contact.Id);
                    if (person != null)
                    {
                        sellerUow.Persons.Delete(person);
                        sellerUow.SaveChanges();
                    }
                    var contactMethodLinks = sellerUow.ContactMethods.All().Where(v => v.PartyContactMethodLinks.Any(c => c.RefParty == contactPartyId) && v.ContactMethodType.Trim() != Constants.CONTACT_METHOD_ADDRESS).ToList();
                    var partyContactMethodLinks = sellerUow.PartyContactMethodLinks.All().Where(v => v.RefParty == contactPartyId).ToList();
                    foreach (var item in partyContactMethodLinks)
                    {
                        sellerUow.PartyContactMethodLinks.Delete(item);
                        sellerUow.SaveChanges();
                    }
                    foreach (var item in contactMethodLinks)
                    {
                        sellerUow.ContactMethods.Delete(item);
                        sellerUow.SaveChanges();
                    }
                    var party = sellerUow.Parties.All().FirstOrDefault(v => v.Id == person.Id);
                    if (party != null)
                    {
                        sellerUow.Parties.Delete(party);
                        sellerUow.SaveChanges();
                    }
                    sellerUow.Commit();
                    result = true;
                }
                Logger.Info("PartyServiceFacade: DeleteContactById() : Exit the method");
                return result;
            }
            catch (Exception ex)
            {
                if (sellerUow != null)
                    sellerUow.Rollback();
                Logger.Error("PartyServiceFacade : DeleteContactById() : Caught an exception" + ex);

                throw;
            }
        }
        public List<ViewModel.Invitee> GetReferenceDetailsBySellerId(long sellerId)
        {
            try
            {
                Logger.Info("PartyServiceFacade: GetReferenceDetailsBySellerId() : Enter the method");
                List<ViewModel.Invitee> referenceDetails = new List<ViewModel.Invitee>();
                var inviteePms = _sellerUow.Invitees.GetInviteeDetailsBySellerId(sellerId);
                if (inviteePms.Count > 0)
                {
                    referenceDetails = inviteePms.ToViewModel();
                }
                Logger.Info("PartyServiceFacade: GetReferenceDetailsBySellerId() : Exit the method");
                return referenceDetails;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : GetReferenceDetailsBySellerId() : Caught an exception" + ex);
                throw;
            }
        }
        public bool AddOrUpdateReferenceDetails(ViewModel.Invitee referenceDetail, long sellerId, long sellerUserPartyId)
        {
            ISupplierUow sellerUow = null;
            Logger.Info("PartyServiceFacade: AddOrUpdateReferenceDetails() : Enter the method");
            try
            {
                var result = false;
                sellerUow = new SupplierUow();
                sellerUow.BeginTransaction();
                var partyId = sellerUow.Parties.All().FirstOrDefault(v => v.Organization.Supplier.Id == sellerId) != null ?
                    sellerUow.Parties.All().FirstOrDefault(v => v.Organization.Supplier.Id == sellerId).Id : 0;
                if (referenceDetail.Id > 0)
                {
                    var inviteePm = sellerUow.Invitees.All().FirstOrDefault(v => v.Id == referenceDetail.Id);
                    if (inviteePm != null)
                    {
                        inviteePm.ClientName = referenceDetail.ClientName;
                        inviteePm.ContactName = referenceDetail.ContactName;
                        inviteePm.JobTitle = referenceDetail.JobTitle;
                        inviteePm.Email = referenceDetail.Email;
                        inviteePm.PhoneNumber = referenceDetail.Phone;
                        inviteePm.MailingAddress = referenceDetail.MailingAddress;
                        inviteePm.Fax = referenceDetail.Fax;
                        inviteePm.ClientRole = referenceDetail.ClientRole;
                        inviteePm.CanWeContact = referenceDetail.CanWeContact;
                        inviteePm.RefLastUpdatedBy = sellerUserPartyId;
                        sellerUow.Invitees.Update(inviteePm);
                        sellerUow.SaveChanges();
                    }
                }
                else
                {
                    var inviteepm = referenceDetail.ToDBModel(sellerUserPartyId);
                    inviteepm.RefReferee = sellerId;
                    sellerUow.Invitees.Add(inviteepm);
                    sellerUow.SaveChanges();
                }
                sellerUow.Commit();
                result = true;
                Logger.Info("PartyServiceFacade: AddOrUpdateReferenceDetails() : Exit the method");
                return result;
            }
            catch (Exception ex)
            {

                if (sellerUow != null)
                    sellerUow.Rollback();
                Logger.Error("PartyServiceFacade : AddOrUpdateReferenceDetails() : Caught an exception" + ex);

                throw;
            }
        }

        public bool DeleteReferenceById(long referenceId)
        {
            ISupplierUow sellerUow = null;

            try
            {
                Logger.Info("PartyServiceFacade: DeleteReferenceById() : Enter the method");
                var result = false;
                sellerUow = new SupplierUow();
                var invitee = sellerUow.Invitees.All().FirstOrDefault(v => v.Id == referenceId);
                if (invitee != null)
                {
                    sellerUow.BeginTransaction();

                    //ToDo:We have to delte mapped buyer supplier invittes
                    var assignedReferences = sellerUow.BuyerSupplierReferences.All().Where(v => v.RefInvitee == referenceId).ToList();
                    foreach (var item in assignedReferences)
                    {
                        sellerUow.BuyerSupplierReferences.Delete(item);
                        sellerUow.SaveChanges();
                    }
                    sellerUow.Invitees.Delete(invitee);
                    sellerUow.SaveChanges();
                    sellerUow.Commit();
                    result = true;
                }
                Logger.Info("PartyServiceFacade: DeleteReferenceById() : Exit the method");
                return result;
            }
            catch (Exception ex)
            {
                if (sellerUow != null)
                    sellerUow.Rollback();
                Logger.Error("PartyServiceFacade : DeleteReferenceById() : Caught an exception" + ex);

                throw;
            }
        }
        public List<ViewModel.BankAccount> GetBankDetailsByOrganisationId(long organisationId)
        {
            var bankDetails = new List<ViewModel.BankAccount>();
            try
            {
                Logger.Info("PartyServiceFacade: GetBankDetailsByOrganisationId() : Enter the method");
                var bankDetailsPMs = _sellerUow.BankAccounts.GetBankDetailsByOrganisationId(organisationId);
                if (bankDetailsPMs.Count > 0)
                {
                    bankDetails = bankDetailsPMs.ToViewModel();
                }
                Logger.Info("PartyServiceFacade: GetBankDetailsByOrganisationId() : Exit the method");
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : GetBankDetailsByOrganisationId() : Caught an exception" + ex);
                throw;
            }
            return bankDetails;
        }
        public bool AddOrUpdateBankDetails(ViewModel.BankAccount bankDetail, long organisationId, long sellerUserPartyId)
        {
            ISupplierUow sellerUow = null;
            Logger.Info("PartyServiceFacade: AddOrUpdateBankDetails() : Enter the method");
            try
            {
                var result = false;
                sellerUow = new SupplierUow();
                sellerUow.BeginTransaction();
                var legalEntityId = sellerUow.LegalEntites.All().FirstOrDefault(v => v.Organizations.Any(c => c.Id == organisationId)) != null ?
                     sellerUow.LegalEntites.All().FirstOrDefault(v => v.Organizations.Any(c => c.Id == organisationId)).Id : 0;
                if (legalEntityId == 0)
                {
                    return result;
                }
                if (bankDetail.Id > 0)
                {
                    var bankPm = sellerUow.BankAccounts.All().FirstOrDefault(v => v.Id == bankDetail.Id);
                    if (bankPm != null)
                    {
                        bankPm.AccountName = bankDetail.AccountName;
                        bankPm.AccountNumber = bankDetail.AccountNumber;
                        bankPm.BranchIdentifier = bankDetail.BranchIdentifier;
                        bankPm.SwiftCode = bankDetail.SwiftCode;
                        bankPm.IBAN = bankDetail.IBAN;
                        bankPm.BankName = bankDetail.BankName;
                        bankPm.Address = bankDetail.Address;
                        bankPm.RefCountryId = bankDetail.RefCountryId;
                        bankPm.PreferredMode = bankDetail.PreferredMode;
                        bankPm.RefLastUpdatedBy = sellerUserPartyId;
                        sellerUow.BankAccounts.Update(bankPm);
                        sellerUow.SaveChanges();
                    }
                }
                else
                {
                    var bankPm = bankDetail.ToDBModel(organisationId);
                    bankPm.RefLegalEntity = legalEntityId;
                    bankPm.RefCreatedBy =  sellerUserPartyId;
                    bankPm.RefLastUpdatedBy = sellerUserPartyId;
                    sellerUow.BankAccounts.Add(bankPm);
                    sellerUow.SaveChanges();
                }
                sellerUow.Commit();
                result = true;
                Logger.Info("PartyServiceFacade: AddOrUpdateBankDetails() : Exit the method");
                return result;
            }
            catch (Exception ex)
            {

                if (sellerUow != null)
                    sellerUow.Rollback();
                Logger.Error("PartyServiceFacade : AddOrUpdateBankDetails() : Caught an exception" + ex);

                throw;
            }
        }
        public bool DeleteBankAccountById(long bankId)
        {
            ISupplierUow sellerUow = null;

            try
            {
                Logger.Info("PartyServiceFacade: DeleteBankAccountById() : Enter the method");
                var result = false;
                sellerUow = new SupplierUow();
                var bankAccount = sellerUow.BankAccounts.All().FirstOrDefault(v => v.Id == bankId);
                if (bankAccount != null)
                {
                    sellerUow.BeginTransaction();

                    //ToDo:We have to delte mapped buyer supplier Banks
                    var legalEntites = sellerUow.LegalEntityProfiles.All().Where(v => v.RefBank == bankId && v.ProfileType == Constants.PROFILE_TYPE_BANK).ToList();
                    foreach (var item in legalEntites)
                    {
                        sellerUow.LegalEntityProfiles.Delete(item);
                        sellerUow.SaveChanges();
                    }
                    sellerUow.BankAccounts.Delete(bankAccount);
                    sellerUow.SaveChanges();
                    sellerUow.Commit();
                    result = true;
                }
                Logger.Info("PartyServiceFacade: DeleteBankAccountById() : Exit the method");
                return result;
            }
            catch (Exception ex)
            {
                if (sellerUow != null)
                    sellerUow.Rollback();
                Logger.Error("PartyServiceFacade : DeleteBankAccountById() : Caught an exception" + ex);

                throw;
            }
        }
        public ProfileSummary GetSellerProfilePercentage(long sellerPartyId, long sellerId, long organisationId)
        {
            try
            {
                Logger.Info("PartyServiceFacade: GetSellerProfilePercentage() : Enter the method");
                var sellerData = new ProfileSummary();
                sellerData = _sellerUow.Suppliers.GetSellerProfilePercentage(sellerPartyId, sellerId, organisationId);
                Logger.Info("PartyServiceFacade: GetSellerProfilePercentage() : Exit the method");

                return sellerData;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : GetSellerProfilePercentage() : Caught an exception" + ex);

                throw;
            }

        }

        public List<BuyerSupplierReferenceList> BuyerSupplierReferenceList(int pageNo, string sortParameter, int sortDirection, string buyerName, long sellerId, long referenceId, out int totalRecords)
        {
            try
            {
                Logger.Info("PartyServiceFacade : CompanyService : BuyerSupplierReferenceList() : Enter the method.");
                var buyerlist = new List<BuyerSupplierReferenceList>();
                buyerlist = _sellerUow.Invitees.BuyerSupplierReferenceList(pageNo, sortParameter, sortDirection, buyerName, sellerId, referenceId, out totalRecords);
                Logger.Info("PartyServiceFacade : CompanyService : BuyerSupplierReferenceList() : Exiting the method.");
                return buyerlist;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : CompanyService : BuyerSupplierReferenceList() : Caught an exception" + ex);
                throw;
            }
        }

        public bool AddOrRemoveBuyerSupplierReferenceDetails(bool isAdd, long buyerId, long referenceId)
        {
            try
            {
                Logger.Info("PartyServiceFacade: AddOrRemoveBuyerSupplierReferenceDetails() : Enter the method.");
                var result = false;
                if (isAdd)
                {
                    var buyerSupplierReference = new BuyerSupplierReference();
                    buyerSupplierReference.RefInvitee = referenceId;
                    buyerSupplierReference.RefReferredTo = buyerId;
                    _sellerUow.BuyerSupplierReferences.Add(buyerSupplierReference);
                    _sellerUow.SaveChanges();
                    result = true;
                }
                else
                {
                    var existing = _sellerUow.BuyerSupplierReferences.All().FirstOrDefault(u => u.RefInvitee == referenceId && u.RefReferredTo == buyerId);
                    if (existing != null)
                    {
                        _sellerUow.BuyerSupplierReferences.Delete(existing);
                        _sellerUow.SaveChanges();
                        result = true;
                    }
                }
                Logger.Info("PartyServiceFacade : AddOrRemoveBuyerSupplierReferenceDetails() : Exiting the method.");
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : AddOrRemoveBuyerSupplierReferenceDetails() : Caught an exception" + ex);
                throw;
            }
        }

        public List<BuyerSupplierAddressList> BuyerSupplierAddressList(int pageNo, string sortParameter, int sortDirection, string buyerName, long sellerPartyId, long addressId, out int totalRecords)
        {
            try
            {
                Logger.Info("PartyServiceFacade : CompanyService : BuyerSupplierReferenceList() : Enter the method.");
                var buyerlist = new List<BuyerSupplierAddressList>();
                buyerlist = _sellerUow.Addresses.BuyerSupplierAddressList(pageNo, sortParameter, sortDirection, buyerName, sellerPartyId, addressId, out totalRecords);
                Logger.Info("PartyServiceFacade : CompanyService : BuyerSupplierReferenceList() : Exiting the method.");
                return buyerlist;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : CompanyService : BuyerSupplierReferenceList() : Caught an exception" + ex);
                throw;
            }
        }
        public bool AddOrRemoveBuyerSupplierAddressDetails(bool isAdd, long buyerPartyId, long refContactMethodId)
        {
            try
            {
                Logger.Info("PartyServiceFacade: AddOrRemoveBuyerSupplierAddressDetails() : Enter the method.");
                var result = false;
                if (isAdd)
                {
                    var entityProfile = new LegalEntityProfile();
                    entityProfile.RefPartyId = buyerPartyId;
                    entityProfile.RefContactMethod = refContactMethodId;
                    entityProfile.ProfileType = Constants.PROFILE_TYPE_ADDRESS;
                    _sellerUow.LegalEntityProfiles.Add(entityProfile);
                    _sellerUow.SaveChanges();
                    result = true;
                }
                else
                {
                    var existingEntityProfile = _sellerUow.LegalEntityProfiles.All().FirstOrDefault(u => u.RefContactMethod == refContactMethodId && u.RefPartyId == buyerPartyId && u.ProfileType.Trim() == Constants.PROFILE_TYPE_ADDRESS);
                    if (existingEntityProfile != null)
                    {
                        _sellerUow.LegalEntityProfiles.Delete(existingEntityProfile);
                        _sellerUow.SaveChanges();
                        result = true;
                    }
                }
                Logger.Info("PartyServiceFacade : AddOrRemoveBuyerSupplierAddressDetails() : Exiting the method.");
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : AddOrRemoveBuyerSupplierAddressDetails() : Caught an exception" + ex);
                throw;
            }
        }

        public int BuyersAssignedToAddress(long addressId)
        {
            try
            {
                Logger.Info("PartyServiceFacade: BuyersAssignedToAddress() : Enter the method.");
                int count = 0;
                count = _sellerUow.LegalEntityProfiles.All().Count(v => v.ContactMethod.Addresses.Any(a => a.Id == addressId) && v.ProfileType == Constants.PROFILE_TYPE_ADDRESS);
                Logger.Info("PartyServiceFacade : BuyersAssignedToAddress() : Exiting the method.");
                return count;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : BuyersAssignedToAddress() : Caught an exception" + ex);
                throw;
            }
         }
        public List<BuyerSupplierBankAccount> BuyerSupplierBankList(int pageNo, string sortParameter, int sortDirection, string buyerName, long organisationId, long bankId, out int totalRecords)
        {
            try
            {
                Logger.Info("PartyServiceFacade BuyerSupplierBankList() : Enter the method.");
                var buyerlist = new List<BuyerSupplierBankAccount>();
                buyerlist = _sellerUow.BankAccounts.BuyerSupplierBankList(pageNo, sortParameter, sortDirection, buyerName, organisationId, bankId, out totalRecords);
                Logger.Info("PartyServiceFacade : BuyerSupplierBankList() : Exiting the method.");
                return buyerlist;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : BuyerSupplierBankList() : Caught an exception" + ex);
                throw;
            }
        }
        public bool AddOrRemoveBuyerSupplierBankDetails(bool isAdd, long buyerPartyId, long bankId)
        {
            try
            {
                Logger.Info("PartyServiceFacade: AddOrRemoveBuyerSupplierBankDetails() : Enter the method.");
                var result = false;
                if (isAdd)
                {
                    var entityProfile = new LegalEntityProfile();
                    entityProfile.RefPartyId = buyerPartyId;
                    entityProfile.RefBank = bankId;
                    entityProfile.ProfileType = Constants.PROFILE_TYPE_BANK;
                    _sellerUow.LegalEntityProfiles.Add(entityProfile);
                    _sellerUow.SaveChanges();
                    result = true;
                }
                else
                {
                    var existingEntityProfile = _sellerUow.LegalEntityProfiles.All().FirstOrDefault(u => u.RefBank == bankId && u.RefPartyId == buyerPartyId && u.ProfileType.Trim() == Constants.PROFILE_TYPE_BANK);
                    if (existingEntityProfile != null)
                    {
                        _sellerUow.LegalEntityProfiles.Delete(existingEntityProfile);
                        _sellerUow.SaveChanges();
                        result = true;
                    }
                }
                Logger.Info("PartyServiceFacade : AddOrRemoveBuyerSupplierBankDetails() : Exiting the method.");
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : AddOrRemoveBuyerSupplierBankDetails() : Caught an exception" + ex);
                throw;
            }
        }

        public List<BuyerSupplierContacts> BuyerSupplierContactsList(int pageNo, string sortParameter, int sortDirection, string buyerName, long sellerId, long contactPartyId, out int totalRecords)
        {
            try
            {
                Logger.Info("PartyServiceFacade : BuyerSupplierContactsList() : Enter the method.");
                var buyerlist = new List<BuyerSupplierContacts>();
                buyerlist = _sellerUow.ContactPersons.BuyerSupplierContactList(pageNo, sortParameter, sortDirection, buyerName, sellerId, contactPartyId, out totalRecords);
                Logger.Info("PartyServiceFacade: BuyerSupplierContactsList() : Exiting the method.");
                return buyerlist;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : BuyerSupplierContactsList() : Caught an exception" + ex);
                throw;
            }
        }
        public bool AddOrUpdateBuyerContacts(bool isAssigned, long buyerPartyId, long contactPartyId,int role,long sellerPartyId , long sellerUserPartyId)
        {
            try
            {
                Logger.Info("PartyServiceFacade: AddOrUpdateBuyerContacts() : Enter the method.");
                var result = false;
                if (!isAssigned)
                {
                    var partyPartyLink = new PartyPartyLink();
                    partyPartyLink.RefParty = contactPartyId;
                    partyPartyLink.RefLinkedParty = buyerPartyId;
                    partyPartyLink.PartyPartyLinkSubType = role.ToString();
                    partyPartyLink.PartyPartyLinkType = Constants.CONTACT_BUYER;
                    partyPartyLink.RefCreatedBy = sellerUserPartyId;
                    partyPartyLink.RefLastUpdatedBy = sellerUserPartyId;
                    _sellerUow.PartyPartyLinks.Add(partyPartyLink);
                    _sellerUow.SaveChanges();
                    result = true;
                }
                else
                {
                    var existing = _sellerUow.PartyPartyLinks.All().FirstOrDefault(u => u.RefParty == contactPartyId && u.RefLinkedParty == buyerPartyId && u.PartyPartyLinkType.Trim() == Constants.CONTACT_BUYER);
                    if (existing != null)
                    {
                        if(role > 0)
                        {
                            existing.PartyPartyLinkSubType = role.ToString();
                            existing.RefLastUpdatedBy = sellerUserPartyId;
                            _sellerUow.PartyPartyLinks.Update(existing);
                            _sellerUow.SaveChanges();
                        }
                        else
                        {
                            _sellerUow.PartyPartyLinks.Delete(existing);
                            _sellerUow.SaveChanges();
                        }
                      
                        result = true;
                    }
                }
                Logger.Info("PartyServiceFacade : AddOrUpdateBuyerContacts() : Exiting the method.");
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : AddOrUpdateBuyerContacts() : Caught an exception" + ex);
                throw;
            }
        }


        #endregion



        public Profile SellerProfileDetails(long sellerPartyId, long buyerPartyId, long buyerUserPartyId)
        {
            try
            {
                Logger.Info("PartyServiceFacade: SellerProfileDetails() : Enter the method.");
                var companyDetails = new Profile();
                companyDetails = _sellerUow.Parties.SellerProfileDetails(sellerPartyId, buyerPartyId, buyerUserPartyId);
                Logger.Info("PartyServiceFacade : SellerProfileDetails() : Exiting the method.");
                return companyDetails;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : SellerProfileDetails() : Caught an exception" + ex);
                throw;
            }
        }

        public bool AddOrRemoveFavouriteSupplier(long buyerUserPartyId, long supplierPartyId, bool isAdd)
        {

            try
            {
                Logger.Info("PartyServiceFacade: AddOrRemoveFavouriteSupplier() : Enter the method.");
                var result = false;
                if(isAdd)
                {
                    var partyPartyLink = new PartyPartyLink();
                    partyPartyLink.RefParty = buyerUserPartyId;
                    partyPartyLink.RefLinkedParty = supplierPartyId;
                    partyPartyLink.PartyPartyLinkType = Constants.BUYER_FAVOURITE_SUPPLIER;
                    partyPartyLink.RefCreatedBy = buyerUserPartyId;
                    partyPartyLink.RefLastUpdatedBy = buyerUserPartyId;
                    _sellerUow.PartyPartyLinks.Add(partyPartyLink);
                    _sellerUow.SaveChanges();
                    result = true;
                }
                else
                {
                    var existing = _sellerUow.PartyPartyLinks.All().FirstOrDefault(v => v.RefParty == buyerUserPartyId && v.RefLinkedParty == supplierPartyId && v.PartyPartyLinkType.Trim() == Constants.BUYER_FAVOURITE_SUPPLIER);
                    if(existing != null)
                    {
                        _sellerUow.PartyPartyLinks.Delete(existing);
                        _sellerUow.SaveChanges();
                        result = true;
                    }
                }
                Logger.Info("PartyServiceFacade : AddOrRemoveFavouriteSupplier() : Exiting the method.");
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : AddOrRemoveFavouriteSupplier() : Caught an exception" + ex);
                throw;
            }
        }


        public bool AddOrRemoveTradingSupplier(long buyerUserPartyId, long buyerSellerPartyId, long supplierPartyId, bool isAdd)
        {

            try
            {
                Logger.Info("PartyServiceFacade: AddOrRemoveTradingSupplier() : Enter the method.");
                var result = false;
                if (isAdd)
                {
                    var partyPartyLink = new PartyPartyLink();
                    partyPartyLink.RefParty = buyerSellerPartyId;
                    partyPartyLink.RefLinkedParty = supplierPartyId;
                    partyPartyLink.PartyPartyLinkType = Constants.BUYER_TRADING_SUPPLIER;
                    partyPartyLink.RefCreatedBy = buyerUserPartyId;
                    partyPartyLink.RefLastUpdatedBy = buyerUserPartyId;
                    _sellerUow.PartyPartyLinks.Add(partyPartyLink);
                    _sellerUow.SaveChanges();
                    result = true;
                }
                else
                {
                    var existing = _sellerUow.PartyPartyLinks.All().FirstOrDefault(v => v.RefParty == buyerSellerPartyId && v.RefLinkedParty == supplierPartyId && v.PartyPartyLinkType.Trim() == Constants.BUYER_TRADING_SUPPLIER);
                    if (existing != null)
                    {
                        _sellerUow.PartyPartyLinks.Delete(existing);
                        _sellerUow.SaveChanges();
                        result = true;
                    }
                }
                Logger.Info("PartyServiceFacade : AddOrRemoveTradingSupplier() : Exiting the method.");
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : AddOrRemoveFavouriteSupplier() : Caught an exception" + ex);
                throw;
            }
        }

        public long GetCompanyPartyIdBasedOnCompanyName(string companyName)
        {
            try
            {
                long partyId = 0;
                Logger.Info("PartyServiceFacade: GetCompanyPartyIdBasedOnCompanyName() : Enter the method.");
                partyId = _sellerUow.Parties.All().FirstOrDefault(v => v.PartyName.ToLower().Trim() == companyName.ToLower().Trim()) != null ?
                    _sellerUow.Parties.All().FirstOrDefault(v => v.PartyName.ToLower().Trim() == companyName.ToLower().Trim()).Id : 0;
                Logger.Info("PartyServiceFacade : GetCompanyPartyIdBasedOnCompanyName() : Exiting the method.");
                return partyId;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : GetCompanyPartyIdBasedOnCompanyName() : Caught an exception" + ex);
                throw;
            }
        }

        public List<SupplierOrganization> GetSupplierOrganization(string fromdate, string toDate, out int total, int pageIndex, short source, int size, int sortDirection, long supplierId = 0, string supplierName = "", long status = (Int64)CompanyStatus.Started, string referrerName = "")
        {
            try
            {
                Logger.Info("PartyServiceFacade : GetSupplierOrganization() : Enter the method");
                var supplierOrganization = new List<SupplierOrganization>();

                supplierOrganization = _sellerUow.Suppliers.GetSupplierOrganization(fromdate, toDate, out total, pageIndex, source, size, sortDirection, supplierId,  supplierName, status, referrerName).ToList();
                total = supplierOrganization.Count;

                Logger.Info("PartyServiceFacade : GetSupplierOrganization() : Exit the method");
                return supplierOrganization;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : GetSupplierOrganization() : Caught an exception" + ex);
                throw;
            }
        }

        public SupplierOrganization GetSupplierDetailsForDashboard(long supplierPartyId)
        {
            try
            {
                Logger.Info("PartyServiceFacade : GetSupplierDetailsForDashboard() : Enter the method");
                var supplierOrganization = new SupplierOrganization();

                supplierOrganization = _sellerUow.Suppliers.GetSupplierDetailsForDashboard(supplierPartyId);

                Logger.Info("PartyServiceFacade : GetSupplierDetailsForDashboard() : Exit the method");
                return supplierOrganization;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : GetSupplierDetailsForDashboard() : Caught an exception" + ex);
                throw;
            }
        }

        public List<string> GetNotVerifiedSupplierNames(string supplierOrg)
        {
            try
            {
                Logger.Info("PartyServiceFacade : GetNotVerifiedSupplierNames() : Enter the method");
                List<string> supplierOrgList = null;

                supplierOrgList = _sellerUow.Suppliers.GetNotVerifiedSupplierNames(supplierOrg);

                Logger.Info("PartyServiceFacade : GetNotVerifiedSupplierNames() : Exit the method");
                return supplierOrgList;
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : GetNotVerifiedSupplierNames() : Caught an exception" + ex);
                throw;
            }
        }

        public List<string> GetSuppliersListForRegistration(string companyName)
        {
            try
            {
                Logger.Info("SupplierRepository : GetSuppliersListForRegistration() : Enter into method");
                Logger.Info("SupplierRepository : GetSuppliersListForRegistration() : Exit from method");

                return _sellerUow.Suppliers.GetSuppliersListForRegistration(companyName);
            }
            catch (Exception ex)
            {
                Logger.Error("SupplierRepository : GetSuppliersListForRegistration() : Caught an exception " + ex);
                throw ex;
            }
        }

        public bool CheckwhetherSupplierNameExists(string companyName, out bool IsNotRegistered, out long campaignPublicDataId)
        {
            var IsAlreadyRegistered = false;
            IsNotRegistered = false;
            try
            {
                Logger.Info("PartyServiceFacade : CheckwhetherSupplierNameExists() : Enter into method");

                var company = _sellerUow.Parties.All().FirstOrDefault(u => u.PartyName.ToLower().Trim() == companyName.ToLower().Trim());
                campaignPublicDataId = 0;
                if (company != null)
                {
                    IsAlreadyRegistered = true;
                }
                if (!IsAlreadyRegistered)
                {
                    var preregister = _sellerUow.CampaignInvitations.All().FirstOrDefault(u => u.SupplierCompanyName.ToLower().Trim() == companyName.ToLower().Trim());
                    if (preregister != null)
                    {
                        IsNotRegistered = true;
                        campaignPublicDataId = preregister.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : CheckwhetherSupplierNameExists() : Caught an exception " + ex);
                throw ex;
            }
            Logger.Info("PartyServiceFacade : CheckwhetherSupplierNameExists() : Exit from method");
            return IsAlreadyRegistered;
        }

        public List<SupplierOrganization> GetSuppliersForVerification(int pageNo, string sortParameter, int sortDirection, out int total, int sourceCheck, int viewOptions, int pageSize, string referrerName = "")
        {
            try
            {
                Logger.Info("PartyServiceFacade : GetSuppliersForVerification() : Enter into method");
                Logger.Info("PartyServiceFacade : GetSuppliersForVerification() : Exit from method");

                return _sellerUow.Suppliers.GetSuppliersForVerification(pageNo, sortParameter, sortDirection, out total, sourceCheck, viewOptions, pageSize, referrerName);
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : GetSuppliersForVerification() : Caught an exception " + ex);
                throw ex;
            }
        }

        public List<SupplierCountBasedOnStage> GetSuppliersCountBasedOnStage(int sourceCheck, int viewOptions, string referrerName)
        {
            var resultModel = new List<SupplierCountBasedOnStage>();
            try
            {
                Logger.Info("PartyServiceFacade : GetSuppliersCountBasedOnStage() : Enter into method");
                resultModel = _sellerUow.Suppliers.GetSuppliersCountBasedOnStage(sourceCheck, viewOptions, referrerName);
                Logger.Info("PartyServiceFacade : GetSuppliersCountBasedOnStage() : Exit from method");
            }
            catch (Exception ex)
            {
                Logger.Error("PartyServiceFacade : GetSuppliersCountBasedOnStage() : Caught an exception " + ex);
                throw ex;
            }
            return resultModel;
        }
    }
}
