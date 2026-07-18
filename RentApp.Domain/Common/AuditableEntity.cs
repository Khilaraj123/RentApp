namespace RentApp.Domain.Common
{
    public class AuditableEntity : AggregateRoot
    {
        public DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}
