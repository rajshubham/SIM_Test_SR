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
    public static class VoucherMapper
    {
        public static DiscountVoucher ToEntityModel(this Voucher voucher)
        {
            return new DiscountVoucher()
            {
                Id = voucher.VoucherId,
                PromotionStartDate = voucher.PromotionStartDate.Value,
                PromotionEndDate = voucher.PromotionEndDate.Value,
                DiscountPercent = voucher.DiscountPercent,
                RefBuyer = voucher.BuyerPartyId,
                PromotionalCode = voucher.PromotionalCode,
                
            };
        }

        public static Voucher ToViewModel(this DiscountVoucher discountVoucher)
        {
            return new Voucher()
            {
                VoucherId = discountVoucher.Id,
                PromotionalCode = discountVoucher.PromotionalCode,
                PromotionStartDate = discountVoucher.PromotionStartDate.Date,
                PromotionEndDate = discountVoucher.PromotionEndDate.Date,
                BuyerPartyId = discountVoucher.RefBuyer,
                DiscountPercent = discountVoucher.DiscountPercent,
                BuyerName = (null != discountVoucher.Buyer) ? discountVoucher.Buyer.Organization.Party.PartyName : String.Empty,
            };
        }

        public static List<Voucher> ToViewModel(this List<DiscountVoucher> discountVoucherList)
        {
            var voucherList = discountVoucherList.Select(u => new Voucher()
            {
                VoucherId = u.Id,
                PromotionalCode = u.PromotionalCode,
                PromotionStartDate = u.PromotionStartDate,
                PromotionEndDate = u.PromotionEndDate,
                BuyerPartyId = u.RefBuyer,
                DiscountPercent = u.DiscountPercent,
                BuyerName = (null != u.Buyer) ? u.Buyer.Organization.Party.PartyName : String.Empty
            }).ToList();
            return voucherList;
        }
    }
}
