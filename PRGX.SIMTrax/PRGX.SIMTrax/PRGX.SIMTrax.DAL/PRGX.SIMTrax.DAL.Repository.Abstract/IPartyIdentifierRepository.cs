using PRGX.SIMTrax.DAL.Entity;
using System.Collections.Generic;

namespace PRGX.SIMTrax.DAL.Repository.Abstract
{
    public interface IPartyIdentifierRepository : IGenericRepository<PartyIdentifier>
    {
        void AddUpdateRange(List<PartyIdentifier> partyIdentifiers);
    }
}
