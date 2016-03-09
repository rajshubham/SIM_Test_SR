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
    public  class InviteeRepository : GenericRepository<Invitee>, IInviteeRepository
    {
        public InviteeRepository(DbContext context) : base(context) { }

        public List<Invitee> GetInviteeDetailsBySellerId(long sellerId)
        {
            try
            {
                var inviteeList = new List<Invitee>();
                Logger.Info("InviteeRepository : GetInviteeDetailsBySellerId() : Enter into method");
                inviteeList = All().Include("BuyerSupplierReferences").Where(v => v.RefReferee == sellerId).ToList();
                Logger.Info("InviteeRepository : GetInviteeDetailsBySellerId() : Exit from method");
                return inviteeList;
            }
            catch (Exception ex)
            {
                Logger.Info("InviteeRepository : GetInviteeDetailsBySellerId() : Caught an Exception" + ex);
                throw;
            }
        }
        public List<BuyerSupplierReferenceList> BuyerSupplierReferenceList(int pageNo, string sortParameter, int sortDirection, string buyerName, long sellerId, long referenceId, out int totalRecords)
        {
            try
            {
                Logger.Info("InviteeRepository : BuyerSupplierReferenceList() : Enter into method");
                var buyerList = new List<BuyerSupplierReferenceList>();
                totalRecords = 0;
                using (var ctx = new SellerContext())
                {
                    var invitee = ctx.Invitees.FirstOrDefault(u => u.Id == referenceId);
                    if (invitee != null)
                    {
                        //ToDo:We have To get Buyers only based on Buyer Supplier Mapping
                        var buyersList = ctx.Buyers.Where(u => u.Organization.Party.PartyName.Contains(buyerName) && u.Organization.Party.IsActive == true).AsQueryable();
                        totalRecords = buyersList.Count();
                        int size = 10;
                        var skipCount = (pageNo - 1) * size;
                        var List = buyersList.OrderBy(u => u.CreatedOn).Skip(skipCount).Take(size).ToList();
                        buyerList = List.Select(u => new BuyerSupplierReferenceList()
                        {
                            BuyerId = u.Id,
                            BuyerName = u.Organization.Party.PartyName,
                            ReferenceId = referenceId,
                            IsAssigned = (u.BuyerSupplierReferences.FirstOrDefault(v => v.RefInvitee == referenceId) != null) ? true : false
                        }).ToList();
                    }

                }
                Logger.Info("InviteeRepository : BuyerSupplierReferenceList() : Exit from method");
                return buyerList;
            }
            catch (Exception ex)
            {
                Logger.Info("InviteeRepository : BuyerSupplierReferenceList() : Caught an Exception" + ex);
                throw;
            }
        }
    }
}
