using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class BuyerSupplierReference : BaseEntity<long>
    {
        //public long Id { get; set; }
        public long RefInvitee { get; set; }
        public long RefReferredTo { get; set; }
        public virtual Buyer Buyer { get; set; }
        public virtual Invitee Invitee { get; set; }
    }
}
