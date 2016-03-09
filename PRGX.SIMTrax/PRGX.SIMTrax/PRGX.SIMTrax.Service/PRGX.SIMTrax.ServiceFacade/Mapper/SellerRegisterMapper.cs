using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Entity.Registration;
using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.Domain.Util;
using PRGX.SIMTrax.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PRGX.SIMTrax.ServiceFacade.Mapper
{
    public static class SellerRegisterMapper
    {
        public static Party ToPartyModelUser(this SellerRegister registerModel)
        {
            var credentials = new List<Credential>();
            var acceptedTermsOfUseList = new List<AcceptedTermsOfUse>();
            var userList = new DAL.Entity.User();
            var peopleList = new Person();

            credentials.Add(new Credential()
            {
                IsLocked = false,
                LoginId = registerModel.Email.Trim(),
                Password = registerModel.Password.Trim(),
                LastLoginDate = DateTime.UtcNow,
                RefCreatedBy = 1
            });
            acceptedTermsOfUseList.Add(new AcceptedTermsOfUse()
            {
                AcceptedDate = DateTime.UtcNow,
                RefTermsOfUse = registerModel.TermsOfUseId
            });
            userList = new DAL.Entity.User()
            {
                UserType = (long)UserType.Supplier,
                NeedsPasswordChange = false,
                AcceptedTermsOfUses = acceptedTermsOfUseList,
                Credentials = credentials,
                RefCreatedBy = 1
            };
            peopleList = new Person()
            {
                FirstName = registerModel.FirstName.Trim(),
                LastName = registerModel.LastName.Trim(),
                JobTitle = !string.IsNullOrWhiteSpace(registerModel.JobTitle) ? registerModel.JobTitle.Trim() : string.Empty,
                PersonType = Constants.PERSON_TYPE_USER,
                User = userList,
                RefCreatedBy = 1
            };
            var party = new Party()
            {
                PartyName = string.Concat(registerModel.FirstName.Trim(), " ", registerModel.LastName.Trim()),
                PartyType = Constants.PARTY_TYPE_PERSON,
                IsActive = true,
                Person = peopleList,
                ProjectSource = (long)ProjectSource.SIM
            };
            return party;
        }

        public static Party ToPartyModelSeller(this SellerRegister registerModel)
        {
            var sellers = new Supplier();
            var organizations = new Organization();
            var legalEntity = new LegalEntity()
            {
                IsActive = true,
                PartyName = registerModel.DummyOrganisationName.Trim(),
                RefCreatedBy = 1
            };
            sellers = new Supplier()
            {
                RefCreatedBy = 1,
            };
            organizations = new Organization()
            {
                Supplier = sellers,
                LegalEntity = legalEntity,
                RefCreatedBy = 1,
                OrganizationType = Constants.ORGANIZATION_TYPE_SELLER,
                Status = (short)registerModel.Status
            };
            return new Party
            {
                PartyName = registerModel.DummyOrganisationName.Trim(),
                PartyType = Constants.PARTY_TYPE_ORGANIZATION,
                IsActive = true,
                RefCreatedBy = 1,
                Organization = organizations,
                ProjectSource = (long)ProjectSource.SIM
            };
        }

        public static Supplier ToSellerModel(this SellerRegister registerModel)
        {
            return new Supplier()
            {
                BusinessDescription = registerModel.BusinessDescription,
                EstablishedYear = registerModel.CompanyYear,
                FacebookAccount = registerModel.OrganisationFacebookaccount,
                IsSubsidary = registerModel.IsSubsidaryStatus,
                LinkedInAccount = registerModel.OrganisationLinkedInaccount,
                MaxContractValue = registerModel.MaxContractValue,
                MinContractValue = registerModel.MinContractValue,
                TradingName = registerModel.Trading,
                TwitterAccount = registerModel.OrganisationTwitteraccount,
                TypeOfSeller = registerModel.TypeOfCompany,
                UltimateParent = registerModel.UltimateParent,
                WebsiteLink = registerModel.WebsiteLink,
                RefCreatedBy = registerModel.UserPartyId,
                RefLastUpdatedBy = registerModel.UserPartyId
            };
        }

        public static Organization ToOrganizationModel(this SellerRegister registerModel, long orgId)
        {
            var orgIndustryCodes = new List<IndustryCodeOrganizationLink>();

            if (!string.IsNullOrWhiteSpace(registerModel.SIC1))
            {
                var industryCode = new IndustryCodeOrganizationLink();
                industryCode.RefIndustryCode = Convert.ToInt64(registerModel.SIC1);
                industryCode.RefOrganization = orgId;
                orgIndustryCodes.Add(industryCode);
            }
            if (!string.IsNullOrWhiteSpace(registerModel.SIC2))
            {
                var industryCode = new IndustryCodeOrganizationLink();
                industryCode.RefIndustryCode = Convert.ToInt64(registerModel.SIC2);
                industryCode.RefOrganization = orgId;
                orgIndustryCodes.Add(industryCode);
            }
            if (!string.IsNullOrWhiteSpace(registerModel.SIC3))
            {
                var industryCode = new IndustryCodeOrganizationLink();
                industryCode.RefIndustryCode = Convert.ToInt64(registerModel.SIC3);
                industryCode.RefOrganization = orgId;
                orgIndustryCodes.Add(industryCode);
            }
            if (!string.IsNullOrWhiteSpace(registerModel.SIC4))
            {
                var industryCode = new IndustryCodeOrganizationLink();
                industryCode.RefIndustryCode = Convert.ToInt64(registerModel.SIC4);
                industryCode.RefOrganization = orgId;
                orgIndustryCodes.Add(industryCode);
            }
            if (!string.IsNullOrWhiteSpace(registerModel.SIC5))
            {
                var industryCode = new IndustryCodeOrganizationLink();
                industryCode.RefIndustryCode = Convert.ToInt64(registerModel.SIC5);
                industryCode.RefOrganization = orgId;
                orgIndustryCodes.Add(industryCode);
            }
            var organization = new Organization()
            {
                BusinessSectorDescription = registerModel.BusinessSectorDescription,
                BusinessSectorId = registerModel.sector,
                EmployeeSize = registerModel.NumberOfEmployees,
                IndustryCodeOrganizationLinks = orgIndustryCodes,
                Status = (short)registerModel.Status,
                TurnOverSize = registerModel.TurnOver,
                RefCreatedBy = registerModel.UserPartyId,
                RefLastUpdatedBy = registerModel.UserPartyId
            };
            return organization;
        }

        public static List<PartyIdentifier> ToPartyIdentifierModel(this SellerRegister registerModel)
        {
            var listPartyIdentifers = new List<PartyIdentifier>();
            foreach (var partyIdentifierTypes in registerModel.IdentifierTypeList)
            {
                PartyIdentifier partyIdentifier = null;
                switch (partyIdentifierTypes.Text)
                {

                    case Constants.IDENTIFIER_TYPE_VAT_NUMBER:
                        if (registerModel.IsVAT && !string.IsNullOrWhiteSpace(registerModel.VATNumber))
                            partyIdentifier = new PartyIdentifier { IdentifierNumber = registerModel.VATNumber };
                        break;
                    case Constants.IDENTIFIER_TYPE_DUNS_NUMBER:
                        if (registerModel.HaveDuns && !string.IsNullOrWhiteSpace(registerModel.DUNSNumber))
                            partyIdentifier = new PartyIdentifier { IdentifierNumber = registerModel.DUNSNumber };
                        break;
                    case Constants.IDENTIFIER_TYPE_REGISTRATION_NUMBER:
                        if (!string.IsNullOrWhiteSpace(registerModel.CompanyRegistrationNumber) && !string.IsNullOrWhiteSpace(registerModel.CompanyRegistrationNumber))
                            partyIdentifier = new PartyIdentifier { IdentifierNumber = registerModel.CompanyRegistrationNumber };
                        break;
                    case Constants.IDENTIFIER_TYPE_PARENT_DUNS_NUMBER:
                        if (registerModel.IsSubsidaryStatus && !string.IsNullOrWhiteSpace(registerModel.ParentDunsNumber))
                            partyIdentifier = new PartyIdentifier { IdentifierNumber = registerModel.ParentDunsNumber };
                        break;
                }

                if (null != partyIdentifier)
                {
                    partyIdentifier.RefPartyIdentifierType = partyIdentifierTypes.Value;
                    partyIdentifier.RefParty = registerModel.SellerPartyId;
                    partyIdentifier.RefLastUpdatedBy = registerModel.UserPartyId;
                    partyIdentifier.RefRegion = registerModel.RefRegionId;

                    listPartyIdentifers.Add(partyIdentifier);
                }
            }
            return listPartyIdentifers;
        }

        public static List<PartyRegionLink> ToPartyRegionLinkModel(this SellerRegister registerModel)
        {
            var partyRegionLinks = new List<PartyRegionLink>();
            foreach (var partyRegion in registerModel.GeoGraphicSalesList.Where(x => x.Selected == true))
            {
                partyRegionLinks.Add(new PartyRegionLink
                {
                    RefParty = registerModel.SellerPartyId,
                    RefRegion = Convert.ToInt64(partyRegion.Value),
                    LinkType = Constants.PARTY_REGION_SALES_REGION
                });
            }
            foreach (var partyRegion in registerModel.GeoGraphicSuppList.Where(x => x.Selected == true))
            {
                partyRegionLinks.Add(new PartyRegionLink
                {
                    RefParty = registerModel.SellerPartyId,
                    RefRegion = Convert.ToInt64(partyRegion.Value),
                    LinkType = Constants.PARTY_REGION_SERVICE_REGION
                });
            }
            return partyRegionLinks;
        }

        public static List<DAL.Entity.Address> ToAddressModel(this SellerRegister registerModel)
        {
            var addressList = new List<DAL.Entity.Address>();

            if (!string.IsNullOrWhiteSpace(registerModel.FirstAddressLine1) && (!string.IsNullOrWhiteSpace(registerModel.FirstAddressCity))
                && registerModel.FirstAddressCountry != null && Convert.ToInt64(registerModel.FirstAddressCountry) > 0 
                && (!string.IsNullOrWhiteSpace (registerModel.FirstAddressPostalCode)))
            {
                addressList.Add(new DAL.Entity.Address()
                {
                    Id = registerModel.PrimaryAddressId,
                    Line1 = registerModel.FirstAddressLine1,
                    Line2 = registerModel.FirstAddressLine2,
                    City = registerModel.FirstAddressCity,
                    RefCountryId = Convert.ToInt64(registerModel.FirstAddressCountry),
                    State = registerModel.FirstAddressState,
                    ZipCode = registerModel.FirstAddressPostalCode,
                    RefLastUpdatedBy = registerModel.UserPartyId,
                    RefCreatedBy = registerModel.UserPartyId,
                    AddressType = (short)AddressType.Primary
                });
            }

            if (registerModel.IsAddressDifferent)
            {
                addressList.Add(new DAL.Entity.Address
                {
                    Id = registerModel.RegisteredAddressId,
                    Line1 = registerModel.SecondAddressLine1,
                    Line2 = registerModel.SecondAddressLine2,
                    City = registerModel.SecondAddressCity,
                    RefCountryId = Convert.ToInt64(registerModel.SecondAddressCountry),
                    State = registerModel.SecondAddressState,
                    ZipCode = registerModel.SecondAddressPostalCode,
                    RefLastUpdatedBy = registerModel.UserPartyId,
                    RefCreatedBy = registerModel.UserPartyId,
                    AddressType = (short)AddressType.Registered
                });
            }

            if (registerModel.IsHeadQuartersAddressDifferent)
            {
                addressList.Add(new DAL.Entity.Address
                {
                    Id = registerModel.HeadQuartersAddressId,
                    Line1 = registerModel.HeadQuartersAddressLine1,
                    Line2 = registerModel.HeadQuartersAddressLine2,
                    City = registerModel.HeadQuartersAddressCity,
                    RefCountryId = Convert.ToInt64(registerModel.HeadQuartersAddressCountry),
                    State = registerModel.HeadQuartersAddressState,
                    ZipCode = registerModel.HeadQuartersAddressPostalCode,
                    RefLastUpdatedBy = registerModel.UserPartyId,
                    RefCreatedBy = registerModel.UserPartyId,
                    AddressType = (short)AddressType.HeadQuarters
                });
            }
            if (registerModel.IsRemittanceAddressDifferent)
            {
                addressList.Add(new DAL.Entity.Address
                {
                    Id = registerModel.RemittanceAddressId,
                    Line1 = registerModel.RemittanceAddressLine1,
                    Line2 = registerModel.RemittanceAddressLine2,
                    City = registerModel.RemittanceAddressCity,
                    RefCountryId = Convert.ToInt64(registerModel.RemittanceAddressCountry),
                    State = registerModel.RemittanceAddressState,
                    ZipCode = registerModel.RemittanceAddressPostalCode,
                    RefCreatedBy = registerModel.UserPartyId,
                    RefLastUpdatedBy = registerModel.UserPartyId,
                    AddressType = (short)AddressType.Remittance
                });
            }
            return addressList;
        }



        public static List<ViewModel.Address> ToViewModel(this List<DAL.Entity.Address> addressListPM)
        {
            List<ViewModel.Address> addressList = new List<ViewModel.Address>();
            addressList = addressListPM.Select(v => new ViewModel.Address()
            {
                Id = v.Id,
                Line1 = v.Line1,
                Line2 = v.Line2,
                Line3 = v.Line3,
                City = v.City,
                RefCountryId = v.RefCountryId,
                State = v.State,
                ZipCode = v.ZipCode,
                AddressType = v.AddressType,
                CountryName = (v.Region != null) ? v.Region.Name : string.Empty,
                AddressTypeValue = CommonMethods.Description((AddressType)(v.AddressType)),
                RefContactMethod = v.RefContactMethod
            }).ToList();

            return addressList;
        }
        public static DAL.Entity.Address ToDBModel(this ViewModel.Address address)
        {
            DAL.Entity.Address addressPM = new DAL.Entity.Address();
            addressPM.Id = address.Id;
            addressPM.Line1 = address.Line1;
            addressPM.Line2 = address.Line2;
            addressPM.City = address.City;
            addressPM.State = address.State;
            addressPM.ZipCode = address.ZipCode;
            addressPM.RefCountryId = address.RefCountryId;
            addressPM.AddressType = address.AddressType;
            return addressPM;
        }

        public static List<Party> ToContactPersonPartyModel(this SellerRegister registerModel)
        {
            var contactPersonPartyList = new List<Party>();

            foreach (var contact in registerModel.ContactDetails.Where(x => !string.IsNullOrWhiteSpace(x.Email)))
            {
               
                var contactPerson = new DAL.Entity.ContactPerson();
                contactPerson = new DAL.Entity.ContactPerson
                {
                    ContactType = (short)contact.ContactType,
                    JobTitle = contact.JobTitle,
                    RefCreatedBy = registerModel.UserPartyId,
                    RefLastUpdatedBy = registerModel.UserPartyId
                } ;
                var person = new Person();
                person = new Person
                {
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    JobTitle = contact.JobTitle,
                    RefCreatedBy = registerModel.UserPartyId,
                    RefLastUpdatedBy = registerModel.UserPartyId,
                    PersonType = Constants.PERSON_TYPE_CONTACT_PERSON,
                    ContactPerson = contactPerson
                };
                var party = new Party()
                {
                    PartyName = string.Concat(contact.FirstName.Trim(), " ", contact.LastName.Trim()),
                    PartyType = Constants.PARTY_TYPE_PERSON,
                    IsActive = true,
                    Person = person,
                    RefCreatedBy = registerModel.UserPartyId,
                    RefLastUpdatedBy = registerModel.UserPartyId
                };
                contactPersonPartyList.Add(party);
            }
            return contactPersonPartyList;
        }

        public static SellerRegister MappingToCompanyDetails(this Party partyModel)
        {
            var sellerRegister = new SellerRegister();
            if (partyModel != null)
            {
                sellerRegister.SellerPartyId = partyModel.Id;
                sellerRegister.OrganisationName = partyModel.PartyName;
                sellerRegister.IsVAT = false;
                sellerRegister.HaveDuns = false;
                var seller = (partyModel.Organization != null) ?
                    partyModel.Organization.Supplier : null;
                if (seller != null)
                {
                    sellerRegister.Trading = seller.TradingName;
                    sellerRegister.TypeOfCompany = Convert.ToInt16(seller.TypeOfSeller);
                    sellerRegister.IsSubsidaryStatus = Convert.ToBoolean(seller.IsSubsidary);
                    sellerRegister.UltimateParent = seller.UltimateParent;
                    sellerRegister.CompanyYear = seller.EstablishedYear;
                }
                var partyIdentifiers = partyModel.PartyIdentifiers.ToList();
                if (partyIdentifiers.Count > 0)
                {
                    sellerRegister.CompanyRegistrationNumber = partyIdentifiers.FirstOrDefault(v => v.PartyIdentifierType.IdentifierType == Constants.IDENTIFIER_TYPE_REGISTRATION_NUMBER) != null ?
                        partyIdentifiers.FirstOrDefault(v => v.PartyIdentifierType.IdentifierType == Constants.IDENTIFIER_TYPE_REGISTRATION_NUMBER).IdentifierNumber : string.Empty;
                    sellerRegister.VATNumber = partyIdentifiers.FirstOrDefault(v => v.PartyIdentifierType.IdentifierType == Constants.IDENTIFIER_TYPE_VAT_NUMBER) != null ?
                      partyIdentifiers.FirstOrDefault(v => v.PartyIdentifierType.IdentifierType == Constants.IDENTIFIER_TYPE_VAT_NUMBER).IdentifierNumber : string.Empty;
                    if (!string.IsNullOrEmpty(sellerRegister.VATNumber))
                        sellerRegister.IsVAT = true;

                    sellerRegister.DUNSNumber = partyIdentifiers.FirstOrDefault(v => v.PartyIdentifierType.IdentifierType == Constants.IDENTIFIER_TYPE_DUNS_NUMBER) != null ?
                                          partyIdentifiers.FirstOrDefault(v => v.PartyIdentifierType.IdentifierType == Constants.IDENTIFIER_TYPE_DUNS_NUMBER).IdentifierNumber : string.Empty;
                    if (!string.IsNullOrEmpty(sellerRegister.DUNSNumber))
                        sellerRegister.HaveDuns = true;
                    sellerRegister.ParentDunsNumber = partyIdentifiers.FirstOrDefault(v => v.PartyIdentifierType.IdentifierType == Constants.IDENTIFIER_TYPE_PARENT_DUNS_NUMBER) != null ?
                                          partyIdentifiers.FirstOrDefault(v => v.PartyIdentifierType.IdentifierType == Constants.IDENTIFIER_TYPE_PARENT_DUNS_NUMBER).IdentifierNumber : string.Empty;
                }
            }
            return sellerRegister;
        }

        public static SellerRegister MappingToCapabilityDetails(this Party partyModel)
        {
            var sellerRegister = new SellerRegister();
            if (partyModel != null)
            {
                sellerRegister.SellerPartyId = partyModel.Id;
                var organisation = (partyModel.Organization!= null) ?
                    partyModel.Organization : null;
                var seller = (organisation != null) ? organisation.Supplier : null;
                if (organisation != null)
                {
                    sellerRegister.NumberOfEmployees = Convert.ToInt64(organisation.EmployeeSize);
                    sellerRegister.TurnOver = Convert.ToInt64(organisation.TurnOverSize);
                    sellerRegister.sector = Convert.ToInt64(organisation.BusinessSectorId);
                    sellerRegister.BusinessSectorDescription = organisation.BusinessSectorDescription;
                    sellerRegister.CompanyIndustryCodes = organisation.IndustryCodeOrganizationLinks.Select(v => new ItemList()
                    {
                        Value = v.RefIndustryCode,
                        Text = v.IndustryCode.SectorName
                    }).ToList();
                }
                if (seller != null)
                {
                    sellerRegister.MaxContractValue = seller.MaxContractValue;
                    sellerRegister.MinContractValue = seller.MinContractValue;
                }
                sellerRegister.CompanyServiceGeoRegions = partyModel.PartyRegionLinks.Where(v => v.LinkType.Trim() == Constants.SERVICE_REGION).Select(v => new ItemList()
                {
                    Value = v.RefRegion,
                    Text = v.Region.Name
                }).ToList();
                sellerRegister.CompanySalesGeoRegions = partyModel.PartyRegionLinks.Where(v => v.LinkType.Trim() == Constants.SALES_REGION).Select(v => new ItemList()
                {
                    Value = v.RefRegion,
                    Text = v.Region.Name
                }).ToList();
            }
            return sellerRegister;
        }
        public static SellerRegister MappingToMarketingDetails(this Party partyModel)
        {
            var sellerRegister = new SellerRegister();
            if (partyModel != null)
            {
                sellerRegister.SellerPartyId = partyModel.Id;
                var organisation = (partyModel.Organization != null) ?
                    partyModel.Organization: null;
                var seller = (organisation != null) ? organisation.Supplier : null;
                if (organisation != null && organisation.Document != null)
                {
                    sellerRegister.LogoFileName = organisation.Document.FileName;
                    sellerRegister.LogoFilePath = organisation.Document.FilePath;
                    sellerRegister.LogoDocumentId = organisation.Document.Id;
                }
                if (seller != null)
                {
                    sellerRegister.BusinessDescription = seller.BusinessDescription;
                    sellerRegister.WebsiteLink = seller.WebsiteLink;
                    sellerRegister.OrganisationFacebookaccount = seller.FacebookAccount;
                    sellerRegister.OrganisationLinkedInaccount = seller.LinkedInAccount;
                    sellerRegister.OrganisationTwitteraccount = seller.TwitterAccount;

                }

            }
            return sellerRegister;
        }
        public static List<IndustryCodeOrganizationLink> MapToIndustryCodeOrganizationLinks(this SellerRegister registerModel, long orgId)
        {
            var orgIndustryCodes = new List<IndustryCodeOrganizationLink>();

            if (!string.IsNullOrWhiteSpace(registerModel.SIC1))
            {
                var industryCode = new IndustryCodeOrganizationLink();
                industryCode.RefIndustryCode = Convert.ToInt64(registerModel.SIC1);
                industryCode.RefOrganization = orgId;
                orgIndustryCodes.Add(industryCode);
            }
            if (!string.IsNullOrWhiteSpace(registerModel.SIC2))
            {
                var industryCode = new IndustryCodeOrganizationLink();
                industryCode.RefIndustryCode = Convert.ToInt64(registerModel.SIC2);
                industryCode.RefOrganization = orgId;
                orgIndustryCodes.Add(industryCode);
            }
            if (!string.IsNullOrWhiteSpace(registerModel.SIC3))
            {
                var industryCode = new IndustryCodeOrganizationLink();
                industryCode.RefIndustryCode = Convert.ToInt64(registerModel.SIC3);
                industryCode.RefOrganization = orgId;
                orgIndustryCodes.Add(industryCode);
            }
            if (!string.IsNullOrWhiteSpace(registerModel.SIC4))
            {
                var industryCode = new IndustryCodeOrganizationLink();
                industryCode.RefIndustryCode = Convert.ToInt64(registerModel.SIC4);
                industryCode.RefOrganization = orgId;
                orgIndustryCodes.Add(industryCode);
            }
            if (!string.IsNullOrWhiteSpace(registerModel.SIC5))
            {
                var industryCode = new IndustryCodeOrganizationLink();
                industryCode.RefIndustryCode = Convert.ToInt64(registerModel.SIC5);
                industryCode.RefOrganization = orgId;
                orgIndustryCodes.Add(industryCode);
            }

            return orgIndustryCodes;
        }

        public static SellerRegister MappingToSellerRegisterModel(this Party partyModel)
        {
            SellerRegister sellerRegister = new SellerRegister();
            if (partyModel != null)
            {
                sellerRegister.OrganisationName = partyModel.PartyName;
                sellerRegister.DummyOrganisationName = partyModel.PartyName;
                sellerRegister.SellerPartyId = partyModel.Id;
                var partyIdentifiers = partyModel.PartyIdentifiers.ToList();
                if (partyIdentifiers.Count > 0)
                {
                    sellerRegister.CompanyRegistrationNumber = partyIdentifiers.Any(v => v.PartyIdentifierType.IdentifierType.Trim().Equals(Constants.IDENTIFIER_TYPE_REGISTRATION_NUMBER))
                        ? partyIdentifiers.FirstOrDefault(v => v.PartyIdentifierType.IdentifierType.Trim().Equals(Constants.IDENTIFIER_TYPE_REGISTRATION_NUMBER)).IdentifierNumber
                        : string.Empty;

                    sellerRegister.VATNumber = partyIdentifiers.Any(v => v.PartyIdentifierType.IdentifierType.Trim().Equals(Constants.IDENTIFIER_TYPE_VAT_NUMBER))
                        ? partyIdentifiers.FirstOrDefault(v => v.PartyIdentifierType.IdentifierType.Trim().Equals(Constants.IDENTIFIER_TYPE_VAT_NUMBER)).IdentifierNumber
                        : string.Empty;
                    if (!string.IsNullOrEmpty(sellerRegister.VATNumber))
                        sellerRegister.IsVAT = true;

                    sellerRegister.DUNSNumber = partyIdentifiers.Any(v => v.PartyIdentifierType.IdentifierType.Trim().Equals(Constants.IDENTIFIER_TYPE_DUNS_NUMBER))
                        ? partyIdentifiers.FirstOrDefault(v => v.PartyIdentifierType.IdentifierType.Trim().Equals(Constants.IDENTIFIER_TYPE_DUNS_NUMBER)).IdentifierNumber
                        : string.Empty;
                    if (!string.IsNullOrEmpty(sellerRegister.DUNSNumber))
                        sellerRegister.HaveDuns = true;

                    sellerRegister.ParentDunsNumber = partyIdentifiers.Any(v => v.PartyIdentifierType.IdentifierType.Trim().Equals(Constants.IDENTIFIER_TYPE_PARENT_DUNS_NUMBER))
                        ? partyIdentifiers.FirstOrDefault(v => v.PartyIdentifierType.IdentifierType.Trim().Equals(Constants.IDENTIFIER_TYPE_PARENT_DUNS_NUMBER)).IdentifierNumber
                        : string.Empty;
                }

                var organisation = (partyModel.Organization != null) ? partyModel.Organization : null;
                if (organisation != null)
                {
                    sellerRegister.NumberOfEmployees = Convert.ToInt64(organisation.EmployeeSize);
                    sellerRegister.TurnOver = Convert.ToInt64(organisation.TurnOverSize);
                    sellerRegister.sector = Convert.ToInt64(organisation.BusinessSectorId);
                    sellerRegister.BusinessSectorDescription = organisation.BusinessSectorDescription;
                    sellerRegister.CompanyIndustryCodes = organisation.IndustryCodeOrganizationLinks.Select(v => new ItemList()
                    {
                        Value = v.RefIndustryCode,
                        Text = string.Concat(v.IndustryCode.SectorName, " (", v.IndustryCode.CodeNumber, ")")
                    }).ToList();
                }

                var seller = (partyModel.Organization != null) ?
                    partyModel.Organization.Supplier : null;
                if (seller != null)
                {
                    sellerRegister.Trading = seller.TradingName;
                    sellerRegister.TypeOfCompany = Convert.ToInt16(seller.TypeOfSeller);
                    sellerRegister.IsSubsidaryStatus = Convert.ToBoolean(seller.IsSubsidary);
                    sellerRegister.UltimateParent = seller.UltimateParent;
                    sellerRegister.CompanyYear = seller.EstablishedYear;
                    sellerRegister.WebsiteLink = seller.WebsiteLink;
                    sellerRegister.OrganisationTwitteraccount = seller.TwitterAccount;
                    sellerRegister.OrganisationFacebookaccount = seller.FacebookAccount;
                    sellerRegister.OrganisationLinkedInaccount = seller.LinkedInAccount;
                    sellerRegister.MaxContractValue = seller.MaxContractValue;
                    sellerRegister.MinContractValue = seller.MinContractValue;
                    sellerRegister.BusinessDescription = seller.BusinessDescription;
                    sellerRegister.IsSubsidaryStatus = seller.IsSubsidary.HasValue ? seller.IsSubsidary.Value : false;
                }

                var partyContactMethodLinks = partyModel.PartyContactMethodLinks.Where(x => x.ContactMethod.ContactMethodType.Trim().Equals(Constants.CONTACT_METHOD_ADDRESS)).ToList();
                var addressDetails = null != partyContactMethodLinks ? partyContactMethodLinks.Select(x => x.ContactMethod.Addresses.FirstOrDefault()).ToList() : null;

                if (addressDetails != null && addressDetails.Any(a => a.AddressType == (short)AddressType.Primary))
                {
                    var primaryAddress = addressDetails.Where(a => a.AddressType == (short)AddressType.Primary).First();
                    sellerRegister.PrimaryAddressId = primaryAddress.Id;
                    sellerRegister.FirstAddressLine1 = primaryAddress.Line1;
                    sellerRegister.FirstAddressLine2 = primaryAddress.Line2;
                    sellerRegister.FirstAddressCity = primaryAddress.City;
                    sellerRegister.FirstAddressCountry = primaryAddress.RefCountryId.HasValue ? primaryAddress.RefCountryId.Value.ToString() : string.Empty;
                    sellerRegister.FirstAddressPostalCode = primaryAddress.ZipCode;
                    sellerRegister.FirstAddressState = primaryAddress.State;
                }

                if (addressDetails != null && addressDetails.Any(a => a.AddressType == (short)AddressType.HeadQuarters))
                {
                    var headQuartersAddress = addressDetails.Where(a => a.AddressType == (short)AddressType.HeadQuarters).First();
                    sellerRegister.HeadQuartersAddressId = headQuartersAddress.Id;
                    sellerRegister.HeadQuartersAddressLine1 = headQuartersAddress.Line1;
                    sellerRegister.HeadQuartersAddressLine2 = headQuartersAddress.Line2;
                    sellerRegister.HeadQuartersAddressPostalCode = headQuartersAddress.ZipCode;
                    sellerRegister.HeadQuartersAddressState = headQuartersAddress.State;
                    sellerRegister.HeadQuartersAddressCountry = headQuartersAddress.RefCountryId.HasValue ? headQuartersAddress.RefCountryId.Value.ToString() : string.Empty;
                    sellerRegister.HeadQuartersAddressCity = headQuartersAddress.City;

                    sellerRegister.IsHeadQuartersAddressDifferent = true;
                }

                if (addressDetails != null && addressDetails.Any(a => a.AddressType == (short)AddressType.Registered))
                {
                    var registeredAddress = addressDetails.Where(a => a.AddressType == (short)AddressType.Registered).First();
                    sellerRegister.RegisteredAddressId = registeredAddress.Id;
                    sellerRegister.SecondAddressLine1 = registeredAddress.Line1;
                    sellerRegister.SecondAddressLine2 = registeredAddress.Line2;
                    sellerRegister.SecondAddressPostalCode = registeredAddress.ZipCode;
                    sellerRegister.SecondAddressState = registeredAddress.State;
                    sellerRegister.SecondAddressCountry = registeredAddress.RefCountryId.HasValue ? registeredAddress.RefCountryId.Value.ToString() : string.Empty;
                    sellerRegister.SecondAddressCity = registeredAddress.City;

                    sellerRegister.IsAddressDifferent = true;
                }

                if (addressDetails != null && addressDetails.Any(a => a.AddressType == (short)AddressType.Remittance))
                {
                    var remittanceAddress = addressDetails.Where(a => a.AddressType == (short)AddressType.Remittance).First();
                    sellerRegister.RemittanceAddressId = remittanceAddress.Id;
                    sellerRegister.RemittanceAddressLine1 = remittanceAddress.Line1;
                    sellerRegister.RemittanceAddressLine2 = remittanceAddress.Line2;
                    sellerRegister.RemittanceAddressPostalCode = remittanceAddress.ZipCode;
                    sellerRegister.RemittanceAddressState = remittanceAddress.State;
                    sellerRegister.RemittanceAddressCountry = remittanceAddress.RefCountryId.HasValue ? remittanceAddress.RefCountryId.Value.ToString() : string.Empty;
                    sellerRegister.RemittanceAddressCity = remittanceAddress.City;

                    sellerRegister.IsRemittanceAddressDifferent = true;
                }

                sellerRegister.CompanyServiceGeoRegions = partyModel.PartyRegionLinks.Where(v => v.LinkType.Trim() == Constants.SERVICE_REGION).Select(v => new ItemList()
                {
                    Value = v.RefRegion,
                    Text = v.Region.Name
                }).ToList();

                sellerRegister.CompanySalesGeoRegions = partyModel.PartyRegionLinks.Where(v => v.LinkType.Trim() == Constants.SALES_REGION).Select(v => new ItemList()
                {
                    Value = v.RefRegion,
                    Text = v.Region.Name
                }).ToList();
            }
            return sellerRegister;
        }


        public static ViewModel.Address ToViewModel(this DAL.Entity.Address addressPM)
        {
            ViewModel.Address address = new ViewModel.Address();
            address.Id = addressPM.Id;
            address.Line1 = addressPM.Line1;
            address.Line2 = addressPM.Line2;
            address.City = addressPM.City;
            address.State = addressPM.State;
            address.ZipCode = addressPM.ZipCode;
            address.RefCountryId = addressPM.RefCountryId;
            address.AddressType = addressPM.AddressType;
            address.CountryName = (addressPM.Region != null) ? addressPM.Region.Name : string.Empty;
            return address;
        }

        public static List<ViewModel.ContactPerson> ToViewModel(this List<DAL.Entity.ContactPerson> contactsPM)
        {
            List<ViewModel.ContactPerson> contacts = new List<ViewModel.ContactPerson>();

            foreach (var item in contactsPM)
            {
                var contact = new ViewModel.ContactPerson();
                contact.Id = item.Id;
                contact.FirstName = item.Person.FirstName;
                contact.LastName = item.Person.LastName;
                var partyContactMethodLinks = item.Person.Party.PartyContactMethodLinks.ToList();
                var email = (partyContactMethodLinks.FirstOrDefault(v => v.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_EMAIL) != null)
                    ? partyContactMethodLinks.FirstOrDefault(v => v.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_EMAIL).ContactMethod.Emails.FirstOrDefault() : null;

                contact.Email = (email != null) ? email.EmailAddress : string.Empty;
                var phoneContactMethods = (partyContactMethodLinks.Count(v => v.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_PHONE) > 0)
                   ? partyContactMethodLinks.Where(v => v.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_PHONE).Select(v => v.ContactMethod).ToList() : new List<ContactMethod>();
                if (phoneContactMethods.Count > 0)
                {
                    contact.Telephone = (phoneContactMethods.FirstOrDefault(v => v.Phones.Any(c => c.Type.Trim() == Constants.PHONE_TYPE_TELEPHONE)) != null) ?
              phoneContactMethods.FirstOrDefault(v => v.Phones.Any(c => c.Type.Trim() == Constants.PHONE_TYPE_TELEPHONE)).Phones.FirstOrDefault() != null ?
              phoneContactMethods.FirstOrDefault(v => v.Phones.Any(c => c.Type.Trim() == Constants.PHONE_TYPE_TELEPHONE)).Phones.FirstOrDefault().PhoneNumber : string.Empty : string.Empty;
                    contact.Fax = (phoneContactMethods.FirstOrDefault(v => v.Phones.Any(c => c.Type.Trim() == Constants.FAX)) != null) ?
                                 phoneContactMethods.FirstOrDefault(v => v.Phones.Any(c => c.Type.Trim() == Constants.FAX)).Phones.FirstOrDefault() != null ?
                                 phoneContactMethods.FirstOrDefault(v => v.Phones.Any(c => c.Type.Trim() == Constants.FAX)).Phones.FirstOrDefault().PhoneNumber : string.Empty : string.Empty;

                }
                contact.JobTitle = item.JobTitle;
                contact.ContactType = item.ContactType;
                contact.ContactPartyId = item.Person.Party.Id;
                contact.SellerPartyId = item.Person.Party.PartyPartyLinks1.FirstOrDefault(c => c.PartyPartyLinkType.Trim() == Constants.PRIMARY_ORGANIZATION) != null ?
                    item.Person.Party.PartyPartyLinks1.FirstOrDefault(c => c.PartyPartyLinkType.Trim() == Constants.PRIMARY_ORGANIZATION).RefLinkedParty : 0;
                contact.AssignedCount = item.Person.Party.PartyPartyLinks1.Count(z => z.PartyPartyLinkType.Trim() == Constants.CONTACT_BUYER);
                contact.RefAddressContactMethod = (partyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS) != null) ?
                partyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS).ContactMethod.Id : 0;
                var address = (item.Person.Party.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS) != null && item.Person.Party.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS).ContactMethod.Addresses.FirstOrDefault() != null) ?
                item.Person.Party.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS).ContactMethod.Addresses.FirstOrDefault() : null;
                contact.MailingAddressValue = (address != null) ? (address.Line1 + "," + address.Line2 + ","
                                                                 + address.City + ","
                                                                 + address.Region.Name) : "-";
                contacts.Add(contact);
            }
            return contacts;
        }

        public static ViewModel.ContactPerson ToViewModel(this DAL.Entity.ContactPerson contactsPM)
        {
            ViewModel.ContactPerson contacts = new ViewModel.ContactPerson();
            contacts.Id = contactsPM.Id;
            contacts.FirstName = contactsPM.Person.FirstName;
            contacts.LastName = contactsPM.Person.LastName;
            contacts.JobTitle = contactsPM.Person.JobTitle;
            contacts.ContactType = contactsPM.ContactType;
            contacts.ContactPartyId = contactsPM.Person.Party.Id;
            contacts.Email = (contactsPM.Person.Party.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_EMAIL) != null) ?
                 contactsPM.Person.Party.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_EMAIL).ContactMethod.Emails.FirstOrDefault() != null ?
                 contactsPM.Person.Party.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_EMAIL).ContactMethod.Emails.FirstOrDefault().EmailAddress : string.Empty : string.Empty;

            var phonePartyLinks = contactsPM.Person.Party.PartyContactMethodLinks.Where(v => v.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_PHONE).ToList();
            contacts.Telephone = (phonePartyLinks.FirstOrDefault(c => c.ContactMethod.Phones.Any(k => k.Type.Trim() == Constants.PHONE_TYPE_TELEPHONE)) != null) ?
                phonePartyLinks.FirstOrDefault(c => c.ContactMethod.Phones.Any(k => k.Type.Trim() == Constants.PHONE_TYPE_TELEPHONE)).ContactMethod.Phones.FirstOrDefault() != null ?
                phonePartyLinks.FirstOrDefault(c => c.ContactMethod.Phones.Any(k => k.Type.Trim() == Constants.PHONE_TYPE_TELEPHONE)).ContactMethod.Phones.FirstOrDefault().PhoneNumber : string.Empty : string.Empty;
            contacts.Fax = (phonePartyLinks.FirstOrDefault(c => c.ContactMethod.Phones.Any(k => k.Type.Trim() == Constants.PHONE_TYPE_FAX)) != null) ?
                 phonePartyLinks.FirstOrDefault(c => c.ContactMethod.Phones.Any(k => k.Type.Trim() == Constants.PHONE_TYPE_FAX)).ContactMethod.Phones.FirstOrDefault() != null ?
                 phonePartyLinks.FirstOrDefault(c => c.ContactMethod.Phones.Any(k => k.Type.Trim() == Constants.PHONE_TYPE_FAX)).ContactMethod.Phones.FirstOrDefault().PhoneNumber : null : null;

            contacts.SellerPartyId = contactsPM.Person.Party.PartyPartyLinks1.FirstOrDefault(c => c.PartyPartyLinkType == Constants.PRIMARY_ORGANIZATION) != null ?
             contactsPM.Person.Party.PartyPartyLinks1.FirstOrDefault(c => c.PartyPartyLinkType == Constants.PRIMARY_ORGANIZATION).RefLinkedParty : 0;

            contacts.RefAddressContactMethod = (contactsPM.Person.Party.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS) != null) ?
                contactsPM.Person.Party.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS).ContactMethod.Id : 0;
            var address = (contactsPM.Person.Party.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS) != null && contactsPM.Person.Party.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS).ContactMethod.Addresses.FirstOrDefault() != null) ?
            contactsPM.Person.Party.PartyContactMethodLinks.FirstOrDefault(c => c.ContactMethod.ContactMethodType.Trim() == Constants.CONTACT_METHOD_ADDRESS).ContactMethod.Addresses.FirstOrDefault() : null;
            contacts.MailingAddressValue = (address != null) ? (address.Line1 + "," + address.Line2 + ","
                                                             + address.City + ","
                                                             + address.Region.Name) : "-";

            return contacts;
        }

        public static Party ToContactPersonPartyModel(this ViewModel.ContactPerson contact, long sellerUserPartyId)
        {
            var contactPerson = new DAL.Entity.ContactPerson();
            contactPerson =new DAL.Entity.ContactPerson
            {
                ContactType = contact.ContactType,
                JobTitle = contact.JobTitle,
                RefCreatedBy = sellerUserPartyId,
                RefLastUpdatedBy = sellerUserPartyId
            } ;
            var person = new Person();
            person = new Person
            {
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                JobTitle = contact.JobTitle,
                RefCreatedBy = sellerUserPartyId,
                RefLastUpdatedBy = sellerUserPartyId,
                PersonType = Constants.PERSON_TYPE_CONTACT_PERSON,
                ContactPerson = contactPerson
            };
            var party = new Party()
            {
                PartyName = string.Concat(contact.FirstName.Trim(), " ", contact.LastName.Trim()),
                PartyType = Constants.PARTY_TYPE_PERSON,
                IsActive = true,
                Person = person
            };

            return party;
        }


        public static List<ViewModel.Invitee> ToViewModel(this List<DAL.Entity.Invitee> InviteePMList)
        {
            List<ViewModel.Invitee> invitees = new List<ViewModel.Invitee>();
            invitees = InviteePMList.Select(v => new ViewModel.Invitee()
            {
                Id = v.Id,
                ClientName = v.ClientName.Trim(),
                ContactName = v.ContactName.Trim(),
                JobTitle = v.JobTitle.Trim(),
                Email = v.Email.Trim(),
                Phone = v.PhoneNumber.Trim(),
                MailingAddress = v.MailingAddress.Trim(),
                Fax = v.Fax.Trim(),
                ClientRole = v.ClientRole.Trim(),
                CanWeContact = v.CanWeContact,
                AssignedCount = v.BuyerSupplierReferences.Count()
            }).ToList();

            return invitees;
        }
        public static DAL.Entity.Invitee ToDBModel(this ViewModel.Invitee referenceDetail, long sellerUserPartyId)
        {
            DAL.Entity.Invitee inviteePm = new DAL.Entity.Invitee();
            inviteePm.ClientName = referenceDetail.ClientName;
            inviteePm.ContactName = referenceDetail.ContactName;
            inviteePm.JobTitle = referenceDetail.JobTitle;
            inviteePm.Email = referenceDetail.Email;
            inviteePm.PhoneNumber = referenceDetail.Phone;
            inviteePm.MailingAddress = referenceDetail.MailingAddress;
            inviteePm.Fax = referenceDetail.Fax;
            inviteePm.ClientRole = referenceDetail.ClientRole;
            inviteePm.CanWeContact = referenceDetail.CanWeContact;
            inviteePm.RefCreatedBy = sellerUserPartyId;
            inviteePm.RefLastUpdatedBy = sellerUserPartyId;
            return inviteePm;
        }
        public static List<ViewModel.BankAccount> ToViewModel(this List<DAL.Entity.BankAccount> bankAccountPMList)
        {
            List<ViewModel.BankAccount> bankDetails = new List<ViewModel.BankAccount>();
            bankDetails = bankAccountPMList.Select(v => new ViewModel.BankAccount()
            {
                Id = v.Id,
                AccountName = v.AccountName.Trim(),
                AccountNumber = v.AccountNumber.Trim(),
                BranchIdentifier = v.BranchIdentifier.Trim(),
                SwiftCode = v.SwiftCode.Trim(),
                IBAN = v.IBAN.Trim(),
                BankName = v.BankName.Trim(),
                Address = v.Address.Trim(),
                CountryName = v.Region.Name.Trim(),
                RefCountryId = v.RefCountryId,
                PreferredMode = v.PreferredMode.Trim(),
                RefLegalEntity = v.RefLegalEntity,
                AssignedCount = v.LegalEntityProfiles.Count()
            }).ToList();
            return bankDetails;
        }
        public static DAL.Entity.BankAccount ToDBModel(this ViewModel.BankAccount bankDetail, long organisationId)
        {
            DAL.Entity.BankAccount bankPm = new DAL.Entity.BankAccount();
            bankPm.AccountName = bankDetail.AccountName;
            bankPm.AccountNumber = bankDetail.AccountNumber;
            bankPm.BranchIdentifier = bankDetail.BranchIdentifier;
            bankPm.SwiftCode = bankDetail.SwiftCode;
            bankPm.IBAN = bankDetail.IBAN;
            bankPm.BankName = bankDetail.BankName;
            bankPm.Address = bankDetail.Address;
            bankPm.RefCountryId = bankDetail.RefCountryId;
            bankPm.PreferredMode = bankDetail.PreferredMode;
            bankPm.RefLastUpdatedBy = organisationId;
            bankPm.RefCreatedBy = organisationId;
            return bankPm;
        }
    }
}
