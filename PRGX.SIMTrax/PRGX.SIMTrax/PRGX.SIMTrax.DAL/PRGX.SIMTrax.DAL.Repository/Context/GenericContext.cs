using PRGX.SIMTrax.DAL.Entity.AuditModel;
using System;
using System.Data.Entity;
using System.Linq;

namespace PRGX.SIMTrax.DAL.Repository.Context
{
    public abstract class GenericContext : DbContext
    {
        public GenericContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
           
        }

        public void ApplySaveRule()
        {
            var modifiedEntries = ChangeTracker.Entries()
               .Where(x => x.Entity is IAuditableEntity
                   && (x.State == System.Data.Entity.EntityState.Added || x.State == System.Data.Entity.EntityState.Modified));

            foreach (var entry in modifiedEntries)
            {
                IAuditableEntity entity = entry.Entity as IAuditableEntity;
                if (entity != null)
                {
                    DateTime now = DateTime.UtcNow.Date;

                    if (entry.State == System.Data.Entity.EntityState.Added)
                    {
                        entity.CreatedOn = now;
                    }
                    else
                    {
                        base.Entry(entity).Property(x => x.CreatedOn).IsModified = false;
                    }

                    entity.LastUpdatedOn = now;
                }
            }

            // check for identity
            // also check for id
            var idEntities = ChangeTracker.Entries().Where(x => x.State == System.Data.Entity.EntityState.Added && x.Entity is IIdEntity<Guid>);
            foreach (var entry in modifiedEntries)
            {
                IIdEntity<Guid> idEntity = entry.Entity as IIdEntity<Guid>;
                if (idEntity != null)
                {
                    idEntity.Id = Guid.NewGuid();
                }
            }
        }
    }
}
