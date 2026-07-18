namespace RentApp.Domain.Common
{
    public class SoftDeleteEntity : AuditableEntity
    {
        public DateTime DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}
