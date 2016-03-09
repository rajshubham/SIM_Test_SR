using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Abstract;
using PRGX.SIMTrax.DAL.Repository.Context;
using PRGX.SIMTrax.Domain.Model;
using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGX.SIMTrax.DAL.Repository.Repository
{
   public class ContactPersonRepository : GenericRepository<ContactPerson>, IContactPersonRepository
    {
        public ContactPersonRepository(DbContext context) : base(context) { }

        public List<ContactPerson> GetContactDetailsByPartyId(long sellerPartyId)
        {
            try
            {
                Logger.Info("ContactPersonRepository : GetContactDetailsByPartyId() : Enter the method");
                var ContactDetails = new List<ContactPerson>();
                ContactDetails = All().Include("Person").Include("Person").Include("Person.Party").Include("Person.Party.PartyContactMethodLinks").Include("Person.Party.PartyContactMethodLinks.ContactMethod.Emails")
                    .Include("Person.Party.PartyContactMethodLinks.ContactMethod.Phones").Include("Person.Party.PartyPartyLinks1").Include("Person.Party.PartyContactMethodLinks.ContactMethod.Addresses").Include("Person.Party.PartyContactMethodLinks.ContactMethod.Addresses.Region").
                    Where(v => v.Person.Party.PartyPartyLinks1.Any(c => c.RefLinkedParty == sellerPartyId && c.PartyPartyLinkType.Trim() == Constants.CONTACT_ORGANIZATION)).ToList();
                Logger.Info("ContactPersonRepository : GetContactDetailsByPartyId() : Enter the method");
                return ContactDetails;
            }
            catch (Exception ex)
            {
                Logger.Error("ContactPersonRepository : GetContactDetailsByPartyId() : Caught an error" + ex);
                throw;
            }
        }

        public ContactPerson GetContactByRoleAndPartyId(long sellerPartyId, int contactType)
        {
            try
            {
                Logger.Info("ContactPersonRepository : GetContactByRoleAndPartyId() : Enter the method");
                var ContactDetails = new ContactPerson();
                ContactDetails = All().Include("Person").Include("Person").Include("Person.Party").Include("Person.Party.PartyContactMethodLinks").Include("Person.Party.PartyContactMethodLinks.ContactMethod.Emails")
                    .Include("Person.Party.PartyContactMethodLinks.ContactMethod.Phones").Include("Person.Party.PartyContactMethodLinks.ContactMethod.Addresses").Include("Person.Party.PartyContactMethodLinks.ContactMethod.Addresses.Region").
                      FirstOrDefault(v => v.Person.Party.PartyPartyLinks1.Any(c => c.RefLinkedParty == sellerPartyId && c.PartyPartyLinkType.Trim() == Constants.CONTACT_ORGANIZATION) && v.ContactType == contactType);
                Logger.Info("ContactPersonRepository : GetContactByRoleAndPartyId() : Enter the method");
                return ContactDetails;
            }
            catch (Exception ex)
            {
                Logger.Error("ContactPersonRepository : GetContactByRoleAndPartyId() : Caught an error" + ex);
                throw;
            }
        }

        public List<BuyerSupplierContacts> BuyerSupplierContactList(int pageNo, string sortParameter, int sortDirection, string buyerName, long sellerId, long contactPartyId, out int totalRecords)
        {
            try
            {
                Logger.Info("ContactPersonRepository : BuyerSupplierContactList() : Enter into method");
                var buyerList = new List<BuyerSupplierContacts>();
                totalRecords = 0;
                using (var ctx = new SellerContext())
                {
                    var contactParty = ctx.Parties.FirstOrDefault(u => u.Id == contactPartyId);
                    if (contactParty != null)
                    {
                        //ToDo:We have To get Buyers only based on Buyer Supplier Mapping
                        var buyersList = ctx.Buyers.Where(u => u.Organization.Party.PartyName.Contains(buyerName) && u.Organization.Party.IsActive == true).AsQueryable();
                        totalRecords = buyersList.Count();
                        int size = 10;
                        var skipCount = (pageNo - 1) * size;
                        var List = buyersList.OrderBy(u => u.CreatedOn).Skip(skipCount).Take(size).ToList();
                        buyerList = List.Select(u => new BuyerSupplierContacts()
                        {
                            BuyerId = u.Id,
                            BuyerPartyId = u.Organization.Party.Id,
                            BuyerName = u.Organization.Party.PartyName,
                            ContactPartyId = contactPartyId,
                            RoleValue = (u.Organization.Party.PartyPartyLinks.FirstOrDefault(v => v.RefParty == contactPartyId && v.PartyPartyLinkType.Trim() == Constants.CONTACT_BUYER) != null)?
                            u.Organization.Party.PartyPartyLinks.FirstOrDefault(v => v.RefParty == contactPartyId && v.PartyPartyLinkType.Trim() == Constants.CONTACT_BUYER).PartyPartyLinkSubType:null,
                            IsAssigned = (u.Organization.Party.PartyPartyLinks.FirstOrDefault(v => v.RefParty == contactPartyId && v.PartyPartyLinkType.Trim() == Constants.CONTACT_BUYER) != null) ? true : false
                        }).ToList();
                    }

                }
                Logger.Info("ContactPersonRepository : BuyerSupplierContactList() : Exit from method");
                return buyerList;
            }
            catch (Exception ex)
            {
                Logger.Info("ContactPersonRepository : BuyerSupplierContactList() : Caught an Exception" + ex);
                throw;
            }
        }
    }
}
