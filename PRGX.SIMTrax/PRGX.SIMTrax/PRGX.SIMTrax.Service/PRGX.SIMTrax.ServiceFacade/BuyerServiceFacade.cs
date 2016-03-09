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
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.ServiceFacade
{
    public class BuyerServiceFacade : IBuyerServiceFacade
    {
        private readonly IBuyerUow _buyerUow;

        public BuyerServiceFacade()
        {
            _buyerUow = new BuyerUow();
        }

        public bool AddBuyerUser(BuyerRegister buyerRegisterModel)
        {
            IBuyerUow _buyerUow = null;
            try
            {
                Logger.Info("BuyerServiceFacade : AddBuyerUser() : Entering the method");

                _buyerUow = new BuyerUow();

                var partyUser = buyerRegisterModel.ToPartyModelUser();
                var partyBuyer = buyerRegisterModel.ToPartyModelBuyer();

                //Let's start transaction
                _buyerUow.BeginTransaction();

                // add party user
                _buyerUow.Parties.Add(partyUser);
                _buyerUow.SaveChanges();

                _buyerUow.Parties.Add(partyBuyer);
                _buyerUow.SaveChanges();

                var partyPartyLink = new PartyPartyLink
                {
                    RefParty = partyUser.Id,
                    RefLinkedParty = partyBuyer.Id,
                    PartyPartyLinkType = Constants.PRIMARY_ORGANIZATION,
                    RefCreatedBy = partyUser.Id,
                    RefLastUpdatedBy = partyUser.Id
                };

                _buyerUow.PartyPartyLinks.Add(partyPartyLink);
                _buyerUow.SaveChanges();

                var contactPersonParty = buyerRegisterModel.ToContactMethodPersonPartyModel(partyUser.Id);
                _buyerUow.Parties.Add(contactPersonParty);
                _buyerUow.SaveChanges();

                var partyPartyLinkContact = new PartyPartyLink
                {
                    RefParty = contactPersonParty.Id,
                    RefLinkedParty = partyBuyer.Id,
                    PartyPartyLinkType = Constants.CONTACT_ORGANIZATION,
                    RefCreatedBy = partyUser.Id,
                    RefLastUpdatedBy = partyUser.Id
                };

                _buyerUow.PartyPartyLinks.Add(partyPartyLinkContact);
                _buyerUow.SaveChanges();

                var contactMethodLinkEmail = new List<PartyContactMethodLink>();
                contactMethodLinkEmail.Add(new PartyContactMethodLink{
                    RefParty = contactPersonParty.Id
                });
                var contactMethodEmail = new ContactMethod
                {
                    ContactMethodType = Constants.CONTACT_METHOD_EMAIL,
                    PartyContactMethodLinks = contactMethodLinkEmail,
                    RefCreatedBy = partyUser.Id,
                    RefLastUpdatedBy = partyUser.Id
                };
                _buyerUow.ContactMethods.Add(contactMethodEmail);
                _buyerUow.SaveChanges();

                _buyerUow.Emails.Add(new DAL.Entity.Email
                {
                    EmailAddress = buyerRegisterModel.BuyerEmail,
                    RefContactMethod = contactMethodEmail.Id
                });
                _buyerUow.SaveChanges();

                var contactMethodLinkPhone = new List<PartyContactMethodLink>();
                contactMethodLinkPhone.Add(new PartyContactMethodLink
                {
                    RefParty = contactPersonParty.Id
                });
                var contactMethodPhone = new ContactMethod
                {
                    ContactMethodType = Constants.CONTACT_METHOD_PHONE,
                    PartyContactMethodLinks = contactMethodLinkPhone,
                    RefCreatedBy = partyUser.Id,
                    RefLastUpdatedBy = partyUser.Id
                };
                _buyerUow.ContactMethods.Add(contactMethodPhone);
                _buyerUow.SaveChanges();

                _buyerUow.Phones.Add(new Phone
                {
                    PhoneNumber = buyerRegisterModel.BuyerTelephone,
                    RefContactMethod = contactMethodPhone.Id,
                    Type = Constants.PHONE_TYPE_TELEPHONE
                });
                _buyerUow.SaveChanges();

                // all fine, let's commit
                _buyerUow.Commit();

                Logger.Info("BuyerServiceFacade : AddBuyerUser() : Exiting the method");
                return true;
            }
            catch (Exception ex)
            {
                if (_buyerUow != null)
                    _buyerUow.Rollback();
                Logger.Error("BuyerServiceFacade : AddBuyerUser() : Caught an exception" + ex);
                throw;
            }
        }

        public List<BuyerOrganization> GetBuyerOrganizations(int status, string fromdate, string toDate, out int total, int pageIndex, long buyerRole, int pageSize, int sortDirection, string sortParameter, string buyerName = "")
        {
            try
            {
                Logger.Info("BuyerServiceFacade : GetBuyerOrganizations() : Enter the method");
                var buyerOrganization = new List<BuyerOrganization>();

                buyerOrganization = _buyerUow.Buyers.GetBuyerOrganization(status, buyerRole, fromdate, toDate, out total, pageIndex, pageSize, sortDirection, sortParameter, buyerName).ToList();
                total = buyerOrganization.Count;

                Logger.Info("BuyerServiceFacade : GetBuyerOrganizations() : Exit the method");
                return buyerOrganization;
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerServiceFacade : GetBuyerOrganizations() : Caught an exception" + ex);
                throw;
            }
        }

        public bool VerifyBuyerCompanyDetails(BuyerRegister buyerRegisterModel, long profileVerifiedBy)
        {
            Logger.Info("BuyerServiceFacade : VerifyBuyerCompanyDetails() : Exit from method");
            IBuyerUow _buyerUow = null;
            var result = false;
            try
            {
                _buyerUow = new BuyerUow();
                _buyerUow.BeginTransaction();

                var primaryContactMethodParty = _buyerUow.Buyers.GetBuyerPrimaryContactPartyId(buyerRegisterModel.BuyerPartyId);
                if (primaryContactMethodParty > 0)
                {
                    var contactMethodLinks = _buyerUow.PartyContactMethodLinks.All().Where(x => x.RefParty == primaryContactMethodParty).Select(x => x.RefContactMethod).ToList();

                    var emailContactMethod = _buyerUow.ContactMethods.All().FirstOrDefault(v => v.ContactMethodType.Trim().Equals(Constants.CONTACT_METHOD_EMAIL) && contactMethodLinks.Contains(v.Id));
                    if (emailContactMethod != null)
                    {
                        var email = _buyerUow.Emails.All().FirstOrDefault(v => v.RefContactMethod == emailContactMethod.Id);
                        if (null != email)
                        {
                            email.EmailAddress = buyerRegisterModel.BuyerEmail;
                            _buyerUow.Emails.Update(email);
                            _buyerUow.SaveChanges();
                        }
                    }
                    var phoneContactMethod = _buyerUow.ContactMethods.All().FirstOrDefault(v => v.ContactMethodType.Trim().Equals(Constants.CONTACT_METHOD_PHONE) && contactMethodLinks.Contains(v.Id));
                    if (phoneContactMethod != null)
                    {
                        var phone = _buyerUow.Phones.All().FirstOrDefault(v => v.RefContactMethod == phoneContactMethod.Id);
                        if (phone != null)
                        {
                            phone.PhoneNumber = buyerRegisterModel.BuyerTelephone;
                            phone.Type = Constants.PHONE_TYPE_TELEPHONE;
                            _buyerUow.Phones.Update(phone);
                            _buyerUow.SaveChanges();
                        }
                    }
                    var person = _buyerUow.Persons.All().FirstOrDefault(v => v.ContactPerson.Id == buyerRegisterModel.ContactPersonId);
                    if (person != null)
                    {
                        person.FirstName = buyerRegisterModel.BuyerFirstName;
                        person.LastName = buyerRegisterModel.BuyerLastName;
                        person.JobTitle = buyerRegisterModel.BuyerJobTitle;
                        person.RefLastUpdatedBy = buyerRegisterModel.UserPartyId;
                        _buyerUow.Persons.Update(person);
                        _buyerUow.SaveChanges();
                    }
                    var contactPerson = _buyerUow.ContactPersons.All().Where(v => v.Id == buyerRegisterModel.ContactPersonId && v.ContactType == (short)ContactType.Primary).FirstOrDefault();
                    if (contactPerson != null)
                    {
                        contactPerson.JobTitle = buyerRegisterModel.BuyerJobTitle;
                        contactPerson.RefLastUpdatedBy = buyerRegisterModel.UserPartyId;
                        _buyerUow.ContactPersons.Update(contactPerson);
                        _buyerUow.SaveChanges();
                    }
                }

                var contactMethodAddressLinks = _buyerUow.PartyContactMethodLinks.All().Where(x => x.RefParty == buyerRegisterModel.BuyerPartyId).Select(x => x.RefContactMethod).ToList();

                var addressContactMethod = _buyerUow.ContactMethods.All().FirstOrDefault(v => v.ContactMethodType.Trim().Equals(Constants.CONTACT_METHOD_ADDRESS) && contactMethodAddressLinks.Contains(v.Id));

                if (null != addressContactMethod)
                {
                    var address = _buyerUow.Addresses.GetAll().FirstOrDefault(a => a.AddressType == (short)AddressType.Primary && a.RefContactMethod == addressContactMethod.Id);
                    if (null != address)
                    {
                        address.Line1 = buyerRegisterModel.BuyerFirstAddressLine1;
                        address.Line2 = buyerRegisterModel.BuyerFirstAddressLine2;
                        address.City = buyerRegisterModel.BuyerFirstAddressCity;
                        address.ZipCode = buyerRegisterModel.BuyerFirstAddressPostalCode;
                        address.LastUpdatedOn = DateTime.UtcNow;
                        address.RefCountryId = Convert.ToInt64(buyerRegisterModel.BuyerFirstAddressCountry);
                        address.RefLastUpdatedBy = buyerRegisterModel.UserPartyId;
                        address.State = buyerRegisterModel.BuyerFirstAddressState;
                        _buyerUow.Addresses.Update(address);
                        _buyerUow.SaveChanges();
                    }
                }

                var buyerUserPerson = _buyerUow.Persons.GetAll().Where(x => x.Id == buyerRegisterModel.UserPartyId).FirstOrDefault();
                if (null != buyerUserPerson)
                {
                    buyerUserPerson.FirstName = buyerRegisterModel.BuyerFirstName;
                    buyerUserPerson.LastName = buyerRegisterModel.BuyerLastName;
                    buyerUserPerson.JobTitle = buyerRegisterModel.BuyerJobTitle;
                    buyerUserPerson.RefLastUpdatedBy = buyerRegisterModel.UserPartyId;
                    _buyerUow.Persons.Update(buyerUserPerson);
                    _buyerUow.SaveChanges();
                }

                var organization = _buyerUow.Organizations.All().FirstOrDefault(u => u.Id == buyerRegisterModel.BuyerPartyId);
                if (organization != null)
                {
                    organization.EmployeeSize = buyerRegisterModel.BuyerNumberOfEmployees;
                    organization.TurnOverSize = buyerRegisterModel.BuyerTurnOver;
                    organization.BusinessSectorId = buyerRegisterModel.BuyerSector;
                    organization.RefLastUpdatedBy = buyerRegisterModel.UserPartyId;
                    organization.ProfileVerifiedOn = DateTime.Now;
                    organization.RefProfileVerifiedBy = profileVerifiedBy;
                    organization.Status = (short)CompanyStatus.ProfileVerified;
                    _buyerUow.Organizations.Update(organization);
                    _buyerUow.SaveChanges();
                }

                _buyerUow.Commit();
                result = true;
            }
            catch (Exception ex)
            {
                if (_buyerUow != null)
                    _buyerUow.Rollback();
                Logger.Error("BuyerServiceFacade : VerifyBuyerCompanyDetails() : Caught an exception " + ex);
                throw ex;
            }
            Logger.Info("BuyerServiceFacade : VerifyBuyerCompanyDetails() : Exit from method");
            return result;
        }

        public BuyerRegister GetBuyerOrganizationDetailsByPartyId(long organizationPartyId)
        {
            try
            {
                BuyerRegister details = null;
                Logger.Info("BuyerServiceFacade : GetBuyerOrganizationDetailsByPartyId() : Enter the method");

                details = _buyerUow.Buyers.GetBuyerOrganizationDetailsByPartyId(organizationPartyId).ToBuyerRegisterModel();

                Logger.Info("BuyerServiceFacade : GetBuyerOrganizationDetailsByPartyId() : Exit the method");
                return details;
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerServiceFacade : GetBuyerOrganizationDetailsByPartyId() : Caught an exception" + ex);
                throw;
            }
        }

        public bool ActivateBuyer(long buyerPartyId, long auditorPartyId, long roleId)
        {
            Logger.Info("BuyerServiceFacade : ActivateBuyer() : Enter the method");
            IBuyerUow _buyerUow = null;
            var result = false;
            try
            {
                _buyerUow = new BuyerUow();
                _buyerUow.BeginTransaction();
                var buyerOrg = _buyerUow.Organizations.GetOrganizationDetail(buyerPartyId);
                if (null != buyerOrg)
                {
                    var buyer = _buyerUow.Buyers.GetAll().FirstOrDefault(b => b.Id == buyerOrg.OrganizationId);
                    if (null != buyer)
                    {
                        buyer.ActivationDate = DateTime.UtcNow;
                        buyer.HasPaid = true;
                        buyer.RefLastUpdatedBy = auditorPartyId;
                    }
                    _buyerUow.SaveChanges();


                    var buyerUsers = _buyerUow.Users.GetPartyUser(buyerPartyId);
                    foreach (var user in buyerUsers)
                    {
                        var existingUserRoleLink = _buyerUow.UserRoleLinks.GetAll().Where(r => r.RefUser == user.Id).ToList();
                        _buyerUow.UserRoleLinks.DeleteRange(existingUserRoleLink);
                        _buyerUow.SaveChanges();

                        var userRoleLink = new UserRoleLink
                        {
                            RefRole = roleId,
                            RefUser = user.Id
                        };
                        _buyerUow.UserRoleLinks.Add(userRoleLink);
                        _buyerUow.SaveChanges();
                    }

                    _buyerUow.Commit();
                    result = true;
                }

                Logger.Info("BuyerServiceFacade : ActivateBuyer() : Exit the method");
                return result;
            }
            catch (Exception ex)
            {
                if (_buyerUow != null)
                    _buyerUow.Rollback();
                Logger.Error("BuyerServiceFacade : ActivateBuyer() : Caught an exception" + ex);
                throw;
            }
        }

        public List<BuyerOrganization> GetNotActivatedBuyerOrganization(int pageIndex, int pageSize, int sortDirection, string sortParameter, out int total)
        {
            try
            {
                Logger.Info("BuyerServiceFacade : GetNotActivatedBuyerOrganization() : Enter the method");
                var buyerOrganization = new List<BuyerOrganization>();

                buyerOrganization = _buyerUow.Buyers.GetNotActivatedBuyerOrganization(pageIndex, pageSize, sortDirection, sortParameter, out total).ToList();
                total = buyerOrganization.Count;

                Logger.Info("BuyerServiceFacade : GetNotActivatedBuyerOrganization() : Exit the method");
                return buyerOrganization;
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerServiceFacade : GetNotActivatedBuyerOrganization() : Caught an exception" + ex);
                throw;
            }
        }

        public List<SupplierDetails> GetSuppliers(BuyerSupplierSearchFilter model, long companyPartyId, long userPartyId, out int totalRecords)
        {
            try
            {
                Logger.Info("BuyerServiceFacade : GetSuppliers() : Enter the method");
                var supplierList = new List<SupplierDetails>();

                supplierList = _buyerUow.Buyers.GetSuppliers(model, companyPartyId, userPartyId, out totalRecords);

                Logger.Info("BuyerServiceFacade : GetSuppliers() : Exit the method");
                return supplierList;
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerServiceFacade : GetSuppliers() : Caught an exception" + ex);
                throw;
            }
        }
        public List<string> GetSuppliersListForIntellisense(string text)
        {
            try
            {
                Logger.Info("BuyerServiceFacade : GetSuppliersListForIntellisense() : Enter the method");
                var supplierList = new List<string>();

                supplierList = _buyerUow.Parties.All().Where(v => v.Organization.OrganizationType.Trim() == Constants.ORGANIZATION_TYPE_SELLER && v.PartyName.ToLower().Trim().Contains(text.ToLower().Trim())).Select(v => v.PartyName).ToList();
                    Logger.Info("BuyerServiceFacade : GetSuppliersListForIntellisense() : Exit the method");
                return supplierList;
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerServiceFacade : GetSuppliersListForIntellisense() : Caught an exception" + ex);
                throw;
            }
        }

        public bool ChangeAccessType(long buyerPartyId, long auditorPartyId, long roleId)
        {
            Logger.Info("BuyerServiceFacade : ActivateBuyer() : Enter the method");
            IBuyerUow _buyerUow = null;
            var result = false;
            try
            {
                _buyerUow = new BuyerUow();
                _buyerUow.BeginTransaction();
                var buyerUsers = _buyerUow.Users.GetPartyUser(buyerPartyId);
                foreach (var user in buyerUsers)
                {
                    var existingUserRoleLink = _buyerUow.UserRoleLinks.GetAll().Where(r => r.RefUser == user.Id).ToList();
                    _buyerUow.UserRoleLinks.DeleteRange(existingUserRoleLink);
                    _buyerUow.SaveChanges();

                    var userRoleLink = new UserRoleLink
                    {
                        RefRole = roleId,
                        RefUser = user.Id
                    };
                    _buyerUow.UserRoleLinks.Add(userRoleLink);
                    _buyerUow.SaveChanges();
                }

                _buyerUow.Commit();
                result = true;

                Logger.Info("BuyerServiceFacade : ChangeAccessType() : Exit the method");
                return result;
            }
            catch (Exception ex)
            {
                if (_buyerUow != null)
                    _buyerUow.Rollback();
                Logger.Error("BuyerServiceFacade : ChangeAccessType() : Caught an exception" + ex);
                throw;
            }
        }

        public List<string> GetVerifiedBuyerNames(string buyerOrg)
        {
            try
            {
                Logger.Info("BuyerServiceFacade : GetVerifiedBuyerNames() : Enter the method");

                Logger.Info("BuyerServiceFacade : GetVerifiedBuyerNames() : Exit the method");
                return _buyerUow.Buyers.GetVerifiedBuyerNames(buyerOrg);
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerServiceFacade : GetVerifiedBuyerNames() : Caught an exception" + ex);
                throw;
            }
        }

        public BuyerOrganization GetBuyerDetailsForDashboard(long partyId)
        {
            try
            {
                Logger.Info("BuyerServiceFacade : GetBuyerDetailsForDashboard() : Enter the method");

                Logger.Info("BuyerServiceFacade : GetBuyerDetailsForDashboard() : Exit the method");
                return _buyerUow.Buyers.GetBuyerDetailsForDashboard(partyId);
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerServiceFacade : GetBuyerDetailsForDashboard() : Caught an exception" + ex);
                throw;
            }
        }

        public List<ItemList> GetBuyersList()
        {
            try
            {
                Logger.Info("BuyerServiceFacade : GetBuyersList() : Enter the method");

                Logger.Info("BuyerServiceFacade : GetBuyersList() : Exit the method");
                return _buyerUow.Buyers.GetBuyersList();
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerServiceFacade : GetBuyersList() : Caught an exception" + ex);
                throw;
            }
        }

        public bool AddorUpdateVoucher(Voucher voucher, long userId)
        {
            IBuyerUow _buyerUow = null;
            Logger.Info("BuyerServiceFacade : AddorUpdateVoucher() : Enter the method");
            bool result = false;
            try
            {
                _buyerUow = new BuyerUow();
                _buyerUow.BeginTransaction();

                if (voucher.VoucherId > 0)
                {
                    var discountVouchers = _buyerUow.DiscountVouchers.GetById(voucher.VoucherId);
                    discountVouchers.Id = voucher.VoucherId;
                    discountVouchers.PromotionStartDate = voucher.PromotionStartDate.Value;
                    discountVouchers.PromotionEndDate = voucher.PromotionEndDate.Value;
                    discountVouchers.DiscountPercent = voucher.DiscountPercent;
                    discountVouchers.RefBuyer = voucher.BuyerPartyId;
                    discountVouchers.PromotionalCode = voucher.PromotionalCode;
                    discountVouchers.RefLastUpdatedBy = userId;
                    _buyerUow.DiscountVouchers.Update(discountVouchers);
                    _buyerUow.SaveChanges();
                }
                else
                {
                    var discountVouchers = voucher.ToEntityModel();
                    discountVouchers.RefCreatedBy = userId;
                    _buyerUow.DiscountVouchers.Add(discountVouchers);
                    _buyerUow.SaveChanges();
                }
                _buyerUow.Commit();
                result = true;
            }
            catch (Exception ex)
            {
                if (_buyerUow != null)
                    _buyerUow.Rollback();
                Logger.Error("BuyerServiceFacade : AddorUpdateVoucher() : Caught an exception" + ex);
                throw ex;
            }
            Logger.Info("BuyerServiceFacade : AddorUpdateVoucher() : Exit the method");
            return result;
        }

        public bool IsVoucherAlreadyExists(string code)
        {
            var result = false;
            try
            {
                Logger.Info("BuyerServiceFacade : IsVoucherAlreadyExists() : Entering into method");
                if (_buyerUow.DiscountVouchers.GetAll().ToList().Find(dv => dv.PromotionalCode.ToLower().Trim() == code.ToLower().Trim()) != null)
                {
                    result = true;
                }               
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerServiceFacade : IsVoucherAlreadyExists() : Caught an exception " + ex);
                throw ex;
            }
            Logger.Info("BuyerServiceFacade : IsVoucherAlreadyExists() : Exit from method");
            return result;
        }

        public Voucher GetDiscountVoucherById(string voucherCode)
        {
            var discountVoucher = new Voucher();
            try
            {
                Logger.Info("BuyerServiceFacade : GetDiscountVoucherById() : Entering into method");
                discountVoucher = _buyerUow.DiscountVouchers.GetAll().SingleOrDefault(dv => dv.PromotionalCode == voucherCode).ToViewModel();
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerServiceFacade : GetDiscountVoucherById() : Caught an exception " + ex);
                throw ex;
            }
            Logger.Info("BuyerServiceFacade : GetDiscountVoucherById() : Exit from method");
            return discountVoucher;
        }

        public List<Voucher> GetAllVouchers(int currentPage, string sortParameter, int sortDirection, out int total, int count, long buyerPartyId = 0)
        {
            var discountVoucherList = new List<Voucher>();
            try
            {
                Logger.Info("BuyerServiceFacade : GetAllVouchers() : Entering into method");
                discountVoucherList = _buyerUow.Buyers.GetAllVouchers(currentPage,sortParameter,sortDirection,out total, count, buyerPartyId).ToViewModel();
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerServiceFacade : GetAllVouchers() : Caught an exception " + ex);
                throw ex;
            }
            Logger.Info("BuyerServiceFacade : GetAllVouchers() : Exit from method");
            return discountVoucherList;
        }

        public int GetMasterVendorValue(long buyerId)
        {
            Logger.Info("BuyerServiceFacade : GetMasterVendorValue() : Entering the method");
            int masterVendorValue = 0;
            try
            {
                var buyer = _buyerUow.Buyers.GetById(buyerId);
                if (buyer != null)
                {
                    masterVendorValue = buyer.MaxCampaignSupplierCount.HasValue ? buyer.MaxCampaignSupplierCount.Value : 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerServiceFacade : GetMasterVendorValue() : Caught an exception" + ex);
                throw ex;
            }
            Logger.Info("BuyerServiceFacade : GetMasterVendorValue() : Exiting the method");
            return masterVendorValue;
        }

        public List<ItemList> GetVoucherListForBuyer(long buyerId)
        {
            try
            {
                Logger.Info("BuyerServiceFacade : GetVoucherListForBuyer() : Entering the method");
                var listVoucherForBuyer = new List<ItemList>();

                listVoucherForBuyer = _buyerUow.DiscountVouchers.GetAll().Where(v => v.RefBuyer == buyerId && v.PromotionEndDate >= DateTime.UtcNow).Select(v => new ItemList
                {
                    Text = v.PromotionalCode,
                    Value = v.Id
                }).ToList();

                Logger.Info("BuyerServiceFacade : GetVoucherListForBuyer() : Exiting the method");
                return listVoucherForBuyer;
            }
            catch (Exception ex)
            {
                Logger.Error("BuyerServiceFacade : GetVoucherListForBuyer() : Caught an exception" + ex);
                throw;
            }
        }
    }
}
