using System;
using System.Collections.Generic;
using PRGX.SIMTrax.DAL.Entity.AuditModel;

namespace PRGX.SIMTrax.DAL.Entity
{
    public partial class MasterDataType:BaseEntity<long>
    {
        public MasterDataType()
        {
            this.MasterDatas = new List<MasterData>();
        }

      //  public long Id { get; set; }
        public string Mnemonic { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<MasterData> MasterDatas { get; set; }

    }
}
