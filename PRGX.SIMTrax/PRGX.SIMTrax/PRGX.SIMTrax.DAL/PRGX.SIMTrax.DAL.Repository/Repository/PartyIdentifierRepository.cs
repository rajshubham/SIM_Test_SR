using PRGX.SIMTrax.DAL.Entity;
using PRGX.SIMTrax.DAL.Repository.Abstract;
using PRGX.SIMTrax.Domain.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PRGX.SIMTrax.DAL.Repository.Repository
{
    public class PartyIdentifierRepository : GenericRepository<PartyIdentifier>, IPartyIdentifierRepository
    {
        public PartyIdentifierRepository(DbContext context) : base(context) { }

        public void AddUpdateRange(List<PartyIdentifier> partyIdentifiers)
        {
            try
            {
                Logger.Info("PartyIdentifierRepository : AddUpdateRange() : Enter the method");
                if (partyIdentifiers.Count > 0)
                {
                    var partyId = partyIdentifiers.FirstOrDefault().RefParty;

                    var identifierList = partyIdentifiers.Select(v => v.RefPartyIdentifierType).ToList();

                    var toDeletePartyIdentifier = this.All()
                            .Where(x => x.RefParty == partyId && !identifierList.Contains(x.RefPartyIdentifierType)).ToList();

                    foreach (var item in toDeletePartyIdentifier)
                    {
                        Delete(item);
                    }
                    foreach (var partyIdentifier in partyIdentifiers)
                    {
                        var partyIdentifierPM = this.All()
                            .Where(x => x.RefParty == partyIdentifier.RefParty && x.RefPartyIdentifierType == partyIdentifier.RefPartyIdentifierType).FirstOrDefault();

                        if (null == partyIdentifierPM)
                        {
                            Add(partyIdentifier);
                        }
                        else
                        {
                            partyIdentifierPM.IdentifierNumber = partyIdentifier.IdentifierNumber;
                            partyIdentifierPM.RefLastUpdatedBy = partyIdentifier.RefLastUpdatedBy;
                            Update(partyIdentifierPM);
                        }
                    }
                }
                Logger.Info("PartyIdentifierRepository : AddUpdateRange() : Exit the method");
            }
            catch (Exception ex)
            {
                Logger.Error("PartyIdentifierRepository : AddUpdateRange() : Caught an error" + ex);
                throw;
            }
        }
    }
}
