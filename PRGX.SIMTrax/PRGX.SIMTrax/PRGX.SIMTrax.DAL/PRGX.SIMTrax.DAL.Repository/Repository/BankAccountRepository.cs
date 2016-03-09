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

namespace PRGX.SIMTrax.DAL.Repository
{
    public class BankAccountRepository : GenericRepository<BankAccount>, IBankAccountRepository
    {
        public BankAccountRepository(DbContext context) : base(context) { }
        public List<BankAccount> GetBankDetailsByOrganisationId(long organisationId)
        {
            try
            {
                Logger.Info("BankAccountRepository : GetBankDetailsByOrganisationId() : Enter the method");
                var bankdetails = new List<BankAccount>();
                bankdetails = this.All().Include("Region").Include("LegalEntity").Include("LegalEntity.Organizations").Include("LegalEntityProfiles").
                    Where(v => v.LegalEntity.Organizations.Any(c => c.Id == organisationId)).ToList();
                Logger.Info("BankAccountRepository : GetBankDetailsByOrganisationId() : Exit the method");
                return bankdetails;
            }
            catch (Exception ex)
            {
                Logger.Error("BankAccountRepository : GetBankDetailsByOrganisationId() : Caught an error" + ex);
                throw;
            }
        }

        public List<BuyerSupplierBankAccount> BuyerSupplierBankList(int pageNo, string sortParameter, int sortDirection, string buyerName, long organisationId, long bankId, out int totalRecords)
        {
            try
            {
                Logger.Info("AddressRepository : BuyerSupplierAddressList() : Enter into method");
                var buyerList = new List<BuyerSupplierBankAccount>();
                totalRecords = 0;
                using (var ctx = new SellerContext())
                {
                    var bankAccount = ctx.BankAccounts.FirstOrDefault(u => u.Id == bankId);
                    if (bankAccount != null)
                    {
                        //ToDo:We have To get Buyers only based on Buyer Supplier Mapping
                        var buyersList = ctx.Buyers.Where(u => u.Organization.Party.PartyName.Contains(buyerName) && u.Organization.Party.IsActive == true).AsQueryable();
                        totalRecords = buyersList.Count();
                        int size = 10;
                        var skipCount = (pageNo - 1) * size;
                        var List = buyersList.OrderBy(u => u.CreatedOn).Skip(skipCount).Take(size).ToList();
                        buyerList = List.Select(u => new BuyerSupplierBankAccount()
                        {
                            BuyerId = u.Id,
                            BuyerName = u.Organization.Party.PartyName,
                            BankId = bankId,
                            RefPartyId = u.Organization.Party.Id,
                            IsAssigned = (u.Organization.Party.LegalEntityProfiles.FirstOrDefault(v => v.RefBank == bankId && v.ProfileType.Trim() == Constants.PROFILE_TYPE_BANK) != null) ? true : false
                        }).ToList();
                    }

                }
                Logger.Info("AddressRepository : BuyerSupplierAddressList() : Exit from method");
                return buyerList;
            }
            catch (Exception ex)
            {
                Logger.Info("AddressRepository : BuyerSupplierAddressList() : Caught an Exception" + ex);
                throw;
            }
        }
    }
}
