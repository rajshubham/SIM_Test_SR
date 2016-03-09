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
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository(DbContext context) : base(context) { }

        public void AddUpdateAddress(Address partyAddress, long contactMethodId)
        {
            try
            {
                Logger.Info("AddressRepository : AddUpdateAddress() : Enter the method");
                var addressPM = this.All()
                        .Where(x => x.RefContactMethod == contactMethodId && x.AddressType == partyAddress.AddressType).FirstOrDefault();

                if (null == addressPM)
                {
                    Add(partyAddress);
                }
                else
                {
                    addressPM.Line1 = partyAddress.Line1;
                    addressPM.Line2 = partyAddress.Line2;
                    addressPM.City = partyAddress.City;
                    addressPM.State = partyAddress.State;
                    addressPM.ZipCode = partyAddress.ZipCode;
                    addressPM.RefCountryId = partyAddress.RefCountryId;
                    addressPM.RefLastUpdatedBy = partyAddress.RefLastUpdatedBy;
                    Update(addressPM);
                }
                Logger.Info("AddressRepository : AddUpdateAddress() : Exit the method");
            }
            catch (Exception ex)
            {
                Logger.Error("AddressRepository : AddUpdateAddress() : Caught an error" + ex);
                throw;
            }
        }


        public List<Address> GetAddressDetailsByPartyId(long PartyId)
        {
            try
            {
                Logger.Info("AddressRepository : GetAddressDetailsByPartyId() : Enter the method");
                var addressList = new List<Address>();
                addressList = All().Include("ContactMethod").Include("ContactMethod.PartyContactMethodLinks").Include("Region").Where(v => v.ContactMethod.PartyContactMethodLinks.Any(c => c.RefParty == PartyId)).ToList();
                Logger.Info("AddressRepository : GetAddressDetailsByPartyId() : Exit the method");
                return addressList;
            }
            catch (Exception ex)
            {
                Logger.Error("AddressRepository : GetAddressDetailsByPartyId() : Caught an error" + ex);
                throw;
            }

        }

        public Address GetAddressDetailsByContactMethodId(long contactMethodId)
        {
            try
            {
                Logger.Info("AddressRepository : GetAddressDetailsByContactMethodId() : Enter the method");
                var address = new Address();
                address = All().Include("Region").FirstOrDefault(v => v.RefContactMethod == contactMethodId);
                Logger.Info("AddressRepository : GetAddressDetailsByContactMethodId() : Exit the method");
                return address;
            }
            catch (Exception ex)
            {
                Logger.Error("AddressRepository : GetAddressDetailsByContactMethodId() : Caught an error" + ex);
                throw;
            }
        }
       
        public List<BuyerSupplierAddressList> BuyerSupplierAddressList(int pageNo, string sortParameter, int sortDirection, string buyerName, long sellerPartyId, long addressId, out int totalRecords)
        {
            try
            {
                Logger.Info("AddressRepository : BuyerSupplierAddressList() : Enter into method");
                var buyerList = new List<BuyerSupplierAddressList>();
                totalRecords = 0;
                using (var ctx = new SellerContext())
                {
                    var address = ctx.Addresses.FirstOrDefault(u => u.Id == addressId);
                    if (address != null)
                    {
                        //ToDo:We have To get Buyers only based on Buyer Supplier Mapping
                        var buyersList = ctx.Buyers.Where(u => u.Organization.Party.PartyName.Contains(buyerName) && u.Organization.Party.IsActive == true).AsQueryable();
                        totalRecords = buyersList.Count();
                        int size = 10;
                        var skipCount = (pageNo - 1) * size;
                        var List = buyersList.OrderBy(u => u.CreatedOn).Skip(skipCount).Take(size).ToList();
                        buyerList = List.Select(u => new BuyerSupplierAddressList()
                        {
                            BuyerId = u.Id,
                            BuyerName = u.Organization.Party.PartyName,
                            AddressId = addressId,
                            RefPartyId = u.Organization.Party.Id,
                            RefContactMethod = address.RefContactMethod,
                            IsAssigned = (u.Organization.Party.LegalEntityProfiles.FirstOrDefault(v => v.RefContactMethod == address.RefContactMethod && v.ProfileType.Trim() == Constants.PROFILE_TYPE_ADDRESS) != null) ? true : false
                        }).ToList();
                    }

                }
                Logger.Info("AddressRepository : BuyerSupplierAddressList() : Exit from method");
                return buyerList;
            }
            catch (Exception ex)
            {
                Logger.Info("AddressRepository : BuyerSupplierReferenceListBuyerSupplierAddressList() : Caught an Exception" + ex);
                throw;
            }
        }
      
    }
}
