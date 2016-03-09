namespace PRGX.SIMTrax.DAL.Entity.AuditModel
{
    public class BaseEntity<T> : IIdEntity<T>
    {
        public virtual T Id { get; set; }
    }
}
