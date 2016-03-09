using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class MasterData : BaseEntity<long>
    {
       // public long Id { get; set; }
        public long RefMasterDataType { get; set; }
        public string Value { get; set; }
        public byte OrderId { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public Nullable<long> RefLocaleId { get; set; }
        public virtual Locale Locale { get; set; }
        public virtual MasterDataType MasterDataType { get; set; }
    }
}
